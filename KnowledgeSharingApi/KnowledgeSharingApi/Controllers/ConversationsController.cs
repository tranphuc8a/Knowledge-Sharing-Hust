using KnowledgeSharingApi.Domains.Models.ApiRequestModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeSharingApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ConversationsController(
        IConversationService conversationService    
    ) : ControllerBase
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
        public async Task<IActionResult> GetConversation(string myUid, string conversationId)
        {
            ServiceResult res = await ConversationService.GetConversation(myUid, conversationId);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
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
        public async Task<IActionResult> GetListConversations(string myUid, int? limit, int? offset)
        {
            ServiceResult res = await ConversationService.GetConversations(myUid, limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Xử lý yêu cầu lấy về cuộc trò chuyện với người kia
        /// </summary>
        /// <param name="userId"> Id của người kia </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("with/{userId}")]
        public async Task<IActionResult> GetConversationWith(string myUid, string userId)
        {
            ServiceResult res = await ConversationService.GetConversationWith(myUid, userId);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Bắt đầu cuộc trò chuyện với người khác
        /// </summary>
        /// <param name="userId"> Id của người kia </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpPost("with/{userId}")]
        public async Task<IActionResult> StartConversationWith(string myUid, string userId)
        {
            ServiceResult res = await ConversationService.StartConversation(myUid, userId);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }


        /// <summary>
        /// Xử lý yêu cầu xóa cuộc trò chuyện
        /// </summary>
        /// <param name="conversationId"> Id của cuộc trò chuyện </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpDelete("{conversationId}")]
        public async Task<IActionResult> DeleteConversation(string myUid, string conversationId)
        {
            ServiceResult res = await ConversationService.DeleteConversation(myUid, conversationId);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }


        /// <summary>
        /// Xử lý yêu cầu cập nhật thông tin cuộc trò chuyện
        /// </summary>
        /// <param name="model"> Thông tin cuộc trò chuyện cần cập nhật </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpPatch]
        public async Task<IActionResult> UpdateConversation(string myUid, [FromBody] UpdateConversationModel model)
        {
            ServiceResult res = await ConversationService.UpdateConversation(myUid, model);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Xử lý yêu cầu cập nhật thông tin người tham gia cuộc trò chuyện
        /// </summary>
        /// <param name="model"> Thông tin cần cập nhật </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpPatch("participant")]
        public async Task<IActionResult> UpdateParticipant(string myUid, [FromBody] UpdateUserConversationModel model)
        {
            ServiceResult res = await ConversationService.UpdateUserConversation(myUid, model);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Đánh dấu đã đọc cuộc trò chuyện
        /// </summary>
        /// <param name="conversationId"> Id cuộc trò chuyện cần đánh dấu đã đọc </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpPost("set-read/{conversationId}")]
        public async Task<IActionResult> SetReadConversation(string myUid, string conversationId)
        {
            ServiceResult res = await ConversationService.SetReadConversation(myUid, conversationId);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
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
        public async Task<IActionResult> GetMessages(string myUid, string conversationId, int? limit, int? offset)
        {
            ServiceResult res = await ConversationService.GetMessages(myUid, conversationId, limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }


        /// <summary>
        /// Xử lý yêu cầu tạo mới một tin nhắn
        /// </summary>
        /// <param name="model"> Tin nhắn cần tạo mới </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpPost("message")]
        public async Task<IActionResult> PostMessage(string myUid, SendMessageModel model)
        {
            ServiceResult res = await ConversationService.SendMessage(myUid, model);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Xử lý yêu cầu gửi mới tin nhắn
        /// </summary>
        /// <param name="model"> Tin nhắn cần gửi </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessage(string myUid, SendMessageModel model)
        {
            ServiceResult res = await ConversationService.SendMessage(myUid, model);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Xử lý yêu cầu xóa một tin nhắn
        /// </summary>
        /// <param name="messageId"> Tin nhắn cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpDelete("message/{messageId}")]
        public async Task<IActionResult> DeleteMessage(string myUid, string messageId)
        {
            ServiceResult res = await ConversationService.DeleteMessage(myUid, messageId);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Xử lý yêu cầu cập nhật tin nhắn
        /// </summary>
        /// <param name="model"> Thông tin cần cập nhật </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpPatch("message")]
        public async Task<IActionResult> UpdateMessage(string myUid, [FromBody] UpdateMessageModel model)
        {
            ServiceResult res = await ConversationService.UpdateMessage(myUid, model);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Xử lý yêu cầu phản hồi tin nhắn
        /// </summary>
        /// <param name="model"> Tin nhắn mới cần phản hồi </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpPost("reply-message")]
        public async Task<IActionResult> ReplyMessage(string myUid, [FromBody] ReplyMessageModel model)
        {
            ServiceResult res = await ConversationService.ReplyMessage(myUid, model);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

    }
}
