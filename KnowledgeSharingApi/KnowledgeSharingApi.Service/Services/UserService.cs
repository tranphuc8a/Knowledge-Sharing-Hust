using KnowledgeSharingApi.Domains.Algorithms;
using KnowledgeSharingApi.Domains.Common;
using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.DecorationRepositories;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Interfaces.Storages;
using KnowledgeSharingApi.Infrastructures.Repositories.DecorationRepositories;
using KnowledgeSharingApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Services
{
    public class UserService(
        IResourceFactory resourceFactory,
        IUserRepository userRepository,
        IProfileRepository profileRepository,
        IDecorationRepository decorationRepository,
        IImageRepository imageRepository,
        IStorage storage,
        IUserRelationRepository userRelationRepository
        ) : IUserService
    {
        #region Attributes
        protected readonly IUserRepository UserRepository = userRepository;
        protected readonly IProfileRepository ProfileRepository = profileRepository;
        protected readonly IResourceFactory ResourceFactory = resourceFactory;
        protected readonly IResponseResource ResponseResource = resourceFactory.GetResponseResource();
        protected readonly IDecorationRepository DecorationRepository = decorationRepository;
        protected readonly IUserRelationRepository UserRelationRepository = userRelationRepository;
        protected readonly IImageRepository ImageRepository = imageRepository;
        protected readonly string ResponseTableName = resourceFactory.GetEntityResource().User();
        protected readonly IStorage Storage = storage;
        // Giá trị mặc định của Limit nếu bằng null
        protected readonly int DefaultLimit = 10;
        #endregion

        #region ForAdmin
        public async Task<ServiceResult> AdminBlockUser(Guid uid)
        {
            // Kiểm tra uid phải tồn tại trong cơ sở dữ liệu
            User? user = await UserRepository.Get(uid);
            if (user == null) return ServiceResult.BadRequest(ResponseResource.NotExistUser());

            // Kiểm tra user phải có role bang User
            if (user.Role != UserRoles.User)
                return new ServiceResult()
                {
                    StatusCode = EStatusCode.Forbidden,
                    UserMessage = ResponseResource.Failure(),
                    DevMessage = ResponseResource.Failure(),
                    Data = user
                };

            // Block user
            user.Role = UserRoles.Banned;
            int res = await UserRepository.Update(uid, user);

            // Kiểm tra update thành công
            if (res <= 0) return ServiceResult.ServerError(ResponseResource.UpdateFailure());

            // Trả về thành công
            return ServiceResult.Success(
                ResponseResource.UpdateSuccess(),
                string.Empty,
                user
            );
        }

        public async Task<ServiceResult> AdminDeleteUser(Guid uid)
        {
            User? user = await UserRepository.Get(uid);
            if (user == null)
                return ServiceResult.BadRequest(ResponseResource.NotExistUser());

            // Kiểm tra user phải có role khác Admin
            if (user.Role == UserRoles.Admin)
                return new ServiceResult()
                {
                    StatusCode = EStatusCode.Forbidden,
                    UserMessage = ResponseResource.Failure(),
                    DevMessage = ResponseResource.Failure(),
                    Data = user
                };


            int res = await UserRepository.Delete(uid);
            if (res <= 0)
                return ServiceResult.ServerError(ResponseResource.DeleteFailure(ResponseTableName));

            return ServiceResult.ServerError(ResponseResource.DeleteSuccess(ResponseTableName));
        }

        public async Task<ServiceResult> AdminGetUserProfile(string unOruid)
        {
            // Lấy về profile
            ViewUser? profile = await UserRepository.GetDetailByUsernameOrUserId(unOruid);

            if (profile == null)
                return ServiceResult.BadRequest(ResponseResource.NotExistUser());

            // Trả về thành công
            return ServiceResult.Success(
                UserMessage: ResponseResource.Success(),
                DevMessage: string.Empty,
                Data: profile
            );
        }

        public async Task<ServiceResult> AdminSearchUser(string searchKey, PaginationDto pagination)
        {
            // Format search key
            searchKey = Unicode.RemoveVietnameseTone(searchKey).ToLower();

            // Lấy về toàn bộ user và Thực hiện truy vấn
            List<ViewUser> lsUser = await UserRepository.GetDetail();
            lsUser = lsUser.GroupBy(ls => ls.UserId).Select(g => g.First()).ToList();

            // Tinh toan similiarity Score
            List<string> listFullname = lsUser.Select(fu => fu.FullName).ToList();
            List<string> listUsername = lsUser.Select(fu => fu.Username).ToList();
            List<string> listEmail = lsUser.Select(fu => fu.Email).ToList();
            List<string> listPhone = lsUser.Select(fu => fu.PhoneNumber ?? "").ToList();

            Dictionary<string, double> scoreFullName = Algorithm.SimilarityList(searchKey, listFullname);
            Dictionary<string, double> scoreUsername = Algorithm.SimilarityList(searchKey, listUsername);
            Dictionary<string, double> scoreEmail = Algorithm.SimilarityList(searchKey, listEmail);
            Dictionary<string, double> scorePhone = Algorithm.SimilarityList(searchKey, listPhone);

            double fullnameWeight = 0.4, usernameWeight = 0.3, emailWeight = 0.2, phoneWeight = 0.1;

            Dictionary<Guid, double> scored = lsUser.ToDictionary(
                u => u.UserId,
                u => fullnameWeight * scoreFullName[u.FullName] +
                        usernameWeight * scoreUsername[u.Username] +
                        emailWeight * scoreEmail[u.Email] +
                        phoneWeight * scorePhone[u.PhoneNumber ?? ""]
            );

            // Sap xep theo scored:
            lsUser = [.. lsUser.OrderByDescending(u => scored[u.UserId])];

            // ap dung filter, pagination (khong ap dung order)
            if (pagination.Filters != null)
                lsUser = UserRepository.ApplyFilter(lsUser, pagination.Filters);
            lsUser = lsUser.Skip(pagination.Offset ?? 0).Take(pagination.Limit ?? 20).ToList();

            // Decoration:
            List<ResponseUserCardModel> res = await DecorationRepository.DecorateResponseUserCardModel(null,
                lsUser.Select(u =>
                {
                    ResponseUserCardModel rUCM = new();
                    rUCM.Copy(u);
                    return rUCM;
                }).ToList());

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.Success(), string.Empty, res);
        }

        public async Task<ServiceResult> AdminUnblockUser(Guid uid)
        {
            // Kiểm tra uid phải tồn tại trong cơ sở dữ liệu
            User? user = await UserRepository.Get(uid);
            if (user == null) return ServiceResult.BadRequest(ResponseResource.NotExistUser());

            // Kiểm tra user phải có role bằng Banned
            if (user.Role != UserRoles.Banned)
                return new ServiceResult()
                {
                    StatusCode = EStatusCode.Forbidden,
                    UserMessage = ResponseResource.Failure(),
                    DevMessage = ResponseResource.Failure(),
                    Data = user
                };

            // Unblock user
            user.Role = UserRoles.User;
            int res = await UserRepository.Update(uid, user);

            // Kiểm tra update thành công
            if (res <= 0) return ServiceResult.ServerError(ResponseResource.UpdateFailure());

            // Trả về thành công
            return ServiceResult.Success(
                ResponseResource.UpdateSuccess(),
                string.Empty,
                user
            );
        }

        public async Task<ServiceResult> AdminUpdateUserInfo(Guid uid, UpdateUserModel model)
        {
            // Kiểm tra uid phải tồn tại trong cơ sở dữ liệu
            User? user = await UserRepository.Get(uid);
            if (user == null) return ServiceResult.BadRequest(ResponseResource.NotExistUser());

            // Kiểm tra user phải có role khác Admin
            if (user.Role == UserRoles.Admin)
                return new ServiceResult()
                {
                    StatusCode = EStatusCode.Forbidden,
                    UserMessage = ResponseResource.Failure(),
                    DevMessage = ResponseResource.Failure(),
                    Data = user
                };

            // Copy dữ liệu từ model vào profile rồi update
            User userToUpdate = (User)user.Clone();
            userToUpdate.Copy(model);
            int res = await UserRepository.Update(uid, userToUpdate);

            // Kiểm tra update thành công
            if (res <= 0) return ServiceResult.ServerError(ResponseResource.UpdateFailure());

            // Trả về thành công
            return ServiceResult.Success(
                ResponseResource.UpdateSuccess(),
                string.Empty,
                userToUpdate
            );
        }

        public async Task<ServiceResult> AdminGetListUser(PaginationDto pagination)
        {
            List<ViewUser> listed = await UserRepository.GetDetail();

            PaginationResponseModel<ViewUser> res = new()
            {
                Total = listed.Count,
                Limit = pagination.Limit,
                Offset = pagination.Offset,
                Results = UserRepository.ApplyPagination(listed, pagination)
            };

            return ServiceResult.Success(ResponseResource.GetMultiSuccess(ResponseTableName), string.Empty, res);
        }

        #endregion



        #region For User
        public async Task<ServiceResult> GetMyUserProfile(Guid myuid)
        {
            // Lấy về profile
            ViewUser? profile = await UserRepository.GetDetail(myuid);
            if (profile == null)
                return ServiceResult.BadRequest(ResponseResource.NotExistUser());

            // Trả về thành công
            return ServiceResult.Success(
                UserMessage: ResponseResource.Success(),
                DevMessage: string.Empty,
                Data: profile
            );
        }

        public async Task<ServiceResult> GetUserDetail(Guid myuid, string unOruid)
        {
            // Kiểm tra profile phải tồn tại
            ViewUser? profile = await UserRepository.GetDetailByUsernameOrUserId(unOruid);
            if (profile == null)
                return ServiceResult.BadRequest(ResponseResource.NotExistUser());

            // Kiểm tra xem có xem được profile của nhau không
            // Kiểm tra block
            if (await UserRelationRepository.CheckBlockEachOther(myuid, profile.UserId))
                return new ServiceResult()
                {
                    IsSuccess = false,
                    StatusCode = EStatusCode.Forbidden,
                    UserMessage = "Không xem được thông tin do bạn đã chặn người dùng này hoặc bạn đã bị người dùng này chặn",
                    DevMessage = ResponseResource.Failure()
                };


            // Trả về thành công
            return ServiceResult.Success(
                UserMessage: ResponseResource.Success(),
                DevMessage: string.Empty,
                Data: profile
            );
        }

        //private static double SimilaritySearch(string searchKey, ViewUser user)
        //{
        //    return Algorithm.LongestCommonSubsequenceContinuous(searchKey, user.FullName);
        //}

        public async Task<ServiceResult> SearchUser(Guid myuid, string searchKey, PaginationDto pagination)
        {
            // Format search key
            searchKey = Unicode.RemoveVietnameseTone(searchKey).ToLower();

            // Lấy về toàn bộ user và Thực hiện truy vấn
            List<ViewUser> lsUser = await UserRepository.GetDetail();

            // Loc khong chan, khong bi banned...
            List<ViewUserRelation> myBlockee = await UserRelationRepository.GetByUserIdAndType(myuid, isActive: false, EUserRelationType.Block);
            List<ViewUserRelation> myBlocker = await UserRelationRepository.GetByUserIdAndType(myuid, isActive: true, EUserRelationType.Block);
            List<Guid> myBlockeeId = myBlockee.Select(mb => mb.SenderId).ToList();
            List<Guid> myBlockerId = myBlocker.Select(mb => mb.ReceiverId).ToList();
            List<Guid> exceptId = myBlockeeId.Union(myBlockerId).Distinct().ToList();
            List<ViewUser> filteredUser = lsUser
                .Where(lsUser => lsUser.Role != UserRoles.Banned && !exceptId.Contains(lsUser.UserId))
                .ToList();
            filteredUser = filteredUser.GroupBy(f => f.UserId).Select(g => g.First()).ToList();

            // Tinh toan similiarity Score
            List<string> listFullname = filteredUser.Select(fu => fu.FullName).ToList();
            List<string> listUsername = filteredUser.Select(fu => fu.Username).ToList();
            List<string> listEmail = filteredUser.Select(fu => fu.Email).ToList();
            List<string> listPhone = filteredUser.Select(fu => fu.PhoneNumber ?? "").ToList();

            Dictionary<string, double> scoreFullName = Algorithm.SimilarityList(searchKey, listFullname);
            Dictionary<string, double> scoreUsername = Algorithm.SimilarityList(searchKey, listUsername);
            Dictionary<string, double> scoreEmail = Algorithm.SimilarityList(searchKey, listEmail);
            Dictionary<string, double> scorePhone = Algorithm.SimilarityList(searchKey, listPhone);

            double fullnameWeight = 0.4, usernameWeight = 0.3, emailWeight = 0.2, phoneWeight = 0.1;

            Dictionary<Guid, double> scored = filteredUser.ToDictionary(
                u => u.UserId,
                u =>    fullnameWeight * scoreFullName[u.FullName] + 
                        usernameWeight * scoreUsername[u.Username] + 
                        emailWeight * scoreEmail[u.Email] + 
                        phoneWeight * scorePhone[u.PhoneNumber ?? ""]
            );

            // Sap xep theo scored:
            filteredUser = [.. filteredUser.OrderByDescending(u => scored[u.UserId])];

            // ap dung filter, pagination (khong ap dung order)
            if (pagination.Filters != null)
                filteredUser = UserRepository.ApplyFilter(filteredUser, pagination.Filters);
            filteredUser = filteredUser.Skip(pagination.Offset ?? 0).Take(pagination.Limit ?? 20).ToList();

            // Decoration:
            List<ResponseUserCardModel> res = await DecorationRepository.DecorateResponseUserCardModel(myuid,
                filteredUser.Select(u =>
                {
                    ResponseUserCardModel rUCM = new();
                    rUCM.Copy(u);
                    return rUCM;
                }).ToList());

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.Success(), string.Empty, res);
        }

        public async Task<ServiceResult> UpdateMyAvatarImage(Guid uid, IFormFile avatar)
        {
            // Kiểm tra uid tồn tại trong database
            ViewUser user = await UserRepository.CheckExistedUser(uid, ResponseResource.NotExistUser());
            Profile profile = new();
            profile.Copy(user);

            // Upload ảnh lên storage lấy về image url
            string? avatarUrl = await Storage.SaveImage(avatar);
            if (avatarUrl == null) return ServiceResult.ServerError(ResponseResource.UpdateFailure());

            _ = await ImageRepository.TryInsertImage(uid, avatarUrl);

            // Cập nhật ảnh đại diện vào db
            profile.Avatar = avatarUrl;
            int row = await ProfileRepository.Update(user.ProfileId, profile);

            // Kiểm tra cập nhật thành công
            if (row <= 0) return ServiceResult.ServerError(ResponseResource.UpdateFailure());
            return ServiceResult.Success(ResponseResource.UpdateSuccess(), string.Empty, profile);
        }

        public async Task<ServiceResult> UpdateMyCoverImage(Guid uid, IFormFile cover)
        {
            // Kiểm tra uid tồn tại trong database
            ViewUser user = await UserRepository.CheckExistedUser(uid, ResponseResource.NotExistUser());
            Profile profile = new();
            profile.Copy(user);

            // Upload file ảnh ra storage và lấy về url của ảnh
            string? coverUrl = await Storage.SaveImage(cover);
            if (coverUrl == null) return ServiceResult.ServerError(ResponseResource.UpdateFailure());

            _ = await ImageRepository.TryInsertImage(uid, coverUrl);

            // Cập nhật url của cover image vào database
            profile.Cover = coverUrl;
            int row = await ProfileRepository.Update(user.ProfileId, profile);

            // Kiểm tra cập nhật thành công
            if (row <= 0) return ServiceResult.ServerError(ResponseResource.UpdateFailure());
            return ServiceResult.Success(ResponseResource.UpdateSuccess(), string.Empty, profile);
        }

        public async Task<ServiceResult> UpdateMyUserProfile(Guid uid, UpdateProfileModel updateModel)
        {
            // Kiểm tra uid tồn tại trong database
            ViewUser user = await UserRepository.CheckExistedUser(uid, ResponseResource.NotExistUser());
            Profile profile = new();
            profile.Copy(user);

            // Copy dữ liệu từ updateModel vào profile rồi update
            profile.Copy(updateModel);
            int res = await ProfileRepository.Update(user.ProfileId, profile);

            // Kiểm tra update thành công
            if (res <= 0) return ServiceResult.ServerError(ResponseResource.UpdateFailure());

            // Trả về thành công
            return ServiceResult.Success(
                ResponseResource.UpdateSuccess(),
                string.Empty,
                profile
            );
        }

        public async Task<ServiceResult> UpdateMyBio(Guid uid, string? newBio)
        {
            if (newBio == null)
                return ServiceResult.BadRequest("Bio không được trống");
            ViewUser user = await UserRepository.CheckExistedUser(uid, ResponseResource.NotExistUser());

            if (newBio == user.Bio)
                return ServiceResult.Success(ResponseResource.UpdateSuccess(), string.Empty, user);

            Profile pro = new();
            pro.Copy(user);
            pro.Bio = newBio;
            int raws = await ProfileRepository.Update(user.ProfileId, pro);
            if (raws <= 0) return ServiceResult.ServerError(ResponseResource.UpdateFailure());
            return ServiceResult.Success(ResponseResource.UpdateSuccess(), string.Empty, pro);
        }

        #endregion
    }
}
