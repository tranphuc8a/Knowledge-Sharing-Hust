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
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> Get(int? limit, int? offset)
        {
            string myUId = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await NotificationService.GetNotifications(Guid.Parse(myUId), limit, offset);
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
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> Get(Guid notiId)
        {
            string myUId = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await NotificationService.GetNotification(Guid.Parse(myUId), notiId);
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
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> Get([FromBody] Guid[] ids)
        {
            string myUId = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await NotificationService.GetNotifications(Guid.Parse(myUId), ids);
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
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> SetReadNotification(Guid notificationId)
        {
            string myUId = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await NotificationService.SetReadNotification(Guid.Parse(myUId), notificationId);
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
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> SetReadNotification([FromBody] Guid[] ids)
        {
            string myUId = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await NotificationService.SetReadNotifications(Guid.Parse(myUId), ids);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
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
            string myUId = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await NotificationService.SetReadNotifications(Guid.Parse(myUId));
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
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> DeleteNotification(Guid notificationId)
        {
            string myUId = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await NotificationService.DeleteNotification(Guid.Parse(myUId), notificationId);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
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
            string myUId = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await NotificationService.DeleteNotifications(Guid.Parse(myUId));
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
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> DeleteNotification([FromBody] Guid[] ids)
        {
            string myUId = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await NotificationService.GetNotifications(Guid.Parse(myUId), ids);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }
    }
}
