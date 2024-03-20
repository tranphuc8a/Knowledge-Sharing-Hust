using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeSharingApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class NotificationsController(
        INotificationService notificationService    
    ) : ControllerBase
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
        public async Task<IActionResult> Get(string userId, int? limit, int? offset)
        {
            ServiceResult res = await NotificationService.GetNotifications(userId, limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Lấy về một thông báo
        /// </summary>
        /// <param name="notiId"> id thông báo cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("{notiId}")]
        public async Task<IActionResult> Get(string userId, string notiId)
        {
            ServiceResult res = await NotificationService.GetNotification(userId, notiId);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Lấy về tất cả danh sách thông báo của người dùng
        /// </summary>
        /// <param name="ids"> Danh sách id của thông báo cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("list")]
        public async Task<IActionResult> Get(string userId, [FromBody] string[] ids)
        {
            ServiceResult res = await NotificationService.GetNotifications(userId, ids);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Đánh dấu đã đọc thông báo
        /// </summary>
        /// <param name="notificationId"> Id của thông báo được đánh dấu </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpPost("set-read-notification/{notificationId}")]
        public async Task<IActionResult> SetReadNotification(string userId, string notificationId)
        {
            ServiceResult res = await NotificationService.SetReadNotification(userId, notificationId);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Đánh dấu đã đọc danh sách thông báo
        /// </summary>
        /// <param name="ids"> Danh sách id của thông báo cần đánh dấu </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpPost("set-read-notification")]
        public async Task<IActionResult> SetReadNotification(string userId, [FromBody] string[] ids)
        {
            ServiceResult res = await NotificationService.SetReadNotifications(userId, ids);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Đánh dấu đã đọc tất cả thông báo
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpPost("set-read-all-notification")]
        public async Task<IActionResult> SetReadNotification(string userId)
        {
            ServiceResult res = await NotificationService.SetReadNotifications(userId);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Xóa thông báo qua id
        /// </summary>
        /// <param name="notificationId"> Id của thông báo cần đánh dấu </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpDelete("{notificationId}")]
        public async Task<IActionResult> DeleteNotification(string userId, string notificationId)
        {
            ServiceResult res = await NotificationService.DeleteNotification(userId, notificationId);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Xóa đi toàn bộ thông báo của người dùng
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpDelete("all")]
        public async Task<IActionResult> DeleteNotification(string userId)
        {
            ServiceResult res = await NotificationService.DeleteNotifications(userId);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Xóa đi danh sách noti theo danh sách id
        /// </summary>
        /// <param name="ids"> Danh sách id của thông báo cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpDelete("list")]
        public async Task<IActionResult> DeleteNotification(string userId, [FromBody] string[] ids)
        {
            ServiceResult res = await NotificationService.GetNotifications(userId, ids);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }
    }
}
