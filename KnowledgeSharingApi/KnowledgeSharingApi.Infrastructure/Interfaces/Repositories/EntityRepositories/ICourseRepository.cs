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
        /// <summary>
        /// Lấy về danh sách user đã đăng ký tham gia khóa học
        /// </summary>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        Task<IEnumerable<ViewCourseRegister>> GetCourseRegisterOfCourse(Guid courseId);

        /// <summary>
        /// Lấy về danh sách khóa học đã đăng ký của một user
        /// </summary>
        /// <param name="userId"> id của user </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        Task<IEnumerable<ViewCourseRegister>> GetCourseRegisterOfUser(Guid userId);


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
    }
}
