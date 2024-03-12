using KnowledgeSharingApi.Domains.Models.Entities;
using KnowledgeSharingApi.Infrastructures.Interfaces.Encrypts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Lấy về user bằng email
        /// </summary>
        /// <param name="email"> email của user cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        Task<User?> GetByEmail(string email);


        /// <summary>
        /// Lấy về user bằng username
        /// </summary>
        /// <param name="username"> username của user cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        Task<User?> GetByUsername(string username);
        
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
        /// <returns></returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        Task<int> UpdatePassword(string username, string newPassword);
    }
}
