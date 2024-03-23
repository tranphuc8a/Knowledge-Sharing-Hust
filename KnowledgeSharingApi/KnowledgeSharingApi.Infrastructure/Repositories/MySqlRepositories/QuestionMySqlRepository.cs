using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Repositories.MySqlRepositories
{
    public class QuestionMySqlRepository(IDbContext dbContext)
        : BaseMySqlRepository<Question>(dbContext), IQuestionRepository
    {
        public async Task<ViewQuestion> CheckExistedQuestion(string questionId, string errorMessage)
        {
            return await DbContext.ViewQuestions
                .Where(question => question.QuestionId.ToString() == questionId)
                .FirstOrDefaultAsync()
                ?? throw new NotExistedEntityException(errorMessage);
        }

        public override async Task<int> Delete(string questionId)
        {
            Guid qId = new(questionId);
            if (!DbContext.UserItems.Any(item => item.UserItemId == qId))
            {
                return 0;
            }

            using IDbContextTransaction transaction = await DbContext.BeginTransaction();
            try
            {
                DbContext.KnowledgeCategories.RemoveRange(DbContext.KnowledgeCategories.Where(item => item.KnowledgeId == qId));
                DbContext.Comments.RemoveRange(DbContext.Comments.Where(item => item.KnowledgeId == qId));
                DbContext.Marks.RemoveRange(DbContext.Marks.Where(item => item.KnowledgeId == qId));
                DbContext.Stars.RemoveRange(DbContext.Stars.Where(item => item.UserItemId == qId));

                DbContext.Questions.RemoveRange(DbContext.Questions.Where(item => item.UserItemId == qId));
                DbContext.Posts.RemoveRange(DbContext.Posts.Where(item => item.UserItemId == qId));
                DbContext.Knowledges.RemoveRange(DbContext.Knowledges.Where(item => item.UserItemId == qId));
                DbContext.UserItems.RemoveRange(DbContext.UserItems.Where(item => item.UserItemId == qId));

                await DbContext.SaveChangeAsync();
                await transaction.CommitAsync();
                return 1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                await transaction.RollbackAsync();
                return 0;
            }
        }

        public async Task<IEnumerable<ViewQuestion>> GetByUserId(string userId)
        {
            List<ViewQuestion> posts = await
                DbContext.ViewQuestions
                .Where(post => post.UserId.ToString() == userId)
                .OrderByDescending(post => post.CreatedTime)
                .ToListAsync();
            return posts;
        }

        public async Task<IEnumerable<ViewQuestion>> GetPublicPosts(int limit, int offset)
        {
            List<ViewQuestion> posts = await
                DbContext.ViewQuestions
                .Where(post => post.Privacy == EPrivacy.Public)
                .OrderByDescending(post => post.CreatedTime)
                .Skip(offset).Take(limit)
                .ToListAsync();
            return posts;
        }

        public async Task<IEnumerable<ViewQuestion>> GetPublicPostsByUserId(string userId)
        {
            List<ViewQuestion> posts = await
                DbContext.ViewQuestions
                .Where(post => post.Privacy == EPrivacy.Public && post.UserId.ToString() == userId)
                .OrderByDescending(post => post.CreatedTime)
                .ToListAsync();
            return posts;
        }

        public async Task<IEnumerable<ViewQuestion>> GetQuestionInCourse(string courseid)
        {
            List<ViewQuestion> posts = await
                DbContext.ViewQuestions
                .Where(post => post.CourseId != null && post.CourseId.ToString() == courseid)
                .OrderByDescending(post => post.CreatedTime)
                .ToListAsync();
            return posts;
        }
    }
}
