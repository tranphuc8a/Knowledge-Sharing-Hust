using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Emails;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Services
{
    public class UserRelationService (
        IUserRelationRepository userRelationRepository,
        IResourceFactory resourceFactory,
        IUserRepository userRepository
    ) : IUserRelationService
    {
        protected readonly IUserRelationRepository UserRelationRepository = userRelationRepository;
        protected readonly IResourceFactory ResourceFactory = resourceFactory;
        protected readonly IResponseResource ResponseResource = resourceFactory.GetResponseResource();
        protected readonly IUserRepository UserRepository = userRepository;
        protected readonly int DefaultLimit = 50;

        public async Task<ServiceResult> GetFriends(string userid, int? limit, int? offset)
        {
            // Kiểm tra user id tồn tại
            User? user = await UserRepository.Get(userid);
            if (user == null) return ServiceResult.BadRequest(ResponseResource.NotExistUser());

            // Format limit, offset
            int limitValue = limit ?? DefaultLimit;
            int offsetValue = offset ?? 0;

            // Lấy danh sách friends của uid
            IEnumerable<ViewUserRelation> ls1 = await UserRelationRepository.GetByUserIdAndType(userid, isActive: true, EUserRelationType.Friend);
            IEnumerable<ResponseFriendItemModel> lsFriend1 = ls1.Select(
                relation => new ResponseFriendItemModel()
                {
                    UserId = relation.ReceiverId,
                    Email = relation.ReceiverEmail,
                    Username = relation.ReceiverUsername,
                    FullName = relation.ReceiverName,
                    Time = relation.Time,
                    IsActive = true
                });
            IEnumerable<ViewUserRelation> ls2 = await UserRelationRepository.GetByUserIdAndType(userid, isActive: false, EUserRelationType.Friend);
            IEnumerable<ResponseFriendItemModel> lsFriend2 = ls1.Select(
                relation => new ResponseFriendItemModel()
                {
                    UserId = relation.SenderId,
                    Email = relation.SenderEmail,
                    Username = relation.SenderUsername,
                    FullName = relation.SenderName,
                    Time = relation.Time,
                    IsActive = false
                });
            IEnumerable<ResponseFriendItemModel> listed = lsFriend1.Concat(lsFriend2).OrderByDescending(item => item.Time);

            // Phân trang và trả về thành công
            PaginationResponseModel<ResponseFriendItemModel> res = new()
            {
                Total = listed.Count(),
                Limit = limitValue,
                Offset = offsetValue,
                Results = listed.Skip(offsetValue).Take(limitValue)
            };
            return ServiceResult.Success(ResponseResource.GetSuccess(), string.Empty, res);
        }
        public async Task<ServiceResult> GetRelations(string userid, EUserRelationType relationType, bool isActive, int? limit, int? offset)
        {
            // Kiểm tra user id tồn tại
            User? user = await UserRepository.Get(userid);
            if (user == null) return ServiceResult.BadRequest(ResponseResource.NotExistUser());

            // Format limit, offset
            int limitValue = limit ?? DefaultLimit;
            int offsetValue = offset ?? 0;

            // Lấy danh sách của uid
            IEnumerable<ViewUserRelation> ls1 = await UserRelationRepository.GetByUserIdAndType(userid, isActive: isActive, relationType);
            IEnumerable<ResponseFriendItemModel> lsRelation = ls1.Select(
                relation => new ResponseFriendItemModel()
                {
                    UserId = isActive ? relation.ReceiverId : relation.SenderId,
                    Email = isActive ? relation.ReceiverEmail : relation.SenderEmail,
                    Username = isActive ? relation.ReceiverUsername : relation.SenderUsername,
                    FullName = isActive ? relation.ReceiverName : relation.SenderName,
                    Time = relation.Time,
                    IsActive = isActive
                }
            ).OrderByDescending(relation => relation.Time);


            // Phân trang và trả về thành công
            PaginationResponseModel<ResponseFriendItemModel> res = new()
            {
                Total = lsRelation.Count(),
                Limit = limitValue,
                Offset = offsetValue,
                Results = lsRelation.Skip(offsetValue).Take(limitValue)
            };
            return ServiceResult.Success(ResponseResource.GetSuccess(), string.Empty, res);
        }



        public Task<ServiceResult> Follow(string myuid, string uid)
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResult> Unfollow(string myuid, string uid)
        {
            throw new NotImplementedException();
        }


        public Task<ServiceResult> Block(string myuid, string uid)
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResult> Unblock(string myuid, string uid)
        {
            throw new NotImplementedException();
        }


        public Task<ServiceResult> AddFriend(string myuid, string uid)
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResult> DeleteFriend(string myuid, string uid)
        {
            throw new NotImplementedException();
        }


        public Task<ServiceResult> ConfirmFriend(string myuid, string requestId)
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResult> DeleteRequest(string myuid, string requestId)
        {
            throw new NotImplementedException();
        }
    }
}
