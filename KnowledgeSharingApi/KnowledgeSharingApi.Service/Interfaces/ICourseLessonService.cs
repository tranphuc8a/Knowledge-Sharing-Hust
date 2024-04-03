using KnowledgeSharingApi.Domains.Models.ApiRequestModels.CourseLessonModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Infrastructures.Encrypts;
using KnowledgeSharingApi.Services.Filters;
using KnowledgeSharingApi.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Interfaces
{
    public interface ICourseLessonService
    {
        #region Get APIes
        /// <summary>
        /// Yêu cầu user lấy về danh sách các bài học đang có trong khóa học
        /// Owner, Member
        /// </summary>
        /// <param name="myUid"> id của user muốn lấy </param>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <param name="limit"> Kích thước trang </param>
        /// <param name="offset"> Độ lệch trang </param>
        /// <returns></returns>
        /// Created: PhucTV (3/4/24)
        /// Modified: None
        Task<ServiceResult> UserGetListCourseParticipants(Guid myUid, Guid courseId, int? limit, int? offset);

        /// <summary>
        /// Yêu cầu Admin lấy về danh sách các bài học đang có trong khóa học
        /// Owner, Member
        /// </summary>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <param name="limit"> Kích thước trang </param>
        /// <param name="offset"> Độ lệch trang </param>
        /// <returns></returns>
        /// Created: PhucTV (3/4/24)
        /// Modified: None
        Task<ServiceResult> AdminGetListCourseParticipants(Guid courseId, int? limit, int? offset);
        #endregion

        #region Create apies

        /// <summary>
        /// Yêu cầu user thêm một bài giảng vào một khóa học
        /// </summary>
        /// <param name="myUid"> id của user cần thực hiện </param>
        /// <param name="model"> thông tin bài học được thêm vào khóa học </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        Task<ServiceResult> AddLessonToCourse(Guid myUid, AddLessonToCourseModel model);


        /// <summary>
        /// Yêu cầu user thêm danh sách bài giảng vào khóa học
        /// </summary>
        /// <param name="myUid"> id user cần thực hiện </param>
        /// <param name="model"> thông tin danh sách bài giảng cần thêm </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        Task<ServiceResult> AddListLessonToCourse(Guid myUid, AddListLessonToCourseModel model);
        #endregion


        #region Delete apies
        /// <summary>
        /// Yêu cầu user xóa một bài học khỏi khóa học
        /// </summary>
        /// <param name="myUid"> Id của user muốn thực hiện </param>
        /// <param name="participantId"> Id của course-lesson cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        Task<ServiceResult> DeleteLessonFromCourse(Guid myUid, Guid participantId);

        /// <summary>
        /// Yêu cầu user xóa danh sách bài học khỏi khóa học
        /// </summary>
        /// <param name="myUid"> Id của user muốn thực hiện </param>
        /// <param name="listParticipantIds"> Danh sách id của course-lesson cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        Task<ServiceResult> DeleteListLessonFromCourse(Guid myUid, IEnumerable<Guid> listParticipantIds);

        #endregion

        #region Update apies
        /// <summary>
        /// Yêu cầu user cập nhật thông tin một bài học trong khóa học
        /// </summary>
        /// <param name="myUid"> id của user muốn thực hiện </param>
        /// <param name="model"> Thông tin cập nhật course-lesson </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        Task<ServiceResult> UpdateLessonInCourse(Guid myUid, UpdateLessonInCourseModel model);

        /// <summary>
        /// Yêu cầu user cập nhật danh sách bài học trong khóa học
        /// </summary>
        /// <param name="myUid"> id của user muốn thực hiện </param>
        /// <param name="model"> Thông tin danh sách course-lesson cần cập nhật </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        Task<ServiceResult> UpdateListLessonInCourse(Guid myUid, IEnumerable<UpdateLessonInCourseModel> model);


        /// <summary>
        /// Yêu cầu user cập nhật thứ tự các bài học trong khóa học
        /// </summary>
        /// <param name="myUid"> id của user muốn thực hiện </param>
        /// <param name="listParticipantIds"> Danh sách id của course-lesson theo thứ tự cần sắp xếp </param>
        /// <returns></returns>
        /// Created: PhucTV (01/04/24)
        /// Modified: None
        Task<ServiceResult> UpdateOffsetOfListLessonInCourse(Guid myUId, Guid[] listParticipantIds); 
        #endregion
    }
}
