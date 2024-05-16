using KnowledgeSharingApi.Domains.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Interfaces
{
    public interface INotificationService
    {
        /// <summary>
        /// Thực hiện lấy về thông báo: 
        /// + Lấy về qua id thông báo
        /// + Lấy về danh sách phân trang
        /// + Lấy về danh sách theo mảng id
        /// </summary>
        /// <param name="userId"> Id của người dùng muốn lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<ServiceResult> GetNotification(Guid userId, Guid notificationId);
        Task<ServiceResult> GetNotifications(Guid userId, PaginationDto page);
        Task<ServiceResult> GetNotifications(Guid userId, Guid[] notiIds);


        /// <summary>
        /// Thực hiện đánh dấu đã đọc thông báo: 
        /// + đã đọc thông báo qua id thông báo
        /// + đã đọc tất cả thông báo
        /// + đã đọc danh sách theo mảng id
        /// </summary>
        /// <param name="userId"> Id của người dùng thực hiện </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<ServiceResult> SetReadNotification(Guid userId, Guid notiId);
        Task<ServiceResult> SetReadNotifications(Guid userId);
        Task<ServiceResult> SetReadNotifications(Guid userId, Guid[] notiIds);


        /// <summary>
        /// Thực hiện xóa thông báo: 
        /// + xóa thông báo qua id thông báo
        /// + xóa tất cả thông báo
        /// + xóa danh sách theo mảng id
        /// </summary>
        /// <param name="userId"> Id của người dùng muốn lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<ServiceResult> DeleteNotification(Guid userId, Guid notiId);
        Task<ServiceResult> DeleteNotifications(Guid userId);
        Task<ServiceResult> DeleteNotifications(Guid userId, Guid[] notiIds);

    }
}
