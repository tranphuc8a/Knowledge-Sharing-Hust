using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Repositories.MySqlRepositories
{
    public class UserRelationMySqlRepository(IDbContext dbContext)
        : BaseMySqlRepository<UserRelation>(dbContext), IUserRelationRepository
    {
        public Task<bool> CheckBlock(Guid blockerId, Guid blockeeId)
        {
            IEnumerable<UserRelation> listBlock = DbContext.UserRelations
                .Where(relation => 
                    relation.UserRelationType == EUserRelationType.Block
                    && relation.SenderId == blockerId
                    && relation.ReceiverId == blockeeId
                );
            return Task.FromResult(listBlock.Any());
        }

        public Task<bool> CheckBlockByUsername(string blockerUsername, string blockeeUsername)
        {
            IEnumerable<ViewUserRelation> listBlock = 
                from relation in DbContext.ViewUserRelations
                where relation.UserRelationType == EUserRelationType.Block
                        && relation.SenderUsername == blockerUsername 
                        && relation.ReceiverUsername == blockeeUsername
                select relation;
            return Task.FromResult(listBlock.Any());

        }

        public Task<bool> CheckBlockByUsernameEachOther(string username1, string username2)
        {
            IEnumerable<ViewUserRelation> listBlock =
                from relation in DbContext.ViewUserRelations
                where relation.UserRelationType == EUserRelationType.Block
                where (relation.SenderUsername == username1 && relation.ReceiverUsername == username2)
                      || (relation.SenderUsername == username2 && relation.ReceiverUsername == username1)
                select relation;
            return Task.FromResult(listBlock.Any());
        }

        public Task<bool> CheckBlockEachOther(Guid user1Id, Guid user2Id)
        {
            IEnumerable<UserRelation> listBlock = DbContext.UserRelations
                .Where(relation =>
                    relation.UserRelationType == EUserRelationType.Block
                    && (relation.SenderId == user1Id || relation.ReceiverId == user2Id)
                    && (relation.SenderId == user2Id || relation.ReceiverId == user1Id)
                );
            return Task.FromResult(listBlock.Any());
        }

        public async Task<IEnumerable<ViewUserRelation>> GetByUserId(Guid userId, bool isActive)
        {
            var query = DbContext.ViewUserRelations.AsQueryable();

            query = isActive
                ? query.Where(relation => relation.SenderId == userId)
                : query.Where(relation => relation.ReceiverId == userId);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<ViewUserRelation>> GetByUserIdAndType(Guid userId, bool isActive, EUserRelationType type)
        {
            // Sử dụng biến 'userIdProperty' để đại diện cho SenderId hay ReceiverId dựa trên 'isActive'
            Expression<Func<ViewUserRelation, bool>> userIdPredicate = isActive
                ? (relation => relation.SenderId == userId)
                : (relation => relation.ReceiverId == userId);

            // Định nghĩa truy vấn với điều kiện 'userIdPredicate' và 'type'
            var query =
                DbContext.ViewUserRelations
                .Where(userIdPredicate)
                .Where(relation => relation.UserRelationType == type);

            return await query.ToListAsync();
        }

        public async Task<EUserRelationType> GetUserRelationType(Guid user1, Guid user2)
        {
            // Lấy về relation
            ViewUserRelation? userRelation = await
                DbContext.ViewUserRelations
                .Where(relation => (
                    relation.SenderId == user1 && relation.ReceiverId == user2
                ) || (
                    relation.SenderId == user2 && relation.ReceiverId == user1
                )).FirstOrDefaultAsync();
            
            // Không quan hệ
            if (userRelation == null) return EUserRelationType.NotInRelation;

            // Bạn bè
            EUserRelationType type = userRelation.UserRelationType;
            if (type == EUserRelationType.Friend)
                return type;

            // Yêu cầu kết bạn
            if (type == EUserRelationType.FriendRequest)
                return user1 == userRelation.SenderId ? EUserRelationType.FriendRequester : EUserRelationType.FriendRequestee;

            // Theo dõi
            if (type == EUserRelationType.Follow)
                return user1 == userRelation.SenderId ? EUserRelationType.Follower : EUserRelationType.Followee;

            // Chặn
            if (type == EUserRelationType.Block)
                return user1 == userRelation.SenderId ? EUserRelationType.Blocker : EUserRelationType.Blockee;

            return EUserRelationType.NotInRelation;
        }

        public async Task<Dictionary<Guid, EUserRelationType>> GetUserRelationType(Guid user1, List<Guid> users2)
        {
            var userRelations = await DbContext.ViewUserRelations
                .Where(relation => (relation.SenderId == user1 && users2.Contains(relation.ReceiverId)) ||
                                   (relation.ReceiverId == user1 && users2.Contains(relation.SenderId)))
                .ToListAsync();

            // Khởi tạo kết quả mặc định là NotInRelation
            var results = users2.ToDictionary(user => user, user => EUserRelationType.NotInRelation);

            // Xác định loại quan hệ cho từng cặp người dùng
            foreach (var userRelation in userRelations)
            {
                var otherUser = userRelation.SenderId == user1 ? userRelation.ReceiverId : userRelation.SenderId;

                var relationType = userRelation.UserRelationType switch
                {
                    EUserRelationType.Friend => EUserRelationType.Friend,
                    EUserRelationType.FriendRequest => user1 == userRelation.SenderId
                        ? EUserRelationType.FriendRequester
                        : EUserRelationType.FriendRequestee,
                    EUserRelationType.Follow => user1 == userRelation.SenderId
                        ? EUserRelationType.Follower
                        : EUserRelationType.Followee,
                    EUserRelationType.Block => user1 == userRelation.SenderId
                        ? EUserRelationType.Blocker
                        : EUserRelationType.Blockee,
                    _ => EUserRelationType.NotInRelation
                };

                results[otherUser] = relationType;
            }

            return results;
        }

    }
}
