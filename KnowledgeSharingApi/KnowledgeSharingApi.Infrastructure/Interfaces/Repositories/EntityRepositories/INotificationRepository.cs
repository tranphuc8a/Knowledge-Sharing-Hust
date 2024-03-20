using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories
{
    public interface INotificationRepository : IRepository<Notification>
    {
        /// <summary>
        /// Xóa toàn bộ thông báo của một người dùng
        /// </summary>
        /// <param name="userId"> id của người dùng muốn xóa </param>
        /// <returns> Số bản ghi ảnh hưởng </returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<int> DeleteAllNotification(string userId);

        /// <summary>
        /// Xóa danh sách thông báo của một người dùng
        /// </summary>
        /// <param name="userId"> id của người dùng muốn xóa </param>
        /// <param name="ids"> danh sách id của thông báo muốn xóa </param>
        /// <returns> Số bản ghi ảnh hưởng </returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<int> DeleteMultiNotificationByUserId(string userId, string[] ids);

        /// <summary>
        /// Lấy về toàn bộ thông báo của một userId
        /// </summary>
        /// <param name="userId"> id của người dùng muốn lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<IEnumerable<Notification>> GetAllNotificationsByUserId(string userId);

        /// <summary>
        /// Lấy về danh sách thông báo của một user
        /// </summary>
        /// <param name="userId"> id của người dùng muốn lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<IEnumerable<Notification>> GetNotificationsByUserId(string userId, int limit, int offset);

        /// <summary>
        /// Cập nhật đã đọc danh sách thông báo
        /// </summary>
        /// <param name="userId"> id của người dùng muốn cập nhật </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<int> SetReadNotification(string userId);
        Task<int> SetReadNotification(string userId, string[] ids);
    }
}
