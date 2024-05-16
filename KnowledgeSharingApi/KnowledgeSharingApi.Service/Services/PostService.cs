using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.DecorationRepositories;
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
        IDecorationRepository decorationRepository,
        IKnowledgeRepository knowledgeRepository,
        ILessonService lessonService
    ) : IPostService
    {
        protected readonly IPostRepository PostRepository = postRepository;
        protected readonly IUserRepository UserRepository = userRepository;
        protected readonly IResourceFactory ResourceFactory = resourceFactory;
        protected readonly IDecorationRepository DecorationRepository = decorationRepository;
        protected readonly IQuestionService QuestionService = questionService;
        protected readonly ILessonService LessonService = lessonService;
        protected readonly IKnowledgeRepository KnowledgeRepository = knowledgeRepository;

        protected readonly IResponseResource ResponseResource = resourceFactory.GetResponseResource();
        protected readonly IEntityResource EntityResource = resourceFactory.GetEntityResource();

        protected readonly string PostResource = resourceFactory.GetEntityResource().Post();
        protected readonly int DefaultLimit = 20;
        protected readonly int NumberOfTopComments = 5;


        ///// <summary>
        ///// Thêm các trường thông tin bổ sung cho Post
        ///// số sao, số bình luận, top bình luận
        ///// </summary>
        ///// <param name="post"> post gốc </param>
        ///// <returns></returns>
        ///// Created: PhucTV (24/3/24)
        ///// Modified: None
        //protected virtual async Task<ResponsePostModel> DecorationRepository.DecorateResponsePostModel(ViewPost post)
        //{
        //    ResponsePostModel model = new();
        //    model.Copy(post);
        //    PaginationResponseModel<ViewComment> listComments =
        //        await KnowledgeRepository.GetListComments(model.UserItemId, NumberOfTopComments, 0);
        //    model.NumberComments = listComments.Total;
        //    model.TopComments = listComments.Results.Select(com =>
        //    {
        //        ResponseCommentModel rescom = new();
        //        rescom.Copy(com);
        //        return rescom;
        //    });

        //    model.AverageStars = await KnowledgeRepository.GetAverageStar(model.UserItemId);
        //    return model;
        //}

        

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

        public async Task<ServiceResult> AdminGetPosts(PaginationDto pagination)
        {
            List<ViewPost> posts = await PostRepository.GetViewPost(pagination);

            return ServiceResult.Success(
                ResponseResource.GetSuccess(PostResource),
                string.Empty,
                await DecorationRepository.DecorateResponsePostModel(null, posts)
            );
        }

        public async Task<ServiceResult> AdminGetUserPosts(Guid userId, PaginationDto pagination)
        {
            _ = await UserRepository.CheckExistedUser(userId, ResponseResource.NotExistUser());

            List<ViewPost> posts = await PostRepository.GetByUserId(userId);
            posts = PostRepository.ApplyPagination(posts, pagination);
            
            return ServiceResult.Success(
                ResponseResource.GetMultiSuccess(PostResource),
                string.Empty,
                await DecorationRepository.DecorateResponsePostModel(null, posts)
            );
        }

        #endregion



        #region Anonymous APIes

        public async Task<ServiceResult> AnonymousGetPosts(PaginationDto pagination)
        {
            List<ViewPost> posts = await PostRepository.GetPublicPosts(pagination);

            return ServiceResult.Success(
                ResponseResource.GetMultiSuccess(PostResource),
                string.Empty,
                (await DecorationRepository.DecorateResponsePostModel(null, posts))
                     //.OfType<IResponseUserItemModel>()
            );
        }

        public async Task<ServiceResult> AnonymousGetUserPosts(Guid userId, PaginationDto pagination)
        {
            ViewUser user = await UserRepository.CheckExistedUser(userId, ResponseResource.NotExistUser());

            List<ViewPost> posts = await PostRepository.GetPublicPostsByUserId(user.UserId);
            posts = PostRepository.ApplyPagination(posts, pagination);

            return ServiceResult.Success(
                ResponseResource.GetMultiSuccess(PostResource),
                string.Empty,
                await DecorationRepository.DecorateResponsePostModel(null, posts)
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

        public async Task<ServiceResult> UserGetMyPosts(Guid myUid, PaginationDto pagination)
        {
            //ViewUser user = await UserRepository.CheckExistedUser(myUid, ResponseResource.NotExistUser());

            List<ViewPost> listPosts = await PostRepository.GetByUserId(myUid);
            listPosts = PostRepository.ApplyPagination(listPosts, pagination);

            return ServiceResult.Success(
                ResponseResource.GetMultiSuccess(PostResource),
                string.Empty,
                await DecorationRepository.DecorateResponsePostModel(myUid, listPosts)
            );
        }

        public async Task<ServiceResult> UserGetPosts(Guid myUid, PaginationDto pagination)
        {
            // Tìm kiếm và lọc thông minh hơn cho myUid

            //_ = await UserRepository.CheckExistedUser(myUid, ResponseResource.NotExistUser());

            List<ViewPost> posts = await PostRepository.GetPublicPosts(pagination);
            // Tương tự anonymous chỉ lấy những post public
            // Tuy nhiên cần cải tiến để lấy thông minh hơn với những infor của myUid

            return ServiceResult.Success(
                ResponseResource.GetMultiSuccess(PostResource),
                string.Empty,
                await DecorationRepository
                    .DecorateResponsePostModel(myUid, posts)
            );
        }

        public async Task<ServiceResult> UserGetUserPosts(Guid myUid, Guid userId, PaginationDto pagination)
        {
            //_ = await UserRepository.CheckExistedUser(myUid, ResponseResource.NotExistUser());
            _ = await UserRepository.CheckExistedUser(userId, ResponseResource.NotExistUser());

            List<ViewPost> posts = await PostRepository.GetPublicPostsByUserId(userId);
            posts = PostRepository.ApplyPagination(posts, pagination);

            return ServiceResult.Success(
                ResponseResource.GetMultiSuccess(PostResource),
                string.Empty,
                await DecorationRepository.DecorateResponsePostModel(myUid, posts)
            );
        }
        #endregion




        #region Get list posts of a category

        public async Task<ServiceResult> AnonymousGetListPostsOfCategory(string catName, PaginationDto pagination)
        {
            List<ViewPost> posts = await PostRepository
                .GetPublicPostsOfCategory(catName, pagination);

            return ServiceResult.Success(
                ResponseResource.GetMultiSuccess(),
                string.Empty,
                await DecorationRepository.DecorateResponsePostModel(null, posts)
            );
        }

        public async Task<ServiceResult> UserGetListPostsOfCategory(Guid myUid, string catName, PaginationDto pagination)
        {
            List<ViewPost> posts = await PostRepository.GetPostsOfCategory(myUid, catName, pagination);

            return ServiceResult.Success(
                ResponseResource.GetMultiSuccess(),
                string.Empty,
                await DecorationRepository.DecorateResponsePostModel(myUid, posts)
            );
        }

        public async Task<ServiceResult> AdminGetListPostsOfCategory(string catName, PaginationDto pagination)
        {
            List<ViewPost> posts = await PostRepository.GetPostsOfCategory(catName, pagination);

            return ServiceResult.Success(
                ResponseResource.GetMultiSuccess(),
                string.Empty,
                await DecorationRepository.DecorateResponsePostModel(null, posts)
            );
        }

        public async Task<ServiceResult> UserGetMyMarkedPosts(Guid myUid, PaginationDto pagination)
        {
            List<ViewPost> listed = await PostRepository.GetMarkedPosts(myUid);
            int total = listed.Count;
            listed = PostRepository.ApplyPagination(listed, pagination);
            PaginationResponseModel<IResponsePostModel> res = new()
            {
                Total = total,
                Limit = pagination.Limit,
                Offset = pagination.Offset,
                Results = await DecorationRepository.DecorateResponsePostModel(myUid, listed)
            };
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(PostResource), string.Empty, res);
        }
        #endregion
    }
}
