using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Repositories.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.KnowledgeRepositories
{
    public interface ILessonRepository : IRepository<Lesson>, IBasePostRepository<ViewLesson>
    {
        /// <summary>
        /// Hàm kiểm tra lesson có tồn tại hay không
        /// CÓ: trả về lesson tương ứng
        /// KHÔNG: throw Not Existed Entity Exception
        /// </summary>
        /// <param name="lessonId"> id của lesson cần kiểm tra </param>
        /// <param name="errorMessage"> message báo lỗi </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        Task<ViewLesson> CheckExistedLesson(Guid lessonId, string errorMessage);


        /// <summary>
        /// Lấy về danh sách lesson trong một course
        /// </summary>
        /// <param name="courseid"> id của course cần lấy </param>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        Task<PaginationResponseModel<ViewLesson>> GetLessonsInCourse(Guid courseid, PaginationDto pagination);


        /// <summary>
        /// Lấy về danh sách tất cả khóa học mà có sử dụng bài giảng hiện tại
        /// </summary>
        /// <param name="lessonId"> id của bài giảng cần lấy</param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        Task<List<Tuple<CourseLesson, ViewCourse>>> GetListCoursesOfLesson(Guid lessonId);
    }
}
