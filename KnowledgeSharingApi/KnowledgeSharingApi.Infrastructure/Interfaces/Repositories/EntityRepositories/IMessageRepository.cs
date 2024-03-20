using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories
{
    public interface IMessageRepository : IRepository<Message>
    {
        /// <summary>
        /// Lấy về chi tiết tin nhắn theo id
        /// </summary>
        /// <param name="messageId"> Id của tin nhắn cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<ViewMessage?> GetDetail(string messageId);
    }
}
