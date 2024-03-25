using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MimeKit.Tnef;
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
        public virtual Task<IEnumerable<ViewUserConversation>> GetParticipants(Guid conversationId)
        {
            return Task.FromResult(
                DbContext.ViewUserConversations
                .Where(participant => participant.ConversationId == conversationId)
                .OrderBy(participant => participant.Time)
                .AsEnumerable()
            );
        }

        public virtual Task<IEnumerable<ViewMessage>> GetMessages(Guid userId, Guid conversationId, int limit, int offset)
        {
            ViewUserConversation userConversation =
                DbContext.ViewUserConversations
                .Where(user => user.UserId == userId)
                .FirstOrDefault() ?? throw new Exception("user and conversation not match");

            return Task.FromResult(
                DbContext.ViewMessages
                .Where(message => message.ConversationId == conversationId && message.Time >= userConversation.LastDeleteTime)
                .OrderByDescending(message => message.Time)
                .Skip(offset).Take(limit)
                .AsEnumerable()
            );
        }

        public virtual Task<IEnumerable<Conversation>> GetListConversationByUserId(Guid userId)
        {
            IQueryable<Conversation> query =
                from conversation in DbContext.Conversations
                join userconversation in DbContext.UserConversations
                    on conversation.ConversationId equals userconversation.ConversationId
                where userconversation.UserId == userId
                select conversation;
            return Task.FromResult(query.AsEnumerable());
        }

        public virtual Task<Conversation?> GetConversationWithUser(Guid userId, Guid id2)
        {
            IQueryable<Conversation> query =
            (
                from conversation in DbContext.Conversations
                join userconversation in DbContext.UserConversations
                    on conversation.ConversationId equals userconversation.ConversationId
                where userconversation.UserId == userId || userconversation.UserId == id2
                group conversation by conversation.ConversationId into groupedConversations
                where groupedConversations.Count() == 2
                select groupedConversations.First()
            );

            return Task.FromResult(query.AsEnumerable().FirstOrDefault());
        }


        public virtual async Task DeleteExpiredMessages(Guid conversationId)
        {
            IEnumerable<ViewUserConversation> participants = await GetParticipants(conversationId);
            DateTime minLastDelete = participants.Select(participant => participant.LastDeleteTime).Min();
            IEnumerable<Message> expired_messages =
                DbContext.Messages
                .Where(message => message.Time <= minLastDelete)
                .AsEnumerable();

            DbContext.Messages.RemoveRange(expired_messages);

            await DbContext.SaveChangesAsync();
        }

        public async Task<ViewUserConversation?> GetParticipant(Guid userId, Guid conversationId)
        {
            return await DbContext.ViewUserConversations
                .Where(p => p.UserId == userId && p.ConversationId == conversationId)
                .FirstOrDefaultAsync();
        }

        public async Task<Conversation> CreateConversation(Profile user1, Profile user2)
        {
            var currentTime = DateTime.UtcNow;

            using var transaction = await DbContext.BeginTransaction();
            try
            {
                var conversation = new Conversation
                {
                    ConversationId = Guid.NewGuid(),
                    ConversationName = $"{user1.FullName} và {user2.FullName}",
                    CreatedTime = currentTime,
                    CreatedBy = user1.FullName
                };

                var userConversation1 = new UserConversation
                {
                    UserConversationId = Guid.NewGuid(),
                    ConversationId = conversation.ConversationId,
                    UserId = user1.UserId,
                    Time = currentTime,
                    LastDeleteTime = currentTime,
                    LastReadTime = currentTime,
                    Nickname = user1.FullName
                };

                var userConversation2 = new UserConversation
                {
                    UserConversationId = Guid.NewGuid(),
                    ConversationId = conversation.ConversationId,
                    UserId = user2.UserId,
                    Time = currentTime,
                    LastDeleteTime = currentTime,
                    LastReadTime = currentTime,
                    Nickname = user2.FullName
                };

                DbContext.Conversations.Add(conversation);
                DbContext.UserConversations.AddRange([userConversation1, userConversation2]);

                await DbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return conversation;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
