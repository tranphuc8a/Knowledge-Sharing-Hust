using KnowledgeSharingApi.Domains.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Interfaces
{
    public interface IPostService
    {
        #region Feed
        /// <summary>
        /// Nhóm API lấy về danh sách bài posts trên feed
        /// </summary>
        /// <param name="myUid"> Id của chính mình </param>
        /// <param name="page"> phan trang </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetPosts(Guid myUid, PaginationDto page);
        Task<ServiceResult> AnonymousGetPosts(PaginationDto page);
        Task<ServiceResult> AdminGetPosts(PaginationDto page);


        /// <summary>
        /// Lấy về danh sách các bài post của mình
        /// </summary>
        /// <param name="myUid"> Id của chính mình </param>
        /// <param name="page"> phan trang </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetMyPosts(Guid myUid, PaginationDto page);


        /// <summary>
        /// Nhóm API lấy về danh sách bài posts của user khác
        /// </summary>
        /// <param name="myUid"> Id của chính mình </param>
        /// <param name="userId"> Id của user muốn lấy post </param>
        /// <param name="page"> phan trang </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetUserPosts(Guid myUid, Guid userId, PaginationDto page);
        Task<ServiceResult> AnonymousGetUserPosts(Guid userId, PaginationDto page);
        Task<ServiceResult> AdminGetUserPosts(Guid userId, PaginationDto page);


        /// <summary>
        /// Nhóm API lấy về danh sách bài đăng công khai có gắn thẻ theo một category name
        /// </summary>
        /// <param name="page"> phan trang </param>
        /// <param name="catName"> Tên của category cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<ServiceResult> AnonymousGetListPostsOfCategory(string catName, PaginationDto page);
        Task<ServiceResult> UserGetListPostsOfCategory(Guid myUid, string catName, PaginationDto page);
        Task<ServiceResult> AdminGetListPostsOfCategory(string catName, PaginationDto page);


        /// <summary>
        /// User lấy về danh sách bài đăng mà mình đã đánh dấu
        /// </summary>
        /// <param name="myUid">id của người lấy </param>
        /// <param name="page"> phan trang </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetMyMarkedPosts(Guid myUid, PaginationDto page);

        #endregion


        #region Delete Posts

        /// <summary>
        /// Nhóm API xóa bài đăng
        /// </summary>
        /// <param name="myUid"> id của user yêu cầu xóa </param>
        /// <param name="postId"> id của bài đăng cần xóa </param>
        /// <returns></returns>
        Task<ServiceResult> UserDeletePost(Guid myUid, Guid postId);
        Task<ServiceResult> AdminDeletePost(Guid postId);

        #endregion


        #region Search APIes

        /// <summary>
        /// User tim kiem bai dang feed
        /// </summary>
        /// <param name="myUid"> id user thuc hien </param>
        /// <param name="search"> tu khoa tim kiem </param>
        /// <param name="pagination"> thuoc tin phan trang </param>
        /// <returns></returns>
        /// Created PhucTV (19/5/24)
        /// Modified None
        Task<ServiceResult> UserSearchPost(Guid myUid, string? search, PaginationDto pagination);

        /// <summary>
        /// User tim kiem bai dang cua chinh minh
        /// </summary>
        /// <param name="myUid"> id user thuc hien </param>
        /// <param name="search"> tu khoa tim kiem </param>
        /// <param name="pagination"> thuoc tin phan trang </param>
        /// <returns></returns>
        /// Created PhucTV (19/5/24)
        /// Modified None
        Task<ServiceResult> UserSearchMyPost(Guid myUid, string? search, PaginationDto pagination);

        /// <summary>
        /// User tim kiem bai dang cua user khac
        /// </summary>
        /// <param name="myUid"> id user thuc hien </param>
        /// <param name="userId"> id user can lay </param>
        /// <param name="search"> tu khoa tim kiem </param>
        /// <param name="pagination"> thuoc tin phan trang </param>
        /// <returns></returns>
        /// Created PhucTV (19/5/24)
        /// Modified None
        Task<ServiceResult> UserSearchUserPost(Guid myUid, Guid userId, string? search, PaginationDto pagination);

        /// <summary>
        /// Admin tim kiem bai dang cua user khac
        /// </summary>
        /// <param name="userId"> id user can lay </param>
        /// <param name="search"> tu khoa tim kiem </param>
        /// <param name="pagination"> thuoc tin phan trang </param>
        /// <returns></returns>
        /// Created PhucTV (19/5/24)
        /// Modified None
        Task<ServiceResult> AdminSearchUserPost(Guid userId, string? search, PaginationDto pagination);

        #endregion
    }
}