using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Repositories.MySqlRepositories
{
    public class MarkMySqlRepository(IDbContext dbContext)
        : BaseMySqlRepository<Mark>(dbContext), IMarkRepository
    {
        public virtual async Task<Dictionary<Guid, bool>> GetUserMarkListKnowledge(Guid userId, IEnumerable<Guid> knowledgeId)
        {
            Dictionary<Guid, bool> result = knowledgeId.ToDictionary(id => id, id => false);

            List<Mark> listMarks = await DbContext.Marks
                .Where(mark => mark.UserId == userId && knowledgeId.Contains(mark.KnowledgeId))
                .ToListAsync();

            foreach (Mark mark in listMarks)
            {
                result[mark.KnowledgeId] = true;
            }

            return result;
        }
    }
}
