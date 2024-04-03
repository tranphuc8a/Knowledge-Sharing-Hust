
using KnowledgeSharingApi.Domains.Algorithms;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.CourseLessonModels;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Repositories.MySqlRepositories
{
    public class CourseLessonMySqlRepository(IDbContext dbContext)
        : BaseMySqlRepository<CourseLesson>(dbContext), ICourseLessonRepository
    {
        public async Task<CourseLesson?> AddLessonToCourse(AddLessonToCourseModel model)
        {
            try
            {
                // model: LessonId, CourseId, LessonTitle
                // Get total lesson of course
                int totalLesson = DbContext.CourseLessons
                    .Where(cl => cl.CourseId == model.CourseId)
                    .Count();

                // Create CourseLesson
                CourseLesson cl = new()
                {
                    // Entity:
                    CreatedBy = "Knowledge Sharing Admin",
                    CreatedTime = DateTime.Now,
                    // Course Lesson:
                    CourseLessonId = Guid.NewGuid(),
                    LessonId = model.LessonId!.Value,
                    CourseId = model.CourseId!.Value,
                    Offset = totalLesson + 1,
                    LessonTitle = model.LessonTitle!
                };

                // Insert
                DbContext.CourseLessons.Add(cl);
                int rows = await DbContext.SaveChangesAsync();
                return rows > 0 ? cl : null;
            }
            catch (Exception)
            {
                // Tra ve null neu co exception
                return null;
            }

        }

        public async Task<IEnumerable<CourseLesson>?> AddListLessonToCourse(AddListLessonToCourseModel model)
        {
            using var transaction = await DbContext.BeginTransaction();
            try
            {
                // Get total lesson of course
                int totalLesson = DbContext.CourseLessons
                    .Where(cl => cl.CourseId == model.CourseId)
                    .Count();

                // Create list CourseLesson
                Guid courseId = model.CourseId!.Value;
                DateTime now = DateTime.Now;
                string createdBy = "Knowledge Sharing Admin";
                IEnumerable<CourseLesson> courseLessons = model.ListLessonModel!
                    .Select(lesson => new CourseLesson
                    {
                        // Entity:
                        CreatedBy = createdBy,
                        CreatedTime = now,
                        // CourseLesson:
                        CourseLessonId = Guid.NewGuid(),
                        CourseId = courseId,
                        LessonId = lesson.LessonId!.Value,
                        Offset = ++totalLesson,
                        LessonTitle = lesson.LessonTitle!
                    });

                // Insert:
                DbContext.CourseLessons.AddRange(courseLessons);
                int rows = await DbContext.SaveChangesAsync();
                return rows > 0 ? courseLessons : null;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return null;
            }
        }

        public async Task<int> DeleteLessonFromCourse(Guid participantId)
        {
            IQueryable<CourseLesson> courseLessons = DbContext.CourseLessons.Where(cl => cl.CourseLessonId == participantId);
            DbContext.CourseLessons.RemoveRange(courseLessons);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteListLessonFromCourse(IEnumerable<Guid> listParticipantIds)
        {
            IQueryable<CourseLesson> courseLessons = DbContext.CourseLessons
                .Where(cl => listParticipantIds.Contains(cl.CourseLessonId));
            DbContext.CourseLessons.RemoveRange(courseLessons);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<CourseLesson>> GetCourseParticipant(Guid courseId)
        {
            return await DbContext.CourseLessons.Where(cl => cl.CourseId == courseId)
                .OrderBy(cl => cl.Offset)
                .ToListAsync();
        }

        public async Task<int> UpdateLessonInCourse(UpdateLessonInCourseModel model)
        {
            IQueryable<CourseLesson> courseLessons = DbContext.CourseLessons
                .Where(cl => cl.CourseLessonId == model.ParticipantId!.Value);
            DateTime now = DateTime.Now;
            string modifiedBy = "Knowledge Sharing Admin";
            foreach (CourseLesson cl in courseLessons)
            {
                cl.LessonTitle = model.LessonTitle!;
                cl.ModifiedBy = modifiedBy;
                cl.ModifiedTime = now;
            }
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateListLessonInCourse(IEnumerable<UpdateLessonInCourseModel> model)
        {
            // Get ve danh sachs courelesson can update
            IEnumerable<Guid> participantIds = model.Select(p => p.ParticipantId!.Value);
            IQueryable<CourseLesson> courseLessons = DbContext.CourseLessons
                .Where(cl => participantIds.Contains(cl.CourseLessonId));

            // Chuan bi update
            DateTime now = DateTime.Now;
            string modifiedBy = "Knowledge Sharing Admin";
            Dictionary<Guid, UpdateLessonInCourseModel> mapGuidToUpdate = model.ToDictionary(
                item => item.ParticipantId!.Value,
                item => item
            );

            // Update:
            foreach (CourseLesson cl in courseLessons)
            {
                cl.ModifiedBy = modifiedBy;
                cl.ModifiedTime = now;
                cl.LessonTitle = mapGuidToUpdate[cl.CourseLessonId].LessonTitle!;
            }

            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateOffsetOfListLessonInCourse(Guid[] listParticipantIds)
        {
            using var transaction = await DbContext.BeginTransaction();
            try
            {
                // Check existed
                IQueryable<CourseLesson> courseLessons = DbContext.CourseLessons
                    .Where(cl => listParticipantIds.Contains(cl.CourseLessonId));
                if (!courseLessons.Any()) throw new NotExistedEntityException("CourseLesson");
                Guid courseId = courseLessons.First()!.CourseId;

                // Prepare update
                Dictionary<Guid, int> mapGuidToOffsets = [];
                for (int i = 0; i < listParticipantIds.Length; i++)
                {
                    mapGuidToOffsets[listParticipantIds[i]] = i + 1;
                }

                // Update:
                foreach (CourseLesson cl in courseLessons)
                {
                    cl.Offset = mapGuidToOffsets[cl.CourseLessonId];
                }
                int rows = await DbContext.SaveChangesAsync();
                if (rows <= 0) throw new Exception();

                // Check permutation:
                IEnumerable<int> listOffsets = await DbContext.CourseLessons
                    .Where(cl => cl.CourseId == courseId).Select(cl => cl.Offset)
                    .ToListAsync();
                IEnumerable<int> pattern = Enumerable.Range(1, listOffsets.Count());
                if (!Algorithm.IsPermutation(pattern, listOffsets))
                    throw new Exception();

                // Pass check permutation:
                await transaction.CommitAsync();
                return rows;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return 0;
            }
        }
    }
}
