using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.DecorationRepositories;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Repositories.DecorationRepositories
{
    public class DecorationRepository(
        IStarRepository starRepository,
        IDbContext dbContext
        ) : BaseMySqlUserItemRepository<UserItem>(dbContext), IDecorationRepository
    {
        protected readonly IStarRepository StarRepository = starRepository;

        public async Task<IEnumerable<ResponseCourseLessonModel>> DecorateResponseCourseLessonModel(Guid? myUid, IEnumerable<CourseLesson> participants, bool isDecorateLesson = false, bool isDecorateCourse = false)
        {
            Dictionary<Guid, ResponseCourseCardModel> mapCourse = [];
            Dictionary<Guid, ResponseLessonModel> mapLesson = [];
            if (isDecorateCourse)
            { // Get list course
                IEnumerable<Guid> listCourseIds = participants.Select(p => p.CourseId).Distinct();
                IEnumerable<ViewCourse> courses = await DbContext.ViewCourses.Where(c => listCourseIds.Contains(c.UserItemId)).ToListAsync();
                IEnumerable<ResponseCourseCardModel> responseCourse = courses.Select(c => (ResponseCourseCardModel)new ResponseCourseCardModel().Copy(c));
                foreach (var course in responseCourse) mapCourse[course.UserItemId] = course;
            }
            if (isDecorateLesson)
            { // get list lesson
                IEnumerable<Guid> listLessonIds = participants.Select(p => p.LessonId).Distinct();
                IEnumerable<ViewLesson> lessons = await DbContext.ViewLessons.Where(l => listLessonIds.Contains(l.UserItemId)).ToListAsync();
                IEnumerable<ResponseLessonModel> responseLesson = await DecorateResponseLessonModel(myUid, lessons);
                foreach (var lesson in responseLesson) mapLesson[lesson.UserItemId] = lesson;
            }

            IEnumerable<ResponseCourseLessonModel> res = participants.Select(part =>
            {
                ResponseCourseLessonModel item = (ResponseCourseLessonModel) new ResponseCourseLessonModel().Copy(part);
                item.Course = mapCourse[item.CourseId];
                item.Lesson = mapLesson[item.LessonId];
                return item;
            });
            return res;
        }

        public virtual async Task<IEnumerable<ResponseLessonModel>> DecorateResponseLessonModel(Guid? myUid, IEnumerable<ViewLesson> lessons)
        {
            List<Guid> lessonIds = lessons.Select(lesson => lesson.UserItemId).ToList();
            Dictionary<Guid, int?>? myStars = null;
            if (myUid != null)
            {
                // calculate myStar from myUid to all lessons
                myStars = await StarRepository.CalculateUserStars(myUid.Value, lessonIds);
            }
            // calculate total stars to all lessons
            Dictionary<Guid, int> totalStars = await StarRepository.CalculateTotalStars(lessonIds);

            // calculate average stars to all lessons
            Dictionary<Guid, double?> averageStars = await StarRepository.CalculateAverageStars(lessonIds);

            // Number comments & Top comments

            // is Mark

            return lessons.Select(lesson =>
            {
                ResponseLessonModel les = new();
                les.Copy(lesson);
                les.MyStars = myStars?[lesson.UserItemId];
                les.TotalStars = totalStars[lesson.UserItemId];
                les.AverageStars = averageStars[lesson.UserItemId];
                return les;
            });
        }

        protected override DbSet<UserItem> GetDbSet()
        {
            return DbContext.UserItems;
        }
    }
}
