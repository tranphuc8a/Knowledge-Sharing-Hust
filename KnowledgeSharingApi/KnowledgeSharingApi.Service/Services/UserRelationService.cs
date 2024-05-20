using Google.Apis.Storage.v1;
using KnowledgeSharingApi.Domains.Algorithms;
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
       
        
        #region Functional methods

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
            List<User?> lsUser = await UserRepository.Get([myUid, uid]);
            int count = lsUser.Where(user => user != null).ToList().Count;
            if (count < 2) throw new ValidatorException(ResponseResource.NotExistUser());
        }

        /// <summary>
        /// Lấy danh sách toàn bộ bạn bè của userId
        /// Đảm bảo rằng userId phải đã tồn tại rồi
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (19/3/24)
        /// Modified: None
        //protected virtual async Task<List<ResponseFriendCardModel>> GetAllFriends(Guid userId)
        //{
        //    // Danh sách bạn bè mà mình là người gửi lời mời
        //    List<ViewUserRelation> ls1 =
        //        (await UserRelationRepository.GetByUserIdAndType(userId, isActive: true, EUserRelationType.Friend));
        //    List<ResponseFriendCardModel> lsFriend1 = ls1.Select(
        //        relation => new ResponseFriendCardModel()
        //        {
        //            FriendId = relation.UserRelationId,
        //            UserId = relation.ReceiverId,
        //            Email = relation.ReceiverEmail,
        //            Username = relation.ReceiverUsername,
        //            FullName = relation.ReceiverName,
        //            Avatar = relation.ReceiverAvatar,
        //            Time = relation.Time,
        //            IsActive = true,
        //            CreatedBy = relation.CreatedBy,
        //            CreatedTime = relation.CreatedTime,
        //            ModifiedBy = relation.ModifiedBy,
        //            ModifiedTime = relation.ModifiedTime
        //        }).ToList();

        //    // Danh sách bạn bè mà mình là người nhận lời mời
        //    List<ViewUserRelation> ls2 =
        //        (await UserRelationRepository.GetByUserIdAndType(userId, isActive: false, EUserRelationType.Friend));
        //    List<ResponseFriendCardModel> lsFriend2 = ls2.Select(
        //        relation => new ResponseFriendCardModel()
        //        {
        //            FriendId = relation.UserRelationId,
        //            UserId = relation.SenderId,
        //            Email = relation.SenderEmail,
        //            Username = relation.SenderUsername,
        //            FullName = relation.SenderName,
        //            Avatar = relation.SenderAvatar,
        //            Time = relation.Time,
        //            IsActive = false,
        //            CreatedBy = relation.CreatedBy,
        //            CreatedTime = relation.CreatedTime,
        //            ModifiedBy = relation.ModifiedBy,
        //            ModifiedTime = relation.ModifiedTime
        //        }).ToList();

        //    // Combine and return
        //    List<ResponseFriendCardModel> listed = [.. lsFriend1.Concat(lsFriend2).OrderByDescending(item => item.Time)];
        //    return listed;
        //}
        protected virtual async Task<List<ResponseFriendCardModel>> GetAllFriends(Guid userId)
        {
            List<ViewUserRelation> friends = await UserRelationRepository.GetFriendsByUserId(userId);
            List<ResponseFriendCardModel> listFriends = friends.Select(fri =>
            {
                bool isActive = fri.SenderId == userId;
                ResponseFriendCardModel res = new()
                {
                    CreatedBy = fri.CreatedBy,
                    CreatedTime = fri.CreatedTime,
                    ModifiedBy = fri.ModifiedBy,
                    ModifiedTime = fri.ModifiedTime,

                    FriendId = fri.UserRelationId,
                    UserId = isActive ? fri.ReceiverId : fri.SenderId,
                    Email = isActive ? fri.ReceiverEmail : fri.SenderEmail,
                    Username = isActive ? fri.ReceiverUsername : fri.SenderUsername,
                    FullName = isActive ? fri.ReceiverName : fri.SenderName,
                    Avatar = isActive ? fri.ReceiverAvatar : fri.SenderAvatar,
                    Time = fri.Time,
                    IsActive = isActive
                };
                return res;
            }).ToList();

            return listFriends;
        }
        /// <summary>
        /// Lấy danh sách toàn bộ quan he cua user
        /// Đảm bảo rằng userId phải đã tồn tại rồi
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <param name="isActive"> user co chu dong thuc hien quan he khong </param>
        /// <param name="relationType"> Loai quan he </param>
        /// <returns></returns>
        /// Created: PhucTV (19/3/24)
        /// Modified: None
        protected virtual async Task<List<ResponseFriendCardModel>> GetAllRelations(Guid userId, EUserRelationType relationType, bool isActive)
        {
            // Lấy danh sách quan hệ relationType của uid
            List<ViewUserRelation> ls1 = await UserRelationRepository.GetByUserIdAndType(userId, isActive: isActive, relationType);
            List<ResponseFriendCardModel> lsRelation = [.. ls1.Select(
                relation => new ResponseFriendCardModel()
                {
                    FriendId = relation.UserRelationId,
                    UserId = isActive ? relation.ReceiverId : relation.SenderId,
                    Email = isActive ? relation.ReceiverEmail : relation.SenderEmail,
                    Username = isActive ? relation.ReceiverUsername : relation.SenderUsername,
                    FullName = isActive ? relation.ReceiverName : relation.SenderName,
                    Avatar = isActive ? relation.ReceiverAvatar : relation.SenderAvatar,
                    Time = relation.Time,
                    IsActive = isActive,
                    CreatedBy = relation.CreatedBy,
                    CreatedTime = relation.CreatedTime,
                    ModifiedBy = relation.ModifiedBy,
                    ModifiedTime = relation.ModifiedTime
                }
            ).OrderByDescending(relation => relation.Time)];
            return lsRelation;
        }

        #endregion
        
        
        public virtual async Task<ServiceResult> GetFriends(Guid userId, PaginationDto pagination)
        {
            // Kiểm tra user id tồn tại
            User? user = await UserRepository.Get(userId);
            if (user == null) return ServiceResult.BadRequest(ResponseResource.NotExistUser());

            // Lấy danh sách friends của uid
            List<ResponseFriendCardModel> listed = await GetAllFriends(userId);

            // Phân trang và trả về thành công
            PaginationResponseModel<ResponseFriendCardModel> res = new()
            {
                Total = listed.Count,
                Limit = pagination.Limit,
                Offset = pagination.Offset,
                Results = UserRepository.ApplyPagination(listed, pagination)
            };
            return ServiceResult.Success(ResponseResource.GetSuccess(), string.Empty, res);
        }
        public virtual async Task<ServiceResult> GetRelations(Guid userId, EUserRelationType relationType, bool isActive, PaginationDto pagination)
        {
            // Kiểm tra user id tồn tại
            User? user = await UserRepository.Get(userId);
            if (user == null) return ServiceResult.BadRequest(ResponseResource.NotExistUser());

            List<ResponseFriendCardModel> lsRelation = await GetAllRelations(userId, relationType, isActive);

            // Phân trang và trả về thành công
            PaginationResponseModel<ResponseFriendCardModel> res = new()
            {
                Total = lsRelation.Count,
                Limit = pagination.Limit,
                Offset = pagination.Offset,
                Results = UserRepository.ApplyPagination(lsRelation, pagination)
            };
            return ServiceResult.Success(ResponseResource.GetSuccess(), string.Empty, res);
        }


        public virtual async Task<ServiceResult> SearchFriends(Guid myUid, string? search, PaginationDto page)
        {
            if (string.IsNullOrWhiteSpace(search))
                return ServiceResult.BadRequest("Từ khóa tìm kiếm không được trống");

            // Get ve danh sach tat ca ban be
            List<ResponseFriendCardModel> allFriends = await GetAllFriends(myUid);

            // Thuc hien tinh toan score cho moi friendid
            List<string> listFullname = allFriends.Select(fu => fu.FullName).ToList();
            List<string> listUsername = allFriends.Select(fu => fu.Username).ToList();
            List<string> listEmail = allFriends.Select(fu => fu.Email).ToList();

            Dictionary<string, double> scoreFullName = Algorithm.SimilarityList(search, listFullname);
            Dictionary<string, double> scoreUsername = Algorithm.SimilarityList(search, listUsername);
            Dictionary<string, double> scoreEmail = Algorithm.SimilarityList(search, listEmail);

            double fullnameWeight = 0.5, usernameWeight = 0.3, emailWeight = 0.2;

            Dictionary<Guid, double> scored = allFriends.ToDictionary(
                friend => friend.FriendId,
                friend => fullnameWeight * scoreFullName[friend.FullName] +
                        usernameWeight * scoreUsername[friend.Username] +
                        emailWeight * scoreEmail[friend.Email]
            );

            // Sap xep ban be theo score
            allFriends = [.. allFriends.OrderByDescending(friend => scored[friend.FriendId])];

            // Paginate
            if (page.Filters != null)
            {
                allFriends = UserRelationRepository.ApplyFilter(allFriends, page.Filters);
            }
            allFriends = allFriends.Skip(page.Offset ?? 0).Take(page.Limit ?? 15).ToList();

            // Tra ve ket qua
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(), string.Empty, allFriends);
        }

        public virtual async Task<ServiceResult> SearchRelations(Guid myUid, string? search, EUserRelationType relationType, bool isActive, PaginationDto page)
        {
            if (string.IsNullOrWhiteSpace(search))
                return ServiceResult.BadRequest("Từ khóa tìm kiếm không được trống");

            // Get ve toan bo relation
            List<ResponseFriendCardModel> listRelations = await GetAllRelations(myUid, relationType, isActive);

            // Tinh toan score
            List<string> listFullname = listRelations.Select(usRelation => usRelation.FullName).ToList();
            List<string> listUsername = listRelations.Select(usRelation => usRelation.Username).ToList();
            List<string> listEmail = listRelations.Select(usRelation => usRelation.Email).ToList();

            Dictionary<string, double> scoreFullName = Algorithm.SimilarityList(search, listFullname);
            Dictionary<string, double> scoreUsername = Algorithm.SimilarityList(search, listUsername);
            Dictionary<string, double> scoreEmail = Algorithm.SimilarityList(search, listEmail);

            double fullnameWeight = 0.5, usernameWeight = 0.3, emailWeight = 0.2;

            Dictionary<Guid, double> scored = listRelations.ToDictionary(
                friend => friend.FriendId,
                friend => fullnameWeight * scoreFullName[friend.FullName] +
                        usernameWeight * scoreUsername[friend.Username] +
                        emailWeight * scoreEmail[friend.Email]
            );

            // Sap xep va Paginate
            listRelations = [.. listRelations.OrderByDescending(rel => scored[rel.FriendId])];
            if (page.Filters != null)
            {
                listRelations = UserRelationRepository.ApplyFilter(listRelations, page.Filters);
            }
            listRelations = listRelations.Skip(page.Offset ?? 0).Take(page.Limit ?? 15).ToList();

            // Tra ve thanh cong
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(), string.Empty, listRelations);
        }



        public virtual async Task<ServiceResult> Follow(Guid myuid, Guid uid)
        {
            if (myuid == uid)
            {
                return ServiceResult.BadRequest("Không thể tự follow");
            }
            // Check myuid và uid có tồn tại
            await CheckExistedUser(myuid, uid);

            // Chinh sach follow: Chi co the follow khi chua co quan he: Block, Friend, Request, Following
            EUserRelationType relationType = await UserRelationRepository.GetUserRelationType(myuid, uid);
            if (relationType == EUserRelationType.Friend)
                return ServiceResult.BadRequest("Không thể theo dõi khi đang là bạn bè");
            if (relationType == EUserRelationType.Blocker)
                return ServiceResult.BadRequest("Không thể theo dõi khi đang chặn user này");
            if (relationType == EUserRelationType.Blockee)
                return ServiceResult.BadRequest("Không thể theo dõi khi đang bị user này chặn");
            if (relationType == EUserRelationType.Requester)
                return ServiceResult.BadRequest("Không thể theo dõi khi đang gửi lời mời kết bạn tới user này");
            if (relationType == EUserRelationType.Requestee)
                return ServiceResult.BadRequest("Hãy phản hồi lời mời kết bạn từ user này trước");
            if (relationType == EUserRelationType.Follower)
                return ServiceResult.BadRequest("Bạn đã theo dõi người dùng này trước đó rồi");

            // OK cho myuid follow uid
            UserRelation userRelation = new()
            {
                SenderId = myuid,
                ReceiverId = uid,
                Time = DateTime.Now,
                UserRelationType = EUserRelationType.Follow,
                UserRelationId = Guid.NewGuid(),
                CreatedTime = DateTime.Now,
                CreatedBy = myuid.ToString()
            };
            Guid? id = await UserRelationRepository.Insert(userRelation.UserRelationId, userRelation);
            if (id == null) return ServiceResult.ServerError(ResponseResource.ServerError());

            // Gửi thông báo tới người dùng kia
            // Làm sau...

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.Success(), string.Empty, userRelation);
        }
        public virtual async Task<ServiceResult> Unfollow(Guid myuid, Guid uid)
        {
            if (myuid == uid)
            {
                return ServiceResult.BadRequest("Không thể tự unfollow");
            }

            // Kiểm tra myuid có follow uid chưa
            List<ViewUserRelation> relations = await UserRelationRepository.GetByUserIdAndType(myuid, true, EUserRelationType.Follow);
            List<ViewUserRelation> relation = relations.Where(relation => relation.ReceiverId == uid).ToList();
            if (relation.Count == 0) return ServiceResult.BadRequest("Bạn chưa follow người dùng này");

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
            if (myuid == uid)
            {
                return ServiceResult.BadRequest("Không thể tự block");
            }
            // Check user tồn tại
            await CheckExistedUser(myuid, uid);

            // Check myuid chưa block uid
            List<ViewUserRelation> relations = (await UserRelationRepository
                .GetByUserIdAndType(myuid, true, EUserRelationType.Block))
                .Where(relation => relation.ReceiverId == uid)
                .ToList();
            if (relations.Count != 0) return ServiceResult.BadRequest("Bạn đã chặn người dùng này rồi");

            // Ok cho myuid block uid
            Guid? id = await UserRelationRepository.AddBlock(myuid, uid);
            if (id == null) return ServiceResult.ServerError(ResponseResource.BlockFailure());

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
                List<ResponseFriendCardModel> friends =
                    (await GetAllFriends(myuid))
                    .Where(friend => friend.UserId == uid)
                    .ToList();
                int deleted = await UserRelationRepository.Delete(
                    friends.Select(friend => friend.FriendId).ToArray());
                if (deleted <= 0) throw new Exception();

                // Xóa quan hệ theo dõi lẫn nhau
                List<ViewUserRelation> followers = (await UserRelationRepository.GetByUserIdAndType(myuid, true, EUserRelationType.Follow))
                    .Where(follower => follower.ReceiverId == uid)
                    .ToList();
                deleted = await UserRelationRepository.Delete(followers.Select(follower => follower.UserRelationId).ToArray());
                if (deleted <= 0) throw new Exception();

                List<ViewUserRelation> followees = (await UserRelationRepository.GetByUserIdAndType(uid, true, EUserRelationType.Follow))
                    .Where(follower => follower.ReceiverId == myuid)
                    .ToList();
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
                Guid? id = await UserRelationRepository.Insert(blocker) ?? throw new Exception();

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
            if (myuid == uid)
            {
                return ServiceResult.BadRequest("Không thể tự unblock");
            }
            // Check user tồn tại
            await CheckExistedUser(myuid, uid);

            // Kiểm tra myuid phải block user rồi
            List<ViewUserRelation> relations = (await UserRelationRepository.GetByUserIdAndType(myuid, true, EUserRelationType.Block))
                .Where(relation => relation.ReceiverId == uid)
                .ToList();
            if (relations.Count == 0) return ServiceResult.BadRequest("Bạn chưa chặn người dùng này");

            // OK xóa block
            int res = await UserRelationRepository.Delete(
                relations.Select(relation => relation.UserRelationId).ToArray());
            if (res <= 0) return ServiceResult.ServerError(ResponseResource.ServerError());

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.Success());
        }


        public virtual async Task<ServiceResult> AddFriend(Guid myuid, Guid uid)
        {
            if (myuid == uid)
            {
                return ServiceResult.BadRequest("Không thể tự thêm bạn bè chính mình");
            }

            // Kiểm tra người dùng tồn tại
            await CheckExistedUser(myuid, uid);

            // Chinh sach gui loi moi ket ban: 
            // khong có quan he: Block, Friend, Request
            EUserRelationType relationType = await UserRelationRepository.GetUserRelationType(myuid, uid);
            if (relationType == EUserRelationType.Friend)
                return ServiceResult.BadRequest("Không thể gửi lời mời kết bạn khi đang là bạn bè");
            if (relationType == EUserRelationType.Blocker)
                return ServiceResult.BadRequest("Không thể gửi lời mời kết bạn khi đang chặn user này");
            if (relationType == EUserRelationType.Blockee)
                return ServiceResult.BadRequest("Không thể gửi lời mời kết bạn khi đang bị user này chặn");
            if (relationType == EUserRelationType.Requester)
                return ServiceResult.BadRequest("Bạn đã gửi lời mời kết bạn trước đó rồi");
            if (relationType == EUserRelationType.Requestee)
                return ServiceResult.BadRequest("Hãy phản hồi lời mời kết bạn từ user này trước");

            // OK, thêm lời mời kết bạn
            UserRelation relation = new()
            {
                UserRelationId = Guid.NewGuid(),
                SenderId = myuid,
                ReceiverId = uid,
                UserRelationType = EUserRelationType.FriendRequest,
                Time = DateTime.Now,
                CreatedTime = DateTime.Now,
                CreatedBy = uid.ToString()
            };
            Guid? id = await UserRelationRepository.Insert(relation.UserRelationId, relation);
            if (id == null) return ServiceResult.ServerError(ResponseResource.ServerError());

            // Gửi thông báo tới người dùng kia
            // Làm sau...

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.Success(), string.Empty, relation);
        }
        public virtual async Task<ServiceResult> DeleteFriend(Guid myuid, Guid uid)
        {
            if (myuid == uid)
            {
                return ServiceResult.BadRequest("Không thể tự xóa bạn bè chính mình");
            }

            // Kiểm tra user tồn tại
            await CheckExistedUser(myuid, uid);

            // Kiểm tra có tồn tại quan hệ bạn bè không
            List<ResponseFriendCardModel> friends =
                (await GetAllFriends(myuid))
                .Where(friend => friend.UserId == uid)
                .ToList();
            if (friends.Count == 0) return ServiceResult.BadRequest("Không thể thực hiện do các bạn hiện không phải bạn bè");

            // OK xóa bạn bè
            int deleted = await UserRelationRepository.Delete(friends.Select(friend => friend.FriendId).ToArray());
            if (deleted <= 0) return ServiceResult.ServerError(ResponseResource.ServerError());

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.Success());
        }


        public virtual async Task<ServiceResult> ConfirmFriend(Guid myuid, Guid requestId, bool isAccept)
        {
            // Kiểm tra có tồn tại lời mời request id
            UserRelation? relation = await UserRelationRepository.Get(requestId);
            if (relation == null) return ServiceResult.BadRequest("Yêu cầu kết bạn không tồn tại");

            // Kiểm tra đúng thật sự request đang mời tới mình
            if (relation.ReceiverId != myuid)
                return ServiceResult.Forbidden("Đây không phải lời mời kết bạn tới bạn");

            if (isAccept)
            {
                // OK thêm quan hệ bạn bè:
                Guid? id = await UserRelationRepository.AddFriend(relation.SenderId, myuid);
                if (id == null) return ServiceResult.ServerError(ResponseResource.ServerError());
            }
            else
            {
                // Xóa lời mời
                int deleted = await UserRelationRepository.Delete(requestId);
                if (deleted <= 0) return ServiceResult.ServerError(ResponseResource.ServerError());
            }

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.Success());
        }
        public virtual async Task<ServiceResult> DeleteRequest(Guid myuid, Guid requestId)
        {
            // Kiểm tra có tồn tại lời mời request id
            UserRelation? relation = await UserRelationRepository.Get(requestId);
            if (relation == null) return ServiceResult.BadRequest("Yêu cầu kết bạn không tồn tại");

            // Kiểm tra đúng thật sự request id đang là của mình
            if (relation.SenderId != myuid) return ServiceResult.Forbidden("Đây không phải lời mời của bạn");

            // Ok xóa quan hệ
            int res = await UserRelationRepository.Delete(relation.UserRelationId);
            if (res <= 0) return ServiceResult.ServerError(ResponseResource.ServerError());

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.Success());
        }

        public virtual async Task<ServiceResult> GetRelationState(Guid myUid, Guid userId)
        {
            // Check myUid va userId ton tai
            ViewUser user = await UserRepository.CheckExistedUser(userId, ResponseResource.NotExistUser());

            // Lay ve UserCardItem
            ResponseUserCardModel res = new();
            res.Copy(user);

            // Decorate
            var dict = await UserRelationRepository.GetDetailUserRelationType(myUid, [userId]);
            res.UserRelationType = dict[userId].UserRelationType;
            res.UserRelationId = dict[userId].UserRelationId;

            // Tra ve thanh cong
            return ServiceResult.Success(ResponseResource.Success(), string.Empty, res);
        }
    }
}
