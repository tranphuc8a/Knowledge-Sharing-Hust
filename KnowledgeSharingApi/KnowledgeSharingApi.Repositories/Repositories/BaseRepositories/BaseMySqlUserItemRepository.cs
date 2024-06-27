using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces;
using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using Microsoft.EntityFrameworkCore;
using System.Data;
using KnowledgeSharingApi.Repositories.Interfaces.Repositories;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;

namespace KnowledgeSharingApi.Repositories.Repositories.BaseRepositories
{
    public abstract class BaseMySqlUserItemRepository<T> : BaseMySqlRepository<T>, IBaseUserItemRepository where T : UserItem
    {
        protected BaseMySqlUserItemRepository(IDbContext dbContext) : base(dbContext)
        {
            TableNameId = "UserItemId";
            //_ = DbContext.Users.FirstOrDefault();
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
            var list = GetDbSet().OrderByDescending(item => item.CreatedTime);
            int total = list.Count();
            PaginationResponseModel<T> res = new()
            {
                Total = total,
                Limit = pagination.Limit,
                Offset = pagination.Offset,
                Results = await ApplyPagination(list, pagination).ToListAsync()
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
            value.CreatedTime = DateTime.UtcNow;
            GetDbSet().Add(value);
            int res = await DbContext.SaveChangesAsync();
            return res > 0 ? id : null;
        }

        public override async Task<Guid?> Insert(T value)
        {
            Guid id = Guid.NewGuid();
            value.CreatedTime = DateTime.UtcNow;
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
            entity.ModifiedTime = DateTime.UtcNow;
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
            // Not:
            Guid? id = await DbContext.UserItems.Where(it => it.UserItemId == userItemId).Select(u => u.UserItemId).FirstOrDefaultAsync();
            if (id == null) return null;

            // Comments
            ViewComment? comment = await DbContext.ViewComments.FindAsync(userItemId);
            if (comment != null)
                return (ResponseCommentModel)new ResponseCommentModel().Copy(comment);

            // Course
            ViewCourse? course = await DbContext.ViewCourses.FindAsync(userItemId);
            if (course != null)
                return (ResponseCourseModel)new ResponseCourseModel().Copy(course);

            // Question:
            ViewQuestion? question = await DbContext.ViewQuestions.FindAsync(userItemId);
            if (question != null)
                return (ResponseQuestionModel)new ResponseQuestionModel().Copy(question);

            // Leson: 
            ViewLesson? lesson = await DbContext.ViewLessons.FindAsync(userItemId);
            if (lesson != null)
                return (ResponseLessonModel)new ResponseLessonModel().Copy(lesson);
            
            return null;
        }

        public virtual async Task<Dictionary<Guid, IResponseUserItemModel?>> GetExactlyResponseUserItemModel(List<Guid> userItemId)
        {
            Dictionary<Guid, IResponseUserItemModel?> res = userItemId
                .Distinct().ToDictionary(id => id, id => (IResponseUserItemModel?)null);

            // Get List commentIds, courseIds, lessonIds and questionsIds:
            userItemId = await DbContext.UserItems.Where(it => userItemId.Contains(it.UserItemId)).Select(u => u.UserItemId).ToListAsync();
            var vComments = await DbContext.ViewComments.Where(com => userItemId.Contains(com.UserItemId)).ToListAsync();
            var knowledgeIds = userItemId.Except(vComments.Select(c => c.UserItemId));
            var vCourses = await DbContext.ViewCourses.Where(course => knowledgeIds.Contains(course.UserItemId)).ToListAsync();
            var postIds = knowledgeIds.Except(vCourses.Select(c => c.UserItemId));
            var vLessons = await DbContext.ViewLessons.Where(lesson => postIds.Contains(lesson.UserItemId)).ToListAsync();
            var questionIds = postIds.Except(vLessons.Select(l => l.UserItemId));
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

        public virtual async Task<IViewUserItem?> GetExactlyUserItem(Guid userItemId)
        {
            // Not:
            Guid? id = await DbContext.UserItems.Where(it => it.UserItemId == userItemId).Select(u => u.UserItemId).FirstOrDefaultAsync();
            if (id == null) return null;

            // Comments
            ViewComment? comment = await DbContext.ViewComments.FindAsync(userItemId);
            if (comment != null) return comment;

            // Course
            ViewCourse? course = await DbContext.ViewCourses.FindAsync(userItemId);
            if (course != null) return course;

            // Question:
            ViewQuestion? question = await DbContext.ViewQuestions.FindAsync(userItemId);
            if (question != null) return question;

            // Leson: 
            ViewLesson? lesson = await DbContext.ViewLessons.FindAsync(userItemId);
            if (lesson != null) return lesson;

            return null;
        }

        public virtual async Task<Dictionary<Guid, IViewUserItem?>> GetExactlyUserItem(List<Guid> userItemId)
        {
            Dictionary<Guid, IViewUserItem?> res = userItemId
                .Distinct().ToDictionary(id => id, id => (IViewUserItem?)null);

            // Get List commentIds, courseIds, lessonIds and questionsIds:
            userItemId = await DbContext.UserItems.Where(it => userItemId.Contains(it.UserItemId)).Select(u => u.UserItemId).ToListAsync();
            var vComments = await DbContext.ViewComments.Where(com => userItemId.Contains(com.UserItemId)).ToListAsync();
            var knowledgeIds = userItemId.Except(vComments.Select(c => c.UserItemId));
            var vCourses = await DbContext.ViewCourses.Where(course => knowledgeIds.Contains(course.UserItemId)).ToListAsync();
            var postIds = knowledgeIds.Except(vCourses.Select(c => c.UserItemId));
            var vLessons = await DbContext.ViewLessons.Where(lesson => postIds.Contains(lesson.UserItemId)).ToListAsync();
            var questionIds = postIds.Except(vLessons.Select(l => l.UserItemId));
            var vQuestions = await DbContext.ViewQuestions.Where(question => questionIds.Contains(question.UserItemId)).ToListAsync();

            // Return result:
            foreach (ViewComment vCom in vComments) res[vCom.UserItemId] = vCom;
            foreach (ViewCourse vCourse in vCourses) res[vCourse.UserItemId] = vCourse;
            foreach (ViewLesson vLes in vLessons) res[vLes.UserItemId] = vLes;
            foreach (ViewQuestion vQues in vQuestions) res[vQues.UserItemId] = vQues;
            return res;
        }

        #endregion
    }
}
