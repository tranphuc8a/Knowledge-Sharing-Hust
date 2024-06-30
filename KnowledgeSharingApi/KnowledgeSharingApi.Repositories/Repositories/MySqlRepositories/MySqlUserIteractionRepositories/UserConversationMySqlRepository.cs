using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.UserIteractionRepositories;
using KnowledgeSharingApi.Repositories.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Repositories.Repositories.MySqlRepositories.MySqlUserIteractionRepositories
{
    public class UserConversationMySqlRepository(IDbContext dbContext)
        : BaseMySqlRepository<UserConversation>(dbContext), IUserConversationRepository
    {
        protected override DbSet<UserConversation> GetDbSet()
        {
            return DbContext.UserConversations;
        }
    }
}
