using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Repositories.MySqlRepositories
{
    public class LessonMySqlRepository(IDbContext dbContext)
        : BaseMySqlUserItemRepository<Lesson>(dbContext), ILessonRepository
    {
        public override async Task<int> Delete(Guid lessonId)
        {
            using var transaction = await DbContext.BeginTransaction();
            try
            {
                // Xóa các Member lesson trước
                IQueryable<CourseLesson> courseLessons = DbContext.CourseLessons
                    .Where(cl => cl.LessonId == lessonId);
                DbContext.CourseLessons.RemoveRange(courseLessons);

                // XÓa lesson sau
                IQueryable<Lesson> lessons = DbContext.Lessons
                    .Where(les => les.UserItemId == lessonId);
                DbContext.Lessons.RemoveRange(lessons);

                int deleted = await DbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return deleted;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public virtual async Task<ViewLesson> CheckExistedLesson(Guid lessonId, string errorMessage)
        {
            return await DbContext.ViewLessons.FindAsync(lessonId)
                ?? throw new NotExistedEntityException(errorMessage);
        }

        public virtual async Task<IEnumerable<ViewLesson>> GetByUserId(Guid userId)
        {
            return await DbContext.ViewLessons
                .Where(les => les.UserId == userId)
                .OrderByDescending(les => les.CreatedTime)
                .ToListAsync();
        }

        public virtual async Task<IEnumerable<ViewLesson>> GetMarkedPosts(Guid userId)
        {
            IQueryable<Guid> listMarkedItemId = DbContext.Marks
                .Where(mark => mark.UserId == userId).Select(mark => mark.KnowledgeId);
            return await DbContext.ViewLessons
                .Where(les => listMarkedItemId.Contains(les.UserItemId))
                .OrderByDescending(les => les.CreatedTime)
                .ToListAsync();
        }

        public virtual async Task<IEnumerable<ViewLesson>> GetPostsOfCategory(string catName, int limit, int offset)
        {
            IQueryable<Guid> listKnowledgeId = DbContext.ViewKnowledgeCategories
                .Where(cat => cat.CategoryName == catName).Select(cat => cat.KnowledgeId);
            return await DbContext.ViewLessons
                .Where(les => listKnowledgeId.Contains(les.UserItemId))
                .OrderByDescending(les => les.CreatedTime)
                .Skip(offset).Take(limit)
                .ToListAsync();
        }

        public async Task<IEnumerable<ViewLesson>> GetPostsOfCategory(Guid myUId, string catName, int limit, int offset)
        {
            // Lấy danh sách id các khóa học mà myUid đang học
            IQueryable<Guid> listCoursesId = DbContext.CourseRegisters
                .Where(cr => cr.UserId == myUId).Select(cr => cr.CourseId).Distinct();

            // Lấy danh sách id của các bài học trong các khóa học này
            IQueryable<Guid> listViewableLessonsId = DbContext.CourseLessons
                .Where(cl => listCoursesId.Contains(cl.CourseId))
                .Select(cl => cl.LessonId).Distinct();

            // Lấy danh sách id của các knowledge có catName
            IQueryable<Guid> listKnowledgesId = DbContext.ViewKnowledgeCategories
                .Where(kc => kc.CategoryName == catName)
                .Select(kc => kc.KnowledgeId).Distinct();

            // Lấy danh sách các Lesson thỏa mãn:
            return await DbContext.ViewLessons
                .Where(les => listKnowledgesId.Contains(les.UserItemId) && (
                    // myUId phải Xem được:
                    // Là chủ
                    (les.UserId == myUId) ||
                    // public
                    (les.Privacy == EPrivacy.Public) ||
                    // Trong các khóa học đã đăng ký
                    (listViewableLessonsId.Contains(les.UserItemId))
                ))
                .OrderByDescending(les => les.CreatedTime)
                .Skip(offset).Take(limit)
                .ToListAsync();
        }

        public async Task<IEnumerable<ViewLesson>> GetPublicPosts(int limit, int offset)
        {
            return await DbContext.ViewLessons.Where(les => les.Privacy == EPrivacy.Public)
                .OrderByDescending(les => les.CreatedTime)
                .Skip(offset).Take(limit).ToListAsync();
        }

        public async Task<IEnumerable<ViewLesson>> GetPublicPostsByUserId(Guid userId)
        {
            return await DbContext.ViewLessons.Where(les => les.UserId == userId)
                .OrderByDescending(les => les.CreatedTime).ToListAsync();
        }

        public async Task<IEnumerable<ViewLesson>> GetPublicPostsOfCategory(string catName, int limit, int offset)
        {
            IQueryable<Guid> listKnIds = DbContext.ViewKnowledgeCategories
                .Where(ct => ct.CategoryName == catName)
                .Select(ct => ct.KnowledgeCategoryId);
            return await DbContext.ViewLessons
                .Where(les => listKnIds.Contains(les.UserItemId))
                .OrderByDescending(les => les.CreatedTime)
                .Skip(offset).Take(limit)
                .ToListAsync();
        }

        public async Task<PaginationResponseModel<ViewLesson>> GetLessonsInCourse(Guid courseid, int limit, int offset)
        {
            IQueryable<Guid> listIds = DbContext.CourseLessons
                .Where(cl => cl.CourseId == courseid)
                .OrderBy(cl => cl.Offset)
                .Select(cl => cl.LessonId);

            int total = listIds.Count();
            listIds = listIds.Skip(offset).Take(limit);

            IEnumerable<ViewLesson> listlessons = await DbContext.ViewLessons
                .Where(les => listIds.Contains(les.UserItemId))
                .ToListAsync();

            return new PaginationResponseModel<ViewLesson>(total, limit, offset, listlessons);
        }

        public async Task<IEnumerable<ViewLesson>> GetViewPost(int limit, int offset)
        {
            return await DbContext.ViewLessons
                .OrderByDescending(l => l.CreatedTime)
                .Skip(offset).Take(limit)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tuple<CourseLesson, ViewCourse>>> GetListCoursesOfLesson(Guid lessonId)
        {
            IEnumerable<CourseLesson> courseLessons = await DbContext.CourseLessons
                .Where(cl => cl.LessonId == lessonId).ToListAsync();

            IEnumerable<Guid> listCoursesId = courseLessons.Select(cl => cl.CourseId).ToList();
            Dictionary<Guid, CourseLesson> mapCourseIdToCourseLesson 
                = courseLessons.ToDictionary(cl => cl.CourseId, cl => cl);

            IEnumerable<ViewCourse> listCourses = await DbContext.ViewCourses
                .Where(c => listCoursesId.Contains(c.UserItemId)).ToListAsync();
            Dictionary<Guid, ViewCourse> mapCourseIdToViewCourse
                = listCourses.ToDictionary(c => c.UserItemId, c => c);

            return listCoursesId.Select(id => Tuple.Create(
                mapCourseIdToCourseLesson[id], mapCourseIdToViewCourse[id]
            ));
        }

        protected override DbSet<Lesson> GetDbSet()
        {
            return DbContext.Lessons;
        }
    }
}
