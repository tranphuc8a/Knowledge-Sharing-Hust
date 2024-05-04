using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Repositories.BaseRepositories;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using MimeKit.Encodings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Exceptions;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace KnowledgeSharingApi.Infrastructures.Repositories.MySqlRepositories
{
    public class CommentMySqlRepository(IDbContext dbContext)
        : BaseMySqlUserItemRepository<Comment>(dbContext), ICommentRepository
    {
        public virtual async Task<ViewComment> CheckExistedComment(Guid commentId, string errorMessage)
        {
            return await DbContext.ViewComments.Where(com => com.UserItemId == commentId)
                .FirstOrDefaultAsync()
                ?? throw new NotExistedEntityException(errorMessage);
        }

        // Override lại hàm xóa comment
        public override async Task<int> Delete(Guid commentId)
        {
            using IDbContextTransaction transaction = await DbContext.Database.BeginTransactionAsync();
            try
            {
                // Chuyển tất cả reply comment về reply null
                string commandText = "UPDATE Comments SET ReplyId = NULL WHERE ReplyId = @p0";
                SqlParameter[] parameters = [new SqlParameter("@p0", commentId)];
                int affectedRows = await DbContext.Database.ExecuteSqlRawAsync(commandText, parameters);

                // Xóa comment
                var commentToDelete = await DbContext.Comments.FindAsync(commentId);
                if (commentToDelete != null)
                {
                    DbContext.Comments.Remove(commentToDelete);
                    affectedRows += await DbContext.SaveChangesAsync();
                }

                // Commit and return affected rows
                await transaction.CommitAsync();
                return affectedRows;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public virtual async Task<PaginationResponseModel<ViewComment>> GetCommentsOfUserInKnowledge(Guid userId, Guid knowledgeId, int limit, int offset)
        {
            IQueryable<ViewComment> comments = DbContext.ViewComments
                .Where(com => com.UserItemId == userId && com.KnowledgeId == knowledgeId);
            int total = comments.Count();
            return new PaginationResponseModel<ViewComment>()
            {
                Total = total,
                Limit = limit,
                Offset = offset,
                Results = await comments.Skip(offset).Take(limit).ToListAsync()
            };
        }

        public virtual async Task<PaginationResponseModel<ViewComment>> GetListCommentsOfKnowledge(Guid knowledgeId, int limit, int offset)
        {
            IQueryable<ViewComment> listComments = DbContext.ViewComments
                .Where(comment => comment.KnowledgeId == knowledgeId)
                .OrderBy(comment => comment.CreatedBy);
            int total = listComments.Count();
            IEnumerable<ViewComment> lsComments = await listComments
                .Skip(offset).Take(limit)
                .ToListAsync();
            return new PaginationResponseModel<ViewComment>(total, limit, offset, lsComments);
        }

        public virtual async Task<Dictionary<Guid, PaginationResponseModel<ViewComment>?>> GetListCommentsOfKnowledge(IEnumerable<Guid> knowledgeIds, int limit)
        {
            var comments = await DbContext.ViewComments
                .Where(comment => knowledgeIds.Any(id => id == comment.KnowledgeId) && comment.ReplyId == null)
                .OrderBy(comment => comment.CreatedBy)
                .ToListAsync();

            var groupedComments = comments
                .GroupBy(comment => comment.KnowledgeId)
                .ToDictionary(group => group.Key, group => group);

            var mapping = groupedComments.ToDictionary(
                kvp => kvp.Key,
                kvp => new PaginationResponseModel<ViewComment>(
                    kvp.Value.Count(),
                    limit,
                    0,
                    kvp.Value.Take(limit)
                )
            );

            var result = knowledgeIds.ToDictionary(
                id => id,
                id =>
                {
                    if (mapping.TryGetValue(id, out PaginationResponseModel<ViewComment>? value))
                    {
                        return value;
                    }
                    else
                    {
                        return null;
                    }
                }
            );

            return result;
        }

        public virtual async Task<PaginationResponseModel<ViewComment>> GetRepliesOfComment(Guid commentId, int limit, int offset)
        {
            IQueryable<ViewComment> comments = DbContext.ViewComments
                .Where(com => com.UserItemId == commentId);
            int total = comments.Count();
            return new PaginationResponseModel<ViewComment>()
            {
                Total = total,
                Limit = limit,
                Offset = offset,
                Results = await comments.Skip(offset).Take(limit).ToListAsync()
            };
        }

        public virtual async Task<Dictionary<Guid, PaginationResponseModel<ViewComment>?>> GetRepliesOfComment(IEnumerable<Guid> commentIds, int limit)
        {
            var comments = await DbContext.ViewComments
                .Where(comment => comment.ReplyId != null && commentIds.Any(id => id == comment.ReplyId))
                .OrderBy(comment => comment.CreatedBy)
                .ToListAsync();

            var groupedComments = comments
                .GroupBy(comment => comment.ReplyId!.Value)
                .ToDictionary(group => group.Key, group => group);

            var mapping = groupedComments.ToDictionary(
                kvp => kvp.Key,
                kvp => new PaginationResponseModel<ViewComment>(
                    kvp.Value.Count(),
                    limit,
                    0,
                    kvp.Value.Take(limit)
                )
            );

            var result = commentIds.ToDictionary(
                id => id,
                id =>
                {
                    if (mapping.TryGetValue(id, out PaginationResponseModel<ViewComment>? value))
                    {
                        return value;
                    }
                    else
                    {
                        return null;
                    }
                }
            );

            return result;
        }

        public virtual async Task<Dictionary<Guid, int>> GetTotalReplies(List<Guid> commentsId)
        {

            Dictionary<Guid, int> result = 
                await DbContext.ViewComments
                .Where(c => c.ReplyId != null && commentsId.Contains(c.ReplyId.Value))
                .GroupBy(c => c.ReplyId!.Value)
                .Select(g => new { CommentId = g.Key, TotalReplies = g.Count() })
                .ToDictionaryAsync(g => g.CommentId, g => g.TotalReplies);

            foreach (var commentId in commentsId)
            {
                if (!result.ContainsKey(commentId))
                    result[commentId] = 0;
            }
            return result;
        }

        protected override DbSet<Comment> GetDbSet()
        {
            return DbContext.Comments;
        }
    }
}
