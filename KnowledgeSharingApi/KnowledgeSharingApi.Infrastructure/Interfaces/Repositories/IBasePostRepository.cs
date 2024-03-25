using KnowledgeSharingApi.Domains.Models.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.Repositories
{
    public interface IBasePostRepository<ReturnType> where ReturnType: class
    {
        /// <summary>
        /// Lấy về danh sách View Post
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<IEnumerable<ReturnType>> GetViewPost(int limit, int offset);

        /// <summary>
        /// Lấy về danh sách bài đăng của một user
        /// </summary>
        /// <param name="userId"> Id của user cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<IEnumerable<ReturnType>> GetByUserId(Guid userId);

        /// <summary>
        /// Lấy về danh sách bài đăng public
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<IEnumerable<ReturnType>> GetPublicPosts(int limit, int offset);

        /// <summary>
        /// Lấy về danh sách bài đăng public của một user
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<IEnumerable<ReturnType>> GetPublicPostsByUserId(Guid userId);


        /// <summary>
        /// Lấy về danh sách bài đăng của một category
        /// </summary>
        /// <param name="catName"> Tên category </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<IEnumerable<ReturnType>> GetPublicPostsOfCategory(string catName, int limit, int offset);
        Task<IEnumerable<ReturnType>> GetPostsOfCategory(string catName, int limit, int offset);
        Task<IEnumerable<ReturnType>> GetPostsOfCategory(Guid myUId, string catName, int limit, int offset);
    }
}
