using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Services.Interfaces;
using OfficeOpenXml.Drawing.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Services
{
    public class PostService(
        IPostRepository postRepository,
        IUserRepository userRepository,
        IResourceFactory resourceFactory,
        IQuestionService questionService,
        IKnowledgeRepository knowledgeRepository,
        ILessonService lessonService
    ) : IPostService
    {
        protected readonly IPostRepository PostRepository = postRepository;
        protected readonly IUserRepository UserRepository = userRepository;
        protected readonly IResourceFactory ResourceFactory = resourceFactory;
        protected readonly IQuestionService QuestionService = questionService;
        protected readonly ILessonService LessonService = lessonService;
        protected readonly IKnowledgeRepository KnowledgeRepository = knowledgeRepository;

        protected readonly IResponseResource ResponseResource = resourceFactory.GetResponseResource();
        protected readonly IEntityResource EntityResource = resourceFactory.GetEntityResource();

        protected readonly string PostResource = resourceFactory.GetEntityResource().Post();
        protected readonly int DefaultLimit = 20;
        protected readonly int NumberOfTopComments = 5;


        /// <summary>
        /// Thêm các trường thông tin bổ sung cho Post
        /// số sao, số bình luận, top bình luận
        /// </summary>
        /// <param name="post"> post gốc </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        protected virtual async Task<ResponsePostItemModel> DecoratePost(ViewPost post)
        {
            ResponsePostItemModel model = new();
            model.Copy(post);
            PaginationResponseModel<ViewComment> listComments =
                await KnowledgeRepository.GetListComments(model.KnowledgeId, NumberOfTopComments, 0);
            model.NumberComments = listComments.Total;
            model.TopComments = listComments.Results;

            model.Star = await KnowledgeRepository.GetAverageStar(model.KnowledgeId);
            return model;
        }

        /// <summary>
        /// Thêm các trường thông tin bổ sung cho danh sách Post
        /// số sao, số bình luận, top bình luận
        /// </summary>
        /// <param name="posts"> Danh sách posts gốc </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        protected virtual async Task<List<ResponsePostItemModel>> DecoratePost(IEnumerable<ViewPost> posts)
        {
            List<ResponsePostItemModel> res = [];
            foreach (ViewPost post in posts)
            {
                res.Add(await DecoratePost(post));
            }
            return res;
        }

        #region Admin APIes



        public async Task<ServiceResult> AdminDeletePost(Guid postId)
        {
            // Kiểm tra post tồn tại
            Post post = await PostRepository.CheckExisted(postId, ResponseResource.NotExist(PostResource));
            
            // Lấy về service thực hiện tương ứng
            IBasePostService basePostService = post.PostType == EPostType.Lesson ? LessonService : QuestionService;

            // Trả về kết quả 
            return await basePostService.AdminDeletePost(postId);
        }

        public async Task<ServiceResult> AdminGetPosts(int? limit, int? offset)
        {
            int offsetValue = offset ?? 0;
            int limitValue = limit ?? DefaultLimit;
            IEnumerable<ViewPost> posts = await PostRepository.GetViewPost(limitValue, offsetValue);

            return ServiceResult.Success(
                ResponseResource.GetSuccess(PostResource),
                string.Empty,
                await DecoratePost(posts)
            );
        }

        public async Task<ServiceResult> AdminGetUserPosts(Guid userId, int? limit, int? offset)
        {
            _ = await UserRepository.CheckExistedUser(userId, ResponseResource.NotExistUser());

            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;

            IEnumerable<ViewPost> posts = 
                (await PostRepository.GetByUserId(userId))
                .Skip(offsetValue).Take(limitValue);
            
            return ServiceResult.Success(
                ResponseResource.GetMultiSuccess(PostResource),
                string.Empty,
                await DecoratePost(posts)
            );
        }

        #endregion

        #region Anonymous APIes

        public async Task<ServiceResult> AnonymousGetPosts(int? limit, int? offset)
        {
            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            IEnumerable<ViewPost> posts = await PostRepository.GetPublicPosts(limitValue, offsetValue);

            return ServiceResult.Success(
                ResponseResource.GetMultiSuccess(PostResource),
                string.Empty,
                await DecoratePost(posts)
            );
        }

        public async Task<ServiceResult> AnonymousGetUserPosts(Guid userId, int? limit, int? offset)
        {
            ViewUser user = await UserRepository.CheckExistedUser(userId, ResponseResource.NotExistUser());

            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            IEnumerable<ViewPost> posts = 
                (await PostRepository.GetPublicPostsByUserId(user.UserId))
                .Skip(offsetValue).Take(limitValue);

            List<ResponsePostItemModel> res = [];
            foreach (ViewPost post in posts)
            {
                res.Add(await DecoratePost(post));
            }

            return ServiceResult.Success(
                ResponseResource.GetMultiSuccess(PostResource),
                string.Empty,
                res
            );
        }

        #endregion


        #region User APIes
        public async Task<ServiceResult> UserDeletePost(Guid myUid, Guid postId)
        {
            Post post = await PostRepository.CheckExisted(postId, ResponseResource.NotExist(PostResource));

            IBasePostService basePostService = post.PostType == EPostType.Lesson ? LessonService : QuestionService;

            return await basePostService.UserDeletePost(myUid, postId);
        }

        public async Task<ServiceResult> UserGetMyPosts(Guid myUid, int? limit, int? offset)
        {
            int offsetValue = offset ?? 0, limitValue = limit ?? DefaultLimit;
            ViewUser user = await UserRepository.CheckExistedUser(myUid, ResponseResource.NotExistUser());

            IEnumerable<ViewPost> listPosts = 
                (await PostRepository.GetByUserId(user.UserId))
                .Skip(offsetValue).Take(limitValue);

            return ServiceResult.Success(
                ResponseResource.GetMultiSuccess(PostResource),
                string.Empty,
                await DecoratePost(listPosts)
            );
        }

        public async Task<ServiceResult> UserGetPosts(Guid myUid, int? limit, int? offset)
        {
            // Tìm kiếm và lọc thông minh hơn cho myUid

            _ = await UserRepository.CheckExistedUser(myUid, ResponseResource.NotExistUser());

            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            IEnumerable<ViewPost> posts = await PostRepository.GetPublicPosts(limitValue, offsetValue);
            // Tương tự anonymous chỉ lấy những post public
            // Tuy nhiên cần cải tiến để lấy thông minh hơn với những infor của myUid

            return ServiceResult.Success(
                ResponseResource.GetMultiSuccess(PostResource),
                string.Empty,
                await DecoratePost(posts)
            );
        }

        public async Task<ServiceResult> UserGetUserPosts(Guid myUid, Guid userId, int? limit, int? offset)
        {
            _ = await UserRepository.CheckExistedUser(myUid, ResponseResource.NotExistUser());
            _ = await UserRepository.CheckExistedUser(userId, ResponseResource.NotExistUser());

            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            IEnumerable<ViewPost> posts =
                (await PostRepository.GetPublicPostsByUserId(userId))
                .Skip(offsetValue).Take(limitValue);

            return ServiceResult.Success(
                ResponseResource.GetMultiSuccess(PostResource),
                string.Empty,
                await DecoratePost(posts)
            );
        }
        #endregion


        #region Get list posts of a category

        public async Task<ServiceResult> AnonymousGetListPostsOfCategory(string catName, int? limit, int? offset)
        {
            IEnumerable<ViewPost> posts = await PostRepository.GetPublicPostsOfCategory(catName, limit ?? DefaultLimit, offset ?? 0);

            return ServiceResult.Success(
                ResponseResource.GetMultiSuccess(),
                string.Empty,
                await DecoratePost(posts)
            );
        }

        public async Task<ServiceResult> UserGetListPostsOfCategory(Guid myUid, string catName, int? limit, int? offset)
        {
            IEnumerable<ViewPost> posts = await PostRepository.GetPostsOfCategory(myUid, catName, limit ?? DefaultLimit, offset ?? 0);

            return ServiceResult.Success(
                ResponseResource.GetMultiSuccess(),
                string.Empty,
                await DecoratePost(posts)
            );
        }

        public async Task<ServiceResult> AdminGetListPostsOfCategory(string catName, int? limit, int? offset)
        {
            IEnumerable<ViewPost> posts = await PostRepository.GetPostsOfCategory(catName, limit ?? DefaultLimit, offset ?? 0);

            return ServiceResult.Success(
                ResponseResource.GetMultiSuccess(),
                string.Empty,
                await DecoratePost(posts)
            );
        }
        #endregion
    }
}
