using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Infrastructures.Encrypts;
using KnowledgeSharingApi.Services.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Interfaces
{
    public interface ICourseRelationService
    {

        #region Admin apies
        /// <summary>
        /// Yêu cầu admin lấy về danh sách người dùng đã đăng ký học trong một khóa học
        /// </summary>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        Task<ServiceResult> AdminGetCourseRegisters(Guid courseId, int? limit, int? offset);

        /// <summary>
        /// Yêu cầu admin xóa một user ra khỏi một course bất kỳ
        /// Chỉ cho phép xòa mỗi lần một register, không cho xóa hàng loạt
        /// Không cho phép xóa user join bằng payment
        /// </summary>
        /// <param name="registerId"> id của register cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        Task<ServiceResult> AdminDeleteUserFromCourse(Guid registerId);

        #endregion

        #region Get APies

        /// <summary>
        /// Yêu cầu user lấy về danh sách những người đã đăng ký tham gia khóa học
        /// Owner hoặc member
        /// </summary>
        /// <param name="myUId"> id của user muốn lấy </param>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <param name="limit"> Số lượng phần tử trang </param>
        /// <param name="offset"> Độ lệch trang </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetRegisters(Guid myUId, Guid courseId, int? limit, int? offset);

        /// <summary>
        /// Yêu cầu chủ khóa học lấy về danh sách invite của một khóa học
        /// Owner
        /// </summary>
        /// <param name="myUid"> id của user muốn lấy </param>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <param name="limit"> Số lượng phần tử trang </param>
        /// <param name="offset"> Độ lệch trang </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetCourseInvites(Guid myUid, Guid courseId, int? limit, int? offset);

        /// <summary>
        /// Yêu cầu chủ khóa học lấy về danh sách request của một khóa học
        /// Owner
        /// </summary>
        /// <param name="myUid"> id của user muốn lấy </param>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <param name="limit"> Số lượng phần tử trang </param>
        /// <param name="offset"> Độ lệch trang </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetCourseRequests(Guid myUid, Guid courseId, int? limit, int? offset);

        /// <summary>
        /// Yêu cầu chủ khóa học lấy về danh sách invite của một khóa học
        /// Owner
        /// </summary>
        /// <param name="myUid"> id của user muốn lấy </param>
        /// <param name="limit"> Số lượng phần tử trang </param>
        /// <param name="offset"> Độ lệch trang </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetMyCourseInvites(Guid myUid, int? limit, int? offset);

        /// <summary>
        /// Yêu cầu user học lấy về danh sách request của mình
        /// Owner
        /// </summary>
        /// <param name="myUId"> id của user muốn lấy </param>
        /// <param name="limit"> Số lượng phần tử trang </param>
        /// <param name="offset"> Độ lệch trang </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetMyCourseRequests(Guid myUId, int? limit, int? offset);

        /// <summary>
        /// Lay ve trang thai quan he giua user va course
        /// </summary>
        /// <param name="myUid"> id user thuc hien </param>
        /// <param name="courseId"> id course can lay </param>
        /// <param name="isFocusCourse"> Quyet dinh gia tri tra ve focus course hay focus user </param>
        /// <returns></returns>
        /// CReated: PhucTV (15/5/24)
        /// Modified: None
        Task<ServiceResult> UserGetCourseRelationStatus(Guid myUid, Guid courseId, bool? isFocusCourse = true);

        /// <summary>
        /// Lay ve trang thai quan he giua user va course
        /// </summary>
        /// <param name="myUid"> id user thuc hien </param>
        /// <param name="userId"> id cua user can lay </param>
        /// <param name="courseId"> id course can lay </param>
        /// <param name="isFocusCourse"> Quyet dinh gia tri tra ve focus course hay focus user </param>
        /// <returns></returns>
        /// CReated: PhucTV (15/5/24)
        /// Modified: None
        Task<ServiceResult> UserGetCourseRelationStatus(Guid myUid, Guid userId, Guid courseId, bool? isFocusCourse = true);

        #endregion


        #region User Register apies

        /// <summary>
        /// Yêu cầu user đăng ký tham gia một khóa học miễn phí
        /// Guest
        /// </summary>
        /// <param name="myUid"> id của user muốn đăng ký </param>
        /// <param name="courseId"> id của khóa học cần đăng ký </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        Task<ServiceResult> UserRegisterCourse(Guid myUid, Guid courseId);

        /// <summary>
        /// Yêu cầu user hủy đăng ký tham gia một khóa học miễn phí
        /// Guest
        /// </summary>
        /// <param name="myUid"> id của user muốn hủy đăng ký </param>
        /// <param name="courseId"> id của khóa học cần hủy đăng ký </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        Task<ServiceResult> UserUnregisterCourse(Guid myUid, Guid courseId);

        /// <summary>
        /// Yêu cầu user kick user khác khỏi khóa học hiện tại của mình
        /// Sẽ có nhiều điều kiện ràng buộc
        /// </summary>
        /// <param name="myUid"> id của user muốn thực hiện </param>
        /// <param name="registerId"> id của phiên user đăng ký khóa học </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        Task<ServiceResult> UserDeleteRegister(Guid myUid, Guid registerId);

        #endregion

        #region User Request apies

        /// <summary>
        /// Yêu cầu user yêu cầu tham gia một khóa học
        /// Guest, khóa học có tính phí
        /// </summary>
        /// <param name="myUid"> id user gửi yêu cầu </param>
        /// <param name="courseId"> id của khóa học cần yêu cầu </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        Task<ServiceResult> UserRequestCourse(Guid myUid, Guid courseId);

        /// <summary>
        /// Yêu cầu user xóa một course request
        /// </summary>
        /// <param name="myUid"> id của user muốn thực hiện</param>
        /// <param name="requestId"> id của request cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        Task<ServiceResult> UserDeleteCourseRequest(Guid myUid, Guid requestId);

        /// <summary>
        /// Yêu cầu user xác nhận yêu cầu tham gia khóa học
        /// </summary>
        /// <param name="myUid"> id của user muốn thực hiện </param>
        /// <param name="requestId"> id của yêu cầu tham gia khóa học </param>
        /// <param name="isAccept"> true - dong y, false - khong dong y </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        Task<ServiceResult> UserConfirmCourseRequest(Guid myUid, Guid requestId, bool isAccept);

        #endregion

        #region User Invite Apies

        /// <summary>
        /// Yêu cầu user invite một user khác tham gia khóa học
        /// </summary>
        /// <param name="myUid"> id của user muốn thực hiện </param>
        /// <param name="courseId"> id của khóa học được invite vào </param>
        /// <param name="userId"> id của user cần invitew </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        Task<ServiceResult> UserInviteUserToCourse(Guid myUid, Guid courseId, Guid userId);

        /// <summary>
        /// Yêu cầu user invite một list user khác tham gia khóa học
        /// </summary>
        /// <param name="courseId"> id của user muốn thực hiện </param>
        /// <param name="courseId"> id của khóa học được invite vào </param>
        /// <param name="listUserIds"> danh sách id của những user cần invitew </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        Task<ServiceResult> UserInviteListUserToCourse(Guid myUid, Guid courseId, IEnumerable<Guid> listUserIds);

        /// <summary>
        /// Yêu cầu user xác nhận lời mời tham gia khóa học
        /// </summary>
        /// <param name="myUid"> id của user muốn thực hiện </param>
        /// <param name="inviteId"> id của lời mời cần xác nhận </param>
        /// <param name="isAccept"> true - dong y, false - khong dong y </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        Task<ServiceResult> UserConfirmCourseInvite(Guid myUid, Guid inviteId, bool isAccept);

        /// <summary>
        /// Yêu cầu user xóa lời mời tham gia khóa học
        /// </summary>
        /// <param name="myUid"> id của user muốn thực hiện </param>
        /// <param name="inviteId"> id của lời mời cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        Task<ServiceResult> UserDeleteCourseInvite(Guid myUid, Guid inviteId);

        #endregion

    }
}
