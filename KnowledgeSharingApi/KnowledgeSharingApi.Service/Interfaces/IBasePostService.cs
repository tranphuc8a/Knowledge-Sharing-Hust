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
        Task<ServiceResult> AnonymousGetPostDetail(string postId);

        #endregion

        #region Admin Apies

        /// <summary>
        /// Admin Lấy về chi tiết bài đăng
        /// </summary>
        /// <param name="postId"> id của bài đăng cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<ServiceResult> AdminGetPostDetail(string postId);

        /// <summary>
        /// Admin lấy về danh sách bài đăng của một khóa học cụ thể
        /// </summary>
        /// <param name="limit"> Số lượng bài đăng cần lấy </param>
        /// <param name="offset"> Độ lệch bài đăng đầu tiên </param>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<ServiceResult> AdminGetListPostsOfCourse(string courseId, int? limit, int? offset);

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
        Task<ServiceResult> UserGetPostDetail(string myUid, string postId);

        /// <summary>
        /// User Lấy về chi tiết bài đăng của chính mình
        /// </summary>
        /// <param name="myUid"> id của chính mình </param>
        /// <param name="postId"> id của bài đăng cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetMyPostDetail(string myUid, string postId);

        /// <summary>
        /// User lấy về danh sách bài đăng của một khóa học cụ thể
        /// </summary>
        /// <param name="myUid"> id user của chính mình </param>
        /// <param name="limit"> Số lượng bài đăng cần lấy </param>
        /// <param name="offset"> Độ lệch bài đăng đầu tiên </param>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetListPostsOfCourse(string myUid, string courseId, int? limit, int? offset);


        /// <summary>
        /// User lấy tạo mới một bài đăng của chính mình (không trong khóa học nào)
        /// </summary>
        /// <param name="myUid"> id user của chính mình </param>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<ServiceResult> UserCreatePost(string myUid, CreatePostModel model);
        


        /// <summary>
        /// User lấy về danh sách bài đăng của một khóa học cụ thể
        /// </summary>
        /// <param name="myUid"> id user của chính mình </param>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<ServiceResult> UserUpdatePost(string myUid, string postId, UpdatePostModel model);

        #endregion
    }
}