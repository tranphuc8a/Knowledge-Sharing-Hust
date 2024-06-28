using KnowledgeSharingApi.Domains.Algorithms;
using KnowledgeSharingApi.Domains.Annotations.Validators;
using KnowledgeSharingApi.Domains.Common;
using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.UserProfileModels;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using KnowledgeSharingApi.Repositories.Interfaces.DecorationRepositories;
using KnowledgeSharingApi.Infrastructures.Interfaces.Storages;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.UserIteractionRepositories;
using KnowledgeSharingApi.Services.Interfaces.UserIteractions;
using KnowledgeSharingApi.Services.Interfaces;

namespace KnowledgeSharingApi.Services.Services.UserIterations
{
    public class UserService(
        IResourceFactory resourceFactory,
        IUserRepository userRepository,
        IProfileRepository profileRepository,
        IDecorationRepository decorationRepository,
        IImageRepository imageRepository,
        IStorage storage,
        ICalculateSearchScoreService calculateSearchScoreService,
        IUserRelationRepository userRelationRepository
        ) : IUserService
    {
        #region Attributes
        protected readonly IUserRepository UserRepository = userRepository;
        protected readonly ICalculateSearchScoreService CalculateSearchScoreService = calculateSearchScoreService;
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
        public virtual async Task<ServiceResult> AdminBlockUser(Guid uid)
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
            user.ModifiedBy = "PhucTV";
            user.ModifiedTime = DateTime.UtcNow;
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

        public virtual async Task<ServiceResult> AdminDeleteUser(Guid uid)
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

        public virtual async Task<ServiceResult> AdminGetUserProfile(string unOruid)
        {
            // Lấy về profile
            ViewUserProfile? profile = await UserRepository.GetDetailByUsernameOrUserId(unOruid);

            if (profile == null)
                return ServiceResult.BadRequest(ResponseResource.NotExistUser());

            // Trả về thành công
            return ServiceResult.Success(
                UserMessage: ResponseResource.Success(),
                DevMessage: string.Empty,
                Data: profile
            );
        }

        public virtual async Task<ServiceResult> AdminSearchUser(string searchKey, PaginationDto pagination)
        {
            // Format search key
            searchKey = Unicode.RemoveVietnameseTone(searchKey).ToLower();

            // Lấy về toàn bộ user và Thực hiện truy vấn
            List<ViewUserProfile> lsUser = await UserRepository.GetDetail();
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

        public virtual async Task<ServiceResult> AdminUnblockUser(Guid uid)
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
            user.ModifiedTime = DateTime.UtcNow;
            user.ModifiedBy = "PhucTV";
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

        public virtual async Task<ServiceResult> PromoteToAdmin(Guid uid)
        {
            User u = await UserRepository.CheckExisted(uid, ResponseResource.NotExistUser());

            if (u.Role == UserRoles.Banned)
                return ServiceResult.BadRequest("Người dùng đang bị khóa tài khoản");
            if (u.Role == UserRoles.Admin)
                return ServiceResult.BadRequest("Người dùng đang là quản trị viên rồi");

            await UserRepository.PromoteToAdmin(uid);
            return ServiceResult.Success(ResponseResource.Success());
        }


        public virtual async Task<ServiceResult> AdminUpdateUserInfo(Guid uid, UpdateUserModel model)
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
            userToUpdate.ModifiedTime = DateTime.UtcNow;
            userToUpdate.ModifiedBy = "PhucTV";
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

        public virtual async Task<ServiceResult> AdminGetListUser(PaginationDto pagination)
        {
            List<ViewUserProfile> listed = await UserRepository.GetDetail();

            PaginationResponseModel<ViewUserProfile> res = new()
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
        public virtual async Task<ServiceResult> GetMyUserProfile(Guid myuid)
        {
            // Lấy về profile
            ViewUserProfile? profile = await UserRepository.GetDetail(myuid);
            if (profile == null)
                return ServiceResult.BadRequest(ResponseResource.NotExistUser());

            // Trả về thành công
            return ServiceResult.Success(
                UserMessage: ResponseResource.Success(),
                DevMessage: string.Empty,
                Data: profile
            );
        }

        public virtual async Task<ServiceResult> GetUserDetail(Guid myuid, string unOruid)
        {
            // Kiểm tra profile phải tồn tại
            ViewUserProfile? profile = await UserRepository.GetDetailByUsernameOrUserId(unOruid);
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

        //private static double SimilaritySearch(string searchKey, ViewUserProfile user)
        //{
        //    return Algorithm.LongestCommonSubsequenceContinuous(searchKey, user.FullName);
        //}

        public virtual async Task<ServiceResult> SearchUser(Guid? myuid, string searchKey, PaginationDto pagination)
        {
            // Lấy về toàn bộ user và Thực hiện truy vấn
            List<(Guid UserId, string FullName, string Username, string Email, string? PhoneNumber, string Role, string? Avatar, string? Cover)> lsUser = 
                (await UserRepository.GetDetail(user => 
                    new { user.UserId, user.FullName, user.Username, user.Email, user.PhoneNumber, user.Role, user.Avatar, user.Cover }))
                .Select(item => (item.UserId, item.FullName, item.Username, item.Email, item.PhoneNumber, item.Role, item.Avatar, item.Cover))
                .ToList();
            List<(Guid UserId, string FullName, string Username, string Email, string? PhoneNumber, string Role, string? Avatar, string? Cover) > filteredUser;

            // Loc khong chan, khong bi banned...
            if (myuid != null)
            {
                List<ViewUserRelation> myBlocks = await UserRelationRepository.GetBlocksByUserId(myuid.Value);
                List<Guid> myBlocksId = myBlocks.Select(mb =>
                {
                    return mb.SenderId == myuid ? mb.ReceiverId : mb.SenderId;
                }).ToList();
                List<Guid> exceptId = myBlocksId.Distinct().ToList();
                filteredUser = lsUser
                    .Where(lsUser => lsUser.Role != UserRoles.Banned && !exceptId.Contains(lsUser.UserId))
                    .ToList();
            }
            else
            {
                filteredUser = lsUser;
            }
            filteredUser = filteredUser.GroupBy(f => f.UserId).Select(g => g.First()).ToList();
            Dictionary<Guid, double> scored = CalculateSearchScoreService.CalculateUserScore(
                searchKey,
                filteredUser.Select(item => (item.UserId, item.FullName, item.Username, item.Email, item.PhoneNumber, (int?) null, (int?) null))
                .ToList());

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
                    ResponseUserCardModel rUCM = new()
                    {
                        UserId = u.UserId,
                        FullName = u.FullName,
                        Username = u.Username,
                        Email = u.Email,
                        PhoneNumber = u.PhoneNumber,
                        Role = u.Role,
                        Avatar = u.Avatar,
                        Cover = u.Cover
                    };
                    return rUCM;
                }).ToList());

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.Success(), string.Empty, res);
        }

        public virtual async Task<ServiceResult> UpdateMyAvatarImage(Guid uid, UploadImageModel avatar)
        {
            // Kiểm tra uid tồn tại trong database
            ViewUserProfile user = await UserRepository.CheckExistedUser(uid, ResponseResource.NotExistUser());
            Profile profile = new();
            profile.Copy(user);
            profile.ModifiedBy = user.Username;
            profile.ModifiedTime = DateTime.UtcNow;

            // Upload ảnh lên storage lấy về image url
            string? avatarUrl = await Storage.SaveImage(avatar.Image);
            if (avatarUrl == null) return ServiceResult.ServerError(ResponseResource.UpdateFailure());

            _ = await ImageRepository.TryInsertImage(uid, avatarUrl);

            // Cập nhật ảnh đại diện vào db
            profile.Avatar = avatarUrl;
            int row = await ProfileRepository.Update(user.ProfileId, profile);

            // Kiểm tra cập nhật thành công
            if (row <= 0) return ServiceResult.ServerError(ResponseResource.UpdateFailure());
            return ServiceResult.Success(ResponseResource.UpdateSuccess(), string.Empty, profile);
        }

        public virtual async Task<ServiceResult> UpdateMyCoverImage(Guid uid, UploadImageModel cover)
        {
            // Kiểm tra uid tồn tại trong database
            ViewUserProfile user = await UserRepository.CheckExistedUser(uid, ResponseResource.NotExistUser());
            Profile profile = new();
            profile.Copy(user);
            profile.ModifiedTime = DateTime.UtcNow;
            profile.ModifiedBy = user.Username;

            // Upload file ảnh ra storage và lấy về url của ảnh
            string? coverUrl = await Storage.SaveImage(cover.Image);
            if (coverUrl == null) return ServiceResult.ServerError(ResponseResource.UpdateFailure());

            _ = await ImageRepository.TryInsertImage(uid, coverUrl);

            // Cập nhật url của cover image vào database
            profile.Cover = coverUrl;
            int row = await ProfileRepository.Update(user.ProfileId, profile);

            // Kiểm tra cập nhật thành công
            if (row <= 0) return ServiceResult.ServerError(ResponseResource.UpdateFailure());
            return ServiceResult.Success(ResponseResource.UpdateSuccess(), string.Empty, profile);
        }

        public virtual async Task<ServiceResult> UpdateMyUserProfile(Guid uid, UpdateProfileModel updateModel)
        {
            // Kiểm tra uid tồn tại trong database
            ViewUserProfile user = await UserRepository.CheckExistedUser(uid, ResponseResource.NotExistUser());
            Profile profile = new();
            profile.Copy(user);

            // Copy dữ liệu từ updateModel vào profile rồi update
            profile.Copy(updateModel);
            profile.ModifiedBy = user.Username;
            profile.ModifiedTime = DateTime.UtcNow;
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

        public virtual async Task<ServiceResult> UpdateMyBio(Guid uid, string? newBio)
        {
            if (newBio == null)
                return ServiceResult.BadRequest("Bio không được trống");
            ViewUserProfile user = await UserRepository.CheckExistedUser(uid, ResponseResource.NotExistUser());

            if (newBio == user.Bio)
                return ServiceResult.Success(ResponseResource.UpdateSuccess(), string.Empty, user);

            Profile pro = new();
            pro.Copy(user);
            pro.Bio = newBio;
            pro.ModifiedTime = DateTime.UtcNow;
            pro.ModifiedBy = user.Username;
            int raws = await ProfileRepository.Update(user.ProfileId, pro);
            if (raws <= 0) return ServiceResult.ServerError(ResponseResource.UpdateFailure());
            return ServiceResult.Success(ResponseResource.UpdateSuccess(), string.Empty, pro);
        }


        public virtual async Task<ServiceResult> UpdateMyAvatarUrl(Guid uid, string? url)
        {
            if (string.IsNullOrEmpty(url)) return ServiceResult.BadRequest(ViConstantResource.URL_EMPTY);
            if (!PropertyValidator.CheckFormatUrl(url)) return ServiceResult.BadRequest(ViConstantResource.URL_FORMAT);

            // Kiểm tra uid tồn tại trong database
            ViewUserProfile user = await UserRepository.CheckExistedUser(uid, ResponseResource.NotExistUser());
            Profile profile = new();
            profile.Copy(user);

            // Copy dữ liệu từ updateModel vào profile rồi update
            profile.Avatar = url;
            profile.ModifiedBy = user.Username;
            profile.ModifiedTime = DateTime.UtcNow;
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

        public virtual async Task<ServiceResult> UpdateMyCoverUrl(Guid uid, string? url)
        {
            if (string.IsNullOrEmpty(url)) return ServiceResult.BadRequest(ViConstantResource.URL_EMPTY);
            if (!PropertyValidator.CheckFormatUrl(url)) return ServiceResult.BadRequest(ViConstantResource.URL_FORMAT);

            // Kiểm tra uid tồn tại trong database
            ViewUserProfile user = await UserRepository.CheckExistedUser(uid, ResponseResource.NotExistUser());
            Profile profile = new();
            profile.Copy(user);

            // Copy dữ liệu từ updateModel vào profile rồi update
            profile.Cover = url;
            profile.ModifiedBy = user.Username;
            profile.ModifiedTime = DateTime.UtcNow;
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

        public virtual async Task<ServiceResult> UpdateMyFullname(Guid uid, string? fullname)
        {
            if (string.IsNullOrEmpty(fullname)) return ServiceResult.BadRequest(ViConstantResource.NAME_EMPTY);

            // Kiểm tra uid tồn tại trong database
            ViewUserProfile user = await UserRepository.CheckExistedUser(uid, ResponseResource.NotExistUser());
            Profile profile = new();
            profile.Copy(user);

            // Copy dữ liệu từ updateModel vào profile rồi update
            profile.FullName = fullname;
            profile.ModifiedBy = user.Username;
            profile.ModifiedTime = DateTime.UtcNow;
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

        public virtual async Task<ServiceResult> UpdateMyGeneralInfo(Guid uid, UpdateGeneralInforModel model)
        {
            // Kiểm tra uid tồn tại trong database
            ViewUserProfile user = await UserRepository.CheckExistedUser(uid, ResponseResource.NotExistUser());
            Profile profile = new();
            profile.Copy(user);

            // Copy dữ liệu từ updateModel vào profile rồi update
            profile.Copy(model);
            profile.ModifiedBy = user.Username;
            profile.ModifiedTime = DateTime.UtcNow;
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


        #endregion
    }
}
