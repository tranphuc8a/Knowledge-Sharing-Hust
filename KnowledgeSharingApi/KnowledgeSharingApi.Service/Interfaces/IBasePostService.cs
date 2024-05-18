using KnowledgeSharingApi.Domains.Models.ApiRequestModels.CreateUserItemModels;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.UpdateUserItemModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Interfaces
{
    public interface IBasePostService : IPostService
    {

        #region Anonymous Apies

        /// <summary>
        /// Anonymous Lấy về chi tiết bài đăng
        /// </summary>
        /// <param name="postId"> id của bài đăng cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<ServiceResult> AnonymousGetPostDetail(Guid postId);

        #endregion

        #region Admin Apies

        /// <summary>
        /// Admin Lấy về chi tiết bài đăng
        /// </summary>
        /// <param name="postId"> id của bài đăng cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<ServiceResult> AdminGetPostDetail(Guid postId);

        /// <summary>
        /// Admin lấy về danh sách bài đăng của một khóa học cụ thể
        /// </summary>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<ServiceResult> AdminGetListPostsOfCourse(Guid courseId, PaginationDto pagination);

        #endregion

        #region User Apies

        /// <summary>
        /// User Lấy về chi tiết bài đăng
        /// </summary>
        /// <param name="myUid"> id của chính mình </param>
        /// <param name="postId"> id của bài đăng cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetPostDetail(Guid myUid, Guid postId);

        /// <summary>
        /// User Lấy về chi tiết bài đăng của chính mình
        /// </summary>
        /// <param name="myUid"> id của chính mình </param>
        /// <param name="postId"> id của bài đăng cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetMyPostDetail(Guid myUid, Guid postId);

        /// <summary>
        /// User lấy về danh sách bài đăng của một khóa học cụ thể
        /// </summary>
        /// <param name="myUid"> id user của chính mình </param>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetListPostsOfCourse(Guid myUid, Guid courseId, PaginationDto pagination);


        /// <summary>
        /// User lấy về danh sách bài đăng của một khóa học cụ thể của mình
        /// </summary>
        /// <param name="myUid"> id user của chính mình </param>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetListPostsOfMyCourse(Guid myUid, Guid courseId, PaginationDto pagination);



        /// <summary>
        /// User lấy tạo mới một bài đăng của chính mình (không trong khóa học nào)
        /// </summary>
        /// <param name="myUid"> id user của chính mình </param>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<ServiceResult> UserCreatePost(Guid myUid, CreatePostModel model);
        


        /// <summary>
        /// User lấy về danh sách bài đăng của một khóa học cụ thể
        /// </summary>
        /// <param name="myUid"> id user của chính mình </param>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<ServiceResult> UserUpdatePost(Guid myUid, Guid postId, UpdatePostModel model);

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