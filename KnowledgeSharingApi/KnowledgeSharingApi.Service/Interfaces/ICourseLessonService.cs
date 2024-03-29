using KnowledgeSharingApi.Domains.Models.ApiRequestModels.CourseLessonModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Infrastructures.Encrypts;
using KnowledgeSharingApi.Services.Filters;
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
        /// <summary>
        /// Yêu cầu user thêm một bài giảng vào một khóa học
        /// </summary>
        /// <param name="myUid"> id của user cần thực hiện </param>
        /// <param name="model"> thông tin quyền riêng tư mới cần cập nhật </param>
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
        Task<ServiceResult> UpdateListLessonInCourse(Guid myUid, UpdateListLessonInCourseModel model);

    }
}
