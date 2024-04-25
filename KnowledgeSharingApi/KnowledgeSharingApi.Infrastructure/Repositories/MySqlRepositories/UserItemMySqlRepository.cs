using Dapper;
using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces;
using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Repositories.MySqlRepositories
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

        public async Task<IResponseUserItemModel?> GetExactlyResponseUserItemModel(Guid userItemId)
        {
            UserItem? res = await DbContext.UserItems.FindAsync(userItemId);
            if (res == null) return null;

            // Comments
            if (res.UserItemType == EUserItemType.Comment)
            {
                ViewComment? comment = await DbContext.ViewComments
                    .Where(com => com.UserItemId == userItemId).FirstOrDefaultAsync();
                return comment != null ? (ResponseCommentModel) new ResponseCommentModel().Copy(comment) : null;
            }

            // Knowledges:
            Knowledge? knowledge = await DbContext.Knowledges.FindAsync(userItemId);
            if (knowledge == null) return null;

            // Member
            if (knowledge.KnowledgeType == EKnowledgeType.Course)
            {
                ViewCourse? course = await DbContext.ViewCourses
                    .Where(c => c.UserItemId == userItemId).FirstOrDefaultAsync();
                return course != null ? (ResponseCourseModel)new ResponseCourseModel().Copy(course) : null;
            }

            // Post
            Post? p = await DbContext.Posts.FindAsync(userItemId);
            if (p == null) return null;

            if (p.PostType == EPostType.Question)
            {
                // Question:
                ViewQuestion? question = await DbContext.ViewQuestions.FindAsync(userItemId);
                return question != null ? (ResponseQuestionModel)new ResponseQuestionModel().Copy(question) : null;
            }
            // Leson: 
            ViewLesson? lesson = await DbContext.ViewLessons.FindAsync(userItemId);
            return lesson != null ? (ResponseLessonModel)new ResponseLessonModel().Copy(lesson) : null;
        }

        public async Task<IUserItemView?> GetExactlyUserItem(Guid userItemId)
        {
            UserItem? res = await DbContext.UserItems.FindAsync(userItemId);
            if (res == null) return null;
            
            // Comments
            if (res.UserItemType == EUserItemType.Comment)
            {
                return await DbContext.ViewComments
                    .Where(com => com.UserItemId == userItemId).FirstOrDefaultAsync();
            }

            // Knowledges:
            Knowledge? knowledge = await DbContext.Knowledges.FindAsync(userItemId);
            if (knowledge == null) return null;
            
            // Member
            if (knowledge.KnowledgeType == EKnowledgeType.Course)
            {
                return await DbContext.ViewCourses
                    .Where(c => c.UserItemId == userItemId).FirstOrDefaultAsync();
            }

            // Post
            Post? p = await DbContext.Posts.FindAsync(userItemId);
            if (p == null) return null;

            if (p.PostType == EPostType.Question)
            {
                // Question:
                return await DbContext.ViewQuestions.FindAsync(userItemId);
            }
            // Leson: 
            return await DbContext.ViewLessons.FindAsync(userItemId);
        }

        protected override DbSet<UserItem> GetDbSet()
        {
            return DbContext.UserItems;
        }
    }
}
