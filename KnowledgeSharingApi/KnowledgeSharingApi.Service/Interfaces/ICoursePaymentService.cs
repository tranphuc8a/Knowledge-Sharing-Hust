using KnowledgeSharingApi.Domains.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Interfaces
{
    public interface ICoursePaymentService : IVerifyByEmailService
    {

        #region Payment course apies

        /// <summary>
        /// Yêu cầu chủ khóa học lấy về danh sách payment của khóa học
        /// Owner
        /// </summary>
        /// <param name="myUid"> id của user muốn lấy </param>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <param name="limit"> Số lượng phần tử trang </param>
        /// <param name="offset"> Độ lệch trang </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetCoursePayments(Guid myUid, Guid courseId, int? limit, int? offset);

        /// <summary>
        /// Yêu cầu user lấy về danh sách các payment mà mình đã thanh toán
        /// Owner của payments
        /// </summary>
        /// <param name="myUid"> id của user muốn lấy </param>
        /// <param name="limit"> Số lượng phần tử trang </param>
        /// <param name="offset"> Độ lệch trang </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetMyPayments(Guid myUid, int? limit, int? offset);

        /// <summary>
        /// Yêu cầu user lấy về chi tiết một payment
        /// Owner của payment hoặc owner của course của payment
        /// </summary>
        /// <param name="myUid"> id của user muốn lấy </param>
        /// <param name="paymentId"> id của payment cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetPayment(Guid myUid, Guid paymentId);

        #endregion


    }
}
