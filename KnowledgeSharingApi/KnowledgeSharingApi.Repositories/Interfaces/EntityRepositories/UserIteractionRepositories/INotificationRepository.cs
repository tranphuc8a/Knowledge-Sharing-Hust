using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Repositories.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.UserIteractionRepositories
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
        Task<int> DeleteAllNotification(Guid userId);

        /// <summary>
        /// Xóa danh sách thông báo của một người dùng
        /// </summary>
        /// <param name="userId"> id của người dùng muốn xóa </param>
        /// <param name="ids"> danh sách id của thông báo muốn xóa </param>
        /// <returns> Số bản ghi ảnh hưởng </returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<int> DeleteMultiNotificationByUserId(Guid userId, Guid[] ids);

        /// <summary>
        /// Lấy về toàn bộ thông báo của một userId
        /// </summary>
        /// <param name="userId"> id của người dùng muốn lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<List<Notification>> GetAllNotificationsByUserId(Guid userId);

        /// <summary>
        /// Lấy về danh sách thông báo của một user
        /// </summary>
        /// <param name="userId"> id của người dùng muốn lấy </param>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<List<Notification>> GetNotificationsByUserId(Guid userId, PaginationDto pagination);

        /// <summary>
        /// Cập nhật đã đọc danh sách thông báo
        /// </summary>
        /// <param name="userId"> id của người dùng muốn cập nhật </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<int> SetReadNotification(Guid userId);
        Task<int> SetReadNotification(Guid userId, Guid[] ids);
    }
}
