using Dapper;
using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces;
using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Entities;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories;
using KnowledgeSharingApi.Domains.Models.Dtos;

namespace KnowledgeSharingApi.Infrastructures.Repositories.BaseRepositories
{
    public abstract class BaseMySqlUserItemRepository<T> : BaseMySqlRepository<T>, IBaseUserItemRepository where T : UserItem
    {
        protected BaseMySqlUserItemRepository(IDbContext dbContext) : base(dbContext)
        {
            TableNameId = "UserItemId";
            _ = DbContext.Users.FirstOrDefault();
        }

        /// <summary>
        /// Lấy về Db set của riêng entity T
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        protected abstract DbSet<T> GetDbSet();


        #region Get 
        public override async Task<T?> Get(Guid id)
        {
            return (T?) (await GetDbSet().FindAsync(id))?.Clone();
        }

        public override async Task<List<T>> Get()
        {
            return await GetDbSet().OrderByDescending(item => item.CreatedTime).ToListAsync();
        }

        public override async Task<PaginationResponseModel<T>> Get(PaginationDto pagination)
        {
            List<T> list = await GetDbSet().OrderByDescending(item => item.CreatedTime).ToListAsync();
            int total = list.Count;
            PaginationResponseModel<T> res = new()
            {
                Total = total,
                Limit = pagination.Limit,
                Offset = pagination.Offset,
                Results = ApplyPagination(list, pagination)
            };
            return res;
        }

        public override async Task<List<T?>> Get(Guid[] ids)
        {
            List<T> lists = await GetDbSet()
                .Where(item => ids.Contains(item.UserItemId)).ToListAsync();
            return ids.Select(id => lists.Where(it => it.UserItemId == id).FirstOrDefault()).ToList();
        }

        #endregion


        #region Insert New Record

        public override async Task<Guid?> Insert(Guid id, T value)
        {
            value.UserItemId = id;
            GetDbSet().Add(value);
            int res = await DbContext.SaveChangesAsync();
            return res > 0 ? id : null;
        }

        public override async Task<Guid?> Insert(T value)
        {
            Guid id = Guid.NewGuid();
            value.UserItemId = id;
            GetDbSet().Add(value);
            int res = await DbContext.SaveChangesAsync();
            return res > 0 ? id : null;
        }

        #endregion


        #region Delete A or Multi Record

        public override async Task<int> Delete(Guid id)
        {
            T? item = await GetDbSet().FindAsync(id);
            if (item != null)
                GetDbSet().Remove(item);
            return await DbContext.SaveChangesAsync();
        }
        public override async Task<int> Delete(Guid[] ids)
        {
            List<T> list = await GetDbSet()
                .Where(item => ids.Contains(item.UserId)).ToListAsync();
            GetDbSet().RemoveRange(list);
            return await DbContext.SaveChangesAsync();
        }
        #endregion


        #region Update A Record
        public override async Task<int> Update(Guid id, T entity)
        {
            T? item = await GetDbSet().FindAsync(id);
            if (item == null) return 0;
            entity.UserItemId = id;
            item.Copy(entity);
            return await DbContext.SaveChangesAsync();
        }
        #endregion


        #region Get exactly:

        public virtual void DevideResponseUserItemModel(List<IResponseUserItemModel> list, List<ResponseCommentModel> comments, List<ResponseCourseModel> courses, List<ResponseLessonModel> lessons, List<ResponseQuestionModel> questions)
        {
            comments = [];
            courses = [];
            lessons = [];
            questions = [];
            foreach (IResponseUserItemModel uit in list)
            {
                if (uit is ResponseCommentModel comment)
                {
                    comments.Add(comment);
                }
                else if (uit is ResponseCourseModel course)
                {
                    courses.Add(course);
                }
                else if (uit is ResponseLessonModel lesson)
                {
                    lessons.Add(lesson);
                }
                else if (uit is ResponseQuestionModel question)
                {
                    questions.Add(question);
                }
            }
        }

        public virtual void DevideUserItem(List<UserItem> list, out List<Comment> comments, out List<Course> courses, out List<Lesson> lessons, out List<Question> questions)
        {
            comments = [];
            courses = [];
            lessons = [];
            questions = [];
            foreach (IResponseUserItemModel uit in list)
            {
                if (uit is Comment comment)
                {
                    comments.Add(comment);
                }
                else if (uit is Course course)
                {
                    courses.Add(course);
                }
                else if (uit is Lesson lesson)
                {
                    lessons.Add(lesson);
                }
                else if (uit is Question question)
                {
                    questions.Add(question);
                }
            }
        }


        public virtual async Task<IResponseUserItemModel?> GetExactlyResponseUserItemModel(Guid userItemId)
        {
            UserItem? res = await DbContext.UserItems.FindAsync(userItemId);
            if (res == null) return null;

            // Comments
            if (res.UserItemType == EUserItemType.Comment)
            {
                ViewComment? comment = await DbContext.ViewComments
                    .Where(com => com.UserItemId == userItemId).FirstOrDefaultAsync();
                return comment != null ? (ResponseCommentModel)new ResponseCommentModel().Copy(comment) : null;
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

        public virtual async Task<Dictionary<Guid, IResponseUserItemModel?>> GetExactlyResponseUserItemModel(List<Guid> userIds)
        {
            Dictionary<Guid, IResponseUserItemModel?> res = userIds
                .ToDictionary(id => id, id => (IResponseUserItemModel?)null);

            // Get List commentIds, courseIds, lessonIds and questionsIds:
            var userItems = await DbContext.UserItems.Where(uit => userIds.Contains(uit.UserItemId)).ToListAsync();
            var knowledgeIds = userItems
                .Where(uit => uit.UserItemType == EUserItemType.Knowledge)
                .Select(uit => uit.UserItemId).ToList();
            var commentIds = userItems.Select(u => u.UserItemId).Except(knowledgeIds).ToList();

            var courseIds = await DbContext.Knowledges
                .Where(kn => kn.KnowledgeType == EKnowledgeType.Course && knowledgeIds.Contains(kn.UserItemId))
                .Select(kn => kn.UserItemId)
                .ToListAsync();

            var postIds = knowledgeIds.Except(courseIds).ToList();

            var lessonIds = await DbContext.Posts
                .Where(p => p.PostType == EPostType.Lesson && postIds.Contains(p.UserItemId))
                .Select(p => p.UserItemId)
                .ToListAsync();

            var questionIds = postIds.Except(lessonIds).ToList();

            var vComments = await DbContext.ViewComments.Where(com => commentIds.Contains(com.UserItemId)).ToListAsync();
            var vCourses = await DbContext.ViewCourses.Where(course => courseIds.Contains(course.UserItemId)).ToListAsync();
            var vLessons = await DbContext.ViewLessons.Where(lesson => lessonIds.Contains(lesson.UserItemId)).ToListAsync();
            var vQuestions = await DbContext.ViewQuestions.Where(question => questionIds.Contains(question.UserItemId)).ToListAsync();

            // Return result:
            foreach (ViewComment vCom in vComments)
            {
                ResponseCommentModel resCom = new();
                res[vCom.UserItemId] = (ResponseCommentModel)resCom.Copy(vCom);
            }
            foreach (ViewCourse vCourse in vCourses)
            {
                ResponseCourseModel resCourse = new();
                res[vCourse.UserItemId] = (ResponseCourseModel)resCourse.Copy(vCourse);
            }
            foreach (ViewLesson vLes in vLessons)
            {
                ResponseLessonModel resLes = new();
                res[vLes.UserItemId] = (ResponseLessonModel)resLes.Copy(vLes);
            }
            foreach (ViewQuestion vQues in vQuestions)
            {
                ResponseQuestionModel resQues = new();
                res[vQues.UserItemId] = (ResponseQuestionModel)resQues.Copy(vQues);
            }
            return res;
        }

        public virtual async Task<IUserItemView?> GetExactlyUserItem(Guid userItemId)
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

        #endregion
    }
}
