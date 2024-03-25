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

        public virtual async Task<double?> GetAverageStar(Guid knowledgeId)
        {
            var hasRatings = await DbContext.Stars.AnyAsync(star => star.UserItemId == knowledgeId);

            if (!hasRatings)
                return null; 

            double? averageStar = await
                DbContext.Stars
                .Where(star => star.UserItemId == knowledgeId)
                .Select(star => (double?)star.Stars)
                .AverageAsync();

            return averageStar;
        }


        public virtual async Task<PaginationResponseModel<ViewComment>> GetListComments(Guid knowledgeId, int limit, int offset)
        {
            List<ViewComment> listComment = await
                DbContext.ViewComments
                .Where(comment => comment.KnowledgeId == knowledgeId)
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

        public virtual async Task<PaginationResponseModel<ViewUser>> GetListUserMaredKnowledge(Guid knowledgeId, int limit, int offset)
        {
            var listUserIds = DbContext.Marks.Where(mark => mark.KnowledgeId == knowledgeId)
                .Select(mark => mark.UserId).Distinct();
            int total = listUserIds.Count();
            List<ViewUser> listUser = await DbContext.ViewUsers
                .Where(user => listUserIds.Contains(user.UserId))
                .Skip(offset).Take(limit)
                .ToListAsync();
            return new PaginationResponseModel<ViewUser>
            {
                Total = total,
                Limit = limit,
                Offset = offset,
                Results = listUser
            };
        }

        public virtual async Task<Mark?> GetMark(Guid userId, Guid knowledgeId)
        {
            return await DbContext.Marks
                .Where(mark => mark.UserId == userId && mark.KnowledgeId == knowledgeId)
                .FirstOrDefaultAsync();
        }
    }
}
