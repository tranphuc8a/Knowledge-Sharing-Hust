using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
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
            // private: join
            ViewCourseRegister? courseRegister = await DbContext.ViewCourseRegisters
                .Where(cr => cr.UserId == userId && cr.CourseId == courseId)
                .FirstOrDefaultAsync();
            return courseRegister != null;
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

        public virtual async Task<PaginationResponseModel<ViewComment>> GetListComments(Guid knowledgeId, int limit, int offset)
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
                Limit = limit,
                Offset = offset,
                Results = listComment.Skip(offset).Take(limit)
            };
            return res;
        }

        public virtual async Task<IEnumerable<ViewComment>> GetListComments(Guid knowledgeId)
        {
            return await DbContext.ViewComments
                .Where(comment => comment.KnowledgeId == knowledgeId)
                .OrderByDescending(comment => comment.CreatedTime)
                .ToListAsync();
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

        protected override DbSet<Knowledge> GetDbSet()
        {
            return DbContext.Knowledges;
        }
    }
}
