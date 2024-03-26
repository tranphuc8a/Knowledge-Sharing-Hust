using Dapper;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Entities;
using KnowledgeSharingApi.Infrastructures.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace KnowledgeSharingApi.Infrastructures.Repositories.BaseRepositories
{
    public abstract class BaseMySqlRepository<T> : IRepository<T> where T : Entity
    {
        public IDbContext DbContext;
        protected string TableName;
        protected string TableNameId;
        protected string NameField;
        public IDbConnection Connection { get; set; }
        public IDbTransaction? Transaction { get; set; }

        public BaseMySqlRepository(IDbContext dbContext)
        {
            if (dbContext is not MySqlDbContext)
            {
                throw new DbContextNotMatchTypeException();
            }
            DbContext = dbContext;
            Connection = dbContext.Connection;

            TableName = typeof(T).Name;
            TableNameId = $"{TableName}Id";
            NameField = $"{TableName}Name";
        }

        #region Get Records
        public virtual async Task<IEnumerable<T>> Get()
        {
            string sqlCommand = $"Select * from {TableName}";
            return await Connection.QueryAsync<T>(sqlCommand, transaction: Transaction);
        }

        public virtual async Task<PaginationResponseModel<T>> Get(int limit, int offset)
        {
            string sqlCommand = $"Select * from {TableName} ";
            PaginationResponseModel<T> res = await GetPagination<T>(sqlCommand, new { limit, offset });
            res.Limit = limit;
            res.Offset = offset;
            return res;
        }

        public virtual async Task<T?> Get(Guid id)
        {
            string sqlCommand = $"Select * from {TableName} where {TableNameId} = @id";
            return await Connection.QueryFirstOrDefaultAsync<T>(sqlCommand, new { id });
        }

        public virtual async Task<IEnumerable<T?>> Get(Guid[] ids)
        {
            string sqlCommand = $"Select * from {TableName} where {TableNameId} in @ids ";
            return await Connection.QueryAsync<T>(sqlCommand, new { ids }, transaction: Transaction);
        }

        #endregion


        #region Insert New Record

        public virtual async Task<Guid?> Insert(Guid id, T entity)
        {
            // Set new Guid for Id of entity
            string entityId = $"{TableNameId}";
            PropertyInfo propertyInfo = typeof(T).GetProperty(entityId)
                ?? throw new NotMatchTypeException();
            propertyInfo.SetValue(entity, id);

            // Create sql Command
            List<string> props = entity.GetProperties().Select(p => p.Name).ToList();
            string entityColumns = string.Join(", ", props);
            string entityParams = string.Join(", ", props.Select(p => $"@{p}").ToList());
            string sqlCommand = $"Insert into {TableName}({entityColumns}) value({entityParams}); ";

            // Excute and return result:
            int res = await Connection.ExecuteAsync(sqlCommand, entity, transaction: Transaction);
            return res > 0 ? id : null;
        }

        public virtual async Task<Guid?> Insert(T entity)
        {
            // Set new Guid for Id of entity
            string entityId = $"{TableNameId}";
            Guid newId = Guid.NewGuid();
            PropertyInfo propertyInfo = typeof(T).GetProperty(entityId)
                ?? throw new NotMatchTypeException();
            propertyInfo.SetValue(entity, newId);

            // Create sql Command
            List<string> props = entity.GetProperties().Select(p => p.Name).ToList();
            string entityColumns = string.Join(", ", props);
            string entityParams = string.Join(", ", props.Select(p => $"@{p}").ToList());
            string sqlCommand = $"Insert into {TableName}({entityColumns}) value({entityParams}); ";

            // Excute and return result:
            int res = await Connection.ExecuteAsync(sqlCommand, entity, transaction: Transaction);
            return res > 0 ? newId : null;
        }
        #endregion


        #region Delete A or Multi Record

        public virtual async Task<int> Delete(Guid id)
        {
            string sqlCommand = $"Delete from {TableName} where {TableNameId} = @id";
            return await Connection.ExecuteAsync(sqlCommand, new { id });
        }
        public virtual async Task<int> Delete(Guid[] ids)
        {
            string sqlCommand = $"Delete from {TableName} where {TableNameId} in @ids";
            return await Connection.ExecuteAsync(sqlCommand, new { ids }, transaction: Transaction);
        }
        #endregion


        #region Update A Record
        public virtual async Task<int> Update(Guid id, T entity)
        {
            // prepare params
            DynamicParameters parameters = new();
            parameters.Add("id", id);
            PropertyInfo[] listProps = entity.GetProperties();
            foreach (var item in listProps)
            {
                parameters.Add(item.Name, item.GetValue(entity));
            }

            // prepare sqlCommand
            string[] listCommands = listProps.Select(p => p.Name).ToArray()
                                             .Where(p => p != $"{TableNameId}").ToArray()
                                             .Select(p => $"{p} = @{p}").ToArray();
            string setCommands = string.Join(", ", listCommands);
            string sqlCommand = $"Update {TableName} set {setCommands} where {TableNameId} = @id";

            // execute:
            return await Connection.ExecuteAsync(sqlCommand, parameters, transaction: Transaction);
        }
        #endregion


        #region Filter Entities
        public virtual async Task<IEnumerable<T>> Filter(string text)
        {
            string templateText = $"%{text}%";
            string sqlCommand = $"Select * from {TableName} " +
                                $"where {NameField} like @templateText or @text like CONCAT('%',{NameField},'%');";
            return await Connection.QueryAsync<T>(sqlCommand, new { text, templateText });
        }

        public virtual async Task<PaginationResponseModel<T>> Filter(string text, int limit, int offset)
        {
            // Tạo truy vấn:
            string templateText = $"%{text}%";
            string subCommand = $"Select * from {TableName} " +
                                $"where {NameField} like @templateText or @text like CONCAT('%',{NameField},'%') ";

            PaginationResponseModel<T> res = await GetPagination<T>(
                subCommand: subCommand,
                parameters: new { templateText, limit, offset }
            );
            res.Limit = limit;
            res.Offset = offset;
            return res;
        }




        #endregion

        public virtual void RegisterTransaction(IDbTransaction transaction)
        {
            this.Transaction = transaction;
        }
        protected virtual async Task<PaginationResponseModel<Q>> GetPagination<Q>(string subCommand, object? parameters = null) where Q: Entity
        {
            string sqlCommand =
                $"CREATE TEMPORARY TABLE temp_selected_records AS {subCommand}; " +
                $"SELECT COUNT(*) AS record_count FROM temp_selected_records; " +
                $"SELECT * FROM temp_selected_records LIMIT @limit OFFSET @offset; " +
                $"DROP TEMPORARY TABLE IF EXISTS temp_selected_records; ";

            // Thực hiện truy vấn
            using var multipleResults = await Connection.QueryMultipleAsync(sqlCommand, parameters, Transaction);

            return new PaginationResponseModel<Q>
            {
                Total = multipleResults.Read<int>().Single(),
                Results = multipleResults.Read<Q>()
            };
        }

        public virtual async Task<T> CheckExisted(Guid entityId, string errorMessage)
        {
            return await Get(entityId) ?? throw new NotExistedEntityException(errorMessage);
        }
    }
}
