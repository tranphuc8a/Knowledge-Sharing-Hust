using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
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
        public virtual async Task<List<ViewUserConversation>> GetParticipants(Guid conversationId)
        {
            return await DbContext.ViewUserConversations
                .Where(participant => participant.ConversationId == conversationId)
                .ToListAsync();
        }

        public virtual async Task<List<ViewMessage>> GetMessages(Guid userId, Guid conversationId, PaginationDto pagination)
        {
            ViewUserConversation userConversation =
                await DbContext.ViewUserConversations
                .Where(user => user.UserId == userId && user.ConversationId == conversationId)
                .FirstOrDefaultAsync() 
                ?? throw new Exception("User and Conversation not match");

            return await ApplyPagination(
                    DbContext.ViewMessages
                    .Where(message => message.ConversationId == conversationId && message.Time >= userConversation.LastDeleteTime)
                    .OrderByDescending(message => message.Time),
                    pagination
                ).ToListAsync();
        }

        public virtual async Task<List<ViewMessage>> GetMessages(Guid conversationId, DateTime lastDeleteTime, PaginationDto pagination)
        {
            return await ApplyPagination(
                    DbContext.ViewMessages
                    .Where(message => message.ConversationId == conversationId && message.CreatedTime >= lastDeleteTime)
                    .OrderByDescending(message => message.CreatedTime),
                    pagination
                ).ToListAsync();
        }

        public virtual async Task<List<Conversation>> GetListConversationByUserId(Guid userId)
        {
            IQueryable<Conversation> query =
                from conversation in DbContext.Conversations
                join userconversation in DbContext.UserConversations
                    on conversation.ConversationId equals userconversation.ConversationId
                where userconversation.UserId == userId
                select conversation;
            return await query.ToListAsync();
        }

        public virtual async Task<Conversation?> GetConversationWithUser(Guid userId, Guid id2)
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

            return await query.FirstOrDefaultAsync();
        }


        public virtual async Task DeleteExpiredMessages(Guid conversationId)
        {
            List<ViewUserConversation> participants = await GetParticipants(conversationId);
            DateTime minLastDelete = participants.Select(participant => participant.LastDeleteTime).Min();

            IQueryable<Message> expired_messages = DbContext.Messages.Where(message => message.Time <= minLastDelete);
            DbContext.Messages.RemoveRange(expired_messages);

            await DbContext.SaveChangesAsync();
        }

        public async Task<ViewUserConversation?> GetParticipant(Guid userId, Guid conversationId)
        {
            return await DbContext.ViewUserConversations
                .Where(p => p.UserId == userId && p.ConversationId == conversationId)
                .FirstOrDefaultAsync();
        }


        /// <summary>
        /// Tạo mới một Conversation giữa hai user
        /// </summary>
        /// <param name="user1"> user 1</param>
        /// <param name="user2"> user 2</param>
        /// <returns></returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        private static Conversation CreateConversationEntity(Profile user1, Profile user2)
        {
            return new Conversation
            {
                ConversationId = Guid.NewGuid(),
                ConversationName = $"{user1.FullName} và {user2.FullName}",
                CreatedTime = DateTime.UtcNow,
                CreatedBy = user1.FullName
            };
        }

        /// <summary>
        /// Tạo mới một 2 participants của một conversation
        /// </summary>
        /// <param name="conversation"> conversation </param>
        /// <param name="user1"> user 1 </param>
        /// <param name="user2"> user 2 </param>
        /// <returns></returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        private static List<UserConversation> CreateUserConversations(Conversation conversation, Profile user1, Profile user2)
        {
            DateTime currentTime = DateTime.UtcNow;
            return [
                new() {
                    UserConversationId = Guid.NewGuid(),
                    ConversationId = conversation.ConversationId,
                    UserId = user1.UserId,
                    Time = currentTime,
                    LastDeleteTime = currentTime,
                    LastReadTime = currentTime,
                    Nickname = user1.FullName
                },
                new() {
                    UserConversationId = Guid.NewGuid(),
                    ConversationId = conversation.ConversationId,
                    UserId = user2.UserId,
                    Time = currentTime,
                    LastDeleteTime = currentTime,
                    LastReadTime = currentTime,
                    Nickname = user2.FullName
                }
            ];
        }

        public async Task<Conversation?> CreateConversation(Guid uid1, Guid uid2)
        {
            await using var transaction = await DbContext.BeginTransaction();
            try
            {
                List<Profile> profiles = await DbContext.Profiles
                    .Where(p => p.UserId == uid1 || p.UserId == uid2).ToListAsync();
                Profile user1 = profiles.Where(p => p.UserId == uid1).First() ?? throw new Exception();
                Profile user2 = profiles.Where(p => p.UserId == uid2).First() ?? throw new Exception();

                var conversation = CreateConversationEntity(user1, user2);
                DbContext.Conversations.Add(conversation);

                var userConversations = CreateUserConversations(conversation, user1, user2);
                DbContext.UserConversations.AddRange(userConversations);

                await DbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return conversation;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return null;
            }
        }

        public async Task<ResponseConversationModel?> GetDetailsConversation(Guid conversationId)
        {
            var query =
                from conversation in DbContext.Conversations
                join participant in DbContext.ViewUserConversations
                    on conversation.ConversationId equals participant.ConversationId
                group participant by conversation into g
                select new {
                    Conversation = g.Key,
                    Participants = g.ToList()
                };
            var tempConversation = await query.FirstOrDefaultAsync();

            if (tempConversation == null) return null;
            ResponseConversationModel res = new();
            res.Copy(tempConversation.Conversation);
            res.Participants = tempConversation.Participants.Select(p =>
            {
                var temp = new ResponseParticipantModel();
                temp.Copy(p);
                return temp;
            }).ToList();

            return res;
        }
    }
}
