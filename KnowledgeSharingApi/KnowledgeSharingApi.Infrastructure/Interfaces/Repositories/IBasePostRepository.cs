using KnowledgeSharingApi.Domains.Models.Dtos;
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
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <returns> Danh sách bài đăng </returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<List<ReturnType>> GetViewPost(PaginationDto pagination);

        /// <summary>
        /// Lấy về danh sách bài đăng của một user
        /// </summary>
        /// <param name="userId"> Id của user cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<List<ReturnType>> GetByUserId(Guid userId);

        /// <summary>
        /// Lấy về danh sách bài đăng public
        /// </summary>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <returns> Danh sách bài đăng </returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<List<ReturnType>> GetPublicPosts(PaginationDto pagination);

        /// <summary>
        /// Lấy về danh sách bài đăng public của một user
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <returns> Danh sách bài đăng </returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<List<ReturnType>> GetPublicPostsByUserId(Guid userId);


        /// <summary>
        /// Lấy về danh sách bài đăng của một category
        /// </summary>
        /// <param name="catName"> Tên category </param>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <returns> Danh sách bài đăng </returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<List<ReturnType>> GetPublicPostsOfCategory(string catName, PaginationDto pagination);
        Task<List<ReturnType>> GetPostsOfCategory(string catName, PaginationDto pagination);
        Task<List<ReturnType>> GetPostsOfCategory(Guid myUId, string catName, PaginationDto pagination);


        /// <summary>
        /// Lấy về danh sách bài đăng đã được mark của một user
        /// </summary>
        /// <param name="userId"> id của user cần lấy  </param>
        /// <returns> Danh sách bài đăng </returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<List<ReturnType>> GetMarkedPosts(Guid userId);
    }
}
