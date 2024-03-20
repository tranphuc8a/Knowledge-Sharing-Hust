using KnowledgeSharingApi.Domains.Models.ApiRequestModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Interfaces
{
    public interface IConversationService
    {
        /// <summary>
        /// Lấy về chi tiết cuộc trò chuyện
        /// </summary>
        /// <param name="myUId"> Id của người muốn lấy </param>
        /// <param name="conversationId"> Id của cuộc trò chuyện muốn lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<ServiceResult> GetConversation(string myUId, string conversationId);

        /// <summary>
        /// Lấy về danh sách cuộc trò chuyện
        /// </summary>
        /// <param name="myUid"> Id của người muốn lấy </param>
        /// <param name="limit"> Thuộc tính phân trang - số lượng cuộc trò chuyện </param>
        /// <param name="offset"> Thuộc tính phân trang - độ lệch bản ghi đầu tiên </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<ServiceResult> GetConversations(string myUid, int? limit, int? offset);

        /// <summary>
        /// Lấy về chi tiết cuộc trò chuyện
        /// </summary>
        /// <param name="myUid"> Id của người muốn lấy </param>
        /// <param name="uid"> Id của người dùng thứ hai </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<ServiceResult> GetConversationWith(string myUid, string uid);

        /// <summary>
        /// Bắt đầu cuộc trò chuyện mới hoặc lấy về cuộc trò chuyện đã có sẵn
        /// </summary>
        /// <param name="myUid"> Id của người muốn lấy </param>
        /// <param name="uid"> Id của người dùng thứ hai </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<ServiceResult> StartConversation(string myUid, string uid);

        /// <summary>
        /// Xóa cuộc trò chuyện
        /// </summary>
        /// <param name="myUid"> Id của người muốn lấy </param>
        /// <param name="conversationId"> Id của cuộc trò chuyện muỗn xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<ServiceResult> DeleteConversation(string myUid, string conversationId);

        /// <summary>
        /// Cập nhật cuộc trò chuyện
        /// </summary>
        /// <param name="myUid"> Id của người muốn lấy </param>
        /// <param name="updateModel"> Thông tin cập nhật </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<ServiceResult> UpdateConversation(string myUid, UpdateConversationModel updateModel);

        /// <summary>
        /// Cập nhật thông tin người tham gia cuộc trò chuyện
        /// </summary>
        /// <param name="myUid"> Id của người muốn lấy </param>
        /// <param name="updateModel"> Thông tin cập nhật </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<ServiceResult> UpdateUserConversation(string myUid, UpdateUserConversationModel updateModel);

        /// <summary>
        /// Đánh dấu đã đọc cuộc trò chuyện
        /// </summary>
        /// <param name="myUid"> Id của người muốn lấy </param>
        /// <param name="conversationId"> Id của cuộc trò chuyện </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<ServiceResult> SetReadConversation(string myUid, string conversationId);

        /// <summary>
        /// Lấy về danh sách tin nhắn của cuộc trò chuyện
        /// </summary>
        /// <param name="myUid"> Id của người muốn lấy </param>
        /// <param name="conversationId"> Id của cuộc trò chuyện muốn lấy </param>
        /// <param name="limit"> Thuộc tính phân trang - số lượng cuộc trò chuyện </param>
        /// <param name="offset"> Thuộc tính phân trang - độ lệch bản ghi đầu tiên </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<ServiceResult> GetMessages(string myUid, string conversationId, int? limit, int? offset);

        /// <summary>
        /// Gửi tin nhắn mới
        /// </summary>
        /// <param name="myUid"> Id của người muốn gửi </param>
        /// <param name="model"> Nội dung tin nhắn muốn gửi </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<ServiceResult> SendMessage(string myUid, SendMessageModel model);

        /// <summary>
        /// Xóa tin nhắn
        /// </summary>
        /// <param name="myUid"> Id của người muốn xóa </param>
        /// <param name="messageId"> Id của tin nhắn muốn xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<ServiceResult> DeleteMessage(string myUid, string messageId);

        /// <summary>
        /// Cập nhật tin nhắn
        /// </summary>
        /// <param name="myUid"> Id của người muốn cập nhật </param>
        /// <param name="model"> Nội dung cập nhật tin nhắn </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<ServiceResult> UpdateMessage(string myUid, UpdateMessageModel model);

        /// <summary>
        /// Phản hồi tin nhắn
        /// </summary>
        /// <param name="myUid"> Id của người muốn thực hiện </param>
        /// <param name="model"> Nội dung tin nhắn muốn phản hồi </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<ServiceResult> ReplyMessage(string myUid, ReplyMessageModel model);
    }
}
