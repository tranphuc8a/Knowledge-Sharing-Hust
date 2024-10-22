﻿using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.UserIteractionRepositories;
using KnowledgeSharingApi.Repositories.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Repositories.Repositories.MySqlRepositories.MySqlUserIteractionRepositories
{
    public class UserRelationMySqlRepository(IDbContext dbContext)
        : BaseMySqlRepository<UserRelation>(dbContext), IUserRelationRepository
    {
        public virtual async Task<bool> CheckBlock(Guid blockerId, Guid blockeeId)
        {
            IQueryable<UserRelation> listBlock = DbContext.UserRelations
                .Where(relation =>
                    relation.UserRelationType == EUserRelationType.Block
                    && relation.SenderId == blockerId
                    && relation.ReceiverId == blockeeId
                );
            return (await listBlock.ToListAsync()).Count > 0;
        }

        public virtual async Task<bool> CheckBlockByUsername(string blockerUsername, string blockeeUsername)
        {
            IQueryable<ViewUserRelation> listBlock =
                from relation in DbContext.ViewUserRelations
                where relation.UserRelationType == EUserRelationType.Block
                        && relation.SenderUsername == blockerUsername
                        && relation.ReceiverUsername == blockeeUsername
                select relation;
            return (await listBlock.ToListAsync()).Count > 0;

        }

        public virtual async Task<bool> CheckBlockByUsernameEachOther(string username1, string username2)
        {
            IQueryable<ViewUserRelation> listBlock =
                from relation in DbContext.ViewUserRelations
                where relation.UserRelationType == EUserRelationType.Block
                where relation.SenderUsername == username1 && relation.ReceiverUsername == username2
                      || relation.SenderUsername == username2 && relation.ReceiverUsername == username1
                select relation;
            return (await listBlock.ToListAsync()).Count > 0;
        }

        public virtual async Task<bool> CheckBlockEachOther(Guid user1Id, Guid user2Id)
        {
            List<UserRelation> listBlock = await DbContext.UserRelations
                .Where(relation =>
                    relation.UserRelationType == EUserRelationType.Block
                    && ((relation.SenderId == user1Id && relation.ReceiverId == user2Id)
                    || (relation.SenderId == user2Id && relation.ReceiverId == user1Id))
                ).ToListAsync();
            return listBlock.Count != 0;
        }

        public virtual async Task<List<ViewUserRelation>> GetByUserId(Guid userId, bool isActive)
        {
            var query = DbContext.ViewUserRelations.AsQueryable();

            query = isActive
                ? query.Where(relation => relation.SenderId == userId)
                : query.Where(relation => relation.ReceiverId == userId);

            return await query.ToListAsync();
        }

        public virtual async Task<List<ViewUserRelation>> GetFriendsByUserId(Guid userId)
        {
            return await DbContext.ViewUserRelations.Where(
                    rel => rel.UserRelationType == EUserRelationType.Friend
                    && (rel.SenderId == userId || rel.ReceiverId == userId)
            ).ToListAsync();
        }

        public virtual async Task<List<ViewUserRelation>> GetByUserIdAndType(Guid userId, bool isActive, EUserRelationType type)
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

        public virtual async Task<List<ViewUserRelation>> GetBlocksByUserId(Guid userId)
        {
            return await DbContext.ViewUserRelations.Where(ur => ur.UserRelationType == EUserRelationType.Block
                            && (ur.SenderId == userId || ur.ReceiverId == userId)).ToListAsync();
        }


        protected virtual UserRelationTypeDto GetUserRelationType(Guid myUid, Guid userId, 
            List<(Guid UserRelationId, EUserRelationType UserRelationType, Guid SenderId, Guid ReceiverId)> userRelations)
        {
            UserRelationTypeDto res = new()
            {
                UserRelationType = EUserRelationType.NotInRelation,
                UserRelationId = null
            };
            if (myUid == userId)
            {
                res.UserRelationType = EUserRelationType.IsMySelf;
                res.UserRelationId = null;
                return res;
            }
            if (userRelations.Count == 0) return res;

            // Check block:
            var block = userRelations
                .Where(relation => relation.UserRelationType == EUserRelationType.Block)
                .FirstOrDefault();
            if (block != default)
            {
                res.UserRelationType = block.SenderId == myUid ? EUserRelationType.Blocker : EUserRelationType.Blockee;
                res.UserRelationId = block.UserRelationId;
                return res;
            }

            // Check friend:
            var friend = userRelations
                .Where(relation => relation.UserRelationType == EUserRelationType.Friend)
                .FirstOrDefault();
            if (friend != default)
            {
                res.UserRelationType = EUserRelationType.Friend;
                res.UserRelationId = friend.UserRelationId;
                return res;
            }

            // Check request:
            var request = userRelations
                .Where(relation => relation.UserRelationType == EUserRelationType.FriendRequest)
                .FirstOrDefault();
            if (request != default)
            {
                res.UserRelationType = request.SenderId == myUid ? EUserRelationType.Requester : EUserRelationType.Requestee;
                res.UserRelationId = request.UserRelationId;
                return res;
            }

            // Check follower:
            var follower = userRelations
                .Where(rel => rel.SenderId == myUid && rel.UserRelationType == EUserRelationType.Follow)
                .FirstOrDefault();
            if (follower != default)
            {
                res.UserRelationType = EUserRelationType.Follower;
                res.UserRelationId = follower.UserRelationId;
                return res;
            }

            // Check followee:
            var followee = userRelations
                .Where(rel => rel.ReceiverId == myUid && rel.UserRelationType == EUserRelationType.Follow)
                .FirstOrDefault();
            if (followee != default)
            {
                res.UserRelationType = EUserRelationType.Followee;
                res.UserRelationId = followee.UserRelationId;
                return res;
            }

            // Not Relation
            return res;
        }

        public virtual async Task<EUserRelationType> GetUserRelationType(Guid user1, Guid user2)
        {
            List<(Guid Id, EUserRelationType type, Guid send, Guid receive)> userRelations = (await
                DbContext.ViewUserRelations
                .Where(relation =>
                    relation.SenderId == user1 && relation.ReceiverId == user2
                 ||
                    relation.SenderId == user2 && relation.ReceiverId == user1
                ).Select(item => new
                {
                    item.UserRelationId,
                    item.UserRelationType,
                    item.SenderId,
                    item.ReceiverId
                }).ToListAsync())
                .Select(item => (item.UserRelationId, item.UserRelationType, item.SenderId, item.ReceiverId))
                .ToList();

            return GetUserRelationType(user1, user2, userRelations).UserRelationType!.Value;
        }

        public virtual async Task<Dictionary<Guid, EUserRelationType>> GetUserRelationType(Guid user1, List<Guid> users2)
        {
            var userRelations = await DbContext.ViewUserRelations
                .Where(relation => relation.SenderId == user1 && users2.Contains(relation.ReceiverId) ||
                                   relation.ReceiverId == user1 && users2.Contains(relation.SenderId))
                .Select(item => new
                {
                    item.UserRelationId,
                    item.UserRelationType,
                    item.SenderId,
                    item.ReceiverId
                })
                .ToListAsync();

            // Khởi tạo kết quả mặc định là NotInRelation
            var results = users2.Distinct().ToDictionary(user => user, user => EUserRelationType.NotInRelation);

            // Xác định loại quan hệ cho từng cặp người dùng
            foreach (Guid otherUser in users2)
            {
                var relations = userRelations
                    .Where(rel => rel.SenderId == otherUser || rel.ReceiverId == otherUser)
                    .Select(rel => (rel.UserRelationId, rel.UserRelationType, rel.SenderId, rel.ReceiverId))
                    .ToList();
                EUserRelationType relationType = GetUserRelationType(user1, otherUser, relations).UserRelationType!.Value;
                results[otherUser] = relationType;
            }

            return results;
        }

        public virtual async Task<Dictionary<Guid, UserRelationTypeDto>> GetDetailUserRelationType(Guid user1, List<Guid> users2)
        {
            var userRelations = await DbContext.ViewUserRelations
                .Where(relation => relation.SenderId == user1 && users2.Contains(relation.ReceiverId) ||
                                   relation.ReceiverId == user1 && users2.Contains(relation.SenderId)).
                Select(item => new
                {
                    item.UserRelationId,
                    item.UserRelationType,
                    item.SenderId,
                    item.ReceiverId
                })
                .ToListAsync();

            // Khởi tạo kết quả mặc định là NotInRelation
            var results = users2.Distinct().ToDictionary(user => user, user => new UserRelationTypeDto()
            {
                UserRelationType = EUserRelationType.NotInRelation,
                UserRelationId = null
            });

            // Xác định loại quan hệ cho từng cặp người dùng
            foreach (Guid otherUser in users2)
            {
                var relations = userRelations
                    .Where(rel => rel.SenderId == otherUser || rel.ReceiverId == otherUser)
                    .Select(rel => (rel.UserRelationId, rel.UserRelationType, rel.SenderId, rel.ReceiverId))
                    .ToList();
                UserRelationTypeDto relationType = GetUserRelationType(user1, otherUser, relations);
                results[otherUser] = relationType;
            }

            return results;
        }


        #region Operation in relations
        public async Task<Guid?> AddBlock(Guid blockerId, Guid blockeeId)
        {
            var transaction = await DbContext.BeginTransaction();
            try
            {
                // Xoa di nhung relation khac
                IQueryable<UserRelation> relationsToDelete = DbContext.UserRelations
                    .Where(rel => rel.UserRelationType != EUserRelationType.Block)
                    .Where(rel => rel.SenderId == blockerId && rel.ReceiverId == blockeeId
                        || rel.SenderId == blockeeId && rel.ReceiverId == blockerId);
                DbContext.UserRelations.RemoveRange(relationsToDelete);

                // Them block
                Guid idToInsert = Guid.NewGuid();
                DateTime now = DateTime.UtcNow;
                UserRelation blockObject = new()
                {
                    // Entity:
                    CreatedBy = blockerId.ToString(),
                    CreatedTime = now,
                    // UserRelation:
                    UserRelationId = idToInsert,
                    SenderId = blockerId,
                    ReceiverId = blockeeId,
                    UserRelationType = EUserRelationType.Block,
                    Time = now
                };
                DbContext.UserRelations.Add(blockObject);

                // Save Change
                int raws = await DbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return raws > 0 ? idToInsert : null;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return null;
            }
        }

        public async Task<Guid?> AddFriend(Guid idUser1, Guid idUser2)
        {
            var transaction = await DbContext.BeginTransaction();
            try
            {
                // Xoa di nhung relation khac
                IQueryable<UserRelation> relationsToDelete = DbContext.UserRelations
                    .Where(rel => rel.UserRelationType != EUserRelationType.Block)
                    .Where(rel => rel.SenderId == idUser1 && rel.ReceiverId == idUser2
                        || rel.SenderId == idUser2 && rel.ReceiverId == idUser1);
                DbContext.UserRelations.RemoveRange(relationsToDelete);

                // Them friend
                Guid idToInsert = Guid.NewGuid();
                DateTime now = DateTime.UtcNow;
                UserRelation friendObject = new()
                {
                    // Entity:
                    CreatedBy = idUser1.ToString(),
                    CreatedTime = now,
                    // UserRelation:
                    UserRelationId = idToInsert,
                    SenderId = idUser1,
                    ReceiverId = idUser2,
                    UserRelationType = EUserRelationType.Friend,
                    Time = now
                };
                DbContext.UserRelations.Add(friendObject);

                // Save Change
                int raws = await DbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return raws > 0 ? idToInsert : null;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return null;
            }
        }

        public async Task<int> DeleteFriend(Guid idUser1, Guid idUser2)
        {
            var transaction = await DbContext.BeginTransaction();
            try
            {
                // Xoa di toan bo user relation
                IQueryable<UserRelation> relationsToDelete = DbContext.UserRelations
                    .Where(rel => rel.UserRelationType != EUserRelationType.Block)
                    .Where(rel => rel.SenderId == idUser1 && rel.ReceiverId == idUser2
                        || rel.SenderId == idUser2 && rel.ReceiverId == idUser1);
                DbContext.UserRelations.RemoveRange(relationsToDelete);

                // Save Change
                int raws = await DbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return raws;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return -1;
            }
        }

        protected override DbSet<UserRelation> GetDbSet()
        {
            return DbContext.UserRelations;
        }

        #endregion
    }
}
