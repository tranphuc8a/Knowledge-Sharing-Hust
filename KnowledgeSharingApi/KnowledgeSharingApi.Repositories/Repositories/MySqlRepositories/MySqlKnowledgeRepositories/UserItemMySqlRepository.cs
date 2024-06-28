using Dapper;
using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces;
using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.KnowledgeRepositories;
using KnowledgeSharingApi.Repositories.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Repositories.Repositories.MySqlRepositories.MySqlKnowledgeRepositories
{
    public class UserItemMySqlRepository(IDbContext dbContext)
        : BaseMySqlUserItemRepository<UserItem>(dbContext), IUserItemRepository
    {
        //public override async Task<UserItem?> Get(Guid id)
        //{
        //    // User? user = DbContext.Users.Where(user => user.UserId == id).FirstOrDefault();
        //    return await DbContext.UserItems.Where(item => item.UserId == id).FirstOrDefaultAsync();
        //    //return await Connection.QueryFirstOrDefaultAsync<UserItem?>(
        //    //    $"Select * from UserItem where UserItemId = @id limit 1;", new { id }
        //    //);
        //}

        protected override DbSet<UserItem> GetDbSet()
        {
            return DbContext.UserItems;
        }
    }
}
