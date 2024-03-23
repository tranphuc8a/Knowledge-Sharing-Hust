using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
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
    public class KnowledgeMySqlRepository(IDbContext dbContext)
        : BaseMySqlRepository<Knowledge>(dbContext), IKnowledgeRepository
    {
        public async Task<double?> GetAverageStar(string knowledgeId)
        {
            double? star = await
                DbContext.Stars
                .Where(star => star.UserItemId.ToString() == knowledgeId)
                .Select(star => star.Stars)
                .AverageAsync();
            return star;
        }

        public async Task<PaginationResponseModel<ViewComment>> GetListComments(string knowledgeId, int limit, int offset)
        {
            List<ViewComment> listComment = await
                DbContext.ViewComments
                .Where(comment => comment.KnowledgeId.ToString() == knowledgeId)
                .OrderByDescending(comment => comment.CreatedTime)
                .ToListAsync();
            PaginationResponseModel<ViewComment> res = new()
            {
                Total = listComment.Count,
                Limit = limit,
                Offset = offset,
                Results = listComment
            };
            return res;
        }
    }
}
