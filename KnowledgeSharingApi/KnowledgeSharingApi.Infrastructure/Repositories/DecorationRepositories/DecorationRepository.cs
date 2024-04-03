using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.DecorationRepositories;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Repositories.DecorationRepositories
{
    public class DecorationRepository : IDecorationRepository
    {
        IStarRepository StarRepository;

        public DecorationRepository(
            IStarRepository starRepository    
        )
        {
            StarRepository = starRepository;
        }

        public Task<IEnumerable<ResponseCourseLessonModel>> DecorateResponseCourseLessonModel(Guid? myUid, IEnumerable<CourseLesson> participants, bool isDecorateLesson = false, bool isDecorateCourse = false)
        {
            throw new NotImplementedException();
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
    }
}
