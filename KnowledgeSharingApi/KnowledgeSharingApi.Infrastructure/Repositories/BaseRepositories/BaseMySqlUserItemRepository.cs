using Dapper;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Entities;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Repositories.BaseRepositories
{
    public abstract class BaseMySqlUserItemRepository<T> : BaseMySqlRepository<T> where T : UserItem
    {
        protected BaseMySqlUserItemRepository(IDbContext dbContext) : base(dbContext)
        {
            TableNameId = "UserItemId";
            _ = DbContext.Users.FirstOrDefault();
        }

        /// <summary>
        /// Lấy về Db set của riêng entity T
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        protected abstract DbSet<T> GetDbSet();


        #region Get 
        public override async Task<T?> Get(Guid id)
        {
            return (T?) (await GetDbSet().FindAsync(id))?.Clone();
        }

        public override async Task<IEnumerable<T>> Get()
        {
            return await GetDbSet().OrderByDescending(item => item.CreatedTime).ToListAsync();
        }

        public override async Task<PaginationResponseModel<T>> Get(int limit, int offset)
        {
            IQueryable<T> list = GetDbSet().OrderByDescending(item => item.CreatedTime);
            int total = list.Count();
            PaginationResponseModel<T> res = new()
            {
                Total = total,
                Limit = limit,
                Offset = offset,
                Results = await list.Skip(offset).Take(limit).ToListAsync()
            };
            return res;
        }

        public override async Task<IEnumerable<T?>> Get(Guid[] ids)
        {
            IEnumerable<T> lists = await GetDbSet()
                .Where(item => ids.Contains(item.UserItemId)).ToListAsync();
            return ids.Select(id => lists.Where(it => it.UserItemId == id).FirstOrDefault()).ToList();
        }

        #endregion


        #region Insert New Record

        public override async Task<Guid?> Insert(Guid id, T value)
        {
            value.UserItemId = id;
            GetDbSet().Add(value);
            int res = await DbContext.SaveChangesAsync();
            return res > 0 ? id : null;
        }

        public override async Task<Guid?> Insert(T value)
        {
            Guid id = Guid.NewGuid();
            value.UserItemId = id;
            GetDbSet().Add(value);
            int res = await DbContext.SaveChangesAsync();
            return res > 0 ? id : null;
        }

        #endregion


        #region Delete A or Multi Record

        public override async Task<int> Delete(Guid id)
        {
            T? item = await GetDbSet().FindAsync(id);
            if (item != null)
                GetDbSet().Remove(item);
            return await DbContext.SaveChangesAsync();
        }
        public override async Task<int> Delete(Guid[] ids)
        {
            IEnumerable<T> list = await GetDbSet()
                .Where(item => ids.Contains(item.UserId)).ToListAsync();
            GetDbSet().RemoveRange(list);
            return await DbContext.SaveChangesAsync();
        }
        #endregion


        #region Update A Record
        public override async Task<int> Update(Guid id, T entity)
        {
            T? item = await GetDbSet().FindAsync(id);
            if (item == null) return 0;
            entity.UserItemId = id;
            item.Copy(entity);
            return await DbContext.SaveChangesAsync();
        }
        #endregion

    }
}
