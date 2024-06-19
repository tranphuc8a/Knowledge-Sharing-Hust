using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Repositories.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.UserIteractionRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Lấy về user bằng email
        /// </summary>
        /// <param name="email"> email của user cần lấy </param>
        /// <returns> User </returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        Task<User?> GetByEmail(string email);


        /// <summary>
        /// Lấy về user bằng username
        /// </summary>
        /// <param name="username"> username của user cần lấy </param>
        /// <returns> User </returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        Task<User?> GetByUsername(string username);

        /// <summary>
        /// Lấy về danh sách chi tiết usser
        /// </summary>
        /// <returns> ViewUserProfile </returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        Task<List<ViewUserProfile>> GetDetail();

        /// <summary>
        /// Lấy về danh sách chi tiết user có phân trang
        /// </summary>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <returns> ViewUserProfile </returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        Task<List<ViewUserProfile>> GetDetail(PaginationDto pagination);

        /// <summary>
        /// Lấy về danh sách chi tiết user theo danh sách id
        /// </summary>
        /// <param name="userIds"> danh sách id user cần lấy </param>
        /// <returns> ViewUserProfile </returns>
        /// Created: PhucTV (27/3/24)
        /// Modified: None
        Task<Dictionary<Guid, ViewUserProfile?>> GetDetail(Guid[] userIds);

        /// <summary>
        /// Lấy về chi tiết user
        /// </summary>
        /// <param name="userId"> userId của user cần lấy </param>
        /// <returns> ViewUserProfile </returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        Task<ViewUserProfile?> GetDetail(Guid userId);

        /// <summary>
        /// Lấy về chi tiết user bằng email
        /// </summary>
        /// <param name="email"> email của user cần lấy </param>
        /// <returns> ViewUserProfile </returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        Task<ViewUserProfile?> GetDetailByEmail(string email);


        /// <summary>
        /// Lấy về chi tiết user bằng username
        /// </summary>
        /// <param name="username"> username của user cần lấy </param>
        /// <returns> ViewUserProfile </returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        Task<ViewUserProfile?> GetDetailByUsername(string username);

        /// <summary>
        /// Lấy về chi tiết user bằng username hoặc userId
        /// </summary>
        /// <param name="unOruid"> unOruid của user cần lấy </param>
        /// <returns> ViewUserProfile </returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        Task<ViewUserProfile?> GetDetailByUsernameOrUserId(string unOruid);

        /// <summary>
        /// Lấy về danh sách chi tiết usser
        /// </summary>
        /// <returns> T </returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        Task<List<T>> GetDetail<T>(Expression<Func<ViewUserProfile, T>> projector);

        /// <summary>
        /// Lấy về danh sách chi tiết user có phân trang
        /// </summary>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <returns> T </returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        Task<List<T>> GetDetail<T>(PaginationDto pagination, Expression<Func<ViewUserProfile, T>> projector);

        /// <summary>
        /// Lấy về danh sách chi tiết user theo danh sách id
        /// </summary>
        /// <param name="userIds"> danh sách id user cần lấy </param>
        /// <returns> T </returns>
        /// Created: PhucTV (27/3/24)
        /// Modified: None
        Task<Dictionary<Guid, T?>> GetDetail<T>(Guid[] userIds, Expression<Func<ViewUserProfile, T>> projector);

        /// <summary>
        /// Lấy về chi tiết user
        /// </summary>
        /// <param name="userId"> userId của user cần lấy </param>
        /// <returns> T </returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        Task<T?> GetDetail<T>(Guid userId, Expression<Func<ViewUserProfile, T>> projector);

        /// <summary>
        /// Lấy về chi tiết user bằng email
        /// </summary>
        /// <param name="email"> email của user cần lấy </param>
        /// <returns> T </returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        Task<T?> GetDetailByEmail<T>(string email, Expression<Func<ViewUserProfile, T>> projector);


        /// <summary>
        /// Lấy về chi tiết user bằng username
        /// </summary>
        /// <param name="username"> username của user cần lấy </param>
        /// <returns> T </returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        Task<T?> GetDetailByUsername<T>(string username, Expression<Func<ViewUserProfile, T>> projector);

        /// <summary>
        /// Lấy về chi tiết user bằng username hoặc userId
        /// </summary>
        /// <param name="unOruid"> unOruid của user cần lấy </param>
        /// <returns> T </returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        Task<T?> GetDetailByUsernameOrUserId<T>(string unOruid, Expression<Func<ViewUserProfile, T>> projector);

        /// <summary>
        /// Kiểm tra xem mật khẩu đúng hay sai
        /// </summary>
        /// <param name="username"> username cần kiểm tra </param>
        /// <param name="password"> mật khẩu cần kiểm tra </param>
        /// <returns> true - đúng, false - sai </returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        Task<bool> CheckPassword(string username, string password);

        /// <summary>
        /// Kiểm tra xem mật khẩu đúng hay sai
        /// </summary>
        /// <param name="username"> username cần kiểm tra </param>
        /// <param name="password"> mật khẩu cần kiểm tra </param>
        /// <param name="hashPassword"> mật khẩu da bi bam </param>
        /// <returns> true - đúng, false - sai </returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        Task<bool> CheckPassword(string username, string password, string hashPassword);

        /// <summary>
        /// Cập nhật mật khẩu cho user
        /// </summary>
        /// <param name="username"> username cần update </param>
        /// <param name="newPassword"> mật khẩu mới </param>
        /// <returns> số bản ghi ảnh hưởng </returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        Task<int> UpdatePassword(string username, string newPassword);


        /// <summary>
        /// Kiểm tra user có tồn tại hay không
        /// Throw Not existed enitty Exception nếu không tồn tại
        /// </summary>
        /// <param name="userId"> id của user cần kiểm tra </param>
        /// <returns> Trả về view user tương ứng </returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<ViewUserProfile> CheckExistedUser(Guid userId, string errorMessage);

        /// <summary>
        /// Dang ky moi mot user
        /// </summary>
        /// <param name="userId"> id của user cần them moi </param>
        /// <param name="user"> thong tin user them moi </param>
        /// <param name="password"> mat khau </param>
        /// <param name="fullName"> Ten cua user dang ky </param>
        /// <param name="avatar"> Avatar cua user dang ky </param>
        /// <returns> Trả về id user tương ứng hoac null neu that bai </returns>
        /// Created: PhucTV (10/4/24)
        /// Modified: None
        Task<Guid?> RegisterUser(Guid userId, User user, string password, string fullName, string? avatar = null);
    }
}
