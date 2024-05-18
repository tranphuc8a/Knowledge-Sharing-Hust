using Dapper;
using KnowledgeSharingApi.Domains.Common;
using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Repositories.BaseRepositories
{
    public abstract class BaseRepository<T> where T : Entity
    {
        protected int DefaultLimit = 15;
        protected int DefaultOffset = 0;

        #region Apply Order

        public virtual IQueryable<T> ApplyOrder(IQueryable<T> query, List<OrderDto> orders)
        {
            if (orders == null || orders.Count == 0)
            {
                return query;
            }

            // Áp dụng sắp xếp đầu tiên
            var parameter = Expression.Parameter(typeof(T), "x");
            IOrderedQueryable<T>? orderedQuery = null;
            bool isFirstOrder = true;

            foreach (var order in orders)
            {
                var property = typeof(T).GetProperty(order.Field);
                if (property == null)
                {
                    // Bỏ qua field không hợp lệ
                    continue;
                }

                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExpression = Expression.Lambda(propertyAccess, parameter);

                if (isFirstOrder)
                {
                    orderedQuery = order.IsAscending
                        ? Queryable.OrderBy(query, (dynamic)orderByExpression)
                        : Queryable.OrderByDescending(query, (dynamic)orderByExpression);
                    isFirstOrder = false;
                }
                else
                {
                    orderedQuery = order.IsAscending
                        ? Queryable.ThenBy(orderedQuery, (dynamic)orderByExpression)
                        : Queryable.ThenByDescending(orderedQuery, (dynamic)orderByExpression);
                }
            }

            return orderedQuery ?? query;
        }

        public virtual IQueryable<Q> ApplyOrder<Q>(IQueryable<Q> query, List<OrderDto> orders)
        {
            if (orders == null || orders.Count == 0)
            {
                return query;
            }

            // Áp dụng sắp xếp đầu tiên
            var parameter = Expression.Parameter(typeof(Q), "x");
            IOrderedQueryable<Q>? orderedQuery = null;
            bool isFirstOrder = true;

            foreach (var order in orders)
            {
                var property = typeof(Q).GetProperty(order.Field);
                if (property == null)
                {
                    // Bỏ qua field không hợp lệ
                    continue;
                }

                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExpression = Expression.Lambda(propertyAccess, parameter);

                if (isFirstOrder)
                {
                    orderedQuery = order.IsAscending
                        ? Queryable.OrderBy(query, (dynamic)orderByExpression)
                        : Queryable.OrderByDescending(query, (dynamic)orderByExpression);
                    isFirstOrder = false;
                }
                else
                {
                    orderedQuery = order.IsAscending
                        ? Queryable.ThenBy(orderedQuery, (dynamic)orderByExpression)
                        : Queryable.ThenByDescending(orderedQuery, (dynamic)orderByExpression);
                }
            }

            return orderedQuery ?? query;
        }

        public virtual List<T> ApplyOrder(List<T> beforeList, List<OrderDto> orders)
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

            foreach (var order in orders)
            {
                var property = typeof(T).GetProperty(order.Field);
                if (property == null)
                {
                    // Bỏ qua field không hợp lệ
                    continue;
                }

                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExpression = Expression.Lambda(propertyAccess, parameter);

                if (isFirstOrder)
                {
                    orderedQuery = order.IsAscending
                        ? Queryable.OrderBy(query, (dynamic)orderByExpression)
                        : Queryable.OrderByDescending(query, (dynamic)orderByExpression);
                    isFirstOrder = false;
                }
                else
                {
                    orderedQuery = order.IsAscending
                        ? Queryable.ThenBy(orderedQuery, (dynamic)orderByExpression)
                        : Queryable.ThenByDescending(orderedQuery, (dynamic)orderByExpression);
                }
            }

            return orderedQuery?.ToList() ?? beforeList;
        }

        public virtual List<Q> ApplyOrder<Q>(List<Q> beforeList, List<OrderDto> orders)
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

            foreach (var order in orders)
            {
                var property = typeof(Q).GetProperty(order.Field);
                if (property == null)
                {
                    // Bỏ qua field không hợp lệ
                    continue;
                }

                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExpression = Expression.Lambda(propertyAccess, parameter);

                if (isFirstOrder)
                {
                    orderedQuery = order.IsAscending
                        ? Queryable.OrderBy(query, (dynamic)orderByExpression)
                        : Queryable.OrderByDescending(query, (dynamic)orderByExpression);
                    isFirstOrder = false;
                }
                else
                {
                    orderedQuery = order.IsAscending
                        ? Queryable.ThenBy(orderedQuery, (dynamic)orderByExpression)
                        : Queryable.ThenByDescending(orderedQuery, (dynamic)orderByExpression);
                }
            }

            return orderedQuery?.ToList() ?? beforeList;
        }

        #endregion



        #region Functional methods

        private static bool FieldExistsInType<Type>(string fieldName)
        {
            return typeof(Type).GetProperty(fieldName) != null;
        }

        private static bool TryParseValue(string value, Type targetType, out object? result)
        {
            return KSTypeConverter.TryParseValue(value, targetType, out result);
        }

        private static ConstantExpression GetNullableExpression(object? value, Type targetType)
        {
            if (value == null)
            {
                return Expression.Constant(null, targetType);
            }
            else
            {
                if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    var underlyingType = Nullable.GetUnderlyingType(targetType);

                    // Kiểm tra nếu value không phải là kiểu underlyingType, tránh việc chuyển đổi không hợp lệ
                    if (value != null && value.GetType() != underlyingType)
                    {
                        throw new InvalidCastException($"Cannot convert value of type '{value.GetType()}' to Nullable<{underlyingType}>.");
                    }

                    // Chuyển đổi giá trị thành kiểu Nullable
                    return Expression.Constant(value, targetType);
                }
                else
                {
                    return Expression.Constant(value);
                }
            }
        }

        #endregion



        #region Apply Filters


        public virtual IQueryable<T> ApplyFilter(IQueryable<T> beforeQuery, List<FilterDto> listFilters)
        {
            IQueryable<T> query = beforeQuery;

            foreach (var filter in listFilters)
            {
                if (FieldExistsInType<T>(filter.Field))
                {
                    query = ApplyFilter(query, filter);
                }
                // Nếu trường không tồn tại, chúng ta có thể bỏ qua hoặc xử lý tùy thuộc vào yêu cầu cụ thể
            }

            return query;
        }
        public virtual IQueryable<T> ApplyFilter(IQueryable<T> query, FilterDto filter)
        {
            return ApplyFilter<T>(query, filter);
        }
        public virtual IQueryable<Type> ApplyFilter<Type>(IQueryable<Type> beforeQuery, List<FilterDto> listFilters)
        {
            IQueryable<Type> query = beforeQuery;

            foreach (var filter in listFilters)
            {
                if (FieldExistsInType<Type>(filter.Field))
                {
                    query = ApplyFilter(query, filter);
                }
                // Nếu trường không tồn tại, chúng ta có thể bỏ qua hoặc xử lý tùy thuộc vào yêu cầu cụ thể
            }

            return query;
        }
        public virtual IQueryable<Type> ApplyFilter<Type>(IQueryable<Type> query, FilterDto filter)
        {
            try
            {
                var parameter = Expression.Parameter(typeof(Type), "x");
                var property = Expression.Property(parameter, filter.Field);
                var propertyType = property.Type;


                if (!TryParseValue(filter.Value, propertyType, out object? value))
                    return query;

                // Tạo biểu thức lọc dựa trên giá trị đã chuyển đổi
                Expression? filterExpression = null;
                MethodInfo? method;
                switch (filter.Operation)
                {
                    case FilterOperations.Equal:
                        if (property.Type.IsGenericType && property.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            filterExpression = Expression.Equal(property, GetNullableExpression(value, property.Type));
                        }
                        else
                        {
                            filterExpression = Expression.Equal(property, Expression.Constant(value));
                        }
                        break;
                    case FilterOperations.GreaterThan:
                        if (property.Type.IsGenericType && property.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            filterExpression = Expression.GreaterThan(property, GetNullableExpression(value, property.Type));
                        }
                        else
                        {
                            filterExpression = Expression.GreaterThan(property, Expression.Constant(value));
                        }
                        break;
                    case FilterOperations.GreaterThanOrEqual:
                        if (property.Type.IsGenericType && property.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            filterExpression = Expression.GreaterThanOrEqual(property, GetNullableExpression(value, property.Type));
                        }
                        else
                        {
                            filterExpression = Expression.GreaterThanOrEqual(property, Expression.Constant(value));
                        }
                        break;
                    case FilterOperations.LessThan:
                        if (property.Type.IsGenericType && property.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            filterExpression = Expression.LessThan(property, GetNullableExpression(value, property.Type));
                        }
                        else
                        {
                            filterExpression = Expression.LessThan(property, Expression.Constant(value));
                        }
                        break;
                    case FilterOperations.LessThanOrEqual:
                        if (property.Type.IsGenericType && property.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            filterExpression = Expression.LessThanOrEqual(property, GetNullableExpression(value, property.Type));
                        }
                        else
                        {
                            filterExpression = Expression.LessThanOrEqual(property, Expression.Constant(value));
                        }
                        break;
                    case FilterOperations.NotEqual:
                        if (property.Type.IsGenericType && property.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            filterExpression = Expression.NotEqual(property, GetNullableExpression(value, property.Type));
                        }
                        else
                        {
                            filterExpression = Expression.NotEqual(property, Expression.Constant(value));
                        }
                        break;
                    case FilterOperations.Contain:
                        method = typeof(string).GetMethod("Contains", [typeof(string)]);
                        if (method != null)
                        {
                            filterExpression = Expression.Call(property, method, Expression.Constant(filter.Value));
                        }
                        break;
                    case FilterOperations.Like:
                        method = typeof(string).GetMethod("Contains", [typeof(string)]);
                        if (method != null)
                        {
                            filterExpression = Expression.Call(property, method, Expression.Constant(filter.Value));
                        }
                        break;
                    // Thêm các trường hợp khác nếu cần thiết
                    default:
                        break;
                }

                // Nếu filterExpression đã được thiết lập, tạo Lambda Expression và áp dụng lọc
                if (filterExpression != null)
                {
                    var lambda = Expression.Lambda<Func<Type, bool>>(filterExpression, parameter);
                    return query.Where(lambda);
                }
                return query;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return query;
            }
        }

        public virtual List<T> ApplyFilter(List<T> beforeList, List<FilterDto> filters)
        {
            List<T> list = beforeList;

            foreach (var filter in filters)
            {
                if (FieldExistsInType<T>(filter.Field))
                {
                    list = ApplyFilter(list, filter);
                }
                // Nếu trường không tồn tại, chúng ta có thể bỏ qua hoặc xử lý tùy thuộc vào yêu cầu cụ thể
            }

            return list;
        }
        public virtual List<T> ApplyFilter(List<T> beforeList, FilterDto filters)
        {
            return ApplyFilter<T>(beforeList, filters);
        }
        public virtual List<Type> ApplyFilter<Type>(List<Type> beforeList, FilterDto filter)
        {
            var parameter = Expression.Parameter(typeof(Type), "x");
            var property = Expression.Property(parameter, filter.Field);
            var propertyType = property.Type;

            if (!TryParseValue(filter.Value, propertyType, out object? value))
                return beforeList;

            // Tạo biểu thức lọc dựa trên giá trị đã chuyển đổi
            Expression? filterExpression = null;
            MethodInfo? method;
            switch (filter.Operation)
            {
                case FilterOperations.Equal:
                    filterExpression = Expression.Equal(property, Expression.Constant(value));
                    break;
                case FilterOperations.GreaterThan:
                    filterExpression = Expression.GreaterThan(property, Expression.Constant(value));
                    break;
                case FilterOperations.GreaterThanOrEqual:
                    filterExpression = Expression.GreaterThanOrEqual(property, Expression.Constant(value));
                    break;
                case FilterOperations.LessThan:
                    filterExpression = Expression.LessThan(property, Expression.Constant(value));
                    break;
                case FilterOperations.LessThanOrEqual:
                    filterExpression = Expression.LessThanOrEqual(property, Expression.Constant(value));
                    break;
                case FilterOperations.NotEqual:
                    filterExpression = Expression.NotEqual(property, Expression.Constant(value));
                    break;
                case FilterOperations.Contain:
                    method = typeof(string).GetMethod("Contains", [typeof(string)]);
                    if (method != null)
                    {
                        filterExpression = Expression.Call(property, method, Expression.Constant(filter.Value));
                    }
                    break;
                case FilterOperations.Like:
                    method = typeof(string).GetMethod("Contains", [typeof(string)]);
                    if (method != null)
                    {
                        filterExpression = Expression.Call(property, method, Expression.Constant(filter.Value));
                    }
                    break;
                // Thêm các trường hợp khác nếu cần thiết
                default:
                    break;
            }

            // Nếu filterExpression đã được thiết lập, tạo Lambda Expression và áp dụng lọc
            if (filterExpression != null)
            {
                var lambda = Expression.Lambda<Func<Type, bool>>(filterExpression, parameter).Compile();
                return beforeList.Where(lambda).ToList();
            }
            return beforeList;
        }
        public virtual List<Type> ApplyFilter<Type>(List<Type> beforeList, List<FilterDto> filters)
        {
            List<Type> list = beforeList;

            foreach (var filter in filters)
            {
                if (FieldExistsInType<Type>(filter.Field))
                {
                    list = ApplyFilter(list, filter);
                }
                // Nếu trường không tồn tại, chúng ta có thể bỏ qua hoặc xử lý tùy thuộc vào yêu cầu cụ thể
            }

            return list;
        }


        #endregion


        #region Apply Pagination

        public virtual List<T> ApplyPagination(List<T> beforeList, PaginationDto? pagination)
        {
            if (pagination == null) return beforeList;

            //  Apply Filter:
            if (pagination.Filters?.Count > 0)
            {
                beforeList = ApplyFilter(beforeList, pagination.Filters);
            }
            // Apply Orders:
            if (pagination.Orders?.Count > 0)
            {
                beforeList = ApplyOrder(beforeList, pagination.Orders);
            }
            // Apply Offset:
            beforeList = beforeList.Skip(pagination.Offset ?? DefaultOffset).ToList();
            // Apply Limit:
            beforeList = beforeList.Take(pagination.Limit ?? DefaultLimit).ToList();
            return beforeList;
        }
        public virtual IQueryable<T> ApplyPagination(IQueryable<T> beforeQuery, PaginationDto? pagination)
        {
            if (pagination == null) return beforeQuery;

            //  Apply Filter:
            if (pagination.Filters?.Count > 0)
            {
                beforeQuery = ApplyFilter(beforeQuery, pagination.Filters);
            }
            // Apply Orders:
            if (pagination.Orders?.Count > 0)
            {
                beforeQuery = ApplyOrder(beforeQuery, pagination.Orders);
            }
            // Apply Offset:
            beforeQuery = beforeQuery.Skip(pagination.Offset ?? DefaultOffset);
            // Apply Limit:
            beforeQuery = beforeQuery.Take(pagination.Limit ?? DefaultLimit);
            return beforeQuery;
        }
        public virtual List<Q> ApplyPagination<Q>(List<Q> beforeList, PaginationDto? pagination)
        {
            if (pagination == null) return beforeList;

            //  Apply Filter:
            if (pagination.Filters?.Count > 0)
            {
                beforeList = ApplyFilter(beforeList, pagination.Filters);
            }
            // Apply Orders:
            if (pagination.Orders?.Count > 0)
            {
                beforeList = ApplyOrder(beforeList, pagination.Orders);
            }
            // Apply Offset:
            beforeList = beforeList.Skip(pagination.Offset ?? DefaultOffset).ToList();
            // Apply Limit:
            beforeList = beforeList.Take(pagination.Limit ?? DefaultLimit).ToList();
            return beforeList;
        }
        public virtual IQueryable<Q> ApplyPagination<Q>(IQueryable<Q> beforeQuery, PaginationDto? pagination)
        {
            if (pagination == null) return beforeQuery;

            //  Apply Filter:
            if (pagination.Filters?.Count > 0)
            {
                beforeQuery = ApplyFilter(beforeQuery, pagination.Filters);
            }
            // Apply Orders:
            if (pagination.Orders?.Count > 0)
            {
                beforeQuery = ApplyOrder(beforeQuery, pagination.Orders);
            }
            // Apply Offset:
            beforeQuery = beforeQuery.Skip(pagination.Offset ?? DefaultOffset);
            // Apply Limit:
            beforeQuery = beforeQuery.Take(pagination.Limit ?? DefaultLimit);
            return beforeQuery;
        }

        #endregion


        #region Get MySql Clause

        public virtual string GetWhereClause<Type>(List<FilterDto>? filters, out DynamicParameters parameters)
        {
            parameters = GetDynamicParameters<Type>(filters);
            if (filters == null || filters.Count == 0)
            {
                return string.Empty;
            }

            List<string> conditions = [];

            foreach (var filter in filters)
            {
                string field = filter.Field;
                string operation = filter.Operation;
                string parameterName = $"@{field}";

                // Check if the field exists in the Type
                var property = typeof(Type).GetProperty(field, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (property == null)
                {
                    continue; // Skip if field does not exist
                }

                string condition;
                switch (operation)
                {
                    case FilterOperations.Equal:
                        condition = $"{field} = {parameterName}";
                        break;
                    case FilterOperations.GreaterThan:
                        condition = $"{field} > {parameterName}";
                        break;
                    case FilterOperations.GreaterThanOrEqual:
                        condition = $"{field} >= {parameterName}";
                        break;
                    case FilterOperations.LessThan:
                        condition = $"{field} < {parameterName}";
                        break;
                    case FilterOperations.LessThanOrEqual:
                        condition = $"{field} <= {parameterName}";
                        break;
                    case FilterOperations.NotEqual:
                        condition = $"{field} != {parameterName}";
                        break;
                    case FilterOperations.Contain:
                        condition = $"{field} LIKE %{parameterName}%";
                        break;
                    case FilterOperations.Like:
                        condition = $"{field} LIKE %{parameterName}%";
                        break;
                    // Add more cases if needed
                    default:
                        continue;
                }

                if (condition != null)
                {
                    conditions.Add(condition);
                }
            }

            return conditions.Count > 0 ? "WHERE " + string.Join(" AND ", conditions) : string.Empty;
        }

        public virtual DynamicParameters GetDynamicParameters<Type>(List<FilterDto>? filters)
        {
            DynamicParameters parameters = new();
            if (filters == null || filters.Count <= 0) return parameters;

            foreach (var filter in filters)
            {
                string field = filter.Field;
                string parameterName = $"@{field}";

                // Check if the field exists in the Type
                var property = typeof(Type).GetProperty(field, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (property == null)
                {
                    continue; // Skip if field does not exist
                }

                if (TryParseValue(filter.Value, property.GetType(), out object? value))
                {
                    parameters.Add(parameterName, value);
                }
            }
            return parameters;
        }

        public virtual string GetOrderClause<Type>(List<OrderDto>? orders)
        {
            if (orders == null || orders.Count == 0)
            {
                return string.Empty;
            }

            List<string> orderConditions = [];

            foreach (var order in orders)
            {
                string field = order.Field;
                string direction = order.IsAscending ? "ASC" : "DESC";

                // Check if the field exists in the Type
                var property = typeof(Type).GetProperty(field, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (property == null)
                {
                    continue; // Skip if field does not exist
                }

                orderConditions.Add($"{field} {direction}");
            }

            return orderConditions.Count > 0 ? "ORDER BY " + string.Join(", ", orderConditions) : string.Empty;
        }

        public virtual string GetLimitOffsetClause(int? limit, int? offset, out DynamicParameters parameters)
        {
            DynamicParameters newParams = new();
            string res = string.Empty;

            limit ??= DefaultLimit;
            offset ??= DefaultOffset;

            res += " LIMIT @limit ";
            newParams.Add("limit", limit);
            res += " OFFSET @offset ";
            newParams.Add("offset", offset);
            
            parameters = newParams;
            return res;
        }

        public virtual DynamicParameters CombineDynamicParameters(params DynamicParameters[] paramList)
        {
            var combinedParams = new DynamicParameters();

            foreach (var param in paramList)
            {
                combinedParams.AddDynamicParams(param);
            }

            return combinedParams;
        }

        #endregion
    }
}
