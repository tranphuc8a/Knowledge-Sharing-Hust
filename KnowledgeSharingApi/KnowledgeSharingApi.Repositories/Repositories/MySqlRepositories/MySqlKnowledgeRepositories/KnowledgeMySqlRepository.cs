using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces;
using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.KnowledgeRepositories;
using KnowledgeSharingApi.Repositories.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Repositories.Repositories.MySqlRepositories.MySqlKnowledgeRepositories
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
            var course = await DbContext.Courses
                .Where(c => c.UserItemId == courseId)
                .Select(c => new {c.UserId, c.Privacy}).FirstOrDefaultAsync()
                ?? throw notExistedException;
            // course public, owner --> true
            if (course.Privacy == EPrivacy.Public || course.UserId == userId) return true;
            
            // private: join, invite
            bool isRegistered = await DbContext.ViewCourseRegisters
                .Where(cr => cr.UserId == userId && cr.CourseId == courseId)
                .AnyAsync();
            if (isRegistered) return true;

            // Lấy danh sách course mà myUid được invite vào
            IQueryable<Guid> listInvitedCourseIds = DbContext.CourseRelations
                .Where(invite => invite.ReceiverId == userId && invite.CourseRelationType == ECourseRelationType.Invite)
                .Select(invite => invite.CourseId);
            if (await listInvitedCourseIds.ContainsAsync(courseId)) return true;
            return false;
        }

        public virtual async Task<bool> CheckLessonAccessible(Guid userId, Guid lessonId)
        {
            Lesson lesson = await DbContext.Lessons.FindAsync(lessonId) ?? throw notExistedException;

            // public, owner : true
            if (lesson.Privacy == EPrivacy.Public || lesson.UserId == userId) return true;

            IQueryable<Guid> courseIds = DbContext.CourseLessons
                .Where(crl => crl.LessonId == lessonId).Select(crl => crl.CourseId);

            // Kiểm tra xem người dùng có đăng ký cho bất kỳ khóa học nào có chứa bài học này không.
            bool isRegistered = await DbContext.CourseRegisters
                .AnyAsync(cr => cr.UserId == userId && courseIds.Contains(cr.CourseId));
            if (isRegistered) return true;

            // Kiem tra xem user co phai la owner cua bat ky khoa hoc nao co chua bai hoc nay khong
            bool isOwner = await DbContext.Courses
                .AnyAsync(c => c.UserId == userId && courseIds.Contains(c.UserItemId));

            return isOwner;
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
            if (isRegistered) return true;

            // Kiem tra xem nguoi dung co phai la chu khoa hoc chua cau hoi nay khong
            bool isOwnerOfCourse = await DbContext.Courses
                .AnyAsync(c => c.UserId == userId && c.UserItemId == question.CourseId.Value);

            return isOwnerOfCourse;
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
            IQueryable<ViewComment> listComment =
                DbContext.ViewComments
                .Where(comment => comment.KnowledgeId == knowledgeId &&
                    // comment là comment bậc 1 (không phải comment reply)
                    comment.ReplyId == null
                )
                .OrderByDescending(comment => comment.CreatedTime);
            PaginationResponseModel<ViewComment> res = new()
            {
                Total = listComment.Count(),
                Limit = pagination.Limit,
                Offset = pagination.Offset,
                Results = await ApplyPagination(listComment, pagination).ToListAsync()
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

        public virtual async Task<PaginationResponseModel<ViewUserProfile>> GetListUserMarkedKnowledge(Guid knowledgeId, PaginationDto pagination)
        {
            var listUserIds = await DbContext.Marks.Where(mark => mark.KnowledgeId == knowledgeId)
                .Select(mark => mark.UserId).Distinct().ToListAsync();
            int total = listUserIds.Count;
            IQueryable<ViewUserProfile> listUser = ApplyPagination(
                DbContext.ViewUserProfiles
                .Where(user => listUserIds.Contains(user.UserId)),
                pagination);
            return new PaginationResponseModel<ViewUserProfile>
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

        public override async Task<IResponseUserItemModel?> GetExactlyResponseUserItemModel(Guid userItemId)
        {
            // Not:
            Guid? id = await DbContext.Knowledges.Where(it => it.UserItemId == userItemId).Select(u => u.UserItemId).FirstOrDefaultAsync();
            if (id == null) return null;

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

        public override async Task<Dictionary<Guid, IResponseUserItemModel?>> GetExactlyResponseUserItemModel(List<Guid> userItemId)
        {
            Dictionary<Guid, IResponseUserItemModel?> res = userItemId
                .Distinct().ToDictionary(id => id, id => (IResponseUserItemModel?)null);

            // Get List courseIds, lessonIds and questionsIds:
            userItemId = await DbContext.Knowledges.Where(it => userItemId.Contains(it.UserItemId)).Select(u => u.UserItemId).ToListAsync();
            var vCourses = await DbContext.ViewCourses.Where(course => userItemId.Contains(course.UserItemId)).ToListAsync();
            var postIds = userItemId.Except(vCourses.Select(c => c.UserItemId));
            var vLessons = await DbContext.ViewLessons.Where(lesson => postIds.Contains(lesson.UserItemId)).ToListAsync();
            var questionIds = postIds.Except(vLessons.Select(l => l.UserItemId));
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

        public override async Task<IViewUserItem?> GetExactlyUserItem(Guid userItemId)
        {
            // Not:
            Guid? id = await DbContext.Knowledges.Where(it => it.UserItemId == userItemId).Select(u => u.UserItemId).FirstOrDefaultAsync();
            if (id == null) return null;

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

        public override async Task<Dictionary<Guid, IViewUserItem?>> GetExactlyUserItem(List<Guid> userItemId)
        {
            Dictionary<Guid, IViewUserItem?> res = userItemId
                .Distinct().ToDictionary(id => id, id => (IViewUserItem?)null);

            // Get List commentIds, courseIds, lessonIds and questionsIds:
            userItemId = await DbContext.Knowledges.Where(it => userItemId.Contains(it.UserItemId)).Select(u => u.UserItemId).ToListAsync();
            var vCourses = await DbContext.ViewCourses.Where(course => userItemId.Contains(course.UserItemId)).ToListAsync();
            var postIds = userItemId.Except(vCourses.Select(c => c.UserItemId));
            var vLessons = await DbContext.ViewLessons.Where(lesson => postIds.Contains(lesson.UserItemId)).ToListAsync();
            var questionIds = postIds.Except(vLessons.Select(l => l.UserItemId));
            var vQuestions = await DbContext.ViewQuestions.Where(question => questionIds.Contains(question.UserItemId)).ToListAsync();

            // Return result:
            foreach (ViewCourse vCourse in vCourses) res[vCourse.UserItemId] = vCourse;
            foreach (ViewLesson vLes in vLessons) res[vLes.UserItemId] = vLes;
            foreach (ViewQuestion vQues in vQuestions) res[vQues.UserItemId] = vQues;
            return res;
        }

        #endregion
    }
}
