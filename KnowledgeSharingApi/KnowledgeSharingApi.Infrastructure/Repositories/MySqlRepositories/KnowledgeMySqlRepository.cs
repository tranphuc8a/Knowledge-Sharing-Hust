using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces;
using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Repositories.MySqlRepositories
{
    public class KnowledgeMySqlRepository(IDbContext dbContext)
        : BaseMySqlUserItemRepository<Knowledge>(dbContext), IKnowledgeRepository
    {
        protected readonly Exception notExistedException = new("Không tồn tại đối tượng");

        public virtual async Task<bool> CheckAccessible(Guid userId, Guid knowledgeId)
        {
            Knowledge knowledge = await DbContext.Knowledges.FindAsync(knowledgeId) ?? throw notExistedException;
            // public, owner -> true
            if (knowledge.Privacy == EPrivacy.Public || knowledge.UserId == userId) return true;
            // Private: 
            if (knowledge.KnowledgeType == EKnowledgeType.Course)
            {
                return await CheckCourseAccessible(userId, knowledgeId);
            }
            else
            {
                return await CheckPostAccessible(userId, knowledgeId);
            }
        }

        public virtual async Task<bool> CheckCourseAccessible(Guid userId, Guid courseId)
        {
            Course course = await DbContext.Courses.FindAsync(courseId) ?? throw notExistedException;
            // course public, owner --> true
            if (course.Privacy == EPrivacy.Public || course.UserId == userId) return true;
            // private: join, invite
            ViewCourseRegister? courseRegister = await DbContext.ViewCourseRegisters
                .Where(cr => cr.UserId == userId && cr.CourseId == courseId)
                .FirstOrDefaultAsync();
            if (courseRegister != null) return true;
            // Lấy danh sách course mà myUid được invite vào
            IQueryable<Guid> listInvitedCourseIds = DbContext.CourseRelations
                .Where(invite => invite.ReceiverId == userId && invite.CourseRelationType == ECourseRelationType.Invite)
                .Select(invite => invite.CourseId);
            if (listInvitedCourseIds.Contains(courseId)) return true;
            return false;
        }

        public virtual async Task<bool> CheckLessonAccessible(Guid userId, Guid lessonId)
        {
            Lesson lesson = await DbContext.Lessons.FindAsync(lessonId) ?? throw notExistedException;

            // public, owner : true
            if (lesson.Privacy == EPrivacy.Public || lesson.UserId == userId) return true;

            // Kiểm tra xem người dùng có đăng ký cho bất kỳ khóa học nào có chứa bài học này không.
            bool isRegistered = await DbContext.CourseLessons
                .AnyAsync(cl => cl.LessonId == lessonId
                    && DbContext.ViewCourseRegisters
                        .Any(cr => cr.UserId == userId && cr.CourseId == cl.CourseId)
                );

            return isRegistered;
        }

        public virtual async Task<bool> CheckPostAccessible(Guid userId, Guid postId)
        {
            Post post = await DbContext.Posts.FindAsync(postId) ?? throw notExistedException;
            // public, owner: OK
            if (post.Privacy == EPrivacy.Public || post.UserId == userId) return true;

            // private: switch type
            if (post.PostType == EPostType.Lesson)
            {
                return await CheckLessonAccessible(userId, postId);
            }
            else
            {
                return await CheckQuestionAccessible(userId, postId);
            }
        }

        public virtual async Task<bool> CheckQuestionAccessible(Guid userId, Guid questionId)
        {
            // Kết hợp truy vấn thông tin câu hỏi và kiểm tra quyền riêng tư
            var question = await DbContext.Questions
                .Where(q => q.UserItemId == questionId)
                .FirstOrDefaultAsync() ?? throw notExistedException;

            // public, owner: true.
            if (question.Privacy == EPrivacy.Public || question.UserId == userId) return true;

            // private: same course.
            if (question.CourseId == null) return false;

            // Kiểm tra xem người dùng có đăng ký khóa học chứa câu hỏi này không
            bool isRegistered = await DbContext.ViewCourseRegisters
                .AnyAsync(cr => cr.UserId == userId && cr.CourseId == question.CourseId.Value);

            return isRegistered;
        }

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

        public virtual async Task<PaginationResponseModel<ViewComment>> GetListComments(Guid knowledgeId, PaginationDto pagination)
        {
            List<ViewComment> listComment = await
                DbContext.ViewComments
                .Where(comment => comment.KnowledgeId == knowledgeId &&
                    // comment là comment bậc 1 (không phải comment reply)
                    comment.ReplyId == null
                )
                .OrderByDescending(comment => comment.CreatedTime)
                .ToListAsync();
            PaginationResponseModel<ViewComment> res = new()
            {
                Total = listComment.Count,
                Limit = pagination.Limit,
                Offset = pagination.Offset,
                Results = ApplyPagination(listComment, pagination)
            };
            return res;
        }

        public virtual async Task<List<ViewComment>> GetListComments(Guid knowledgeId)
        {
            return await DbContext.ViewComments
                .Where(comment => comment.KnowledgeId == knowledgeId)
                .OrderByDescending(comment => comment.CreatedTime)
                .ToListAsync();
        }

        public virtual async Task<PaginationResponseModel<ViewUser>> GetListUserMaredKnowledge(Guid knowledgeId, PaginationDto pagination)
        {
            var listUserIds = await DbContext.Marks.Where(mark => mark.KnowledgeId == knowledgeId)
                .Select(mark => mark.UserId).Distinct().ToListAsync();
            int total = listUserIds.Count;
            IQueryable<ViewUser> listUser = ApplyPagination(
                DbContext.ViewUsers
                .Where(user => listUserIds.Contains(user.UserId)),
                pagination);
            return new PaginationResponseModel<ViewUser>
            {
                Total = total,
                Limit = pagination.Limit,
                Offset = pagination.Offset,
                Results = await listUser.ToListAsync()
            };
        }

        public virtual async Task<Mark?> GetMark(Guid userId, Guid knowledgeId)
        {
            return await DbContext.Marks
                .Where(mark => mark.UserId == userId && mark.KnowledgeId == knowledgeId)
                .FirstOrDefaultAsync();
        }

        protected override DbSet<Knowledge> GetDbSet()
        {
            return DbContext.Knowledges;
        }


        #region Get Exactly:

        public override async Task<IResponseUserItemModel?> GetExactlyResponseUserItemModel(Guid knowledgeId)
        {
            // Knowledges:
            Knowledge? knowledge = await DbContext.Knowledges.FindAsync(knowledgeId);
            if (knowledge == null) return null;

            // Member
            if (knowledge.KnowledgeType == EKnowledgeType.Course)
            {
                ViewCourse? course = await DbContext.ViewCourses
                    .Where(c => c.UserItemId == knowledgeId).FirstOrDefaultAsync();
                return course != null ? (ResponseCourseModel)new ResponseCourseModel().Copy(course) : null;
            }

            // Post
            Post? p = await DbContext.Posts.FindAsync(knowledgeId);
            if (p == null) return null;

            if (p.PostType == EPostType.Question)
            {
                // Question:
                ViewQuestion? question = await DbContext.ViewQuestions.FindAsync(knowledgeId);
                return question != null ? (ResponseQuestionModel)new ResponseQuestionModel().Copy(question) : null;
            }
            // Leson: 
            ViewLesson? lesson = await DbContext.ViewLessons.FindAsync(knowledgeId);
            return lesson != null ? (ResponseLessonModel)new ResponseLessonModel().Copy(lesson) : null;
        }

        public override async Task<Dictionary<Guid, IResponseUserItemModel?>> GetExactlyResponseUserItemModel(List<Guid> knowledgeIds)
        {
            Dictionary<Guid, IResponseUserItemModel?> res = knowledgeIds.Distinct()
                .ToDictionary(id => id, id => (IResponseUserItemModel?)null);

            // Get List courseIds, lessonIds and questionsIds:
            var knowledges = await DbContext.Knowledges
                .Where(knowledge => knowledgeIds.Contains(knowledge.UserItemId))
                .ToListAsync();
            var courseIds = knowledges
                .Where(knowledge => knowledge.KnowledgeType == EKnowledgeType.Course)
                .Select(knowledge => knowledge.UserItemId).ToList();
            var postIds = knowledges.Select(u => u.UserItemId).Except(courseIds).ToList();

            var lessonIds = await DbContext.Posts
                .Where(p => p.PostType == EPostType.Lesson && postIds.Contains(p.UserItemId))
                .Select(p => p.UserItemId)
                .ToListAsync();

            var questionIds = postIds.Except(lessonIds).ToList();

            var vCourses = await DbContext.ViewCourses.Where(course => courseIds.Contains(course.UserItemId)).ToListAsync();
            var vLessons = await DbContext.ViewLessons.Where(lesson => lessonIds.Contains(lesson.UserItemId)).ToListAsync();
            var vQuestions = await DbContext.ViewQuestions.Where(question => questionIds.Contains(question.UserItemId)).ToListAsync();

            // Return result:
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

        public override async Task<IUserItemView?> GetExactlyUserItem(Guid knowledgeId)
        {
            // Knowledges:
            Knowledge? knowledge = await DbContext.Knowledges.FindAsync(knowledgeId);
            if (knowledge == null) return null;

            // Member
            if (knowledge.KnowledgeType == EKnowledgeType.Course)
            {
                return await DbContext.ViewCourses
                    .Where(c => c.UserItemId == knowledgeId).FirstOrDefaultAsync();
            }

            // Post
            Post? p = await DbContext.Posts.FindAsync(knowledgeId);
            if (p == null) return null;

            if (p.PostType == EPostType.Question)
            {
                // Question:
                return await DbContext.ViewQuestions.FindAsync(knowledgeId);
            }
            // Leson: 
            return await DbContext.ViewLessons.FindAsync(knowledgeId);
        }

        #endregion
    }
}
