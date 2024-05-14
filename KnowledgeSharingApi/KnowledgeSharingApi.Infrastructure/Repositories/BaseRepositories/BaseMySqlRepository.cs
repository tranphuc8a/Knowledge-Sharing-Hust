using Dapper;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Entities;
using KnowledgeSharingApi.Infrastructures.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
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

        //protected abstract DbSet<T> GetDbContext();

        #region Get Ordered Methods

        public virtual IQueryable<T> GetOrderedQueryable(IQueryable<T> query, List<(string Field, bool Ascending)> orders)
        {
            if (orders == null || orders.Count == 0)
            {
                return query;
            }

            // Áp dụng sắp xếp đầu tiên
            var parameter = Expression.Parameter(typeof(T), "x");
            IOrderedQueryable<T>? orderedQuery = null;
            bool isFirstOrder = true;

            foreach (var (Field, Ascending) in orders)
            {
                var property = typeof(T).GetProperty(Field);
                if (property == null)
                {
                    // Bỏ qua field không hợp lệ
                    continue;
                }

                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExpression = Expression.Lambda(propertyAccess, parameter);

                if (isFirstOrder)
                {
                    orderedQuery = Ascending
                        ? Queryable.OrderBy(query, (dynamic)orderByExpression)
                        : Queryable.OrderByDescending(query, (dynamic)orderByExpression);
                    isFirstOrder = false;
                }
                else
                {
                    orderedQuery = Ascending
                        ? Queryable.ThenBy(orderedQuery, (dynamic)orderByExpression)
                        : Queryable.ThenByDescending(orderedQuery, (dynamic)orderByExpression);
                }
            }

            return orderedQuery ?? query;
        }

        public virtual List<T> GetOrderedList(List<T> beforeList, List<(string Field, bool Ascending)> orders)
        {
            if (orders == null || orders.Count == 0)
            {
                return beforeList;
            }

            // Chuyển List thành IQueryable để sử dụng Linq cho sắp xếp
            IQueryable<T> query = beforeList.AsQueryable();

            // Áp dụng sắp xếp đầu tiên
            var parameter = Expression.Parameter(typeof(T), "x");
            IOrderedQueryable<T>? orderedQuery = null;
            bool isFirstOrder = true;

            foreach (var (Field, Ascending) in orders)
            {
                var property = typeof(T).GetProperty(Field);
                if (property == null)
                {
                    // Bỏ qua field không hợp lệ
                    continue;
                }

                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExpression = Expression.Lambda(propertyAccess, parameter);

                if (isFirstOrder)
                {
                    orderedQuery = Ascending
                        ? Queryable.OrderBy(query, (dynamic)orderByExpression)
                        : Queryable.OrderByDescending(query, (dynamic)orderByExpression);
                    isFirstOrder = false;
                }
                else
                {
                    orderedQuery = Ascending
                        ? Queryable.ThenBy(orderedQuery, (dynamic)orderByExpression)
                        : Queryable.ThenByDescending(orderedQuery, (dynamic)orderByExpression);
                }
            }

            return orderedQuery?.ToList() ?? beforeList;
        }

        public virtual List<Q> GetOrderedList<Q>(List<Q> beforeList, List<(string Field, bool Ascending)> orders)
        {
            if (orders == null || orders.Count == 0)
            {
                return beforeList;
            }

            // Chuyển List thành IQueryable để sử dụng Linq cho sắp xếp
            IQueryable<Q> query = beforeList.AsQueryable();

            // Áp dụng sắp xếp đầu tiên
            var parameter = Expression.Parameter(typeof(Q), "x");
            IOrderedQueryable<Q>? orderedQuery = null;
            bool isFirstOrder = true;

            foreach (var (Field, Ascending) in orders)
            {
                var property = typeof(Q).GetProperty(Field);
                if (property == null)
                {
                    // Bỏ qua field không hợp lệ
                    continue;
                }

                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExpression = Expression.Lambda(propertyAccess, parameter);

                if (isFirstOrder)
                {
                    orderedQuery = Ascending
                        ? Queryable.OrderBy(query, (dynamic)orderByExpression)
                        : Queryable.OrderByDescending(query, (dynamic)orderByExpression);
                    isFirstOrder = false;
                }
                else
                {
                    orderedQuery = Ascending
                        ? Queryable.ThenBy(orderedQuery, (dynamic)orderByExpression)
                        : Queryable.ThenByDescending(orderedQuery, (dynamic)orderByExpression);
                }
            }

            return orderedQuery?.ToList() ?? beforeList;
        }


        #endregion

        #region Get Records
        public virtual async Task<IEnumerable<T>> Get()
        {
            string sqlCommand = $"Select * from {TableName}";
            return await Connection.QueryAsync<T>(sqlCommand, transaction: Transaction);
        }

        public virtual async Task<PaginationResponseModel<T>> Get(int limit, int offset)
        {
            //DbSet<T> dbContext = GetDbContext();
            //int count = dbContext.Count();
            //List<T> listObjects = await dbContext.Skip(offset).Take(limit).ToListAsync();
            string sqlCommand = $"Select * from {TableName} ";
            PaginationResponseModel<T> res = await GetPagination<T>(sqlCommand, limit: limit, offset: offset);
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

        public virtual async Task<PaginationResponseModel<T>> Filter(string text, int limit, int offset, List<(string, bool)> order)
        {
            // Tạo truy vấn:
            string templateText = $"%{text}%";
            string subCommand = $"Select * from {TableName} " +
                                $"where {NameField} like @templateText or @text like CONCAT('%',{NameField},'%') ";

            PaginationResponseModel<T> res = await GetPagination<T>(
                subCommand: subCommand,
                limit: limit, offset: offset, orders: order,
                subCommandParameters: new { templateText }
            );
            return res;
        }




        #endregion

        public virtual void RegisterTransaction(IDbTransaction transaction)
        {
            this.Transaction = transaction;
        }
        protected virtual async Task<PaginationResponseModel<Q>> GetPagination<Q>(
            string subCommand, 
            int limit, int offset, 
            List<(string Field, bool IsAscending)>? orders = null,
            object? subCommandParameters = null) where Q : Entity
        {
            var orderClause = string.Empty;
            if (orders != null)
            {
                // Lọc ra những Orders hợp lệ với các props của lớp Q
                var validFields = typeof(Q).GetProperties()
                    .Select(prop => prop.Name).ToHashSet();

                var validOrders = orders.Where(order => validFields.Contains(order.Field)).ToList();

                // Tạo câu lệnh ORDER BY
                orderClause = validOrders.Count > 0
                    ? "ORDER BY " + string.Join(", ", validOrders.Select(order => $"{order.Field} {(order.IsAscending ? "ASC" : "DESC")}"))
                    : string.Empty;
            }

            // Tạo quy vấn
            string sqlCommand =
                $"SET sql_require_primary_key=OFF; " +
                $"CREATE TEMPORARY TABLE temp_selected_records AS {subCommand}; " +
                $"SELECT COUNT(*) AS record_count FROM temp_selected_records; " +
                $"SELECT * FROM temp_selected_records {orderClause} LIMIT @limit OFFSET @offset; " +
                $"DROP TEMPORARY TABLE IF EXISTS temp_selected_records;";

            // Tạo đối tượng dynamic để kết hợp subCommandParameters với limit và offset
            var queryParameters = new DynamicParameters(subCommandParameters);
            queryParameters.Add("limit", limit);
            queryParameters.Add("offset", offset);

            // Thực hiện truy vấn
            using var multipleResults = await Connection.QueryMultipleAsync(sqlCommand, queryParameters, Transaction);

            return new PaginationResponseModel<Q>
            {
                Total = multipleResults.Read<int>().Single(),
                Limit = limit,
                Offset = offset,
                Results = multipleResults.Read<Q>(),
            };
        }

        public virtual async Task<T> CheckExisted(Guid entityId, string errorMessage)
        {
            return await Get(entityId) ?? throw new NotExistedEntityException(errorMessage);
        }
    }
}
