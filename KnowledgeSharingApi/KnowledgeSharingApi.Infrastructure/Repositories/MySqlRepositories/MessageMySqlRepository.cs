using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Repositories.MySqlRepositories
{
    public class MessageMySqlRepository(IDbContext dbContext)
        : BaseMySqlRepository<Message>(dbContext), IMessageRepository
    {
        public Task<ViewMessage?> GetDetail(Guid messageId)
        {
            return Task.FromResult(
                DbContext.ViewMessages
                .Where(message => message.MessageId == messageId)
                .FirstOrDefault()
            );
        }
    }
}
