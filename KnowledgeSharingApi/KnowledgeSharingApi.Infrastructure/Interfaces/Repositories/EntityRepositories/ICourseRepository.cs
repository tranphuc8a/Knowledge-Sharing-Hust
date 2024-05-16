using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories
{
    public interface ICourseRepository : IRepository<Course>
    {
        #region Course Register

        /// <summary>
        /// Lấy về danh sách user đã đăng ký tham gia khóa học
        /// </summary>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        Task<List<ViewCourseRegister>> GetRegistersOfCourse(Guid courseId);

        /// <summary>
        /// Lấy về danh sách khóa học đã đăng ký của một user
        /// </summary>
        /// <param name="userId"> id của user </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        Task<List<ViewCourseRegister>> GetRegistersOfUser(Guid userId);

        /// <summary>
        /// Kiểm tra một user có tham gia khóa học không
        /// CÓ: Trả về ViewCourseRegister tương ứng
        /// KHÔNG: Ném ra NotExistedEntityException
        /// </summary>
        /// <param name="userId"> id của user cần kiểm tra </param>
        /// <param name="courseId"> id của khóa học cần kiểm tra </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        Task<ViewCourseRegister?> GetViewCourseRegister(Guid userId, Guid courseId); 
        #endregion

        #region Get View Courses

        /// <summary>
        /// Kiểm tra khóa học tồn tại và trả về chính khóa học
        /// </summary>
        /// <param name="courseId"> id của khóa học cần kiểm tra </param>
        /// <param name="errorMessage"> Cảnh báo lỗi </param>
        /// <returns> ViewCourse | throw NotExistedEntityException </returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        Task<ViewCourse> CheckExistedCourse(Guid courseId, string errorMessage);

        /// <summary>
        /// Lấy về ViewCourse theo id
        /// </summary>
        /// <param name="courseId"> id của course cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<ViewCourse?> GetViewCourse(Guid courseId);

        /// <summary>
        /// Lấy về Danh sách ViewCourse theo danh sách id
        /// </summary>
        /// <param name="courseIds"> danh sách id của các khóa học cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<ViewCourse?>> GetViewCourse(Guid[] courseIds);

        /// <summary>
        /// Lấy về Danh sách view course
        /// </summary>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<ViewCourse>> GetViewCourse(PaginationDto pagination);

        /// <summary>
        /// Lấy về Danh sách view course
        /// </summary>
        /// <param name="myUid"> id của user muốn lấy </param>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<ViewCourse>> GetViewCourse(Guid myUid, PaginationDto pagination);

        /// <summary>
        /// Lấy về Danh sách view course công khai
        /// </summary>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<ViewCourse>> GetPublicViewCourse(PaginationDto pagination);

        /// <summary>
        /// Lấy về Danh sách view course công khai của một user
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<ViewCourse>> GetPublicViewCourseOfUser(Guid userId);

        /// <summary>
        /// Lấy về Danh sách view course của một user (cho admin)
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<ViewCourse>> GetViewCourseOfUser(Guid userId);

        /// <summary>
        /// Lấy về Danh sách view course của một user cho một user khác
        /// </summary>
        /// <param name="myUid"> id của user muốn lấy </param>
        /// <param name="userId"> id của user cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<ViewCourse>> GetViewCourseOfUser(Guid myUid, Guid userId);

        /// <summary>
        /// Lấy về Danh sách view course của một catefories
        /// </summary>
        /// <param name="catName"> tên category muốn lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<ViewCourse>> GetViewCourseOfCategory(string catName, PaginationDto pagination);

        /// <summary>
        /// Lấy về Danh sách view course public của một catefories
        /// </summary>
        /// <param name="catName"> tên category muốn lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<ViewCourse>> GetPublicViewCourseOfCategory(string catName, PaginationDto pagination);

        /// <summary>
        /// Lấy về Danh sách view course của một catefories theo user
        /// </summary>
        /// <param name="myUid"> id của user muốn lấy </param>
        /// <param name="catName"> tên category muốn lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<ViewCourse>> GetViewCourseOfCategory(Guid myUid, string catName, PaginationDto pagination);

        /// <summary>
        /// Lấy về Danh sách view course của một user đã mark
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<ViewCourse>> GetMarkedCoursesOfUse(Guid userId, PaginationDto pagination);

        #endregion


    }
}
