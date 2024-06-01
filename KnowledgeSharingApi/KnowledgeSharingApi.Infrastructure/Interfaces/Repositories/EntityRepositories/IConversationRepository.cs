using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories
{
    public interface IConversationRepository : IRepository<Conversation>
    {
        /// <summary>
        /// Lấy về danh sách người tham dự của một conversation
        /// </summary>
        /// <param name="conversationId"> id của cuộc hội thoại muốn lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<List<ViewUserConversation>> GetParticipants(Guid conversationId);


        /// <summary>
        /// Lấy về phiên người tham dự một conversation
        /// </summary>
        /// <param name="userId"> id của người tham dự muốn lấy </param>
        /// <param name="conversationId"> id của cuộc hội thoại muốn lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<ViewUserConversation?> GetParticipant(Guid userId, Guid conversationId);


        /// <summary>
        /// Tạo cuộc hội thoại giữa hai người id1 và id2
        /// </summary>
        /// <param name="user1"> profile của người tham dự số 1 </param>
        /// <param name="user2"> profile của người tham dự số 2 </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<Conversation?> CreateConversation(Guid user1, Guid user2);


        /// <summary>
        /// Lấy về danh sách tin nhắn của một cuộc hội thoại
        /// </summary> 
        /// <param name="userId"> id cua user muon lay </param>
        /// <param name="conversationId"> id của cuộc hội thoại muốn lấy </param>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<List<ViewMessage>> GetMessages(Guid userId, Guid conversationId, PaginationDto pagination);


        /// <summary>
        /// Lấy về danh sách cuộc trò chuyện của userId
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<List<Conversation>> GetListConversationByUserId(Guid userId);

        /// <summary>
        /// Lấy về cuộc trò chuyện của userId với user khác
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <param name="id2"> id của user kia </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<Conversation?> GetConversationWithUser(Guid userId, Guid id2);

        /// <summary>
        /// Xóa đi những tin nhắn bị dư thừa của cuộc trò chuyện
        /// </summary>
        /// <param name="conversationId"> Cuộc trò chuyện cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task DeleteExpiredMessages(Guid conversationId);
    }
}
