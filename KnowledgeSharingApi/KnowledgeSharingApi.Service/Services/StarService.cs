using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.DecorationRepositories;
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
        protected readonly IPostRepository PostRepository;
        protected readonly IDecorationRepository DecorationRepository;
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
            IUserRepository userRepository,
            IPostRepository postRepository,
            IDecorationRepository decorationRepository
        )
        {
            ResourceFactory = resourceFactory;
            EntityResource = ResourceFactory.GetEntityResource();
            ResponseResource = ResourceFactory.GetResponseResource();

            StarRepository = starRepository;
            KnowledgeRepository = knowledgeRepository;
            UserItemRepository = userItemRepository;
            UserRepository = userRepository;
            PostRepository = postRepository;
            DecorationRepository = decorationRepository;

            StarResourcse = EntityResource.Star();
            NotExistedItem = ResponseResource.NotExist(EntityResource.UserItem());
        }

        public async Task<ServiceResult> AdminGetUserItemStars(Guid userItemId, PaginationDto pagination)
        {
            // CHeck userItem existed:
            _ = await UserItemRepository.CheckExisted(userItemId, NotExistedItem);

            List<Star> listStars = await StarRepository.GetListStarOfUserItem(userItemId);
            int total = listStars.Count;
            listStars = StarRepository.ApplyPagination(listStars, pagination);
            PaginationResponseModel<ResponseStarModel> res = new()
            {
                Total = total,
                Limit = pagination.Limit,
                Offset = pagination.Offset,
                Results = await DecorationRepository
                    .DecorateResponseStarModel(null, listStars, isDecorateUser: true, isDecorateItem: false)
            };
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(StarResourcse), string.Empty, res);
        }

        public async Task<ServiceResult> AnonymousGetUserItemStars(Guid userItemId, PaginationDto pagination)
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
            return await AdminGetUserItemStars(userItemId, pagination);
        }

        public async Task<ServiceResult> UserGetMyScoredComments(Guid myUid, PaginationDto pagination)
        {
            // Lấy về
            List<Tuple<ViewComment, Star>> stars = await StarRepository.GetStaredComments(myUid);
            int total = stars.Count;
            stars = StarRepository.ApplyPagination(stars, pagination);
            List<ResponseStarModel> results = stars.Select(star =>
            {
                ResponseStarModel model = (ResponseStarModel) new ResponseStarModel().Copy(star.Item2);
                model.Item = (ResponseCommentModel) new ResponseCommentModel().Copy(star.Item1);
                return model;
            }).ToList();

            // DecorateResponseLessonModel
            PaginationResponseModel<ResponseStarModel> res = new()
            {
                Total = total,
                Limit = pagination.Limit,
                Offset = pagination.Offset,
                Results = results
            };

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.GetMultiFailure(StarResourcse), string.Empty, res);
        }

        public async Task<ServiceResult> UserGetMyScoredCourses(Guid myUid, PaginationDto pagination)
        {
            // Lấy về
            List<Tuple<ViewCourse, Star>> stars = await StarRepository.GetStaredCourses(myUid);
            int total = stars.Count;
            stars = StarRepository.ApplyPagination(stars, pagination);
            List<ResponseStarModel> results = stars.Select(star =>
            {
                ResponseStarModel model = (ResponseStarModel)new ResponseStarModel().Copy(star.Item2);
                model.Item = (ResponseCourseModel)new ResponseCourseModel().Copy(star.Item1);
                return model;
            }).ToList();

            // DecorateResponseLessonModel
            PaginationResponseModel<ResponseStarModel> res = new(total, pagination.Limit, pagination.Offset, results);

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.GetMultiFailure(StarResourcse), string.Empty, res);
        }

        public async Task<ServiceResult> UserGetMyScoredLessons(Guid myUid, PaginationDto pagination)
        {
            // Lấy về
            List<Tuple<ViewLesson, Star>> stars = await StarRepository.GetStaredLessons(myUid);
            int total = stars.Count;
            stars = StarRepository.ApplyPagination(stars, pagination);
            List<ResponseStarModel> results = stars.Select(star =>
            {
                ResponseStarModel model = (ResponseStarModel)new ResponseStarModel().Copy(star.Item2);
                model.Item = (ResponseLessonModel)new ResponseLessonModel().Copy(star.Item1);
                return model;
            }).ToList();

            // DecorateResponseLessonModel
            PaginationResponseModel<ResponseStarModel> res = new(total, pagination.Limit, pagination.Offset, results);

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.GetMultiFailure(StarResourcse), string.Empty, res);
        }

        public async Task<ServiceResult> UserGetMyScoredPosts(Guid myUid, PaginationDto pagination)
        {
            // Lấy về
            List<Tuple<ViewPost, Star>> stars = (await StarRepository.GetStaredPosts(myUid));
            int total = stars.Count;
            stars = StarRepository.ApplyPagination(stars, pagination);
            
            Dictionary<Guid, IResponseUserItemModel?> itemsDict = await PostRepository
                .GetExactlyResponseUserItemModel(stars.Select(star => star.Item1.UserItemId)
                .ToList());
            List<ResponseStarModel> results = stars.Select(star =>
            {
                ResponseStarModel model = (ResponseStarModel)new ResponseStarModel().Copy(star.Item2);
                if (itemsDict.TryGetValue(model.Item?.UserItemId ?? Guid.Empty, out IResponseUserItemModel? value))
                {
                    model.Item = (ResponsePostModel?) value;
                }
                return model;
            }).ToList();

            // DecorateResponseLessonModel
            PaginationResponseModel<ResponseStarModel> res = new(total, pagination.Limit, pagination.Offset, results);

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.GetMultiFailure(StarResourcse), string.Empty, res);
        }

        public async Task<ServiceResult> UserGetMyScoredQuestions(Guid myUid, PaginationDto pagination)
        {
            // Lấy về
            List<Tuple<ViewQuestion, Star>> stars = await StarRepository.GetStaredQuestions(myUid);
            int total = stars.Count;
            stars = StarRepository.ApplyPagination(stars, pagination);
            List<ResponseStarModel> results = stars.Select(star =>
            {
                ResponseStarModel model = (ResponseStarModel)new ResponseStarModel().Copy(star.Item2);
                model.Item = (ResponseQuestionModel)new ResponseQuestionModel().Copy(star.Item1);
                return model;
            }).ToList();

            // DecorateResponseLessonModel
            PaginationResponseModel<ResponseStarModel> res = new(total, pagination.Limit, pagination.Offset, results);

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.GetMultiFailure(StarResourcse), string.Empty, res);
        }

        public async Task<ServiceResult> UserGetMyScoredUserItems(Guid myUid, PaginationDto pagination)
        {
            // Get về và phân trang
            List<Star> listStars = await StarRepository.GetListStarOfUser(myUid);
            int total = listStars.Count;
            listStars = StarRepository.ApplyPagination(listStars, pagination);

            // DecorateResponseLessonModel
            List<ResponseStarModel> res = await DecorationRepository
                .DecorateResponseStarModel(myUid, listStars, isDecorateUser: false, isDecorateItem: true);

            // Return Response
            PaginationResponseModel<ResponseStarModel> pageRes = new(total, pagination.Limit, pagination.Offset, res);
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(StarResourcse), string.Empty, pageRes);
        }

        public async Task<ServiceResult> UserGetUserItemStars(Guid myUid, Guid userItemId, PaginationDto pagination)
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
            return await AdminGetUserItemStars(userItemId, pagination);
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
                _ = await KnowledgeRepository.CheckExisted(userItemId, NotExistedItem);
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
            } 
            else
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
