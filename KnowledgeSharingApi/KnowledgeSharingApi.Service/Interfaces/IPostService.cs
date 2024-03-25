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
        /// <param name="limit"> Số lượng bài post cần lấy </param>
        /// <param name="offset"> Độ lệch bản ghi đầu </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetPosts(Guid myUid, int? limit, int? offset);
        Task<ServiceResult> AnonymousGetPosts(int? limit, int? offset);
        Task<ServiceResult> AdminGetPosts(int? limit, int? offset);


        /// <summary>
        /// Lấy về danh sách các bài post của mình
        /// </summary>
        /// <param name="myUid"> Id của chính mình </param>
        /// <param name="limit"> Số lượng bài post cần lấy </param>
        /// <param name="offset"> Độ lệch bản ghi đầu </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetMyPosts(Guid myUid, int? limit, int? offset);


        /// <summary>
        /// Nhóm API lấy về danh sách bài posts của user khác
        /// </summary>
        /// <param name="myUid"> Id của chính mình </param>
        /// <param name="userId"> Id của user muốn lấy post </param>
        /// <param name="limit"> Số lượng bài post cần lấy </param>
        /// <param name="offset"> Độ lệch bản ghi đầu </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetUserPosts(Guid myUid, Guid userId, int? limit, int? offset);
        Task<ServiceResult> AnonymousGetUserPosts(Guid userId, int? limit, int? offset);
        Task<ServiceResult> AdminGetUserPosts(Guid userId, int? limit, int? offset);


        /// <summary>
        /// Nhóm API lấy về danh sách bài đăng công khai có gắn thẻ theo một category name
        /// </summary>
        /// <param name="limit"> Số lượng bài đăng cần lấy </param>
        /// <param name="offset"> Độ lệch bài đăng đầu tiên </param>
        /// <param name="catName"> Tên của category cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<ServiceResult> AnonymousGetListPostsOfCategory(string catName, int? limit, int? offset);
        Task<ServiceResult> UserGetListPostsOfCategory(Guid myUid, string catName, int? limit, int? offset);
        Task<ServiceResult> AdminGetListPostsOfCategory(string catName, int? limit, int? offset);

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
    }
}
