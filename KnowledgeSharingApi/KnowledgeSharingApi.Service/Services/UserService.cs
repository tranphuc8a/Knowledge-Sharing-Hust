using KnowledgeSharingApi.Domains.Algorithms;
using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Interfaces.Storages;
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
        IStorage storage, 
        IUserRelationRepository userRelationRepository
        ) : IUserService
    {
        #region Attributes
        protected readonly IUserRepository UserRepository = userRepository;
        protected readonly IProfileRepository ProfileRepository = profileRepository;
        protected readonly IResourceFactory ResourceFactory = resourceFactory;
        protected readonly IResponseResource ResponseResource = resourceFactory.GetResponseResource();
        protected readonly IUserRelationRepository UserRelationRepository = userRelationRepository;
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

            // Kiểm tra user phải có role bằng Banned
            if (user.Role != UserRoles.Banned)
                return new ServiceResult()
                {
                    StatusCode = EStatusCode.Forbidden,
                    UserMessage = ResponseResource.Failure(),
                    DevMessage = ResponseResource.Failure(),
                    Data = user
                };

            // Block user
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

        public async Task<ServiceResult> AdminSearchUser(string searchKey, int? limit, int? offset)
        {
            // Format limit và offset
            int offsetValue = offset ??= 0;
            int limitValue = limit ??= DefaultLimit;
            searchKey = searchKey.ToLower();

            // Lấy về toàn bộ user và Thực hiện truy vấn
            IEnumerable<ViewUser> lsUser = await UserRepository.GetDetail();
            IEnumerable<ViewUser> filteredUser = lsUser
                .Select(user => new
                {
                    User = user,
                    similarityScore = SimilaritySearch(searchKey, user)
                })
                .OrderByDescending(user => user.similarityScore)
                .Select(user => user.User)
                .ToList();

            // Lọc không chặn, làm sau

            // Trả về thành công
            PaginationResponseModel<ViewUser> res = new()
            {
                Total = filteredUser.Count(),
                Limit = limitValue,
                Offset = offsetValue,
                Results = filteredUser.Skip(offsetValue).Take(limitValue)
            };
            return ServiceResult.Success(ResponseResource.Success(), string.Empty, res);
        }

        public async Task<ServiceResult> AdminUnblockUser(Guid uid)
        {
            // Kiểm tra uid phải tồn tại trong cơ sở dữ liệu
            User? user = await UserRepository.Get(uid);
            if (user == null) return ServiceResult.BadRequest(ResponseResource.NotExistUser());

            // Kiểm tra user phải có role bằng User
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
            user.Copy(model);
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

        public async Task<ServiceResult> AdminGetListUser(int? limit, int? offset)
        {
            int limitValue = limit ?? DefaultLimit;
            int offsetValue = offset ?? 0;

            IEnumerable<ViewUser> listed = await UserRepository.GetDetail();

            PaginationResponseModel<ViewUser> res = new()
            {
                Total = listed.Count(),
                Limit = limitValue,
                Offset = offsetValue,
                Results = listed.Skip(offsetValue).Take(limitValue)
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
                    UserMessage = ResponseResource.Failure(),
                    DevMessage = ResponseResource.Failure()
                };


            // Trả về thành công
            return ServiceResult.Success(
                UserMessage: ResponseResource.Success(),
                DevMessage: string.Empty,
                Data: profile
            );
        }

        private static double SimilaritySearch(string searchKey, ViewUser user)
        {
            return Algorithm.LongestCommonSubsequenceContinuous(searchKey, user.FullName);
        }

        public async Task<ServiceResult> SearchUser(Guid myuid, string searchKey, int? limit, int? offset)
        {
            // Format limit và offset
            int offsetValue = offset ??= 0;
            int limitValue = limit ??= DefaultLimit;
            searchKey = searchKey.ToLower();

            // Lấy về toàn bộ user và Thực hiện truy vấn
            IEnumerable<ViewUser> lsUser = await UserRepository.GetDetail();
            IEnumerable<ViewUser> filteredUser = lsUser
                .Where(lsUser => lsUser.Role != UserRoles.Banned)
                .Select(user => new
                {
                    User = user,
                    similarityScore = SimilaritySearch(searchKey, user)
                })
                .OrderByDescending(user => user.similarityScore)
                .Select(user => user.User)
                .ToList();

            // Lọc không chặn, làm sau

            // Trả về thành công
            PaginationResponseModel<ViewUser> res = new()
            {
                Total = filteredUser.Count(),
                Limit = limitValue,
                Offset = offsetValue,
                Results = filteredUser.Skip(offsetValue).Take(limitValue)
            };
            return ServiceResult.Success(ResponseResource.Success(), string.Empty, res);
        }

        public async Task<ServiceResult> UpdateMyAvatarImage(Guid uid, IFormFile avatar)
        {
            // Kiểm tra uid tồn tại trong database
            Profile? profile = await ProfileRepository.Get(uid);
            if (profile == null) return ServiceResult.BadRequest(ResponseResource.NotExistUser());

            // Upload ảnh lên storage lấy về image url
            string? avatarUrl = await Storage.SaveImage(avatar);
            if (avatarUrl == null) return ServiceResult.BadRequest(ResponseResource.UpdateFailure());

            // Cập nhật ảnh đại diện vào db
            profile.Avatar = avatarUrl;
            int row = await ProfileRepository.Update(uid, profile);

            // Kiểm tra cập nhật thành công
            if (row <= 0) return ServiceResult.BadRequest(ResponseResource.UpdateFailure());
            return ServiceResult.Success(ResponseResource.UpdateSuccess(), string.Empty, profile);
        }

        public async Task<ServiceResult> UpdateMyCoverImage(Guid uid, IFormFile cover)
        {
            // Kiểm tra uid phải tồn tại trong db
            Profile? profile = await ProfileRepository.Get(uid);
            if (profile == null) return ServiceResult.BadRequest(ResponseResource.NotExistUser());

            // Upload file ảnh ra storage và lấy về url của ảnh
            string coverUrl = await Storage.SaveImage(cover);
            if (coverUrl == null) return ServiceResult.BadRequest(ResponseResource.UpdateFailure());

            // Cập nhật url của cover image vào database
            profile.Cover = coverUrl;
            int row = await ProfileRepository.Update(uid, profile);

            // Kiểm tra cập nhật thành công
            if (row <= 0) return ServiceResult.BadRequest(ResponseResource.UpdateFailure());
            return ServiceResult.Success(ResponseResource.UpdateSuccess(), string.Empty, profile);
        }

        public async Task<ServiceResult> UpdateMyUserProfile(Guid uid, UpdateProfileModel updateModel)
        {
            // Kiểm tra uid phải tồn tại trong cơ sở dữ liệu
            Profile? profile = await ProfileRepository.Get(uid);
            if (profile == null) return ServiceResult.BadRequest(ResponseResource.NotExistUser());

            // Copy dữ liệu từ updateModel vào profile rồi update
            profile.Copy(updateModel);
            int res = await ProfileRepository.Update(uid, profile);

            // Kiểm tra update thành công
            if (res <= 0) return ServiceResult.ServerError(ResponseResource.UpdateFailure());

            // Trả về thành công
            return ServiceResult.Success(
                ResponseResource.UpdateSuccess(),
                string.Empty,
                profile
            );
        }


        #endregion
    }
}
