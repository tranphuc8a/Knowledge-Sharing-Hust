using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.UserIteractionRepositories;
using KnowledgeSharingApi.Repositories.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Repositories.Repositories.MySqlRepositories.MySqlUserIteractionRepositories
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
