using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Infrastructures.Encrypts;
using KnowledgeSharingApi.Services.Filters;
using KnowledgeSharingApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KnowledgeSharingApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class NotificationsController(
        INotificationService notificationService    
    ) : BaseController
    {
        protected readonly INotificationService NotificationService = notificationService;

        /// <summary>
        /// Lấy về tất cả danh sách thông báo của người dùng
        /// </summary>
        /// <param name="limit"> Số lượng thông báo cần lấy </param>
        /// <param name="offset"> Dộ lệch thông báo </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> Get(int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await NotificationService.GetNotifications(GetCurrentUserIdStrictly(), pagination);
            return StatusCode(res);
        }

        /// <summary>
        /// Lấy về một thông báo
        /// </summary>
        /// <param name="notiId"> id thông báo cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("{notiId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> Get(Guid notiId)
        {
            ServiceResult res = await NotificationService.GetNotification(GetCurrentUserIdStrictly(), notiId);
            return StatusCode(res);
        }

        /// <summary>
        /// Lấy về tất cả danh sách thông báo của người dùng
        /// </summary>
        /// <param name="ids"> Danh sách id của thông báo cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("list")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> Get([FromBody] Guid[] ids)
        {
            ServiceResult res = await NotificationService.GetNotifications(GetCurrentUserIdStrictly(), ids);
            return StatusCode(res);
        }

        /// <summary>
        /// Đánh dấu đã đọc thông báo
        /// </summary>
        /// <param name="notificationId"> Id của thông báo được đánh dấu </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpPost("set-read-notification/{notificationId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> SetReadNotification(Guid notificationId)
        {
            ServiceResult res = await NotificationService.SetReadNotification(GetCurrentUserIdStrictly(), notificationId);
            return StatusCode(res);
        }

        /// <summary>
        /// Đánh dấu đã đọc danh sách thông báo
        /// </summary>
        /// <param name="ids"> Danh sách id của thông báo cần đánh dấu </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpPost("set-read-notification")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> SetReadNotification([FromBody] Guid[] ids)
        {
            ServiceResult res = await NotificationService.SetReadNotifications(GetCurrentUserIdStrictly(), ids);
            return StatusCode(res);
        }

        /// <summary>
        /// Đánh dấu đã đọc tất cả thông báo
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpPost("set-read-all-notification")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> SetReadNotification()
        {
            ServiceResult res = await NotificationService.SetReadNotifications(GetCurrentUserIdStrictly());
            return StatusCode(res);
        }

        /// <summary>
        /// Xóa thông báo qua id
        /// </summary>
        /// <param name="notificationId"> Id của thông báo cần đánh dấu </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpDelete("{notificationId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> DeleteNotification(Guid notificationId)
        {
            ServiceResult res = await NotificationService.DeleteNotification(GetCurrentUserIdStrictly(), notificationId);
            return StatusCode(res);
        }

        /// <summary>
        /// Xóa đi toàn bộ thông báo của người dùng
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpDelete("all")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> DeleteNotification()
        {
            ServiceResult res = await NotificationService.DeleteNotifications(GetCurrentUserIdStrictly());
            return StatusCode(res);
        }

        /// <summary>
        /// Xóa đi danh sách noti theo danh sách id
        /// </summary>
        /// <param name="ids"> Danh sách id của thông báo cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpDelete("list")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> DeleteNotification([FromBody] Guid[] ids)
        {
            ServiceResult res = await NotificationService.GetNotifications(GetCurrentUserIdStrictly(), ids);
            return StatusCode(res);
        }
    }
}
