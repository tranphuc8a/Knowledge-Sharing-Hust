using KnowledgeSharingApi.Domains.Models.ApiRequestModels.ConversationModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Infrastructures.Encrypts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Services.Filters;
using KnowledgeSharingApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KnowledgeSharingApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ConversationsController(
        IConversationService conversationService    
    ) : BaseController
    {
        protected readonly IConversationService ConversationService = conversationService;

        /// <summary>
        /// Xử lý yêu cầu lấy về chi tiết cuộc trò chuyện
        /// </summary>
        /// <param name="conversationId"> id của cuộc trò chuyện cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("my/{conversationId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> GetConversation(Guid conversationId)
        {
            ServiceResult res = await ConversationService.GetConversation(GetCurrentUserIdStrictly(), conversationId);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu lấy về danh sách cuộc trò chuyện
        /// </summary>
        /// <param name="limit"> Số lượng cuộc trò chuyện muốn lấy </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("my")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> GetListConversations(int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await ConversationService.GetConversations(GetCurrentUserIdStrictly(), pagination);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu lấy về cuộc trò chuyện với người kia
        /// </summary>
        /// <param name="userId"> Id của người kia </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("with/{userId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> GetConversationWith(Guid userId)
        {
            ServiceResult res = await ConversationService.GetConversationWith(GetCurrentUserIdStrictly(), userId);
            return StatusCode(res);
        }

        /// <summary>
        /// Bắt đầu cuộc trò chuyện với người khác
        /// </summary>
        /// <param name="userId"> Id của người kia </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpPost("with/{userId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> StartConversationWith(Guid userId)
        {
            ServiceResult res = await ConversationService.StartConversation(GetCurrentUserIdStrictly(), userId);
            return StatusCode(res);
        }


        /// <summary>
        /// Xử lý yêu cầu xóa cuộc trò chuyện
        /// </summary>
        /// <param name="conversationId"> Id của cuộc trò chuyện </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpDelete("{conversationId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> DeleteConversation(Guid conversationId)
        {
            ServiceResult res = await ConversationService.DeleteConversation(GetCurrentUserIdStrictly(), conversationId);
            return StatusCode(res);
        }


        /// <summary>
        /// Xử lý yêu cầu cập nhật thông tin cuộc trò chuyện
        /// </summary>
        /// <param name="model"> Thông tin cuộc trò chuyện cần cập nhật </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpPatch]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UpdateConversation([FromBody] UpdateConversationModel model)
        {
            ServiceResult res = await ConversationService.UpdateConversation(GetCurrentUserIdStrictly(), model);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu cập nhật thông tin người tham gia cuộc trò chuyện
        /// </summary>
        /// <param name="model"> Thông tin cần cập nhật </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpPatch("participant")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UpdateParticipant([FromBody] UpdateUserConversationModel model)
        {
            ServiceResult res = await ConversationService.UpdateUserConversation(GetCurrentUserIdStrictly(), model);
            return StatusCode(res);
        }

        /// <summary>
        /// Đánh dấu đã đọc cuộc trò chuyện
        /// </summary>
        /// <param name="conversationId"> Id cuộc trò chuyện cần đánh dấu đã đọc </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpPost("set-read/{conversationId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> SetReadConversation(Guid conversationId)
        {
            ServiceResult res = await ConversationService.SetReadConversation(GetCurrentUserIdStrictly(), conversationId);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu lấy về danh sách tin nhắn trong cuộc trò chuyện
        /// </summary>
        /// <param name="conversationId"> Id cuộc trò chuyện cần lấy </param>
        /// <param name="limit"> Số lượng tin nhắn </param>
        /// <param name="offset"> Độ lệch tin nhắn đầu </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("messages/{conversationId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> GetMessages(Guid conversationId, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await ConversationService.GetMessages(GetCurrentUserIdStrictly(), conversationId, pagination);
            return StatusCode(res);
        }


        /// <summary>
        /// Xử lý yêu cầu tạo mới một tin nhắn
        /// </summary>
        /// <param name="model"> Tin nhắn cần tạo mới </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpPost("message")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> PostMessage(SendMessageModel model)
        {
            ServiceResult res = await ConversationService.SendMessage(GetCurrentUserIdStrictly(), model);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu gửi mới tin nhắn
        /// </summary>
        /// <param name="model"> Tin nhắn cần gửi </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpPost("send-message")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> SendMessage(SendMessageModel model)
        {
            ServiceResult res = await ConversationService.SendMessage(GetCurrentUserIdStrictly(), model);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu xóa một tin nhắn
        /// </summary>
        /// <param name="messageId"> Tin nhắn cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpDelete("message/{messageId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> DeleteMessage(Guid messageId)
        {
            ServiceResult res = await ConversationService.DeleteMessage(GetCurrentUserIdStrictly(), messageId);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu cập nhật tin nhắn
        /// </summary>
        /// <param name="model"> Thông tin cần cập nhật </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpPatch("message")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UpdateMessage([FromBody] UpdateMessageModel model)
        {
            ServiceResult res = await ConversationService.UpdateMessage(GetCurrentUserIdStrictly(), model);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu phản hồi tin nhắn
        /// </summary>
        /// <param name="model"> Tin nhắn mới cần phản hồi </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpPost("reply-message")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> ReplyMessage([FromBody] ReplyMessageModel model)
        {
            ServiceResult res = await ConversationService.ReplyMessage(GetCurrentUserIdStrictly(), model);
            return StatusCode(res);
        }

    }
}
