using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.DecorationRepositories
{
    public interface IDecorationRepository
    {
        /// <summary>
        /// Bổ sung thêm thông tin cho ResponseLessonModel từ viewLesson
        /// </summary>
        /// <param name="myUid"> id của user thực hiện </param>
        /// <param name="lessons"> Danh sách các view lessons </param>
        /// <returns></returns>
        /// Created: PhucTV (3/4/24)
        /// Modified: None
        Task<List<ResponseLessonModel>> DecorateResponseLessonModel(Guid? myUid, List<ViewLesson> lessons);

        /// <summary>
        /// Bổ sung thêm thông tin cho ResponseQuestionModel từ viewQuestion
        /// </summary>
        /// <param name="myUid"> id của user thực hiện </param>
        /// <param name="questions"> Danh sách các view questions </param>
        /// <returns></returns>
        /// Created: PhucTV (3/5/24)
        /// Modified: None
        Task<List<ResponseQuestionModel>> DecorateResponseQuestionModel(Guid? myUid, List<ViewQuestion> questions);

        /// <summary>
        /// Bổ sung thêm thông tin cho ResponseCourseModel từ viewCourse
        /// </summary>
        /// <param name="myUid"> id của user thực hiện </param>
        /// <param name="courses"> Danh sách các viewCourses </param>
        /// <returns></returns>
        /// Created: PhucTV (3/5/24)
        /// Modified: None
        Task<List<ResponseCourseModel>> DecorateResponseCourseModel(Guid? myUid, List<ViewCourse> courses);

        /// <summary>
        /// Bổ sung thêm thông tin cho ResponseCourseLessonModel từ CourseLesson
        /// </summary>
        /// <param name="myUid"> id của user thực hiện </param>
        /// <param name="participants"> Danh sách bài giảng trong khóa học </param>
        /// <param name="isDecorateCourse"> true - có, false - không decorate Lesson </param>
        /// <param name="isDecorateLesson"> true - có, false - không decorate Course </param>
        /// <returns></returns>
        /// Created: PhucTV (3/4/24)
        /// Modified: None
        Task<List<ResponseCourseLessonModel>> DecorateResponseCourseLessonModel(Guid? myUid, 
            List<CourseLesson> participants, bool isDecorateLesson = false, bool isDecorateCourse = false);
    }
}
