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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace KnowledgeSharingApi.Infrastructures.Repositories.MySqlRepositories
{
    public class QuestionMySqlRepository(IDbContext dbContext)
        : BaseMySqlRepository<Question>(dbContext), IQuestionRepository
    {
        public async Task<ViewQuestion> CheckExistedQuestion(Guid questionId, string errorMessage)
        {
            return await DbContext.ViewQuestions
                .Where(question => question.QuestionId == questionId)
                .FirstOrDefaultAsync()
                ?? throw new NotExistedEntityException(errorMessage);
        }

        public override async Task<int> Delete(Guid questionId)
        {
            if (!DbContext.UserItems.Any(item => item.UserItemId == questionId))
                return 0;

            using IDbContextTransaction transaction = await DbContext.BeginTransaction();
            try
            {
                DbContext.KnowledgeCategories.RemoveRange(DbContext.KnowledgeCategories.Where(item => item.KnowledgeId == questionId));
                DbContext.Comments.RemoveRange(DbContext.Comments.Where(item => item.KnowledgeId == questionId));
                DbContext.Marks.RemoveRange(DbContext.Marks.Where(item => item.KnowledgeId == questionId));
                DbContext.Stars.RemoveRange(DbContext.Stars.Where(item => item.UserItemId == questionId));

                DbContext.Questions.RemoveRange(DbContext.Questions.Where(item => item.UserItemId == questionId));
                //DbContext.Posts.RemoveRange(DbContext.Posts.Where(item => item.UserItemId == questionId));
                //DbContext.Knowledges.RemoveRange(DbContext.Knowledges.Where(item => item.UserItemId == questionId));
                //DbContext.UserItems.RemoveRange(DbContext.UserItems.Where(item => item.UserItemId == questionId));

                int rows = await DbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return rows;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                await transaction.RollbackAsync();
                return 0;
            }
        }

        public override async Task<Guid?> Insert(Guid questionId, Question question)
        {
            question.UserItemId = question.KnowledgeId = question.PostId = question.QuestionId = questionId;
            DbContext.Questions.Add(question);
            int rows = await DbContext.SaveChangesAsync();
            return rows > 0 ? questionId : null;
        }

        public override async Task<int> Update(Guid questionId, Question updatedQuestion)
        {
            Question questionToUpdate = await DbContext.Questions.FindAsync(questionId) 
                ?? throw new Exception("Question not found.");

            updatedQuestion.UserItemId = updatedQuestion.KnowledgeId 
                = updatedQuestion.PostId = updatedQuestion.QuestionId = questionId;

            DbContext.Entry(questionToUpdate).CurrentValues.SetValues(updatedQuestion);

            return await DbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ViewQuestion>> GetByUserId(Guid userId)
        {
            List<ViewQuestion> posts = await
                DbContext.ViewQuestions
                .Where(post => post.UserId == userId)
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

        public async Task<IEnumerable<ViewQuestion>> GetPublicPostsByUserId(Guid userId)
        {
            List<ViewQuestion> posts = await
                DbContext.ViewQuestions
                .Where(post => post.Privacy == EPrivacy.Public && post.UserId == userId)
                .OrderByDescending(post => post.CreatedTime)
                .ToListAsync();
            return posts;
        }

        public async Task<IEnumerable<ViewQuestion>> GetQuestionInCourse(Guid courseid)
        {
            List<ViewQuestion> posts = await
                DbContext.ViewQuestions
                .Where(post => post.CourseId != null && post.CourseId == courseid)
                .OrderByDescending(post => post.CreatedTime)
                .ToListAsync();
            return posts;
        }


        public async Task<ViewQuestion?> GetQuestionDetail(Guid questionId)
        {
            return await DbContext.ViewQuestions
                .Where(ques => ques.QuestionId == questionId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ViewQuestion>> GetViewPost(int limit, int offset)
        {
            return await DbContext.ViewQuestions
                .OrderByDescending(question => question.CreatedTime)
                .Skip(offset).Take(limit)
                .ToListAsync();
        }

        public async Task<IEnumerable<ViewQuestion>> GetPublicPostsOfCategory(string catName, int limit, int offset)
        {
            var knowledgesId = DbContext.ViewKnowledgeCategories
                .Where(k => k.CategoryName == catName)
                .Select(k => k.KnowledgeId)
                .Distinct();
            var posts = await DbContext.ViewQuestions
                .Where(post => post.Privacy == EPrivacy.Public)
                .Where(post => knowledgesId.Contains(post.KnowledgeId)) // Lọc post dựa vào danh sách ID đã lấy
                .OrderByDescending(post => post.CreatedTime)
                .Skip(offset).Take(limit)
                .ToListAsync();
            return posts;
        }

        public async Task<IEnumerable<ViewQuestion>> GetPostsOfCategory(string catName, int limit, int offset)
        {
            var knowledgesId = DbContext.ViewKnowledgeCategories
                .Where(k => k.CategoryName == catName)
                .Select(k => k.KnowledgeId)
                .Distinct();

            var posts = await DbContext.ViewQuestions
                .Where(post => knowledgesId.Contains(post.KnowledgeId)) // Lọc post dựa vào danh sách ID đã lấy
                .OrderByDescending(post => post.CreatedTime)
                .Skip(offset).Take(limit)
                .ToListAsync();

            return posts;
        }

        public async Task<IEnumerable<ViewQuestion>> GetPostsOfCategory(Guid myUId, string catName, int limit, int offset)
        {
            var knowledgesId = DbContext.ViewKnowledgeCategories
                .Where(k => k.CategoryName == catName)
                .Select(k => k.KnowledgeId)
                .Distinct();

            // Lấy danh sách CourseId mà User đã đăng ký
            var registeredCourseIds = DbContext.CourseRegisters
                .Where(c => c.UserId == myUId)
                .Select(c => c.CourseId)
                .Distinct()
                .ToList(); // Dùng ToList để tải kết quả về và tránh query lại nhiều lần

            var posts = await DbContext.ViewQuestions
                .Where(post => knowledgesId.Contains(post.KnowledgeId) && // Lọc post dựa vào danh sách ID đã lấy
                    (post.CourseId == null || registeredCourseIds.Contains(post.CourseId.Value)) &&
                    (post.CourseId != null || post.Privacy == EPrivacy.Public)
                )
                .OrderByDescending(post => post.CreatedTime)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();

            return posts;
        }

    }
}
