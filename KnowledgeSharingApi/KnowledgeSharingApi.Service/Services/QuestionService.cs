using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.CreateUserItemModels;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.UpdateUserItemModels;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.DecorationRepositories;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Interfaces.UnitOfWorks;
using KnowledgeSharingApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;

namespace KnowledgeSharingApi.Services.Services
{
    public class QuestionService : IQuestionService
    {
        protected readonly IResourceFactory ResourceFactory;
        protected readonly IResponseResource ResponseResource;
        protected readonly IEntityResource EntityResource;
        protected readonly IQuestionRepository QuestionRepository;
        protected readonly ICourseRepository CourseRepository;
        protected readonly IKnowledgeRepository KnowledgeRepository;
        protected readonly IDecorationRepository DecorationRepository;
        protected readonly IUserRepository UserRepository;
        //        protected readonly IUnitOfWork UnitOfWork = unitOfWork

        protected readonly string QuestionResource;
        protected readonly int DefaultLimit = 20;
        protected readonly int NumberOfTopComments = 5;
        protected readonly string NotExistedQuestion, GetQuestionSuccess, GetMultiQuestionSuccess;

        public QuestionService(
            IQuestionRepository questionRepository,
            ICourseRepository courseRepository,
            IKnowledgeRepository knowledgeRepository,
            IUserRepository userRepository,
            IDecorationRepository decorationRepository,
            IResourceFactory resourceFactory
    )
        {
            ResourceFactory = resourceFactory;
            QuestionRepository = questionRepository;
            CourseRepository = courseRepository;
            KnowledgeRepository = knowledgeRepository;
            DecorationRepository = decorationRepository;
            UserRepository = userRepository;
            ResponseResource = resourceFactory.GetResponseResource();
            EntityResource = resourceFactory.GetEntityResource();
            QuestionResource = EntityResource.Question();

            NotExistedQuestion = ResponseResource.NotExist(QuestionResource);
            GetQuestionSuccess = ResponseResource.GetSuccess(QuestionResource);
            GetMultiQuestionSuccess = ResponseResource.GetMultiSuccess(QuestionResource);
        }


        #region Admin Apies
        public async Task<ServiceResult> AdminDeletePost(Guid postId)
        {
            // Kiểm tra câu hỏi tồn tại
            _ = await QuestionRepository.CheckExistedQuestion(postId, NotExistedQuestion);

            int res = await QuestionRepository.Delete(postId);
            if (res <= 0) return ServiceResult.ServerError(ResponseResource.DeleteFailure(QuestionResource));

            return ServiceResult.Success(ResponseResource.DeleteSuccess());
        }

        public async Task<ServiceResult> AdminGetListPostsOfCourse(Guid courseId, int? limit, int? offset)
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
                Results = await DecorationRepository.DecorateResponseQuestionModel(
                    null,
                    listQuestion.Skip(offsetValue).Take(limitValue).ToList()
                )
            };

            return ServiceResult.Success(
                GetMultiQuestionSuccess,
                string.Empty,
                res
            );
        }

        public async Task<ServiceResult> AdminGetPostDetail(Guid postId)
        {
            ViewQuestion question = await QuestionRepository.CheckExistedQuestion(postId, NotExistedQuestion);

            ResponseQuestionModel questionItemModel = (await DecorationRepository.DecorateResponseQuestionModel(null, [question])).First();

            return ServiceResult.Success(
                GetQuestionSuccess, string.Empty, questionItemModel);
        }

        public async Task<ServiceResult> AdminGetPosts(int? limit, int? offset)
        {
            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            IEnumerable<ViewQuestion> questions = await QuestionRepository.GetViewPost(limitValue, offsetValue);
            IEnumerable<ResponseQuestionModel> res = await DecorationRepository.DecorateResponseQuestionModel(null, questions.ToList());
            return ServiceResult.Success(GetMultiQuestionSuccess, string.Empty, res);
        }

        public async Task<ServiceResult> AdminGetUserPosts(Guid userId, int? limit, int? offset)
        {
            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            _ = await UserRepository.CheckExistedUser(userId, ResponseResource.NotExistUser());

            IEnumerable<ViewQuestion> questions = await QuestionRepository.GetByUserId(userId);
            questions = questions.Skip(offsetValue).Take(limitValue);

            IEnumerable<ResponseQuestionModel> res = await DecorationRepository.DecorateResponseQuestionModel(null, questions.ToList());
            return ServiceResult.Success(GetMultiQuestionSuccess, string.Empty, res);
        }
        #endregion

        #region Anonymous APIes
        public async Task<ServiceResult> AnonymousGetPostDetail(Guid postId)
        {
            ViewQuestion question = await QuestionRepository.CheckExistedQuestion(postId, NotExistedQuestion);

            if (question.Privacy != EPrivacy.Public)
                return ServiceResult.Forbidden("Câu hỏi này đang ở chế độ riêng tư");

            return ServiceResult.Success(GetQuestionSuccess, string.Empty, 
                (await DecorationRepository.DecorateResponseQuestionModel(null, [question])).First());
        }

        public async Task<ServiceResult> AnonymousGetPosts(int? limit, int? offset)
        {
            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            IEnumerable<ViewQuestion> questions = await QuestionRepository.GetPublicPosts(limitValue, offsetValue);

            return ServiceResult.Success(GetMultiQuestionSuccess, string.Empty, 
                await DecorationRepository.DecorateResponseQuestionModel(null, questions.ToList()));
        }

        public async Task<ServiceResult> AnonymousGetUserPosts(Guid userId, int? limit, int? offset)
        {
            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            _ = await UserRepository.CheckExistedUser(userId, ResponseResource.NotExistUser());

            IEnumerable<ViewQuestion> questions = await QuestionRepository.GetPublicPostsByUserId(userId);
            questions = questions.Skip(offsetValue).Take(limitValue);

            return ServiceResult.Success(GetMultiQuestionSuccess, string.Empty, 
                await DecorationRepository.DecorateResponseQuestionModel(null, questions.ToList()));
        }
        #endregion

        #region User APIes
        public async Task<ServiceResult> ConfirmQuestion(Guid myUid, Guid questionId, bool isConfirm)
        {
            ViewQuestion question = await QuestionRepository.CheckExistedQuestion(questionId, NotExistedQuestion);
            if (question.UserId != myUid)
                return ServiceResult.Forbidden("Đây không phải bài thảo luận của bạn");
            question.IsAccept = isConfirm;
            Question questionToUpdate = new();
            questionToUpdate.Copy(question);
            await QuestionRepository.Update(questionId, questionToUpdate);
            return ServiceResult.Success(ResponseResource.UpdateSuccess());
        }

        public async Task<ServiceResult> UserCreatePost(Guid myUid, CreatePostModel model)
        {
            ViewUser user = await UserRepository.CheckExistedUser(myUid, ResponseResource.NotExistUser());

            if (model is not CreateQuestionModel questionModel)
                throw new NotMatchTypeException();

            // Kiểm tra Member tồn tại và user phài join course
            if (questionModel.CourseId.HasValue)
            {
                Guid courseId = questionModel.CourseId.Value;
                _ = await CourseRepository.CheckExisted(courseId, ResponseResource.NotExist(EntityResource.Course()));

                // Kiểm tra user đã join course hay chưa, đợi course repository viết
                ViewCourseRegister? courseRegister = await CourseRepository.GetViewCourseRegister(myUid, courseId);
                if (courseRegister == null)
                    return ServiceResult.Forbidden("Bạn chưa đăng ký tham gia khóa học này");
            }

            // OK, tạo câu hỏi
            Guid newId = Guid.NewGuid();
            Question question = new()
            {
                // Entity:
                CreatedBy = user.FullName,
                CreatedTime = DateTime.Now,
                // UserItem:
                UserItemId = newId,
                UserId = user.UserId,
                UserItemType = EUserItemType.Knowledge,
                // Knowledge:
                KnowledgeType = EKnowledgeType.Post,
                Privacy = questionModel.CourseId != null ? EPrivacy.Private : EPrivacy.Public,
                Views = 0,
                IsBlockComment = false,
                // Post:
                PostType = EPostType.Question,
                // Question
                IsAccept = false
            };
            question.Copy(model);

            // Insert categories nếu có:
            // ...

            // Insert câu hỏi
            Guid? inserted = await QuestionRepository.Insert(question);
            if (inserted == null) return ServiceResult.ServerError(ResponseResource.InsertFailure(QuestionResource));

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.InsertSuccess(QuestionResource));
        }

        public async Task<ServiceResult> UserDeletePost(Guid myUid, Guid postId)
        {
            // Kiểm tra user tồn tại
            // Không nhất thiết

            // Kiểm tra post tồn tại
            ViewQuestion question = await QuestionRepository.CheckExistedQuestion(postId, NotExistedQuestion);

            // Kiểm tra user là chủ post
            if (question.UserId != myUid)
                return ServiceResult.Forbidden("Đây không phải khóa học của bạn");

            // OK thực hiện xóa
            int res = await QuestionRepository.Delete(postId);
            if (res <= 0) return ServiceResult.ServerError(ResponseResource.DeleteFailure(QuestionResource));

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.DeleteSuccess(QuestionResource));
        }

        #region User Gets
        public async Task<ServiceResult> UserGetListPostsOfCourse(Guid myUid, Guid courseId, int? limit, int? offset)
        {
            // Kiểm tra course tồn tại và user phải tham gia course
            Course course = await CourseRepository.CheckExisted(courseId,
                   ResponseResource.NotExist(EntityResource.Course()));

            // Kiểm tra user đã join course hay chưa
            ViewCourseRegister? registerCourse = await CourseRepository.GetViewCourseRegister(myUid, courseId);
            if (registerCourse == null)
                return ServiceResult.Forbidden("Bạn chưa đăng ký tham gia khóa học này");

            // Lấy về danh sách câu hỏi trong course
            IEnumerable<ViewQuestion> questions = await QuestionRepository.GetQuestionInCourse(courseId);
            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            questions = questions.Skip(limitValue).Take(offsetValue);

            // Trả về thành công
            return ServiceResult.Success(GetMultiQuestionSuccess, string.Empty, 
                await DecorationRepository.DecorateResponseQuestionModel(myUid, questions.ToList()));
        }

        public async Task<ServiceResult> UserGetListPostsOfMyCourse(Guid myUid, Guid courseId, int? limit, int? offset)
        {
            // Kiểm tra user tồn tại (không nhất thiết)

            // Kiểm tra khóa học tồn tại và phải là khóa học của mình
            Course course = await CourseRepository.CheckExisted(courseId,
                   ResponseResource.NotExist(EntityResource.Course()));
            if (course.UserId != myUid)
                return ServiceResult.Forbidden("Bạn không phải chủ của khóa học này");

            // Lấy về danh sách câu hỏi trong course
            IEnumerable<ViewQuestion> questions = await QuestionRepository.GetQuestionInCourse(courseId);
            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            questions = questions.Skip(limitValue).Take(offsetValue);

            // Trả về thành công
            return ServiceResult.Success(GetMultiQuestionSuccess, string.Empty, 
                await DecorationRepository.DecorateResponseQuestionModel(myUid, questions.ToList()));
        }

        public async Task<ServiceResult> UserGetMyPostDetail(Guid myUid, Guid postId)
        {
            // Kiểm tra post tồn tại
            ViewQuestion question = await QuestionRepository.CheckExistedQuestion(postId, NotExistedQuestion);

            // Kiểm tra chủ nhân
            if (question.UserId != myUid)
                return ServiceResult.Forbidden("Bạn không phải chủ nhân của bài thảo luận này");

            // Trả về thành công
            return ServiceResult.Success(GetQuestionSuccess, string.Empty, 
                (await DecorationRepository.DecorateResponseQuestionModel(myUid, [question])).First());
        }

        public async Task<ServiceResult> UserGetMyPosts(Guid myUid, int? limit, int? offset)
        {
            // Kiểm tra user tồn tại
            _ = await UserRepository.CheckExistedUser(myUid, ResponseResource.NotExistUser());

            // Lấy về và trả về thành công
            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            IEnumerable<ViewQuestion> questions = await QuestionRepository.GetByUserId(myUid);
            questions = questions.Skip(offsetValue).Take(limitValue);

            return ServiceResult.Success(GetMultiQuestionSuccess, string.Empty, 
                await DecorationRepository.DecorateResponseQuestionModel(myUid, questions.ToList()));
        }

        public async Task<ServiceResult> UserGetPostDetail(Guid myUid, Guid postId)
        {
            // Kiểm tra user tồn tại, không nhất thiết

            // Kiểm tra postId tồn tại
            ViewQuestion question = await QuestionRepository.CheckExistedQuestion(postId, NotExistedQuestion);
            if (question.UserId == myUid)
                return await UserGetMyPostDetail(myUid, postId);

            // Kiểm tra nếu question là private thì có chung khóa học không
            if (question.Privacy != EPrivacy.Public)
            {
                // Mặc định là không pass qua filter này
                bool isAccessible = false;
                if (question.CourseId != null)
                {
                    // Cho một cơ hội kiểm tra chung khóa học không
                    ViewCourseRegister? courseRegister = await CourseRepository.GetViewCourseRegister(myUid, question.CourseId.Value);
                    isAccessible = courseRegister != null;
                }
                if (!isAccessible)
                    return ServiceResult.Forbidden("Bài thảo luận này ở trạng thái riêng tư");
            }

            // OK pass qua bộ lọc, Trả về thành công
            return ServiceResult.Success(GetQuestionSuccess, string.Empty,
                (await DecorationRepository.DecorateResponseQuestionModel(myUid, [question])).First());
        }

        public async Task<ServiceResult> UserGetPosts(Guid myUid, int? limit, int? offset)
        {
            // Chỉ lấy public question, tính toán sắp xếp theo độ ưu tiên của riêng myUId, làm sau

            // Lấy về danh sách public question
            IEnumerable<ViewQuestion> questions = await QuestionRepository.GetPublicPosts(limit ?? DefaultLimit, offset ?? 0);

            // Trả về thành công
            return ServiceResult.Success(GetMultiQuestionSuccess, string.Empty, 
                await DecorationRepository.DecorateResponseQuestionModel(myUid, questions.ToList()));
        }

        public async Task<ServiceResult> UserGetUserPosts(Guid myUid, Guid userId, int? limit, int? offset)
        {
            // Kiểm tra myUid tồn tại (không nhất thiết) và userId tồn tại
            _ = await UserRepository.CheckExistedUser(userId, ResponseResource.NotExistUser());

            // Lấy về danh sách userId question? (có lấy trong private question không?)
            // Để đơn giản, hiện chỉ lấy public question
            IEnumerable<ViewQuestion> questions = await QuestionRepository.GetPublicPostsByUserId(userId);
            questions = questions.Skip(offset ?? 0).Take(limit ?? DefaultLimit);

            // Trả về thành công
            return ServiceResult.Success(GetMultiQuestionSuccess, string.Empty,
                await DecorationRepository.DecorateResponseQuestionModel(myUid, questions.ToList()));
        }
        #endregion

        public async Task<ServiceResult> UserUpdatePost(Guid myUid, Guid postId, UpdatePostModel model)
        {
            // Kiểm tra myUId tồn tại (không nhất thiết)

            // Kiểm tra model đúng
            if (model is not UpdateQuestionModel updateModel)
                throw new NotMatchTypeException();

            // Kiểm tra postId tồn tại, và chủ nhân là myUid
            ViewQuestion question = await QuestionRepository.CheckExistedQuestion(postId, NotExistedQuestion);
            if (question.UserId != myUid)
                return ServiceResult.Forbidden("Đây không phải là bài thảo luận của bạn");

            // Cập nhật question
            Question toUpdate = new();
            toUpdate.Copy(model);
            int res = await QuestionRepository.Update(postId, toUpdate);
            if (res <= 0) return ServiceResult.ServerError(ResponseResource.UpdateFailure(QuestionResource));

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.UpdateSuccess(QuestionResource));
        }

        #endregion

        #region Get of a category

        public async Task<ServiceResult> AnonymousGetListPostsOfCategory(string catName, int? limit, int? offset)
        {
            IEnumerable<ViewQuestion> posts = await QuestionRepository.GetPublicPostsOfCategory(catName, limit ?? DefaultLimit, offset ?? 0);

            return ServiceResult.Success(
                ResponseResource.GetMultiSuccess(),
                string.Empty,
                await DecorationRepository.DecorateResponseQuestionModel(null, posts.ToList())
            );
        }

        public async Task<ServiceResult> UserGetListPostsOfCategory(Guid myUid, string catName, int? limit, int? offset)
        {
            IEnumerable<ViewQuestion> posts = await QuestionRepository.GetPostsOfCategory(myUid, catName, limit ?? DefaultLimit, offset ?? 0);

            return ServiceResult.Success(
                ResponseResource.GetMultiSuccess(),
                string.Empty,
                await DecorationRepository.DecorateResponseQuestionModel(myUid, posts.ToList())
            );
        }

        public async Task<ServiceResult> AdminGetListPostsOfCategory(string catName, int? limit, int? offset)
        {
            IEnumerable<ViewQuestion> posts = await QuestionRepository.GetPostsOfCategory(catName, limit ?? DefaultLimit, offset ?? 0);

            return ServiceResult.Success(
                ResponseResource.GetMultiSuccess(),
                string.Empty,
                await DecorationRepository.DecorateResponseQuestionModel(null, posts.ToList())
            );
        }

        public async Task<ServiceResult> UserGetMyMarkedPosts(Guid myUid, int? limit, int? offset)
        {
            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            IEnumerable<ViewQuestion> listed = await QuestionRepository.GetMarkedPosts(myUid);
            int total = listed.Count();
            listed = listed.Skip(offsetValue).Take(limitValue);
            PaginationResponseModel<ResponseQuestionModel> res = new()
            {
                Total = total,
                Limit = limitValue,
                Offset = offsetValue,
                Results = await DecorationRepository.DecorateResponseQuestionModel(myUid, listed.ToList())
            };
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(QuestionResource), string.Empty, res);
        }

        #endregion
    }
}
