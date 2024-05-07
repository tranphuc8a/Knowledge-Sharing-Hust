using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Entities;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.Encrypts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories
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
        /// <returns> ViewUser </returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        Task<IEnumerable<ViewUser>> GetDetail();

        /// <summary>
        /// Lấy về danh sách chi tiết user có phân trang
        /// </summary>
        /// <param name="limit"> thuộc tính phân trang - số bản ghi cần lấy </param>
        /// <param name="offset"> thuộc tính phân trang - độ lệch bản ghi </param>
        /// <returns> ViewUser </returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        Task<IEnumerable<ViewUser>> GetDetail(int limit, int offset);

        /// <summary>
        /// Lấy về danh sách chi tiết user theo danh sách id
        /// </summary>
        /// <param name="userIds"> danh sách id user cần lấy </param>
        /// <returns> ViewUser </returns>
        /// Created: PhucTV (27/3/24)
        /// Modified: None
        Task<Dictionary<Guid, ViewUser?>> GetDetail(Guid[] userIds);

        /// <summary>
        /// Lấy về chi tiết user
        /// </summary>
        /// <param name="userId"> userId của user cần lấy </param>
        /// <returns> ViewUser </returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        Task<ViewUser?> GetDetail(Guid userId);

        /// <summary>
        /// Lấy về chi tiết user bằng email
        /// </summary>
        /// <param name="email"> email của user cần lấy </param>
        /// <returns> ViewUser </returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        Task<ViewUser?> GetDetailByEmail(string email);


        /// <summary>
        /// Lấy về chi tiết user bằng username
        /// </summary>
        /// <param name="username"> username của user cần lấy </param>
        /// <returns> ViewUser </returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        Task<ViewUser?> GetDetailByUsername(string username);

        /// <summary>
        /// Lấy về chi tiết user bằng username hoặc userId
        /// </summary>
        /// <param name="unOruid"> unOruid của user cần lấy </param>
        /// <returns> ViewUser </returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        Task<ViewUser?> GetDetailByUsernameOrUserId(string unOruid);

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
        Task<ViewUser> CheckExistedUser(Guid userId, string errorMessage);

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
