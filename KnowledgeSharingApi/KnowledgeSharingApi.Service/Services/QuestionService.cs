using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.CreateUserItemModels;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.UpdateUserItemModels;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Interfaces.UnitOfWorks;
using KnowledgeSharingApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Services
{
    public class QuestionService(
        IQuestionRepository questionRepository,
        ICourseRepository courseRepository,
        IKnowledgeRepository knowledgeRepository,
        IResourceFactory resourceFactory
    ) : IQuestionService
    {
        protected readonly IResourceFactory ResourceFactory = resourceFactory;
        protected readonly IResponseResource ResponseResource = resourceFactory.GetResponseResource();
        protected readonly IQuestionRepository QuestionRepository = questionRepository;
        protected readonly ICourseRepository CourseRepository = courseRepository;
        protected readonly IKnowledgeRepository KnowledgeRepository = knowledgeRepository;
//        protected readonly IUnitOfWork UnitOfWork = unitOfWork

        protected readonly string QuestionResource = resourceFactory.GetEntityResource().Question();
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
        protected virtual async Task<ResponseQuestionItemModel> DecoratePost(ViewQuestion post)
        {
            ResponseQuestionItemModel model = new();
            model.Copy(post);
            PaginationResponseModel<ViewComment> listComments =
                await KnowledgeRepository.GetListComments(model.KnowledgeId.ToString(), NumberOfTopComments, 0);
            model.NumberComments = listComments.Total;
            model.TopComments = listComments.Results;

            model.Star = await KnowledgeRepository.GetAverageStar(model.KnowledgeId.ToString());
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
        protected virtual async Task<List<ResponseQuestionItemModel>> DecoratePost(IEnumerable<ViewQuestion> posts)
        {
            List<ResponseQuestionItemModel> res = [];
            foreach (ViewQuestion post in posts)
            {
                res.Add(await DecoratePost(post));
            }
            return res;
        }

        #region Admin Apies
        public async Task<ServiceResult> AdminDeletePost(string postId)
        {
            // Kiểm tra câu hỏi tồn tại
            _ = await QuestionRepository.CheckExistedQuestion(postId, ResponseResource.NotExist(QuestionResource));

            int res = await QuestionRepository.Delete(postId);
            if (res <= 0) return ServiceResult.ServerError(ResponseResource.DeleteFailure(QuestionResource));

            return ServiceResult.Success(ResponseResource.DeleteSuccess());
        }

        public async Task<ServiceResult> AdminGetListPostsOfCourse(string courseId, int? limit, int? offset)
        {
            string CourseResource = ResourceFactory.GetEntityResource().Course();
            _ = await CourseRepository.CheckExisted(courseId, ResponseResource.NotExist(CourseResource));

            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            IEnumerable<ViewQuestion> listQuestion = await QuestionRepository.GetQuestionInCourse(courseId);

            PaginationResponseModel<ViewQuestion> res = new()
            {
                Total = listQuestion.Count(),
                Limit = limitValue,
                Offset = offsetValue,
                Results = await DecoratePost(
                    listQuestion.Skip(offsetValue).Take(limitValue)
                )
            };

            return ServiceResult.Success(
                ResponseResource.GetMultiSuccess(QuestionResource),
                string.Empty,
                res
            );
        }

        public Task<ServiceResult> AdminGetPostDetail(string postId)
        {

        }

        public Task<ServiceResult> AdminGetPosts(int? limit, int? offset)
        {

        }

        public Task<ServiceResult> AdminGetUserPosts(string userId, int? limit, int? offset)
        {

        }
        #endregion

        #region Anonymous APIes
        public Task<ServiceResult> AnonymousGetPostDetail(string postId)
        {

        }

        public Task<ServiceResult> AnonymousGetPosts(int? limit, int? offset)
        {

        }

        public Task<ServiceResult> AnonymousGetUserPosts(string userId, int? limit, int? offset)
        {

        }
        #endregion

        #region User APIes
        public Task<ServiceResult> ConfirmQuestion(string myUid, string questionId, bool isConfirm)
        {

        }

        public Task<ServiceResult> UserCreatePost(string myUid, CreatePostModel model)
        {

        }

        public Task<ServiceResult> UserDeletePost(string myUid, string postId)
        {

        }

        #region User Gets
        public Task<ServiceResult> UserGetListPostsOfCourse(string myUid, string courseId, int? limit, int? offset)
        {

        }

        public Task<ServiceResult> UserGetMyPostDetail(string myUid, string postId)
        {

        }

        public Task<ServiceResult> UserGetMyPosts(string myUid, int? limit, int? offset)
        {

        }

        public Task<ServiceResult> UserGetPostDetail(string myUid, string postId)
        {

        }

        public Task<ServiceResult> UserGetPosts(string myUid, int? limit, int? offset)
        {

        }

        public Task<ServiceResult> UserGetUserPosts(string myUid, string userId, int? limit, int? offset)
        {

        }
        #endregion

        public Task<ServiceResult> UserUpdatePost(string myUid, string postId, UpdatePostModel model)
        {

        }
        #endregion
    }
}
