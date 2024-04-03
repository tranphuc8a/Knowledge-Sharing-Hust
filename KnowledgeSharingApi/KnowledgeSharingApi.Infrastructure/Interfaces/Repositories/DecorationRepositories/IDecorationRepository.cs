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
        /// Bổ sung thêm thông tin cho ResponseLessonModel từ lesson
        /// </summary>
        /// <param name="myUid"> id của user thực hiện </param>
        /// <param name="lessons"> Danh sách lesssons </param>
        /// <returns></returns>
        /// Created: PhucTV (3/4/24)
        /// Modified: None
        Task<IEnumerable<ResponseLessonModel>> DecorateResponseLessonModel(Guid? myUid, IEnumerable<ViewLesson> lessons);

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
        Task<IEnumerable<ResponseCourseLessonModel>> DecorateResponseCourseLessonModel(Guid? myUid, 
            IEnumerable<CourseLesson> participants, bool isDecorateLesson = false, bool isDecorateCourse = false);
    }
}
