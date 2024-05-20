using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeSharingApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SocketsController(
        ILiveChatSocketService liveChatSocketService,
        INotificationSocketService notificationSocketService,
        INewMessageNotificationSocketService newMessageNotificationSocketService
        ) : BaseController
    {
        protected readonly ILiveChatSocketService liveChatSocketService = liveChatSocketService;
        protected readonly INotificationSocketService notificationSocketService = notificationSocketService;
        protected readonly INewMessageNotificationSocketService newMessageNotificationSocketService = newMessageNotificationSocketService;

        /// <summary>
        /// Xử lý yêu cầu kết nối tới socket live chat
        /// </summary>
        /// <param name="token"> token kết nối </param>
        /// <returns></returns>
        /// Created: PhucTV (13/3/24)
        /// Modified: None
        [HttpGet("live-chat-socket")]
        public virtual async Task<IActionResult> ConnectToLiveChatSocket(string? token)
        {
            ServiceResult res = await liveChatSocketService.ConnectSocket(HttpContext, token);
            return StatusCode(res);
        }


        /// <summary>
        /// Xử lý yêu cầu kết nối tới socket thông báo
        /// </summary>
        /// <param name="token"> token kết nối </param>
        /// <returns></returns>
        /// Created: PhucTV (13/3/24)
        /// Modified: None
        [HttpGet("notification-socket")]
        public virtual async Task<IActionResult> ConnectToNotificationSocket(string? token)
        {
            ServiceResult res = await notificationSocketService.ConnectSocket(HttpContext, token);
            return StatusCode(res);
        }


        /// <summary>
        /// Xử lý yêu cầu kết nối tới socket tin nhắn mới
        /// </summary>
        /// <param name="token"> token kết nối </param>
        /// <returns></returns>
        /// Created: PhucTV (13/3/24)
        /// Modified: None
        [HttpGet("new-message-notification-socket")]
        public virtual async Task<IActionResult> ConnectToNewMessageNotificationSocket(string? token)
        {
            ServiceResult res = await newMessageNotificationSocketService.ConnectSocket(HttpContext, token);
            return StatusCode(res);
        }
    }
}
