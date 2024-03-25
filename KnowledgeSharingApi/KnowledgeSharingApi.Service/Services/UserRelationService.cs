using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Emails;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Interfaces.UnitOfWorks;
using KnowledgeSharingApi.Services.Interfaces;
using MimeKit.Tnef;
using OfficeOpenXml.ConditionalFormatting.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Services
{
    public class UserRelationService(
        IUserRelationRepository userRelationRepository,
        IResourceFactory resourceFactory,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork
    ) : IUserRelationService
    {
        protected readonly IUserRelationRepository UserRelationRepository = userRelationRepository;
        protected readonly IResourceFactory ResourceFactory = resourceFactory;
        protected readonly IResponseResource ResponseResource = resourceFactory.GetResponseResource();
        protected readonly IUserRepository UserRepository = userRepository;
        protected readonly IUnitOfWork UnitOfWork = unitOfWork;
        protected readonly int DefaultLimit = 50;

        /// <summary>
        /// Kiểm tra hai người có chặn nhau không
        /// </summary>
        /// <param name="myUid"> Id của mình </param>
        /// <param name="uid"> Id của người khác </param>
        /// <returns></returns>
        /// <exception cref="ValidatorException"> Hai người dùng chặn nhau </exception>
        /// Created: PhucTV (19/3/24)
        /// Modified: None
        protected virtual async Task CheckBlockEachOther(Guid myUid, Guid uid)
        {
            bool isBlocker = await UserRelationRepository.CheckBlock(myUid, uid);
            if (isBlocker) throw new ValidatorException("Không thể thực hiện vì bạn đã chặn người dùng này");

            bool isBlockee = await UserRelationRepository.CheckBlock(uid, myUid);
            if (isBlockee) throw new ValidatorException("Không thể thực hiện vì bạn đã bị người dùng này chặn");
        }

        /// <summary>
        /// Kiểm tra xem người dùng có tồn tại không
        /// </summary>
        /// <param name="myUid"> Id của mình </param>
        /// <param name="uid"> Id của người khác </param>
        /// <returns></returns>
        /// <exception cref="ValidatorException"> Có một id nào đó không tồn tại </exception>
        /// Created: PhucTV (19/3/24)
        /// Modified: None
        protected virtual async Task CheckExistedUser(Guid myUid, Guid uid)
        {
            IEnumerable<User> lsUser = await UserRepository.Get([myUid, uid]);
            if (lsUser.Count() < 2) throw new ValidatorException(ResponseResource.NotExistUser());
        }

        /// <summary>
        /// Lấy danh sách toàn bộ bạn bè của userId
        /// Đảm bảo rằng userid phải đã tồn tại rồi
        /// </summary>
        /// <param name="userid"> id của user cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (19/3/24)
        /// Modified: None
        protected virtual async Task<IEnumerable<ResponseFriendItemModel>> GetAllFriends(Guid userid)
        {
            // Danh sách bạn bè mà mình là người gửi lời mời
            IEnumerable<ViewUserRelation> ls1 =
                await UserRelationRepository.GetByUserIdAndType(userid, isActive: true, EUserRelationType.Friend);
            IEnumerable<ResponseFriendItemModel> lsFriend1 = ls1.Select(
                relation => new ResponseFriendItemModel()
                {
                    FriendId = relation.UserRelationId,
                    UserId = relation.ReceiverId,
                    Email = relation.ReceiverEmail,
                    Username = relation.ReceiverUsername,
                    FullName = relation.ReceiverName,
                    Avatar = relation.ReceiverAvatar,
                    Time = relation.Time,
                    IsActive = true
                });

            // Danh sách bạn bè mà mình là người nhận lời mời
            IEnumerable<ViewUserRelation> ls2 =
                await UserRelationRepository.GetByUserIdAndType(userid, isActive: false, EUserRelationType.Friend);
            IEnumerable<ResponseFriendItemModel> lsFriend2 = ls1.Select(
                relation => new ResponseFriendItemModel()
                {
                    FriendId = relation.UserRelationId,
                    UserId = relation.SenderId,
                    Email = relation.SenderEmail,
                    Username = relation.SenderUsername,
                    FullName = relation.SenderName,
                    Avatar = relation.SenderAvatar,
                    Time = relation.Time,
                    IsActive = false
                });

            // Combine and return
            IEnumerable<ResponseFriendItemModel> listed = lsFriend1.Concat(lsFriend2).OrderByDescending(item => item.Time);
            return listed;
        }

        public virtual async Task<ServiceResult> GetFriends(Guid userid, int? limit, int? offset)
        {
            // Kiểm tra user id tồn tại
            User? user = await UserRepository.Get(userid);
            if (user == null) return ServiceResult.BadRequest(ResponseResource.NotExistUser());

            // Format limit, offset
            int limitValue = limit ?? DefaultLimit;
            int offsetValue = offset ?? 0;

            // Lấy danh sách friends của uid
            IEnumerable<ResponseFriendItemModel> listed = await GetAllFriends(userid);

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
        public virtual async Task<ServiceResult> GetRelations(Guid userid, EUserRelationType relationType, bool isActive, int? limit, int? offset)
        {
            // Kiểm tra user id tồn tại
            User? user = await UserRepository.Get(userid);
            if (user == null) return ServiceResult.BadRequest(ResponseResource.NotExistUser());

            // Format limit, offset
            int limitValue = limit ?? DefaultLimit;
            int offsetValue = offset ?? 0;

            // Lấy danh sách quan hệ relationType của uid
            IEnumerable<ViewUserRelation> ls1 = await UserRelationRepository.GetByUserIdAndType(userid, isActive: isActive, relationType);
            IEnumerable<ResponseFriendItemModel> lsRelation = ls1.Select(
                relation => new ResponseFriendItemModel()
                {
                    UserId = isActive ? relation.ReceiverId : relation.SenderId,
                    Email = isActive ? relation.ReceiverEmail : relation.SenderEmail,
                    Username = isActive ? relation.ReceiverUsername : relation.SenderUsername,
                    FullName = isActive ? relation.ReceiverName : relation.SenderName,
                    Avatar = isActive ? relation.ReceiverAvatar : relation.SenderAvatar,
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



        public virtual async Task<ServiceResult> Follow(Guid myuid, Guid uid)
        {
            // Check myuid và uid có tồn tại
            await CheckExistedUser(myuid, uid);

            // Check my uid đã follow uid hay chưa
            IEnumerable<ViewUserRelation>? relations = await UserRelationRepository.GetByUserIdAndType(myuid, true, EUserRelationType.Follow);
            if (relations.Any(relation => relation.ReceiverId == uid))
            {
                return ServiceResult.BadRequest("Bạn đã theo dõi người dùng này rồi");
            }

            // Check myuid và uid có chặn nhau hay không
            await CheckBlockEachOther(myuid, uid);

            // OK cho myuid follow uid
            UserRelation userRelation = new()
            {
                SenderId = myuid,
                ReceiverId = uid,
                Time = DateTime.Now,
                UserRelationType = EUserRelationType.Follow,
                CreatedTime = DateTime.Now,
                CreatedBy = myuid.ToString()
            };
            Guid? id = await UserRelationRepository.Insert(userRelation);
            if (id == null) return ServiceResult.ServerError(ResponseResource.ServerError());

            // Gửi thông báo tới người dùng kia
            // Làm sau...

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.Success());
        }
        public virtual async Task<ServiceResult> Unfollow(Guid myuid, Guid uid)
        {
            // Kiểm tra myuid và uid phải tồn tại
            await CheckExistedUser(myuid, uid);

            // Kiểm tra myuid và uid không chặn nhau
            await CheckBlockEachOther(myuid, uid);

            // Kiểm tra myuid có follow uid chưa
            IEnumerable<ViewUserRelation> relations = await UserRelationRepository.GetByUserIdAndType(myuid, true, EUserRelationType.Follow);
            IEnumerable<ViewUserRelation> relation = relations.Where(relation => relation.ReceiverId == uid);
            if (!relation.Any()) return ServiceResult.BadRequest("Bạn chưa follow người dùng này");

            // OK cho bỏ follow
            int res = await UserRelationRepository.Delete(
                relation.Select(relation => relation.UserRelationId).ToArray());
            if (res <= 0)
                return ServiceResult.ServerError(ResponseResource.ServerError());

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.Success());
        }


        public virtual async Task<ServiceResult> Block(Guid myuid, Guid uid)
        {
            // Check user tồn tại
            await CheckExistedUser(myuid, uid);

            // Check myuid chưa block uid
            IEnumerable<ViewUserRelation> relations = (await UserRelationRepository
                .GetByUserIdAndType(myuid, true, EUserRelationType.Block))
                .Where(relation => relation.ReceiverId == uid);
            if (relations.Any()) return ServiceResult.BadRequest("Bạn đã chặn người dùng này rồi mà");

            // Ok cho myuid block uid
            await ResolveBlock(myuid, uid);

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.Success());
        }
        protected virtual async Task ResolveBlock(Guid myuid, Guid uid)
        {
            try
            {
                // Begin Transaction và Đăng ký cho repository
                UnitOfWork.BeginTransaction();
                UnitOfWork.RegisterRepository(UserRelationRepository);

                // Xóa quan hệ bạn bè
                IEnumerable<ResponseFriendItemModel> friends =
                    (await GetAllFriends(myuid))
                    .Where(friend => friend.UserId == uid);
                int deleted = await UserRelationRepository.Delete(
                    friends.Select(friend => friend.FriendId).ToArray());
                if (deleted <= 0) throw new Exception();

                // Xóa quan hệ theo dõi lẫn nhau
                IEnumerable<ViewUserRelation> followers = (await UserRelationRepository.GetByUserIdAndType(myuid, true, EUserRelationType.Follow))
                    .Where(follower => follower.ReceiverId == uid);
                deleted = await UserRelationRepository.Delete(followers.Select(follower => follower.UserRelationId).ToArray());
                if (deleted <= 0) throw new Exception();

                IEnumerable<ViewUserRelation> followees = (await UserRelationRepository.GetByUserIdAndType(uid, true, EUserRelationType.Follow))
                    .Where(follower => follower.ReceiverId == myuid);
                deleted = await UserRelationRepository.Delete(followers.Select(follower => follower.UserRelationId).ToArray());
                if (deleted <= 0) throw new Exception();

                // Thực hiện thêm blocker
                UserRelation blocker = new()
                {
                    SenderId = myuid,
                    ReceiverId = uid,
                    UserRelationType = EUserRelationType.Block,
                    Time = DateTime.Now,
                    CreatedTime = DateTime.Now,
                    CreatedBy = myuid.ToString()
                };
                Guid? id = await UserRelationRepository.Insert(blocker);
                if (id == null) throw new Exception();

                // Commit Transaction
                UnitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                UnitOfWork.RollbackTransaction();
                throw new Exception(ResponseResource.ServerError());
            }
        }
        public virtual async Task<ServiceResult> Unblock(Guid myuid, Guid uid)
        {
            // Check user tồn tại
            await CheckExistedUser(myuid, uid);

            // Kiểm tra myuid phải block user rồi
            IEnumerable<ViewUserRelation> relations = (await UserRelationRepository.GetByUserIdAndType(myuid, true, EUserRelationType.Block))
                .Where(relation => relation.ReceiverId == uid);
            if (!relations.Any()) return ServiceResult.BadRequest("Bạn đã chặn người dùng này đâu");

            // OK xóa block
            int res = await UserRelationRepository.Delete(
                relations.Select(relation => relation.UserRelationId).ToArray());
            if (res <= 0) return ServiceResult.ServerError(ResponseResource.ServerError());

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.Success());
        }


        public virtual async Task<ServiceResult> AddFriend(Guid myuid, Guid uid)
        {
            // Kiểm tra người dùng tồn tại
            await CheckExistedUser(myuid, uid);

            // Kiểm tra không chặn nhau
            await CheckBlockEachOther(myuid, uid);

            // Kiểm tra phải chưa phải là bạn bè
            IEnumerable<ResponseFriendItemModel> friends = (await GetAllFriends(myuid))
                .Where(friend => friend.UserId == uid);
            if (friends.Any()) return ServiceResult.BadRequest("Các bạn đã là bạn bè của nhau rồi mà");

            // Kiểm tra phải chưa gửi lời mời kết bạn
            IEnumerable<ViewUserRelation> requestings = (await UserRelationRepository.GetByUserIdAndType(myuid, true, EUserRelationType.FriendRequest))
                .Where(requesting => requesting.ReceiverId == uid);
            if (requestings.Any()) return ServiceResult.BadRequest("Bạn đã gửi lời mời kết bạn tới người dùng này trước đó rồi, hãy kiểm tra lại");

            // Kiểm tra xem uid phải chưa gửi lời mời kết bạn
            IEnumerable<ViewUserRelation> requesteds = (await UserRelationRepository.GetByUserIdAndType(uid, true, EUserRelationType.FriendRequest))
                .Where(requested => requested.ReceiverId == myuid);
            if (requesteds.Any()) return ServiceResult.BadRequest("Người dùng này đã gửi lời mời kết bạn tới bạn, không cần gửi lại lời mời kết bạn tới họ");

            // OK, thêm lời mời kết bạn
            UserRelation relation = new()
            {
                SenderId = myuid,
                ReceiverId = uid,
                UserRelationType = EUserRelationType.FriendRequest,
                Time = DateTime.Now,
                CreatedTime = DateTime.Now,
                CreatedBy = uid.ToString()
            };
            Guid? id = await UserRelationRepository.Insert(relation);
            if (id == null) return ServiceResult.ServerError(ResponseResource.ServerError());

            // Gửi thông báo tới người dùng kia
            // Làm sau...

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.Success());
        }
        public virtual async Task<ServiceResult> DeleteFriend(Guid myuid, Guid uid)
        {
            // Kiểm tra user tồn tại
            await CheckExistedUser(myuid, uid);

            // Kiểm tra có tồn tại quan hệ bạn bè không
            IEnumerable<ResponseFriendItemModel> friends =
                (await GetAllFriends(myuid))
                .Where(friend => friend.UserId == uid);
            if (!friends.Any()) return ServiceResult.BadRequest("Hai người có phải bạn bè đâu");

            // OK xóa bạn bè
            int deleted = await UserRelationRepository.Delete(friends.Select(friend => friend.FriendId).ToArray());
            if (deleted <= 0) return ServiceResult.ServerError(ResponseResource.ServerError());

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.Success());
        }


        public virtual async Task<ServiceResult> ConfirmFriend(Guid myuid, Guid requestId, bool isAccept)
        {
            // Kiểm tra người dùng tồn tại
            User? user = await UserRepository.Get(myuid);
            if (user == null) return ServiceResult.BadRequest(ResponseResource.NotExistUser());

            // Kiểm tra có tồn tại lời mời request id
            UserRelation? relation = await UserRelationRepository.Get(myuid);
            if (relation == null) return ServiceResult.BadRequest("Yêu cầu kết bạn không tồn tại");

            // Kiểm tra đúng thật sự request đang mời tới mình
            if (relation.ReceiverId != myuid)
                return new ServiceResult()
                {
                    IsSuccess = false,
                    StatusCode = EStatusCode.Forbidden,
                    UserMessage = "Đây không phải lời mời kết bạn tới bạn",
                    DevMessage = "Đây không phải lời mời kết bạn tới bạn"
                };

            if (isAccept)
            {
                // OK cập nhật quan hệ từ request thành friend
                relation.UserRelationType = EUserRelationType.Friend;
                relation.Time = DateTime.Now;
                relation.CreatedTime = DateTime.Now;
                int res = await UserRelationRepository.Update(relation.UserRelationId, relation);
                if (res <= 0) return ServiceResult.ServerError(ResponseResource.ServerError());
            }
            else
            {
                // Xóa lời mời
                int deleted = await UserRelationRepository.Delete(myuid);
                if (deleted <= 0) return ServiceResult.ServerError(ResponseResource.ServerError());
            }

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.Success());
        }
        public virtual async Task<ServiceResult> DeleteRequest(Guid myuid, Guid requestId)
        {
            // Kiểm tra người dùng tồn tại
            User? user = await UserRepository.Get(myuid);
            if (user == null) return ServiceResult.BadRequest(ResponseResource.NotExistUser());

            // Kiểm tra có tồn tại lời mời request id
            UserRelation? relation = await UserRelationRepository.Get(myuid);
            if (relation == null) return ServiceResult.BadRequest("Yêu cầu kết bạn không tồn tại");

            // Kiểm tra đúng thật sự request id đang là của mình
            if (relation.SenderId != myuid) return new ServiceResult()
            {
                IsSuccess = false,
                StatusCode = EStatusCode.Forbidden,
                UserMessage = "Đây không phải lời mời của bạn",
                DevMessage = "Đây không phải lời mời của bạn"
            };

            // Ok xóa quan hệ
            int res = await UserRelationRepository.Delete(relation.UserRelationId);
            if (res <= 0) return ServiceResult.ServerError(ResponseResource.ServerError());

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.Success());
        }
    }
}
