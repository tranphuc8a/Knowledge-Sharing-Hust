using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Services.Interfaces;
using MimeKit.Tnef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Services
{
    public class StarService : IStarService
    {
        protected readonly IResourceFactory ResourceFactory;
        protected readonly IEntityResource EntityResource;
        protected readonly IResponseResource ResponseResource;

        protected readonly IStarRepository StarRepository;
        protected readonly IKnowledgeRepository KnowledgeRepository;
        protected readonly IUserItemRepository UserItemRepository;
        protected readonly IUserRepository UserRepository;


        protected readonly int DefaultLimit = 20;
        protected readonly string StarResourcse, NotExistedItem;

        public StarService(
            IResourceFactory resourceFactory,
            IStarRepository starRepository,
            IKnowledgeRepository knowledgeRepository,
            IUserItemRepository userItemRepository,
            IUserRepository userRepository
        )
        {
            ResourceFactory = resourceFactory;
            EntityResource = ResourceFactory.GetEntityResource();
            ResponseResource = ResourceFactory.GetResponseResource();

            StarRepository = starRepository;
            KnowledgeRepository = knowledgeRepository;
            UserItemRepository = userItemRepository;
            UserRepository = userRepository;

            StarResourcse = EntityResource.Star();
            NotExistedItem = ResponseResource.NotExist(EntityResource.UserItem());
        }

        protected virtual async Task<IEnumerable<ResponseStarModel>> Decorate
            (IEnumerable<Star> listStars, bool isDecorateUser = false, bool isDecorateItem = false)
        {
            Dictionary<Guid, ResponseUserCardModel>? mapUsers = null;
            Dictionary<Guid, ResponseUserItemModel>? mapItems = null;
            if (isDecorateUser)
            {
                mapUsers = [];
                // Get Dictionary <userid, ViewUser -> ResponseUserCardModel>
                Dictionary<Guid, ViewUser?> listUsers = await UserRepository
                    .GetDetail(listStars.Select(star => star.UserId).ToArray());
                foreach (var item in listUsers)
                {
                    ViewUser? user = item.Value;
                    ResponseUserCardModel resUser = new();
                    if (user != null) resUser.Copy(user);
                    mapUsers[item.Key] = resUser;
                }
            }
            if (isDecorateItem)
            {
                // Get Dictionay <useritemid, UserItem -> ResponseUserItemModel>
            }
            IEnumerable<ResponseStarModel> res = listStars.Select(star =>
            {
                ResponseStarModel resStar = new();
                resStar.Copy(star);
                // decorate list user
                if (isDecorateUser)
                {
                    resStar.User = mapUsers?[star.UserId];
                }

                // decorate list item
                if (isDecorateItem)
                {
                    resStar.Item = mapItems?[star.UserItemId];
                }

                return resStar;
            }).ToList();
            return res;
        }

        public async Task<ServiceResult> AdminGetUserItemStars(Guid userItemId, int? limit, int? offset)
        {
            // CHeck userItem existed:
            _ = await UserItemRepository.CheckExisted(userItemId, NotExistedItem);

            IEnumerable<Star> listStars = await StarRepository.GetListStarOfUserItem(userItemId);
            int total = listStars.Count();
            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            listStars = listStars.Skip(offsetValue).Take(limitValue);
            PaginationResponseModel<ResponseStarModel> res = new()
            {
                Total = total,
                Limit = limitValue,
                Offset = offsetValue,
                Results = await Decorate(listStars, isDecorateUser: true, isDecorateItem: false)
            };
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(StarResourcse), string.Empty, res);
        }

        public async Task<ServiceResult> AnonymousGetUserItemStars(Guid userItemId, int? limit, int? offset)
        {
            // Kiểm tra quyền truy cập
            UserItem item = await UserItemRepository.CheckExisted(userItemId, NotExistedItem);
            if (item.UserItemType == EUserItemType.Knowledge)
            {
                Knowledge knowledge = await KnowledgeRepository.CheckExisted(userItemId, NotExistedItem);
                if (knowledge.Privacy != EPrivacy.Public)
                    return ServiceResult.Forbidden("Không có quyền truy cập tài nguyên này");
            }

            // Trả về giống như admin lấy:
            return await AdminGetUserItemStars(userItemId, limit, offset);
        }

        public async Task<ServiceResult> UserGetMyScoredComments(Guid myUid, int? limit, int? offset)
        {
            // Lấy về
            IEnumerable<Tuple<ViewComment, Star>> stars = await StarRepository.GetStaredComments(myUid);
            int total = stars.Count();
            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            stars = stars.Skip(offsetValue).Take(limitValue);
            IEnumerable<ResponseStarModel> results = stars.Select(star =>
            {
                ResponseStarModel model = (ResponseStarModel) new ResponseStarModel().Copy(star.Item2);
                model.Item = (ResponseCommentModel) new ResponseCommentModel().Copy(star.Item1);
                return model;
            });

            // Decorate
            PaginationResponseModel<ResponseStarModel> res = new()
            {
                Total = total,
                Limit = limitValue,
                Offset = offsetValue,
                Results = results
            };

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.GetMultiFailure(StarResourcse), string.Empty, res);
        }

        public async Task<ServiceResult> UserGetMyScoredCourses(Guid myUid, int? limit, int? offset)
        {
            // Lấy về
            IEnumerable<Tuple<ViewCourse, Star>> stars = await StarRepository.GetStaredCourses(myUid);
            int total = stars.Count();
            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            stars = stars.Skip(offsetValue).Take(limitValue);
            IEnumerable<ResponseStarModel> results = stars.Select(star =>
            {
                ResponseStarModel model = (ResponseStarModel)new ResponseStarModel().Copy(star.Item2);
                model.Item = (ResponseCourseModel)new ResponseCourseModel().Copy(star.Item1);
                return model;
            });

            // Decorate
            PaginationResponseModel<ResponseStarModel> res = new(total, limitValue, offsetValue, results);

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.GetMultiFailure(StarResourcse), string.Empty, res);
        }

        public async Task<ServiceResult> UserGetMyScoredLessons(Guid myUid, int? limit, int? offset)
        {
            // Lấy về
            IEnumerable<Tuple<ViewLesson, Star>> stars = await StarRepository.GetStaredLessons(myUid);
            int total = stars.Count();
            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            stars = stars.Skip(offsetValue).Take(limitValue);
            IEnumerable<ResponseStarModel> results = stars.Select(star =>
            {
                ResponseStarModel model = (ResponseStarModel)new ResponseStarModel().Copy(star.Item2);
                model.Item = (ResponseLessonModel)new ResponseLessonModel().Copy(star.Item1);
                return model;
            });

            // Decorate
            PaginationResponseModel<ResponseStarModel> res = new(total, limitValue, offsetValue, results);

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.GetMultiFailure(StarResourcse), string.Empty, res);
        }

        public async Task<ServiceResult> UserGetMyScoredPosts(Guid myUid, int? limit, int? offset)
        {
            // Lấy về
            IEnumerable<Tuple<ViewPost, Star>> stars = await StarRepository.GetStaredPosts(myUid);
            int total = stars.Count();
            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            stars = stars.Skip(offsetValue).Take(limitValue);
            IEnumerable<ResponseStarModel> results = stars.Select(star =>
            {
                ResponseStarModel model = (ResponseStarModel)new ResponseStarModel().Copy(star.Item2);
                model.Item = (ResponsePostModel)new ResponsePostModel().Copy(star.Item1);
                return model;
            });

            // Decorate
            PaginationResponseModel<ResponseStarModel> res = new(total, limitValue, offsetValue, results);

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.GetMultiFailure(StarResourcse), string.Empty, res);
        }

        public async Task<ServiceResult> UserGetMyScoredQuestions(Guid myUid, int? limit, int? offset)
        {
            // Lấy về
            IEnumerable<Tuple<ViewQuestion, Star>> stars = await StarRepository.GetStaredQuestions(myUid);
            int total = stars.Count();
            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            stars = stars.Skip(offsetValue).Take(limitValue);
            IEnumerable<ResponseStarModel> results = stars.Select(star =>
            {
                ResponseStarModel model = (ResponseStarModel)new ResponseStarModel().Copy(star.Item2);
                model.Item = (ResponseQuestionModel)new ResponseQuestionModel().Copy(star.Item1);
                return model;
            });

            // Decorate
            PaginationResponseModel<ResponseStarModel> res = new(total, limitValue, offsetValue, results);

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.GetMultiFailure(StarResourcse), string.Empty, res);
        }

        public async Task<ServiceResult> UserGetMyScoredUserItems(Guid myUid, int? limit, int? offset)
        {
            // Get về và phân trang
            IEnumerable<Star> listStars = await StarRepository.GetListStarOfUser(myUid);
            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            int total = listStars.Count();
            listStars = listStars.Skip(offsetValue).Take(limitValue);

            // Decorate
            List<ResponseStarModel> res = [];
            foreach (Star star in listStars)
            {
                ResponseStarModel resStar = (ResponseStarModel)new ResponseStarModel().Copy(star);
                resStar.Item = await UserItemRepository.GetExactlyResponseUserItemModel(star.UserItemId);
                res.Add(resStar);
            }

            // Return Response
            PaginationResponseModel<ResponseStarModel> pageRes = new(total, limitValue, offsetValue, res);
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(StarResourcse), string.Empty, pageRes);
        }

        public async Task<ServiceResult> UserGetUserItemStars(Guid myUid, Guid userItemId, int? limit, int? offset)
        {
            // Kiểm tra quyền truy cập
            UserItem item = await UserItemRepository.CheckExisted(userItemId, NotExistedItem);
            if (item.UserItemType == EUserItemType.Knowledge)
            {
                _ = await KnowledgeRepository.CheckExisted(userItemId, NotExistedItem);
                bool isAccessible = await KnowledgeRepository.CheckAccessible(myUid, userItemId);
                if (!isAccessible)
                    return ServiceResult.Forbidden("Bạn không có quyền truy cập tài nguyên này");
            }

            // Trả về như admin lấy
            return await AdminGetUserItemStars(userItemId, limit, offset);
        }

        public async Task<ServiceResult> UserPutScores(Guid myUid, PutScoreModel scoreModel)
        {
            // Kiểm tra score hợp lệ
            int? score = scoreModel.Score;
            if (score == null || score < 0 || score > 5)
                return ServiceResult.BadRequest("Số sao không hợp lệ");

            // Kiểm tra item tồn tại và quyền truy cập
            Guid userItemId = scoreModel.UserItemId ?? Guid.Empty;
            UserItem item = await UserItemRepository.CheckExisted(userItemId, NotExistedItem);
            if (item.UserItemType == EUserItemType.Knowledge)
            {
                Knowledge knowledge = await KnowledgeRepository.CheckExisted(userItemId, NotExistedItem);
                bool isAccessible = await KnowledgeRepository.CheckAccessible(myUid, userItemId);
                if (!isAccessible)
                    return ServiceResult.Forbidden("Bạn không có quyền truy cập tài nguyên này");
            }

            // Kiểm tra đã star trước đó chưa
            Star? star = await StarRepository.GetStarOfUserAndUserItem(myUid, userItemId);
            if (star == null)
            {
                // Chưa: Add
                star = new Star()
                {
                    // Entity:
                    CreatedBy = myUid.ToString(),
                    CreatedTime = DateTime.Now,
                    // Star:
                    StarId = Guid.NewGuid(),
                    UserId = myUid,
                    UserItemId = userItemId,
                    Stars = scoreModel.Score ?? 0
                };
                Guid? res = await StarRepository.Insert(star.StarId, star);
                if (res == null) return ServiceResult.ServerError(ResponseResource.InsertFailure(StarResourcse));
            } else
            {
                // Rồi: Update
                star.Stars = scoreModel.Score ?? 0;
                await StarRepository.Update(star.StarId, star);
            }

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.Success());
        }
    }
}
