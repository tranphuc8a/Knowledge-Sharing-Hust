﻿using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Repositories.Repositories.BaseRepositories;
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
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.KnowledgeRepositories;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;

namespace KnowledgeSharingApi.Repositories.Repositories.MySqlRepositories.MySqlKnowledgeRepositories
{
    public class CommentMySqlRepository(IDbContext dbContext)
        : BaseMySqlUserItemRepository<Comment>(dbContext), ICommentRepository
    {
        public virtual async Task<ViewComment> CheckExistedComment(Guid commentId, string errorMessage)
        {
            return (ViewComment)((await DbContext.ViewComments.Where(com => com.UserItemId == commentId)
                .FirstOrDefaultAsync())?.Clone()
                ?? throw new NotExistedEntityException(errorMessage));
        }

        // Override lại hàm xóa comment
        public override async Task<int> Delete(Guid commentId)
        {
            using IDbContextTransaction transaction = await DbContext.BeginTransaction();
            try
            {
                // Chuyển tất cả reply comment về reply null
                DateTime now = DateTime.UtcNow;
                string adminName = "PhucTV";

                var commentsToUpdate = DbContext.Comments.Where(c => c.ReplyId == commentId);
                foreach (var comment in commentsToUpdate)
                {
                    comment.ReplyId = null;
                    comment.ModifiedTime = now;
                    comment.ModifiedBy = adminName;
                }
                int rows1 = await DbContext.SaveChangesAsync();

                // Xóa comment
                var commentToDelete = await DbContext.Comments.FindAsync(commentId);
                if (commentToDelete != null)
                {
                    DbContext.Comments.Remove(commentToDelete);
                }

                int affectedRows = rows1 + await DbContext.SaveChangesAsync();

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

        public virtual async Task<PaginationResponseModel<ViewComment>> GetCommentsOfUserInKnowledge(Guid userId, Guid knowledgeId, PaginationDto pagination)
        {
            IQueryable<ViewComment> comments = DbContext.ViewComments
                .Where(com => com.UserId == userId && com.KnowledgeId == knowledgeId);
            int total = comments.Count();
            return new PaginationResponseModel<ViewComment>()
            {
                Total = total,
                Limit = pagination.Limit,
                Offset = pagination.Offset,
                Results = await ApplyPagination(comments, pagination).ToListAsync()
            };
        }

        public virtual async Task<PaginationResponseModel<ViewComment>> GetListCommentsOfKnowledge(Guid knowledgeId, PaginationDto pagination)
        {
            IQueryable<ViewComment> listComments = DbContext.ViewComments
                .Where(comment => comment.KnowledgeId == knowledgeId)
                .OrderBy(comment => comment.CreatedBy);
            int total = listComments.Count();
            return new PaginationResponseModel<ViewComment>(total, pagination.Limit, pagination.Offset, 
                await ApplyPagination(listComments, pagination).ToListAsync());
        }

        public virtual async Task<Dictionary<Guid, PaginationResponseModel<ViewComment>?>> GetListCommentsOfKnowledge(List<Guid> knowledgeIds, PaginationDto pagination)
        {
            var comments = DbContext.ViewComments
                .Where(comment => knowledgeIds.Any(id => id == comment.KnowledgeId) && comment.ReplyId == null);

            var groupedComments = await comments
                .GroupBy(comment => comment.KnowledgeId)
                .ToDictionaryAsync(group => group.Key, group => new
                {
                    total = group.Count(),
                    list = ApplyPagination(group.AsQueryable(), pagination).ToList()
                });

            var result = knowledgeIds.Distinct().ToDictionary(
                id => id,
                id =>
                {
                    if (groupedComments.TryGetValue(id, out var value))
                    {
                        return new PaginationResponseModel<ViewComment>(
                            value.total,
                            pagination.Limit,
                            pagination.Offset,
                            value.list
                        );
                    }
                    return null;
                }
            );

            return result;
        }

        public virtual async Task<List<ViewComment>> GetListCommentsOfKnowledge(Guid knowledgeId)
        {
            return await DbContext.ViewComments.Where(com => com.KnowledgeId == knowledgeId)
                .OrderByDescending(comment => comment.CreatedTime).ToListAsync();
        }

        public virtual async Task<List<ViewComment>> GetListCommentsOfUser(Guid userId)
        {
            return await DbContext.ViewComments.Where(com => com.UserId == userId)
                .OrderByDescending(comment => comment.CreatedTime).ToListAsync();
        }

        public virtual async Task<List<ViewComment>> GetListCommentsOfUserInKnowledge(Guid userId, Guid knowledgeId)
        {
            return await DbContext.ViewComments.Where(com => com.UserId == userId && com.KnowledgeId == knowledgeId)
                .OrderByDescending(comment => comment.CreatedTime).ToListAsync();
        }

        public virtual async Task<PaginationResponseModel<ViewComment>> GetRepliesOfComment(Guid commentId, PaginationDto pagination)
        {
            IQueryable<ViewComment> comments = DbContext.ViewComments
                .Where(com => com.ReplyId == commentId);
            int total = comments.Count();
            return new PaginationResponseModel<ViewComment>()
            {
                Total = total,
                Limit = pagination.Limit,
                Offset = pagination.Offset,
                Results = await ApplyPagination(comments, pagination).ToListAsync()
            };
        }

        public virtual async Task<Dictionary<Guid, PaginationResponseModel<ViewComment>?>> GetRepliesOfComment(List<Guid> commentIds, PaginationDto pagination)
        {
            var comments = DbContext.ViewComments
                .Where(comment => comment.ReplyId != null && commentIds.Any(id => id == comment.ReplyId));

            var groupedComments = await comments
                .GroupBy(comment => comment.ReplyId!.Value)
                .ToDictionaryAsync(group => group.Key, group => new
                {
                    total = group.Count(),
                    list = ApplyPagination(group.AsQueryable(), pagination).ToList()
                });

            var result = commentIds.Distinct().ToDictionary(
                id => id,
                id =>
                {
                    if (groupedComments.TryGetValue(id, out var value))
                    {
                        return new PaginationResponseModel<ViewComment>()
                        {
                            Total = value.total,
                            Offset = pagination.Offset,
                            Limit = pagination.Limit,
                            Results = value.list
                        };
                    }
                    return null;
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
