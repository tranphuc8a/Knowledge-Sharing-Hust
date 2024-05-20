using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.CreateUserItemModels;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.UpdateUserItemModels;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.DecorationRepositories;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Interfaces.Storages;
using KnowledgeSharingApi.Infrastructures.Interfaces.UnitOfWorks;
using KnowledgeSharingApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Mysqlx.Crud;
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
        protected readonly ICalculateKnowledgeSearchScore CalculateKnowledgeSearchScore;
        protected readonly ICourseRepository CourseRepository;
        protected readonly IKnowledgeRepository KnowledgeRepository;
        protected readonly IImageRepository ImageRepository;
        protected readonly IStorage Storage;
        protected readonly IDecorationRepository DecorationRepository;
        protected readonly IUserRepository UserRepository;
        protected readonly ICategoryRepository CategoryRepository;
        //        protected readonly IUnitOfWork UnitOfWork = unitOfWork

        protected readonly string QuestionResource;
        protected readonly int DefaultLimit = 20;
        protected readonly int NumberOfTopComments = 5;
        protected readonly string NotExistedQuestion, GetQuestionSuccess, GetMultiQuestionSuccess;

        public QuestionService(
            IQuestionRepository questionRepository,
            ICourseRepository courseRepository,
            ICalculateKnowledgeSearchScore calculateKnowledgeSearchScore,
            IKnowledgeRepository knowledgeRepository,
            IUserRepository userRepository,
            IImageRepository imageRepository,
            IDecorationRepository decorationRepository,
            IStorage storage,
            ICategoryRepository categoryRepository,
            IResourceFactory resourceFactory
        )
        {
            ResourceFactory = resourceFactory;
            QuestionRepository = questionRepository;
            CourseRepository = courseRepository;
            KnowledgeRepository = knowledgeRepository;
            DecorationRepository = decorationRepository;
            CalculateKnowledgeSearchScore = calculateKnowledgeSearchScore;
            CategoryRepository = categoryRepository;
            ImageRepository = imageRepository;
            UserRepository = userRepository;
            Storage = storage;
            ResponseResource = resourceFactory.GetResponseResource();
            EntityResource = resourceFactory.GetEntityResource();
            QuestionResource = EntityResource.Question();

            NotExistedQuestion = ResponseResource.NotExist(QuestionResource);
            GetQuestionSuccess = ResponseResource.GetSuccess(QuestionResource);
            GetMultiQuestionSuccess = ResponseResource.GetMultiSuccess(QuestionResource);
        }

        #region Functional methods

        protected virtual Question CreateQuestion(ViewUser user, CreateQuestionModel questionModel, string? thumbnail)
        {
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
                Thumbnail = thumbnail,
                KnowledgeType = EKnowledgeType.Post,
                Privacy = questionModel.CourseId != null ? EPrivacy.Private : EPrivacy.Public,
                Views = 0,
                IsBlockComment = false,
                // Post:
                PostType = EPostType.Question,
                // Question
                IsAccept = false
            };
            return question;
        }

        #endregion


        #region Admin Apies
        public virtual async Task<ServiceResult> AdminDeletePost(Guid postId)
        {
            // Kiểm tra câu hỏi tồn tại
            _ = await QuestionRepository.CheckExisted(postId, NotExistedQuestion);

            int res = await QuestionRepository.Delete(postId);
            if (res <= 0) return ServiceResult.ServerError(ResponseResource.DeleteFailure(QuestionResource));

            return ServiceResult.Success(ResponseResource.DeleteSuccess());
        }

        public virtual async Task<ServiceResult> AdminGetListPostsOfCourse(Guid courseId, PaginationDto pagination)
        {
            string CourseResource = ResourceFactory.GetEntityResource().Course();
            _ = await CourseRepository.CheckExisted(courseId, ResponseResource.NotExist(CourseResource));

            List<ViewQuestion> listQuestion = await QuestionRepository.GetQuestionInCourse(courseId);

            PaginationResponseModel<IResponseQuestionModel> res = new()
            {
                Total = listQuestion.Count,
                Limit = pagination.Limit,
                Offset = pagination.Offset,
                Results = await DecorationRepository.DecorateResponseQuestionModel(
                    null,
                    QuestionRepository.ApplyPagination(listQuestion, pagination)
                )
            };

            return ServiceResult.Success(
                GetMultiQuestionSuccess,
                string.Empty,
                res
            );
        }

        public virtual async Task<ServiceResult> AdminGetPostDetail(Guid postId)
        {
            ViewQuestion question = await QuestionRepository.CheckExistedQuestion(postId, NotExistedQuestion);

            IResponseQuestionModel questionItemModel = (await DecorationRepository.DecorateResponseQuestionModel(null, [question])).First();

            return ServiceResult.Success(
                GetQuestionSuccess, string.Empty, questionItemModel);
        }

        public virtual async Task<ServiceResult> AdminGetPosts(PaginationDto pagination)
        {
            List<ViewQuestion> questions = await QuestionRepository.GetViewPost(pagination);
            List<IResponseQuestionModel> res = await DecorationRepository.DecorateResponseQuestionModel(null, questions);
            return ServiceResult.Success(GetMultiQuestionSuccess, string.Empty, res);
        }

        public virtual async Task<ServiceResult> AdminGetUserPosts(Guid userId, PaginationDto pagination)
        {
            _ = await UserRepository.CheckExisted(userId, ResponseResource.NotExistUser());

            List<ViewQuestion> questions = await QuestionRepository.GetByUserId(userId, pagination);
            //questions = QuestionRepository.ApplyPagination(questions, pagination);

            List<IResponseQuestionModel> res = await DecorationRepository.DecorateResponseQuestionModel(null, questions);
            return ServiceResult.Success(GetMultiQuestionSuccess, string.Empty, res);
        }
        #endregion

        #region Anonymous APIes
        public virtual async Task<ServiceResult> AnonymousGetPostDetail(Guid postId)
        {
            ViewQuestion question = await QuestionRepository.CheckExistedQuestion(postId, NotExistedQuestion);

            if (question.Privacy != EPrivacy.Public)
                return ServiceResult.Forbidden("Câu hỏi này đang ở chế độ riêng tư");

            return ServiceResult.Success(GetQuestionSuccess, string.Empty, 
                (await DecorationRepository.DecorateResponseQuestionModel(null, [question])).First());
        }

        public virtual async Task<ServiceResult> AnonymousGetPosts(PaginationDto pagination)
        {
            List<ViewQuestion> questions = await QuestionRepository.GetPublicPosts(pagination);

            return ServiceResult.Success(GetMultiQuestionSuccess, string.Empty, 
                await DecorationRepository.DecorateResponseQuestionModel(null, questions));
        }

        public virtual async Task<ServiceResult> AnonymousGetUserPosts(Guid userId, PaginationDto pagination)
        {
            _ = await UserRepository.CheckExisted(userId, ResponseResource.NotExistUser());

            List<ViewQuestion> questions = await QuestionRepository.GetPublicPostsByUserId(userId, pagination);
            //questions = QuestionRepository.ApplyPagination(questions, pagination);

            return ServiceResult.Success(GetMultiQuestionSuccess, string.Empty, 
                await DecorationRepository.DecorateResponseQuestionModel(null, questions));
        }
        #endregion

        #region User APIes
        public virtual async Task<ServiceResult> ConfirmQuestion(Guid myUid, Guid questionId, bool isConfirm)
        {
            Question question = await QuestionRepository.CheckExisted(questionId, NotExistedQuestion);
            if (question.UserId != myUid)
                return ServiceResult.Forbidden("Đây không phải bài thảo luận của bạn");
            question.IsAccept = isConfirm;
            Question questionToUpdate = new();
            questionToUpdate.Copy(question);
            await QuestionRepository.Update(questionId, questionToUpdate);
            return ServiceResult.Success(ResponseResource.UpdateSuccess());
        }

        public virtual async Task<ServiceResult> UserCreatePost(Guid myUid, CreatePostModel model)
        {
            ViewUser user = await UserRepository.CheckExistedUser(myUid, ResponseResource.NotExistUser());

            if (model is not CreateQuestionModel questionModel)
                throw new NotMatchTypeException();

            // Kiểm tra Course tồn tại và user phài join course hoặc là chủ course
            if (questionModel.CourseId.HasValue)
            {
                Guid courseId = questionModel.CourseId.Value;
                Course courseToCheck = await CourseRepository.CheckExisted(courseId, ResponseResource.NotExist(EntityResource.Course()));

                // Kiểm tra user đã join course hay chưa, đợi course repository viết
                if (courseToCheck.UserId != myUid)
                {   // Trong trường hợp không là chủ nhân, check thêm phải join course
                    ViewCourseRegister? courseRegister = await CourseRepository.GetViewCourseRegister(myUid, courseId);
                    if (courseRegister == null)
                        return ServiceResult.Forbidden("Bạn chưa đăng ký tham gia khóa học này");
                }
            }

            // Post thumbnail if existed:
            string? thumbnail = await Storage.SaveImage(model.Thumbnail);
            _ = await ImageRepository.TryInsertImage(myUid, thumbnail);

            // OK, tạo câu hỏi
            Question question = CreateQuestion(user, questionModel, thumbnail);
            question.Copy(model);

            // Insert câu hỏi
            Guid? inserted = await QuestionRepository.Insert(question);
            if (inserted == null) return ServiceResult.ServerError(ResponseResource.InsertFailure(QuestionResource));

            // Insert categories nếu có:
            if (model.Categories != null && model.Categories.Count != 0)
            {
                _ = await CategoryRepository.UpdateKnowledgeCategories(inserted.Value, model.Categories);
            }

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.InsertSuccess(QuestionResource), string.Empty, question);
        }
        
        public virtual async Task<ServiceResult> UserUpdatePost(Guid myUid, Guid postId, UpdatePostModel model)
        {
            // Kiểm tra myUId tồn tại (không nhất thiết)

            // Kiểm tra model đúng
            if (model is not UpdateQuestionModel updateModel)
                throw new NotMatchTypeException();

            // Kiểm tra postId tồn tại, và chủ nhân là myUid
            Question question = await QuestionRepository.CheckExisted(postId, NotExistedQuestion);
            if (question.UserId != myUid)
                return ServiceResult.Forbidden("Đây không phải là bài thảo luận của bạn");


            // update thumbnail:
            string? thumbnail = await Storage.SaveImage(model.Thumbnail);
            _ = await ImageRepository.TryInsertImage(myUid, thumbnail);

            // Cập nhật question
            Question toUpdate = new();
            toUpdate.Copy(question);
            toUpdate.Copy(updateModel);
            if (thumbnail != null) toUpdate.Thumbnail = thumbnail;
            int res1 = await QuestionRepository.Update(postId, toUpdate);
            int res2 = 0;

            // Update categories:
            if (model.Categories != null && model.Categories.Count != 0)
            {
                res2 = await CategoryRepository.UpdateKnowledgeCategories(postId, model.Categories);
            }

            if (res1 + res2 <= 0) return ServiceResult.ServerError(ResponseResource.UpdateFailure(QuestionResource));
            // Trả về thành công
            return ServiceResult.Success(ResponseResource.UpdateSuccess(QuestionResource), string.Empty, toUpdate);
        }

        public virtual async Task<ServiceResult> UserDeletePost(Guid myUid, Guid postId)
        {
            // Kiểm tra user tồn tại
            // Không nhất thiết

            // Kiểm tra post tồn tại
            Question question = await QuestionRepository.CheckExisted(postId, NotExistedQuestion);

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
        public virtual async Task<ServiceResult> UserGetListPostsOfCourse(Guid myUid, Guid courseId, PaginationDto pagination)
        {
            // Kiểm tra course tồn tại và user phải tham gia course
            _ = await CourseRepository.CheckExisted(courseId,
                   ResponseResource.NotExist(EntityResource.Course()));

            // Kiểm tra user đã join course hay chưa
            ViewCourseRegister? registerCourse = await CourseRepository.GetViewCourseRegister(myUid, courseId);
            if (registerCourse == null)
                return ServiceResult.Forbidden("Bạn chưa đăng ký tham gia khóa học này");

            // Lấy về danh sách câu hỏi trong course
            List<ViewQuestion> questions = await QuestionRepository.GetQuestionInCourse(courseId);
            questions = QuestionRepository.ApplyPagination(questions, pagination);

            // Trả về thành công
            return ServiceResult.Success(GetMultiQuestionSuccess, string.Empty, 
                await DecorationRepository.DecorateResponseQuestionModel(myUid, questions));
        }

        public virtual async Task<ServiceResult> UserGetListPostsOfMyCourse(Guid myUid, Guid courseId, PaginationDto pagination)
        {
            // Kiểm tra user tồn tại (không nhất thiết)

            // Kiểm tra khóa học tồn tại và phải là khóa học của mình
            Course course = await CourseRepository.CheckExisted(courseId,
                   ResponseResource.NotExist(EntityResource.Course()));
            if (course.UserId != myUid)
                return ServiceResult.Forbidden("Bạn không phải chủ của khóa học này");

            // Lấy về danh sách câu hỏi trong course
            List<ViewQuestion> questions = await QuestionRepository.GetQuestionInCourse(courseId);
            questions = QuestionRepository.ApplyPagination(questions, pagination);

            // Trả về thành công
            return ServiceResult.Success(GetMultiQuestionSuccess, string.Empty, 
                await DecorationRepository.DecorateResponseQuestionModel(myUid, questions));
        }

        public virtual async Task<ServiceResult> UserGetMyPostDetail(Guid myUid, Guid postId)
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

        public virtual async Task<ServiceResult> UserGetMyPosts(Guid myUid, PaginationDto pagination)
        {
            // Kiểm tra user tồn tại
            _ = await UserRepository.CheckExisted(myUid, ResponseResource.NotExistUser());

            // Lấy về và trả về thành công
            List<ViewQuestion> questions = await QuestionRepository.GetByUserId(myUid, pagination);
            //questions = QuestionRepository.ApplyPagination(questions, pagination);

            return ServiceResult.Success(GetMultiQuestionSuccess, string.Empty, 
                await DecorationRepository.DecorateResponseQuestionModel(myUid, questions));
        }

        public virtual async Task<ServiceResult> UserGetPostDetail(Guid myUid, Guid postId)
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

        public virtual async Task<ServiceResult> UserGetPosts(Guid myUid, PaginationDto pagination)
        {
            // Chỉ lấy public question, tính toán sắp xếp theo độ ưu tiên của riêng myUId, làm sau

            // Lấy về danh sách public question
            List<ViewQuestion> questions = await QuestionRepository.GetPublicPosts(pagination);

            // Trả về thành công
            return ServiceResult.Success(GetMultiQuestionSuccess, string.Empty, 
                await DecorationRepository.DecorateResponseQuestionModel(myUid, questions));
        }

        public virtual async Task<ServiceResult> UserGetUserPosts(Guid myUid, Guid userId, PaginationDto pagination)
        {
            // Kiểm tra myUid tồn tại (không nhất thiết) và userId tồn tại
            _ = await UserRepository.CheckExisted(userId, ResponseResource.NotExistUser());

            // Lấy về danh sách userId question? (có lấy trong private question không?)
            // Để đơn giản, hiện chỉ lấy public question
            List<ViewQuestion> questions = await QuestionRepository.GetPublicPostsByUserId(userId, pagination);
            //questions = QuestionRepository.ApplyPagination(questions, pagination);

            // Trả về thành công
            return ServiceResult.Success(GetMultiQuestionSuccess, string.Empty,
                await DecorationRepository.DecorateResponseQuestionModel(myUid, questions));
        }
        #endregion


        #endregion

        #region Get of a category

        public virtual async Task<ServiceResult> AnonymousGetListPostsOfCategory(string catName, PaginationDto pagination)
        {
            List<ViewQuestion> posts = await QuestionRepository.GetPublicPostsOfCategory(catName, pagination);

            return ServiceResult.Success(
                ResponseResource.GetMultiSuccess(),
                string.Empty,
                await DecorationRepository.DecorateResponseQuestionModel(null, posts)
            );
        }

        public virtual async Task<ServiceResult> UserGetListPostsOfCategory(Guid myUid, string catName, PaginationDto pagination)
        {
            List<ViewQuestion> posts = await QuestionRepository.GetPostsOfCategory(myUid, catName, pagination);

            return ServiceResult.Success(
                ResponseResource.GetMultiSuccess(),
                string.Empty,
                await DecorationRepository.DecorateResponseQuestionModel(myUid, posts)
            );
        }

        public virtual async Task<ServiceResult> AdminGetListPostsOfCategory(string catName, PaginationDto pagination)
        {
            List<ViewQuestion> posts = await QuestionRepository.GetPostsOfCategory(catName, pagination);

            return ServiceResult.Success(
                ResponseResource.GetMultiSuccess(),
                string.Empty,
                await DecorationRepository.DecorateResponseQuestionModel(null, posts)
            );
        }

        public virtual async Task<ServiceResult> UserGetMyMarkedPosts(Guid myUid, PaginationDto pagination)
        {
            List<ViewQuestion> listed = await QuestionRepository.GetMarkedPosts(myUid);
            int total = listed.Count;
            listed = QuestionRepository.ApplyPagination(listed, pagination);
            PaginationResponseModel<IResponseQuestionModel> res = new()
            {
                Total = total,
                Limit = pagination.Limit,
                Offset = pagination.Offset,
                Results = await DecorationRepository.DecorateResponseQuestionModel(myUid, listed)
            };
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(QuestionResource), string.Empty, res);
        }

        #endregion



        #region Search APIs

        public virtual async Task<ServiceResult> UserSearchPost(Guid myUid, string? search, PaginationDto pagination)
        {
            // normalized search key
            if (string.IsNullOrWhiteSpace(search))
                return ServiceResult.BadRequest("Từ khóa rỗng");
            search = search.ToLower();

            // Get posts
            List<ViewQuestion> listPost = await QuestionRepository.GetPublicPosts();

            // calculate score
            List<(Guid, string, string, string, string?)> listShortPost = listPost
                .Select(p => (p.UserItemId, p.Title, p.FullName, p.Content, p.Abstract)).ToList();
            Dictionary<Guid, double> scored = CalculateKnowledgeSearchScore.Calculate(search, listShortPost);

            // order by score
            listPost = [.. listPost.OrderByDescending(p => scored[p.UserItemId])];

            // apply pagination
            if (pagination.Filters != null)
            {
                listPost = QuestionRepository.ApplyFilter(listPost, pagination.Filters);
            }
            listPost = listPost.Skip(pagination.Offset ?? 0).Take(pagination.Limit ?? 15).ToList();

            // decoration
            List<IResponseQuestionModel> res = await DecorationRepository.DecorateResponseQuestionModel(myUid, listPost);

            // return 
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(), string.Empty, res);
        }

        public virtual async Task<ServiceResult> UserSearchMyPost(Guid myUid, string? search, PaginationDto pagination)
        {
            // normalized search key
            if (string.IsNullOrWhiteSpace(search))
                return ServiceResult.BadRequest("Từ khóa rỗng");
            search = search.ToLower();

            // Get posts
            List<ViewQuestion> listPost = await QuestionRepository.GetByUserId(myUid);

            // calculate score
            List<(Guid, string, string, string, string?)> listShortPost = listPost
                .Select(p => (p.UserItemId, p.Title, p.FullName, p.Content, p.Abstract)).ToList();
            Dictionary<Guid, double> scored = CalculateKnowledgeSearchScore.Calculate(search, listShortPost);

            // order by score
            listPost = [.. listPost.OrderByDescending(p => scored[p.UserItemId])];

            // apply pagination
            if (pagination.Filters != null)
            {
                listPost = QuestionRepository.ApplyFilter(listPost, pagination.Filters);
            }
            listPost = listPost.Skip(pagination.Offset ?? 0).Take(pagination.Limit ?? 15).ToList();

            // decoration
            List<IResponseQuestionModel> res = await DecorationRepository.DecorateResponseQuestionModel(myUid, listPost);

            // return 
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(), string.Empty, res);
        }

        public virtual async Task<ServiceResult> UserSearchUserPost(Guid myUid, Guid userId, string? search, PaginationDto pagination)
        {
            // normalized search key
            if (string.IsNullOrWhiteSpace(search))
                return ServiceResult.BadRequest("Từ khóa rỗng");
            search = search.ToLower();

            // Get posts
            List<ViewQuestion> listPost = await QuestionRepository.GetPublicPostsByUserId(userId);

            // calculate score
            List<(Guid, string, string, string, string?)> listShortPost = listPost
                .Select(p => (p.UserItemId, p.Title, p.FullName, p.Content, p.Abstract)).ToList();
            Dictionary<Guid, double> scored = CalculateKnowledgeSearchScore.Calculate(search, listShortPost);

            // order by score
            listPost = [.. listPost.OrderByDescending(p => scored[p.UserItemId])];

            // apply pagination
            if (pagination.Filters != null)
            {
                listPost = QuestionRepository.ApplyFilter(listPost, pagination.Filters);
            }
            listPost = listPost.Skip(pagination.Offset ?? 0).Take(pagination.Limit ?? 15).ToList();

            // decoration
            List<IResponseQuestionModel> res = await DecorationRepository.DecorateResponseQuestionModel(myUid, listPost);

            // return 
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(), string.Empty, res);
        }

        public virtual async Task<ServiceResult> AdminSearchUserPost(Guid userId, string? search, PaginationDto pagination)
        {
            // normalized search key
            if (string.IsNullOrWhiteSpace(search))
                return ServiceResult.BadRequest("Từ khóa rỗng");
            search = search.ToLower();

            // Get posts
            List<ViewQuestion> listPost = await QuestionRepository.GetByUserId(userId);

            // calculate score
            List<(Guid, string, string, string, string?)> listShortPost = listPost
                .Select(p => (p.UserItemId, p.Title, p.FullName, p.Content, p.Abstract)).ToList();
            Dictionary<Guid, double> scored = CalculateKnowledgeSearchScore.Calculate(search, listShortPost);

            // order by score
            listPost = [.. listPost.OrderByDescending(p => scored[p.UserItemId])];

            // apply pagination
            if (pagination.Filters != null)
            {
                listPost = QuestionRepository.ApplyFilter(listPost, pagination.Filters);
            }
            listPost = listPost.Skip(pagination.Offset ?? 0).Take(pagination.Limit ?? 15).ToList();

            // decoration
            List<IResponseQuestionModel> res = await DecorationRepository.DecorateResponseQuestionModel(null, listPost);

            // return 
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(), string.Empty, res);
        }
        #endregion
    }
}
