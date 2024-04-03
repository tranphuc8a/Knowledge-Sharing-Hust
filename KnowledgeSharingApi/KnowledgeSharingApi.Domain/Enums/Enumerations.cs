using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Enums
{
    /// <summary>
    /// Danh sách giới tính
    /// </summary>
    /// Created: PhucTV (9/1/24)
    /// Modified: None
    public enum EGender
    {
        /// <summary>
        /// Giới tính nam
        /// </summary>
        Male = 0,

        /// <summary>
        /// Giới tính nữ
        /// </summary>
        Female = 1,

        /// <summary>
        /// Giới tính khác
        /// </summary>
        Other = 2
    }


    /// <summary>
    /// Danh sách trạng thái công việc
    /// </summary>
    /// Created: PhuccTV (9/1/24)
    /// Modified: None
    public enum EWorkStatus
    {
        /// <summary>
        /// Đang có việc làm
        /// </summary>
        Workful = 0,

        /// <summary>
        /// Thất nghiệp
        /// </summary>
        Workless = 1,

        /// <summary>
        /// Đang thực tập
        /// </summary>
        Intern = 2,

        /// <summary>
        /// Nghỉ hưu
        /// </summary>
        Rest = 3,

        /// <summary>
        /// Đang học
        /// </summary>
        Study = 4
    }


    /// <summary>
    /// Danh sách mã trả về
    /// </summary>
    /// Created: PhucTV (18/1/24)
    /// Modified: None
    public enum EStatusCode
    {
        /// <summary>
        /// Thành công
        /// </summary>
        Success = 200,
        Created = 201,
        Accepted = 202,
        NoContent = 204,

        /// <summary>
        /// Client Error
        /// </summary>
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        Conflict = 409,

        /// <summary>
        /// Server Error
        /// </summary>
        ServerError = 500,
        NotImplemented = 501,
        BadGateWay = 502,
        ServiceUnavailable = 503,
        GatewayTimeout = 504


    }


    /// <summary>
    /// Danh sách Phân quyền người dùng
    /// </summary>
    /// Created: PhucTV (20/1/24)
    /// Modified: None
    public enum EUserRole
    {
        User = 0,
        Admin = 1,
        Guest = 2
    }


    /// <summary>
    /// Danh sách loại code cần xác minh
    /// </summary>
    /// Created: PhucTV (21/1/24)
    /// Modified: None
    public enum EVerifyCodeType
    {
        Register = 0,
        ForgotPassword = 1,
        CancelUser = 2,
        Payment = 3
    }



    /// <summary>
    /// Danh sách các bước trong một pipe, procedure nào đó
    /// Gồm: step 1 -> step 5
    /// </summary>
    /// Created: PhucTV (17/3/24)
    /// Modified: None
    public enum EProcedureStep
    {
        Step1, Step2, Step3, Step4, Step5,
        Step6, Step7, Step8, Step9, Step10
    }


    /// <summary>
    /// Danh sách phân loại quan hệ user
    /// Gồm: Bạn bè, lời mời kết bạn, theo dõi, chặn
    /// </summary>
    /// Created: PhucTV (11/3/24)
    /// Modified: None
    public enum EUserRelationType
    {
        Friend = 0,
        FriendRequest = 1,
        Follow = 2,
        Block = 3,

        // Friend Request:
        Requester = 11,
        FriendRequester = 11,
        Requestee = 12,
        FriendRequestee = 12,

        // Folow:
        Follower = 21,
        Followee = 22,

        // Block:
        Blocker = 31,
        Blockee = 32,

        // Not in Relation
        NotInRelation = -1
    }


    /// <summary>
    /// Quyền riêng tư
    /// </summary>
    /// Created: PhucTV (11/3/24)
    /// Modified: None
    public enum EPrivacy
    {
        Private = 0,
        Public = 1,
    }


    /// <summary>
    /// Loại UserItem
    /// </summary>
    /// Created: PhucTV (11/3/24)
    /// Modified: None
    public enum EUserItemType
    {
        Knowledge = 0,
        Comment = 1
    }


    /// <summary>
    /// Loại phần tử kiến thức
    /// </summary>
    /// Created: PhucTV (11/3/24)
    /// Modified: None
    public enum EKnowledgeType
    {
        Post = 0,
        Course = 1
    }


    /// <summary>
    /// Loại bài đăng
    /// </summary>
    /// Created: PhucTV (11/3/24)
    /// Modified: None
    public enum EPostType
    {
        Lesson = 0,
        Question = 1
    }

    /// <summary>
    /// Loại quan hệ khóa học
    /// </summary>
    /// Created: PhucTV (11/3/24)
    /// Modified: None
    public enum ECourseRelationType
    {
        Request = 0,
        Invite = 1
    }


    /// <summary>
    /// Các lại vai trò của một user thường với một khóa học
    /// </summary>
    /// Created: PhucTV (29/3/24)
    /// Modified: None
    public enum ECourseRoleType
    {
        // Không có quyền truy cập
        // Khóa học private và không có bất kỳ quan hệ nào (member|invite)
        NotAccessible = 0,

        // Chỉ được phép xem trang thông tin giới thiệu khóa học
        // Chưa đăng ký, khóa học public hoặc (khóa học private + được chủ khóa học invite)
        Guest = 1,

        // Được phép truy cập mọi tài nguyên trong khóa học
        // Đã đăng ký là thành viên khóa học
        Member = 2,

        // Chủ nhân của khóa học
        Owner = 3
    }
}
