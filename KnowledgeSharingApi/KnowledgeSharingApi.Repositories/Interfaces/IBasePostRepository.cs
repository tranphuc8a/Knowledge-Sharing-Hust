using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Repositories.Interfaces.Repositories
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
        /// Lấy về danh sách View Post cho mot userId cu the
        /// </summary>
        /// <param name="userId"> id user thuc hien </param>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <returns> Danh sách bài đăng </returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<List<ReturnType>> GetViewPost(Guid userId, PaginationDto pagination);

        /// <summary>
        /// Lấy về danh sách bài đăng của một user
        /// </summary>
        /// <param name="userId"> Id của user cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<List<ReturnType>> GetByUserId(Guid userId);

        /// <summary>
        /// Lấy về danh sách bài đăng của một user
        /// </summary>
        /// <param name="userId"> Id của user cần lấy </param>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<List<ReturnType>> GetByUserId(Guid userId, PaginationDto pagination);

        /// <summary>
        /// Lấy về danh sách bài đăng public
        /// </summary>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <returns> Danh sách bài đăng </returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<List<ReturnType>> GetPublicPosts(PaginationDto pagination);
        Task<List<ReturnType>> GetPublicPosts();  // Get All - Not pagination

        /// <summary>
        /// Lấy về danh sách bài đăng public của một user
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <returns> Danh sách bài đăng </returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<List<ReturnType>> GetPublicPostsByUserId(Guid userId);

        /// <summary>
        /// Lấy về danh sách bài đăng public của một user
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <returns> Danh sách bài đăng </returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<List<ReturnType>> GetPublicPostsByUserId(Guid userId, PaginationDto pagination);


        /// <summary>
        /// Lấy về danh sách bài đăng của một category
        /// Bai dang public - chi lay nhung post public
        /// Bai dang binh thuong - lay moi bai dang (cho admin)
        /// Bai dang filtered - Lay bai dang public va bai dang xem duoc (cho user)
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

        /// <summary>
        /// Lấy về danh sách bài đăng đã được mark của một user
        /// </summary>
        /// <param name="userId"> id của user cần lấy  </param>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <returns> Danh sách bài đăng </returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<List<ReturnType>> GetMarkedPosts(Guid userId, PaginationDto pagination);


        /// <summary>
        /// Lấy về danh sách View Post
        /// </summary>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <returns> Danh sách bài đăng </returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<List<T>> GetViewPost<T>(PaginationDto pagination, Expression<Func<ReturnType, T>> projector);

        /// <summary>
        /// Lấy về danh sách View Post cho mot userId cu the
        /// </summary>
        /// <param name="userId"> id user thuc hien </param>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <returns> Danh sách bài đăng </returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<List<T>> GetViewPost<T>(Guid userId, PaginationDto pagination, Expression<Func<ReturnType, T>> projector);

        /// <summary>
        /// Lấy về danh sách bài đăng của một user
        /// </summary>
        /// <param name="userId"> Id của user cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<List<T>> GetByUserId<T>(Guid userId, Expression<Func<ReturnType, T>> projector);

        /// <summary>
        /// Lấy về danh sách bài đăng của một user
        /// </summary>
        /// <param name="userId"> Id của user cần lấy </param>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<List<T>> GetByUserId<T>(Guid userId, PaginationDto pagination, Expression<Func<ReturnType, T>> projector);

        /// <summary>
        /// Lấy về danh sách bài đăng public
        /// </summary>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <returns> Danh sách bài đăng </returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<List<T>> GetPublicPosts<T>(PaginationDto pagination, Expression<Func<ReturnType, T>> projector);
        Task<List<T>> GetPublicPosts<T>(Expression<Func<ReturnType, T>> projector);  // Get All - Not pagination

        /// <summary>
        /// Lấy về danh sách bài đăng public của một user
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <returns> Danh sách bài đăng </returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<List<T>> GetPublicPostsByUserId<T>(Guid userId, Expression<Func<ReturnType, T>> projector);

        /// <summary>
        /// Lấy về danh sách bài đăng public của một user
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <returns> Danh sách bài đăng </returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<List<T>> GetPublicPostsByUserId<T>(Guid userId, PaginationDto pagination, Expression<Func<ReturnType, T>> projector);


        /// <summary>
        /// Lấy về danh sách bài đăng của một category
        /// Bai dang public - chi lay nhung post public
        /// Bai dang binh thuong - lay moi bai dang (cho admin)
        /// Bai dang filtered - Lay bai dang public va bai dang xem duoc (cho user)
        /// </summary>
        /// <param name="catName"> Tên category </param>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <returns> Danh sách bài đăng </returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<List<T>> GetPublicPostsOfCategory<T>(string catName, PaginationDto pagination, Expression<Func<ReturnType, T>> projector);
        Task<List<T>> GetPostsOfCategory<T>(string catName, PaginationDto pagination, Expression<Func<ReturnType, T>> projector);
        Task<List<T>> GetPostsOfCategory<T>(Guid myUId, string catName, PaginationDto pagination, Expression<Func<ReturnType, T>> projector);


        /// <summary>
        /// Lấy về danh sách bài đăng đã được mark của một user
        /// </summary>
        /// <param name="userId"> id của user cần lấy  </param>
        /// <returns> Danh sách bài đăng </returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<List<T>> GetMarkedPosts<T>(Guid userId, Expression<Func<ReturnType, T>> projector);

        /// <summary>
        /// Lấy về danh sách bài đăng đã được mark của một user
        /// </summary>
        /// <param name="userId"> id của user cần lấy  </param>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <returns> Danh sách bài đăng </returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<List<T>> GetMarkedPosts<T>(Guid userId, PaginationDto pagination, Expression<Func<ReturnType, T>> projector);
    }
}
