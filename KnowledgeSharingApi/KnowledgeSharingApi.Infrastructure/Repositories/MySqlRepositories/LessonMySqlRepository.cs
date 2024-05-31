using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
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
            return (ViewLesson) ((await DbContext.ViewLessons.FindAsync(lessonId))?.Clone()
                ?? throw new NotExistedEntityException(errorMessage));
        }

        public virtual async Task<List<ViewLesson>> GetByUserId(Guid userId)
        {
            return await DbContext.ViewLessons
                .Where(les => les.UserId == userId)
                .OrderByDescending(les => les.CreatedTime)
                .ToListAsync();
        }

        public virtual async Task<List<ViewLesson>> GetByUserId(Guid userId, PaginationDto pagination)
        {
            return await ApplyPagination( 
                    DbContext.ViewLessons
                    .Where(les => les.UserId == userId)
                    .OrderByDescending(les => les.CreatedTime),
                    pagination)
                .ToListAsync();
        }

        public virtual async Task<List<ViewLesson>> GetMarkedPosts(Guid userId)
        {
            IQueryable<Guid> listMarkedItemId = DbContext.Marks
                .Where(mark => mark.UserId == userId).Select(mark => mark.KnowledgeId);
            return await DbContext.ViewLessons
                .Where(les => listMarkedItemId.Contains(les.UserItemId))
                .OrderByDescending(les => les.CreatedTime)
                .ToListAsync();
        }

        public virtual async Task<List<ViewLesson>> GetMarkedPosts(Guid userId, PaginationDto pagination)
        {
            IQueryable<Guid> listMarkedItemId = DbContext.Marks
                .Where(mark => mark.UserId == userId).Select(mark => mark.KnowledgeId);
            return await ApplyPagination(
                DbContext.ViewLessons
                .Where(les => listMarkedItemId.Contains(les.UserItemId))
                .OrderByDescending(les => les.CreatedTime), pagination)
                .ToListAsync();
        }

        public virtual async Task<List<ViewLesson>> GetPostsOfCategory(string catName, PaginationDto pagination)
        {
            IQueryable<Guid> listKnowledgeId = DbContext.ViewKnowledgeCategories
                .Where(cat => cat.CategoryName == catName).Select(cat => cat.KnowledgeId);
            return await ApplyPagination(
                    DbContext.ViewLessons
                    .Where(les => listKnowledgeId.Contains(les.UserItemId))
                    .OrderByDescending(les => les.CreatedTime),
                    pagination
                ).ToListAsync();
        }

        public virtual async Task<List<ViewLesson>> GetPostsOfCategory(Guid myUId, string catName, PaginationDto pagination)
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
            return await ApplyPagination(
                    DbContext.ViewLessons
                    .Where(les => listKnowledgesId.Contains(les.UserItemId) && (
                        // myUId phải Xem được:
                        // Là chủ
                        (les.UserId == myUId) ||
                        // public
                        (les.Privacy == EPrivacy.Public) ||
                        // Trong các khóa học đã đăng ký
                        (listViewableLessonsId.Contains(les.UserItemId))
                    ))
                    .OrderByDescending(les => les.CreatedTime),
                    pagination                                
                ).ToListAsync();
        }

        public virtual async Task<List<ViewLesson>> GetPublicPosts()
        {
            return await DbContext.ViewLessons.Where(vl => vl.Privacy == EPrivacy.Public).ToListAsync();
        }

        public async Task<List<ViewLesson>> GetPublicPosts(PaginationDto pagination)
        {
            return await ApplyPagination(
                    DbContext.ViewLessons.Where(les => les.Privacy == EPrivacy.Public)
                    .OrderByDescending(les => les.CreatedTime),
                    pagination
                ).ToListAsync();
        }

        public async Task<List<ViewLesson>> GetPublicPostsByUserId(Guid userId)
        {
            return await DbContext.ViewLessons.Where(les => les.UserId == userId && les.Privacy == EPrivacy.Public)
                .OrderByDescending(les => les.CreatedTime).ToListAsync();
        }

        public async Task<List<ViewLesson>> GetPublicPostsByUserId(Guid userId, PaginationDto pagination)
        {
            return await ApplyPagination(
                DbContext.ViewLessons.Where(les => les.UserId == userId && les.Privacy == EPrivacy.Public)
                .OrderByDescending(les => les.CreatedTime), pagination).ToListAsync();
        }

        public async Task<List<ViewLesson>> GetPublicPostsOfCategory(string catName, PaginationDto pagination)
        {
            IQueryable<Guid> listKnIds = DbContext.ViewKnowledgeCategories
                .Where(ct => ct.CategoryName == catName)
                .Select(ct => ct.KnowledgeCategoryId);
            return await ApplyPagination(
                    DbContext.ViewLessons
                    .Where(les => listKnIds.Contains(les.UserItemId) && les.Privacy == EPrivacy.Public)
                    .OrderByDescending(les => les.CreatedTime),
                    pagination
                ).ToListAsync();
        }

        public async Task<PaginationResponseModel<ViewLesson>> GetLessonsInCourse(Guid courseid, PaginationDto pagination)
        {
            List<Guid> listIds = await DbContext.CourseLessons
                .Where(cl => cl.CourseId == courseid)
                .Select(cl => cl.LessonId).Distinct()
                .ToListAsync();
            int total = listIds.Count;

            IQueryable<ViewLesson> listlessons = ApplyPagination(
                DbContext.ViewLessons
                .Where(les => listIds.Contains(les.UserItemId)),
                pagination
            );

            return new PaginationResponseModel<ViewLesson>(total, pagination.Limit, pagination.Offset, await listlessons.ToListAsync());
        }

        public async Task<List<ViewLesson>> GetViewPost(PaginationDto pagination)
        {
            return await ApplyPagination(
                    DbContext.ViewLessons
                    .OrderByDescending(l => l.CreatedTime),
                    pagination)
                .ToListAsync();
        }

        public async Task<List<ViewLesson>> GetViewPost(Guid userId, PaginationDto pagination)
        {
            // Lấy danh sách id các khóa học mà myUid đang học
            IQueryable<Guid> listCoursesId = DbContext.CourseRegisters
                .Where(cr => cr.UserId == userId).Select(cr => cr.CourseId).Distinct();

            // Lấy danh sách id của các bài học trong các khóa học này
            IQueryable<Guid> listViewableLessonsId = DbContext.CourseLessons
                .Where(cl => listCoursesId.Contains(cl.CourseId))
                .Select(cl => cl.LessonId).Distinct();

            // Lấy danh sách các Lesson thỏa mãn:
            return await ApplyPagination(
                    DbContext.ViewLessons
                    .Where(les => (
                            // userId phải Xem được:
                            // Là chủ
                            (les.UserId == userId) ||
                            // public
                            (les.Privacy == EPrivacy.Public) ||
                            // Trong các khóa học đã đăng ký
                            (listViewableLessonsId.Contains(les.UserItemId)
                    )))
                    .OrderByDescending(les => les.CreatedTime),
                    pagination
                ).ToListAsync();
        }

        public async Task<List<Tuple<CourseLesson, ViewCourse>>> GetListCoursesOfLesson(Guid lessonId)
        {
            List<CourseLesson> courseLessons = await DbContext.CourseLessons
                .Where(cl => cl.LessonId == lessonId)
                .GroupBy(cl => cl.CourseId)
                .Select(group => group.First())
                .ToListAsync();

            List<Guid> listCoursesId = courseLessons.Select(cl => cl.CourseId).ToList();
            Dictionary<Guid, CourseLesson> mapCourseIdToCourseLesson 
                = courseLessons.ToDictionary(cl => cl.CourseId, cl => cl);

            List<ViewCourse> listCourses = await DbContext.ViewCourses
                .Where(c => listCoursesId.Contains(c.UserItemId)).ToListAsync();
            Dictionary<Guid, ViewCourse> mapCourseIdToViewCourse
                = listCourses.ToDictionary(c => c.UserItemId, c => c);

            return listCoursesId.Select(id => Tuple.Create(
                mapCourseIdToCourseLesson[id], mapCourseIdToViewCourse[id]
            )).ToList();
        }

        protected override DbSet<Lesson> GetDbSet()
        {
            return DbContext.Lessons;
        }

    }
}
