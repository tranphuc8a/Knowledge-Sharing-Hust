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
    public class ConversationMySqlRepository(IDbContext dbContext)
        : BaseMySqlRepository<Conversation>(dbContext), IConversationRepository
    {
        public virtual Task<IEnumerable<ViewUserConversation>> GetParticipants(string conversationId)
        {
            return Task.FromResult(
                DbContext.ViewUserConversations
                .Where(participant => participant.ConversationId.ToString() == conversationId)
                .OrderBy(participant => participant.Time)
                .AsEnumerable()
            );
        }

        public virtual Task<IEnumerable<ViewMessage>> GetMessages(string userId, string conversationId, int limit, int offset)
        {
            ViewUserConversation userConversation =
                DbContext.ViewUserConversations
                .Where(user => user.UserId.ToString() == userId)
                .FirstOrDefault() ?? throw new Exception("user and conversation not match");

            return Task.FromResult(
                DbContext.ViewMessages
                .Where(message => message.ConversationId.ToString() == conversationId && message.Time >= userConversation.LastDeleteTime)
                .OrderByDescending(message => message.Time)
                .Skip(offset).Take(limit)
                .AsEnumerable()
            );
        }

        public virtual Task<IEnumerable<Conversation>> GetListConversationByUserId(string userId)
        {
            IQueryable<Conversation> query =
                from conversation in DbContext.Conversations
                join user_conversation in DbContext.UserConversations
                    on conversation.ConversationId equals user_conversation.ConversationId
                where user_conversation.UserId.ToString() == userId
                select conversation;
            return Task.FromResult(query.AsEnumerable());
        }

        public virtual Task<Conversation?> GetConversationWithUser(string userId, string id2)
        {
            IQueryable<Conversation> query =
            (
                from conversation in DbContext.Conversations
                join user_conversation in DbContext.UserConversations
                    on conversation.ConversationId equals user_conversation.ConversationId
                where user_conversation.UserId.ToString() == userId || user_conversation.UserId.ToString() == id2
                group conversation by conversation.ConversationId into groupedConversations
                where groupedConversations.Count() == 2
                select groupedConversations.First()
            );

            return Task.FromResult(query.AsEnumerable().FirstOrDefault());
        }

        
        public virtual async Task DeleteExpiredMessages(string conversationId)
        {
            IEnumerable<ViewUserConversation> participants = await GetParticipants(conversationId);
            DateTime minLastDelete = participants.Select(participant => participant.LastDeleteTime).Min();
            IEnumerable<Message> expired_messages =
                DbContext.Messages
                .Where(message => message.Time <= minLastDelete)
                .AsEnumerable();

            DbContext.Messages.RemoveRange(expired_messages);

            await DbContext.SaveChangeAsync();
        }
    }
}
