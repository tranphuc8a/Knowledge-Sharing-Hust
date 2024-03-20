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
        Task<IEnumerable<ViewUserConversation>> GetParticipants(string conversationId);


        /// <summary>
        /// Lấy về danh sách tin nhắn của một cuộc hội thoại
        /// </summary>
        /// <param name="conversationId"> id của cuộc hội thoại muốn lấy </param>
        /// <param name="limit"> Số lượng tin nhắn muốn lấy </param>
        /// <param name="offset"> Độ lệch tin nhắn đầu </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<IEnumerable<ViewMessage>> GetMessages(string userId, string conversationId, int limit, int offset);


        /// <summary>
        /// Lấy về danh sách cuộc trò chuyện của userId
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<IEnumerable<Conversation>> GetListConversationByUserId(string userId);

        /// <summary>
        /// Lấy về cuộc trò chuyện của userId với user khác
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <param name="id2"> id của user kia </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<Conversation?> GetConversationWithUser(string userId, string id2);

        /// <summary>
        /// Xóa đi những tin nhắn bị dư thừa của cuộc trò chuyện
        /// </summary>
        /// <param name="conversationId"> Cuộc trò chuyện cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task DeleteExpiredMessages(string conversationId);
    }
}
