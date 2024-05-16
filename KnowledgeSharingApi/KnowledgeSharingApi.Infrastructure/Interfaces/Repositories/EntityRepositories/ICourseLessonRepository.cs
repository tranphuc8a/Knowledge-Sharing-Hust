using KnowledgeSharingApi.Domains.Models.ApiRequestModels.CourseLessonModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories
{
    public interface ICourseLessonRepository : IRepository<CourseLesson>
    {
        #region Create
        /// <summary>
        /// Yêu cầu user thêm một bài giảng vào một khóa học
        /// </summary>
        /// <param name="model"> thông tin bài học được thêm vào khóa học </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        Task<CourseLesson?> AddLessonToCourse(AddLessonToCourseModel model);


        /// <summary>
        /// Yêu cầu user thêm danh sách bài giảng vào khóa học
        /// </summary>
        /// <param name="model"> thông tin danh sách bài giảng cần thêm </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        Task<List<CourseLesson>?> AddListLessonToCourse(AddListLessonToCourseModel model);
        #endregion


        #region Delete
        /// <summary>
        /// Yêu cầu user xóa một bài học khỏi khóa học
        /// </summary>
        /// <param name="participantId"> Id của course-lesson cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        Task<int> DeleteLessonFromCourse(Guid participantId);

        /// <summary>
        /// Yêu cầu user xóa danh sách bài học khỏi khóa học
        /// </summary>
        /// <param name="listParticipantIds"> Danh sách id của course-lesson cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        Task<int> DeleteListLessonFromCourse(List<Guid> listParticipantIds);
        #endregion

        #region Get

        /// <summary>
        /// Lay ve toan bo bai giang trong mot khoa hoc cu the
        /// </summary>
        /// <param name="courseId"> id cua khoa hoc muon lay</param>
        /// <returns></returns>
        /// Created: PhucTV (3/4/24)
        /// Modified: None
        Task<List<CourseLesson>> GetCourseParticipant(Guid courseId);
        
        #endregion

        #region Update
        /// <summary>
        /// Yêu cầu user cập nhật thông tin một bài học trong khóa học
        /// </summary>
        /// <param name="model"> Thông tin cập nhật course-lesson </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        Task<int> UpdateLessonInCourse(UpdateLessonInCourseModel model);

        /// <summary>
        /// Yêu cầu user cập nhật danh sách bài học trong khóa học
        /// </summary>
        /// <param name="model"> Thông tin danh sách course-lesson cần cập nhật </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        Task<int> UpdateListLessonInCourse(List<UpdateLessonInCourseModel> model);


        /// <summary>
        /// Yêu cầu user cập nhật thứ tự các bài học trong khóa học
        /// </summary>
        /// <param name="listParticipantIds"> Danh sách id của course-lesson theo thứ tự cần sắp xếp </param>
        /// <returns></returns>
        /// Created: PhucTV (01/04/24)
        /// Modified: None
        Task<int> UpdateOffsetOfListLessonInCourse(Guid[] listParticipantIds); 
        #endregion
    }
}
