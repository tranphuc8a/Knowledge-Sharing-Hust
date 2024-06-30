using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.KnowledgeRepositories;
using KnowledgeSharingApi.Repositories.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Repositories.Repositories.MySqlRepositories.MySqlKnowledgeRepositories
{
    public class PostEditHistoryMySqlRepository(IDbContext dbContext)
        : BaseMySqlRepository<PostEditHistory>(dbContext), IPostEditHistoryRepository
    {
        protected override DbSet<PostEditHistory> GetDbSet()
        {
            return DbContext.PostEditHistories;
        }
    }
}
