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
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.DecorationRepositories;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Interfaces.Storages;
using KnowledgeSharingApi.Services.Interfaces;
using Mysqlx.Crud;
using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Services
{
    public class LessonService : ILessonService
    {
        protected readonly IResourceFactory ResourceFactory;
        protected readonly IResponseResource ResponseResource;
        protected readonly IEntityResource EntityResource;

        protected readonly ILessonRepository LessonRepository;
        protected readonly IStarRepository StarRepository;
        protected readonly ICategoryRepository CategoryRepository;
        protected readonly ICourseRepository CourseRepository;
        protected readonly IImageRepository ImageRepository;
        protected readonly IDecorationRepository DecorationRepository;
        protected readonly IKnowledgeRepository KnowledgeRepository;
        protected readonly IUserRepository UserRepository;
        protected readonly IStorage Storage;

        protected readonly string LessonResource, NotExistedLesson, ExistedLesson;
        protected readonly int DefaultLimit = 20;

        public LessonService(
            IResourceFactory resourceFactory,
            ILessonRepository lessonRepository,
            IStarRepository starRepository,
            ICourseRepository courseRepository,
            IUserRepository userRepository,
            ICategoryRepository categoryRepository,
            IKnowledgeRepository knowledgeRepository,
            IDecorationRepository decorationRepository,
            IImageRepository imageRepository,
            IStorage storage
        )
        {
            ResourceFactory = resourceFactory;
            ResponseResource = ResourceFactory.GetResponseResource();
            EntityResource = ResourceFactory.GetEntityResource();

            DecorationRepository = decorationRepository;
            KnowledgeRepository = knowledgeRepository;
            LessonRepository = lessonRepository;
            StarRepository = starRepository;
            CategoryRepository = categoryRepository;
            CourseRepository = courseRepository;
            ImageRepository = imageRepository;
            UserRepository = userRepository;
            Storage = storage;

            LessonResource = EntityResource.Lesson();
            NotExistedLesson = ResponseResource.NotExist(LessonResource);
            ExistedLesson = ResponseResource.ExistName(LessonResource);
        }

        #region Support methods

        

        protected virtual Lesson CreateLesson(ViewUser user, CreateLessonModel model, string? thumbnail)
        {
            Lesson lesson = new()
            {
                // Entity:
                CreatedTime = DateTime.UtcNow,
                CreatedBy = user.FullName,
                // UserItem:
                UserItemId = Guid.NewGuid(),
                UserId = user.UserId,
                UserItemType = EUserItemType.Knowledge,
                // Knowledge:
                Title = model.Title!,
                Abstract = model.Abstract,
                Thumbnail = thumbnail,
                Views = 0,
                KnowledgeType = EKnowledgeType.Post,
                Privacy = model.Privacy!.Value,
                IsBlockComment = false,
                // Post:
                Content = model.Content!,
                PostType = EPostType.Lesson,
                // Lesson:
                EstimateTimeInMinutes = model.EstimateTimeInMinutes!.Value
            };
            return lesson;
        }

        #endregion


        #region Admin services
        public virtual async Task<ServiceResult> AdminDeletePost(Guid postId)
        {
            // Kiểm tra lesson tồn tại
            ViewLesson lesson = await LessonRepository.CheckExistedLesson(postId, NotExistedLesson);

            // OK xóa
            // Override xóa trong repo để xóa cả những course lesson tương ứng
            int deleted = await LessonRepository.Delete(lesson.UserItemId);
            if (deleted <= 0) return ServiceResult.ServerError(ResponseResource.DeleteFailure(LessonResource));

            return ServiceResult.Success(ResponseResource.DeleteSuccess(LessonResource));
        }

        public virtual async Task<ServiceResult> AdminGetListPostsOfCategory(string catName, int? limit, int? offset)
        {
            IEnumerable<ViewLesson> listLessons =
                await LessonRepository.GetPostsOfCategory(catName, limit ?? DefaultLimit, offset ?? 0);
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(LessonResource),
                string.Empty, await DecorationRepository.DecorateResponseLessonModel(null, listLessons.ToList()));
        }

        public async Task<ServiceResult> AdminGetListPostsOfCourse(Guid courseId, int? limit, int? offset)
        {
            // Check course exist
            _ = await CourseRepository.CheckExisted(courseId, ResponseResource.NotExist(EntityResource.Course()));

            // Get list lesson
            PaginationResponseModel<ViewLesson> lessons =
                await LessonRepository.GetLessonsInCourse(courseId, limit ?? DefaultLimit, offset ?? 0);

            // DecorateResponseLessonModel
            List<IResponseLessonModel> listResLessons = await DecorationRepository.DecorateResponseLessonModel(null, lessons.Results.ToList());

            // Return Success
            PaginationResponseModel<IResponseLessonModel> res =
                new(lessons.Total, lessons.Limit, lessons.Offset, listResLessons);
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(LessonResource), string.Empty, res);
        }

        public async Task<ServiceResult> AdminGetPostDetail(Guid postId)
        {
            // Check lesson exist
            ViewLesson lesson = await LessonRepository.CheckExistedLesson(postId, NotExistedLesson);

            // DecorateResponseLessonModel
            IResponseLessonModel res = (await DecorationRepository.DecorateResponseLessonModel(null, [lesson])).First();

            // return success
            return ServiceResult.Success(ResponseResource.GetSuccess(LessonResource), string.Empty, res);
        }

        public async Task<ServiceResult> AdminGetPosts(int? limit, int? offset)
        {
            // Get all paginated
            IEnumerable<ViewLesson> lessons =
                await LessonRepository.GetViewPost(limit ?? DefaultLimit, offset ?? 0);

            // DecorateResponseLessonModel
            List<IResponseLessonModel> res = await DecorationRepository.DecorateResponseLessonModel(null, lessons.ToList());

            // Return Success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(LessonResource), string.Empty, res);
        }

        public async Task<ServiceResult> AdminGetUserPosts(Guid userId, int? limit, int? offset)
        {
            // CHeck user existed:
            _ = await UserRepository.CheckExisted(userId, ResponseResource.NotExistUser());

            // Get all paginated
            IEnumerable<ViewLesson> lessons = await LessonRepository.GetByUserId(userId);
            lessons = lessons.Skip(offset ?? 0).Take(limit ?? DefaultLimit);

            // DecorateResponseLessonModel
            List<IResponseLessonModel> res = await DecorationRepository.DecorateResponseLessonModel(null, lessons.ToList());

            // Return Success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(LessonResource), string.Empty, res);
        }
        #endregion

        #region Anonymous services
        public async Task<ServiceResult> AnonymousGetListPostsOfCategory(string catName, int? limit, int? offset)
        {
            // Get public list post
            IEnumerable<ViewLesson> listLessons =
                await LessonRepository.GetPublicPostsOfCategory(catName, limit ?? DefaultLimit, offset ?? 0);

            // DecorateResponseLessonModel
            List<IResponseLessonModel> res = await DecorationRepository.DecorateResponseLessonModel(null, listLessons.ToList());

            // Return success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(LessonResource), string.Empty, res);
        }

        public async Task<ServiceResult> AnonymousGetPostDetail(Guid postId)
        {
            // Check Lesson exist
            ViewLesson lesson = await LessonRepository.CheckExistedLesson(postId, NotExistedLesson);

            // DecorateResponseLessonModel
            IResponseLessonModel res = (await DecorationRepository.DecorateResponseLessonModel(null, [lesson])).First();

            // Return Success
            return ServiceResult.Success(ResponseResource.GetSuccess(LessonResource), string.Empty, res);
        }

        public async Task<ServiceResult> AnonymousGetPosts(int? limit, int? offset)
        {
            // Get public lessons
            IEnumerable<ViewLesson> listLessons =
                await LessonRepository.GetPublicPosts(limit ?? DefaultLimit, offset ?? 0);

            // DecorateResponseLessonModel
            List<IResponseLessonModel> res = await DecorationRepository.DecorateResponseLessonModel(null, listLessons.ToList());

            // Return success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(LessonResource), string.Empty, res);
        }

        public async Task<ServiceResult> AnonymousGetUserPosts(Guid userId, int? limit, int? offset)
        {
            // Check user existed
            _ = await UserRepository.CheckExisted(userId, ResponseResource.NotExistUser());

            // Get public lessons and Pagination
            IEnumerable<ViewLesson> lessons =
                await LessonRepository.GetPublicPostsByUserId(userId);
            lessons = lessons.Skip(offset ?? 0).Take(limit ?? DefaultLimit);

            // DecorateResponseLessonModel
            List<IResponseLessonModel> res = await DecorationRepository.DecorateResponseLessonModel(null, lessons.ToList());

            // Return Success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(LessonResource), string.Empty, res);
        }

        #endregion


        #region User operation services

        public async Task<ServiceResult> ChangePrivacy(Guid myUid, ChangeKnowledgePrivacyModel model)
        {
            // Check lesson existed and owner
            Lesson lesson = await LessonRepository.CheckExisted(model.KnowledgeId ?? Guid.Empty, NotExistedLesson);
            if (lesson.UserId != myUid)
                return ServiceResult.Forbidden("Đây không phải bài giảng của bạn");

            // update privacy
            if (model.Privacy == null)
                return ServiceResult.BadRequest(ViConstantResource.PRIVACY_EMPTY);
            lesson.Privacy = model.Privacy.Value;
            int res = await LessonRepository.Update(lesson.UserItemId, lesson);
            if (res <= 0) return ServiceResult.ServerError(ResponseResource.UpdateFailure(LessonResource));

            // return success
            return ServiceResult.Success(ResponseResource.UpdateSuccess(LessonResource));
        }

        public async Task<ServiceResult> UserCreatePost(Guid myUid, CreatePostModel model)
        {
            // Check Create Lesson Model type:
            if (model is not CreateLessonModel lessonModel) throw new NotMatchTypeException();

            // CHeck user existed:
            ViewUser user = await UserRepository.CheckExistedUser(myUid, ResponseResource.NotExistUser());

            // Post thumbnail if existed:
            string? thumbnail = await Storage.SaveImage(model.Thumbnail);
            _ = await ImageRepository.TryInsertImage(myUid, thumbnail);

            // Tạo mới Lesson
            Lesson lesson = CreateLesson(user, lessonModel, thumbnail);

            // Insert Lesson
            Guid? res = await LessonRepository.Insert(lesson.UserItemId, lesson);
            if (res == null) return ServiceResult.ServerError(ResponseResource.InsertFailure(LessonResource));

            // Insert categories nếu có:
            if (model.Categories != null && model.Categories.Any())
            {
                _ = await CategoryRepository.UpdateKnowledgeCategories(lesson.UserItemId, model.Categories.ToList());
            }

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.InsertSuccess(LessonResource), string.Empty, lesson);
        }

        public async Task<ServiceResult> UserDeletePost(Guid myUid, Guid postId)
        {
            // Check lesson exist and owner
            ViewLesson lesson = await LessonRepository.CheckExistedLesson(postId, NotExistedLesson);
            if (lesson.UserId != myUid) return ServiceResult.Forbidden("Bài giảng không phải của bạn");

            // Check lesson not in any course
            IEnumerable<Tuple<CourseLesson, ViewCourse>> listCourses = await LessonRepository.GetListCoursesOfLesson(postId);
            if (listCourses.Any())
                return ServiceResult.BadRequest("Không thể xóa do bài giảng hiện đang ở trong khóa học khác");

            // Delete
            int deleted = await LessonRepository.Delete(lesson.UserItemId);
            if (deleted <= 0) return ServiceResult.ServerError(ResponseResource.DeleteFailure(LessonResource));

            // Return Success
            return ServiceResult.Success(ResponseResource.DeleteSuccess(LessonResource));
        }

        public async Task<ServiceResult> UserUpdatePost(Guid myUid, Guid postId, UpdatePostModel model)
        {
            // Kiểm tra lesson tồn tại và do user làm chủ
            Lesson lesson = await LessonRepository.CheckExisted(postId, NotExistedLesson);
            if (lesson.UserId != myUid)
                return ServiceResult.Forbidden("Không phải bài giảng của bạn");

            if (model is not UpdateLessonModel lessonModel)
                throw new NotMatchTypeException();

            // update thumbnail:
            string? thumbnail = await Storage.SaveImage(model.Thumbnail);
            _ = await ImageRepository.TryInsertImage(myUid, thumbnail);

            // cập nhật 
            Lesson lessonToUpdate = new();
            lessonToUpdate.Copy(lesson);
            lessonToUpdate.Copy(lessonModel);
            if (thumbnail != null) lessonToUpdate.Thumbnail = thumbnail;
            int updated1 = await LessonRepository.Update(postId, lessonToUpdate);
            int updated2 = 0;

            // Update categories nếu có:
            if (model.Categories != null && model.Categories.Any())
            {
                updated2 = await CategoryRepository.UpdateKnowledgeCategories(postId, model.Categories.ToList());
            }

            if (updated1 + updated2 <= 0) return ServiceResult.ServerError(ResponseResource.UpdateFailure(LessonResource));
            // Trả về thành công
            return ServiceResult.Success(ResponseResource.UpdateSuccess(LessonResource), string.Empty, lesson);
        } 
        
        #endregion


        #region User get services

        public async Task<ServiceResult> UserGetListPostsOfCategory(Guid myUid, string catName, int? limit, int? offset)
        {
            IEnumerable<ViewLesson> listLessons =
                await LessonRepository.GetPostsOfCategory(myUid, catName, limit ?? DefaultLimit, offset ?? 0);
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(LessonResource),
                string.Empty, await DecorationRepository.DecorateResponseLessonModel(myUid, listLessons.ToList()));
        }

        public async Task<ServiceResult> UserGetListPostsOfCourse(Guid myUid, Guid courseId, int? limit, int? offset)
        {
            // Check course exist
            _ = await CourseRepository.CheckExisted(courseId, ResponseResource.NotExist(EntityResource.Course()));

            // Check joined course
            ViewCourseRegister? registered = await CourseRepository.GetViewCourseRegister(myUid, courseId);
            if (registered == null) return ServiceResult.Forbidden("Bạn chưa tham gia khóa học này");

            // Get list lesson
            PaginationResponseModel<ViewLesson> lessons =
                await LessonRepository.GetLessonsInCourse(courseId, limit ?? DefaultLimit, offset ?? 0);

            // DecorateResponseLessonModel
            List<IResponseLessonModel> listResLessons = await DecorationRepository.DecorateResponseLessonModel(myUid, lessons.Results.ToList());

            // Return Success
            PaginationResponseModel<IResponseLessonModel> res =
                new(lessons.Total, lessons.Limit, lessons.Offset, listResLessons);
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(LessonResource), string.Empty, res);
        }

        public async Task<ServiceResult> UserGetListPostsOfMyCourse(Guid myUid, Guid courseId, int? limit, int? offset)
        {
            // Check course exist and owner
            Course course = 
                await CourseRepository.CheckExisted(courseId, ResponseResource.NotExist(EntityResource.Course()));
            if (course.UserId != myUid)
                return ServiceResult.Forbidden("Đây không phải là khóa học của bạn");

            // Get list lesson
            PaginationResponseModel<ViewLesson> lessons =
                await LessonRepository.GetLessonsInCourse(courseId, limit ?? DefaultLimit, offset ?? 0);

            // DecorateResponseLessonModel
            List<IResponseLessonModel> listResLessons = await DecorationRepository.DecorateResponseLessonModel(myUid, lessons.Results.ToList());

            // Return Success
            PaginationResponseModel<IResponseLessonModel> res =
                new(lessons.Total, lessons.Limit, lessons.Offset, listResLessons);
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(LessonResource), string.Empty, res);
        }

        public async Task<ServiceResult> UserGetMyMarkedPosts(Guid myUid, int? limit, int? offset)
        {
            // GEt list về
            IEnumerable<ViewLesson> listLessons = await LessonRepository.GetMarkedPosts(myUid);

            // Pagination
            int total = listLessons.Count();
            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            listLessons = listLessons.Skip(offsetValue).Take(limitValue);

            // DecorateResponseLessonModel 
            List<IResponseLessonModel> listResLesson = await DecorationRepository.DecorateResponseLessonModel(myUid, listLessons.ToList());

            // Trả về thành công
            PaginationResponseModel<IResponseLessonModel> res = new(total, limitValue, offsetValue, listResLesson);
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(), string.Empty, res);
        }

        public async Task<ServiceResult> UserGetMyPostDetail(Guid myUid, Guid postId)
        {
            // Check lesson existed and owner
            ViewLesson lesson = await LessonRepository.CheckExistedLesson(postId, NotExistedLesson);
            if (lesson.UserId != myUid)
                return ServiceResult.Forbidden("Bài giảng này không phải của bạn");

            // DecorateResponseLessonModel
            IResponseLessonModel resLesson = (await DecorationRepository.DecorateResponseLessonModel(myUid, [lesson])).First();

            // return Success
            return ServiceResult.Success(ResponseResource.GetSuccess(LessonResource), string.Empty, resLesson);
        }

        public async Task<ServiceResult> UserGetMyPosts(Guid myUid, int? limit, int? offset)
        {
            // Get all paginated
            IEnumerable<ViewLesson> lessons = await LessonRepository.GetByUserId(myUid);
            lessons = lessons.Skip(offset ?? 0).Take(limit ?? DefaultLimit);

            // DecorateResponseLessonModel
            List<IResponseLessonModel> res = await DecorationRepository.DecorateResponseLessonModel(myUid, lessons.ToList());

            // Return Success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(LessonResource), string.Empty, res);
        }

        public async Task<ServiceResult> UserGetPostDetail(Guid myUid, Guid postId)
        {
            // Check lesson exist
            ViewLesson lesson = await LessonRepository.CheckExistedLesson(postId, NotExistedLesson);
            if (lesson.UserId == myUid) return await UserGetMyPostDetail(myUid, postId);

            // Check user can access lesson:
            bool isAccessible = await KnowledgeRepository.CheckAccessible(myUid, postId);
            if (!isAccessible)
                return ServiceResult.Forbidden("Bạn không có quyền truy cập bài giảng này");

            // DecorateResponseLessonModel
            IResponseLessonModel res = (await DecorationRepository.DecorateResponseLessonModel(myUid, [lesson])).First();

            // return success
            return ServiceResult.Success(ResponseResource.GetSuccess(LessonResource), string.Empty, res);
        }

        public async Task<ServiceResult> UserGetPosts(Guid myUid, int? limit, int? offset)
        {
            // Get all paginated
            IEnumerable<ViewLesson> lessons =
                await LessonRepository.GetPublicPosts(limit ?? DefaultLimit, offset ?? 0);

            // DecorateResponseLessonModel
            List<IResponseLessonModel> res = await DecorationRepository.DecorateResponseLessonModel(myUid, lessons.ToList());

            // Return Success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(LessonResource), string.Empty, res);
        }

        public async Task<ServiceResult> UserGetUserPosts(Guid myUid, Guid userId, int? limit, int? offset)
        {
            // CHeck user existed:
            _ = await UserRepository.CheckExisted(userId, ResponseResource.NotExistUser());

            // Get all paginated
            IEnumerable<ViewLesson> lessons = await LessonRepository.GetPublicPostsByUserId(userId);
            lessons = lessons.Skip(offset ?? 0).Take(limit ?? DefaultLimit);

            // DecorateResponseLessonModel
            List<IResponseLessonModel> res = await DecorationRepository.DecorateResponseLessonModel(myUid, lessons.ToList());

            // Return Success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(LessonResource), string.Empty, res);
        }

        public async Task<ServiceResult> UserGetListCourseOfLesson(Guid myUId, Guid lessonId, int? limit, int? offset)
        {
            // Kiểm tra lesson tồn tại và là của myUId
            ViewLesson lesson = await LessonRepository.CheckExistedLesson(lessonId, NotExistedLesson);
            if (lesson.UserId != myUId)
                return ServiceResult.Forbidden("Bài giảng không phải của bạn");

            // Lấy về danh sách khóa học (ViewCourse)
            IEnumerable<Tuple<CourseLesson, ViewCourse>> listCourses = await LessonRepository.GetListCoursesOfLesson(lessonId);

            // Phân trang
            int total = listCourses.Count();
            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            listCourses = listCourses.Skip(offsetValue).Take(limitValue);
            
            // DecorateResponseLessonModel:
            // Thêm chi tiết xem bài học hiện tại là bài học thứ bao nhiêu của khóa học
            // Nên trả về ViewCourseLesson không ?? --> Hay lại sử dụng ResponseCourseLessonModel
            // -> Nên trả về ResponseCourseLessonModel hơn
            IEnumerable<ResponseCourseLessonModel> listParticipants = listCourses.Select(participant =>
            {
                ResponseCourseLessonModel resItem = 
                    (ResponseCourseLessonModel)new ResponseCourseLessonModel().Copy(participant.Item1);
                resItem.Lesson = (ResponseLessonModel)new ResponseLessonModel().Copy(lesson);
                resItem.Course = (ResponseCourseModel)new ResponseCourseCardModel().Copy(participant.Item2);
                return resItem;
            }).ToList();

            // Trả về OK
            PaginationResponseModel<ResponseCourseLessonModel> res = new(total, limitValue, offsetValue, listParticipants);
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(), string.Empty, res);
        }
        #endregion

    }
}
