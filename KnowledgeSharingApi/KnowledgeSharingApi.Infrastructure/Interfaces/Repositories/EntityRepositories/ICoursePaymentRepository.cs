using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories
{
    public interface ICoursePaymentRepository : IRepository<CoursePayment>
    {
        /// <summary>
        /// Lấy về danh sách payment của một course, user
        /// </summary>
        /// <param name="courseId"> id của course cần lấy </param>
        /// <param name="userId"> id của user cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (30/3/24)
        /// Modified: None
        Task<IEnumerable<ViewCoursePayment>> GetByCourse(Guid courseId);
        Task<IEnumerable<ViewCoursePayment>> GetByUser(Guid userId);


        /// <summary>
        /// Lấy về hóa đơn thanh toán của một khóa học
        /// </summary>
        /// <param name="paymentId"> id của hóa đơn thanh toán khóa học </param>
        /// <returns></returns>
        /// Created: PhucTV (30/3/24)
        /// Modfied: None
        Task<ViewCoursePayment?> GetCoursePayment(Guid paymentId);

        /// <summary>
        /// Lấy về hóa đơn thanh toán của một user tới khóa học
        /// </summary>
        /// <param name="userId"> id của người thanh toán </param>
        /// <param name="courseId"> id của khóa học thanh toán </param>
        /// <returns></returns>
        /// Created: PhucTV (30/3/24)
        /// Modfied: None
        Task<IEnumerable<CoursePayment>> GetCoursePayment(Guid userId, Guid courseId);

        /// <summary>
        /// Thực hiện thanh toán userId cho khóa học courseId
        /// </summary>
        /// <param name="userId"> id của user thanh toán </param>
        /// <param name="courseId"> id của khóa học được đăng ký </param>
        /// <returns></returns>
        /// Created: PhucTV (30/3/24)
        /// Modified: None
        Task<int> UserPayCourse(Guid userId, Guid courseId);
    }

}
