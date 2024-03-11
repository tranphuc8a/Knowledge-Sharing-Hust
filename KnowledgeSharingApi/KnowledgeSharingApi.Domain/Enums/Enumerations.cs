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
        ForgotPassword = 1
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
        Block = 3
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
        Course = 1,
        Post = 0
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

}
