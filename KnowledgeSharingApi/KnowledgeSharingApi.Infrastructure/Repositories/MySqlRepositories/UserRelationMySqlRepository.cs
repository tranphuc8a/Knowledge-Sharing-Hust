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

        public Task<IEnumerable<ViewUserRelation>> GetByUserId(Guid userId, bool isActive)
        {
            if (isActive)
            {
                var query1 =
                    from relation in DbContext.ViewUserRelations
                    where relation.SenderId == userId
                    select relation;
                return Task.FromResult(query1.AsEnumerable<ViewUserRelation>());
            }
            var query =
                from relation in DbContext.ViewUserRelations
                where relation.ReceiverId == userId
                select relation;
            return  Task.FromResult(query.AsEnumerable<ViewUserRelation>());
        }

        public Task<IEnumerable<ViewUserRelation>> GetByUserIdAndType(Guid userId, bool isActive, EUserRelationType type)
        {
            if(isActive)
            {
                var query1 =
                    from relation in DbContext.ViewUserRelations
                    where relation.SenderId == userId && relation.UserRelationType == type
                    select relation;
                return Task.FromResult(query1.AsEnumerable<ViewUserRelation>());
            }
            var query =
                from relation in DbContext.ViewUserRelations
                where relation.ReceiverId == userId && relation.UserRelationType == type
                select relation;
            return Task.FromResult(query.AsEnumerable<ViewUserRelation>());
        }
    }
}
