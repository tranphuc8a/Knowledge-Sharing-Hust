using Dapper;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Models.Entities;
using KnowledgeSharingApi.Infrastructures.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories;
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
    public class BaseMySqlRepository<T> : IRepository<T> where T : Entity
    {
        public IDbContext DbContext;
        protected readonly string TableName;
        protected readonly string NameField;
        protected IDbConnection Connection;
        public BaseMySqlRepository(IDbContext dbContext)
        {
            if (dbContext is not MySqlDbContext)
            {
                throw new DbContextNotMatchingException();
            }
            DbContext = dbContext;
            Connection = dbContext.Connection;

            TableName = typeof(T).Name;
            NameField = $"{TableName}Name";
        }

        #region Get Records
        public virtual async Task<IEnumerable<T>> Get()
        {
            string sqlCommand = $"Select * from {TableName}";
            return await Connection.QueryAsync<T>(sqlCommand, transaction: DbContext.Transaction);
        }

        public virtual async Task<IEnumerable<T>> Get(int limit, int offset)
        {
            // Get with pagination
            string sqlCommand = $"Select * from {TableName} limit @limit offset @offset";
            return await Connection.QueryAsync<T>(sqlCommand, new { limit, offset }, transaction: DbContext.Transaction);
        }

        public virtual async Task<T?> Get(string id)
        {
            string sqlCommand = $"Select * from {TableName} where {TableName}Id = @id";
            return await Connection.QueryFirstOrDefaultAsync<T>(sqlCommand, new { id }, transaction: DbContext.Transaction);
        }

        #endregion


        #region Insert New Record

        public virtual async Task<string?> Insert(T entity)
        {
            string entityId = $"{TableName}Id";
            List<string> props = entity.GetProperties().Select(p => p.Name).ToList()
                                        .Where(p => p != entityId).ToList();
            string entityColumns = string.Join(", ", props);
            string entityParams = string.Join(", ", props.Select(p => $"@{p}").ToList());
            string sqlCommand = $"Set @id = UUID(); " +
                                $"Insert into {TableName}({entityId}, {entityColumns}) value(@id, {entityParams}); " +
                                $"Select @id;";
            return await Connection.QueryFirstOrDefaultAsync<string>(sqlCommand, entity, transaction: DbContext.Transaction);
        }
        #endregion


        #region Delete A or Multi Record

        public virtual async Task<int> Delete(string id)
        {
            string sqlCommand = $"Delete from {TableName} where {TableName}Id = @id";
            return await Connection.ExecuteAsync(sqlCommand, new { id });
        }
        public virtual async Task<int> Delete(string[] ids)
        {
            string sqlCommand = $"Delete from {TableName} where {TableName}Id in @ids";
            return await Connection.ExecuteAsync(sqlCommand, new { ids }, transaction: DbContext.Transaction);
        }
        #endregion


        #region Update A Record
        public virtual async Task<int> Update(string id, T entity)
        {
            // prepare params
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("id", id);
            PropertyInfo[] listProps = entity.GetProperties();
            foreach (var item in listProps)
            {
                parameters.Add(item.Name, item.GetValue(entity));
            }

            // prepare sqlCommand
            string[] listCommands = listProps.Select(p => p.Name).ToArray()
                                             .Where(p => p != $"{TableName}Id").ToArray()
                                             .Select(p => $"{p} = @{p}").ToArray();
            string setCommands = string.Join(", ", listCommands);
            string sqlCommand = $"Update {TableName} set {setCommands} where {TableName}Id = @id";

            // execute:
            return await Connection.ExecuteAsync(sqlCommand, parameters, transaction: DbContext.Transaction);
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

        public virtual async Task<IEnumerable<T>> Filter(string text, int limit, int offset)
        {
            string templateText = $"%{text}%";
            string sqlCommand = $"Select * from {TableName} " +
                                $"where {NameField} like @templateText or @text like CONCAT('%',{NameField},'%') " +
                                $"limit @limit offset @offset;";
            return await Connection.QueryAsync<T>(sqlCommand, new { text, templateText, limit, offset });
        }

     
        #endregion
    }
}
