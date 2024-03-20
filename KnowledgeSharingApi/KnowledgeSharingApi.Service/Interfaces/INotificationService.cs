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
        Task<ServiceResult> GetNotification(string userId, string notificationId);
        Task<ServiceResult> GetNotifications(string userId, int? limit, int? offset);
        Task<ServiceResult> GetNotifications(string userId, string[] notiIds);


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
        Task<ServiceResult> SetReadNotification(string userId, string notiId);
        Task<ServiceResult> SetReadNotifications(string userId);
        Task<ServiceResult> SetReadNotifications(string userId, string[] notiIds);


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
        Task<ServiceResult> DeleteNotification(string userId, string notiId);
        Task<ServiceResult> DeleteNotifications(string userId);
        Task<ServiceResult> DeleteNotifications(string userId, string[] notiIds);

    }
}
