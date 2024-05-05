using KnowledgeSharingApi.Domains.Enums;
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
using KnowledgeSharingApi.Services.Interfaces;
using Microsoft.VisualBasic;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Services
{
    public class CourseService : ICourseService
    {
        protected readonly ICourseRepository CourseRepository;
        protected readonly IStarRepository StarRepository;
        protected readonly IUserRepository UserRepository;
        protected readonly IKnowledgeRepository KnowledgeRepository;
        protected readonly IDecorationRepository DecorationRepository;
        protected readonly ICategoryRepository CategoryRepository;
        protected readonly IImageRepository ImageRepository;
        protected readonly IStorage Storage;

        protected readonly IResourceFactory ResourceFactory;
        protected readonly IResponseResource ResponseResource;
        protected readonly IEntityResource EntityResource;

        protected readonly int DefaultLimit = 20;
        protected readonly string CourseResource;
        protected readonly string NotExistedCourse, ExistedCourse;

        public CourseService(
            ICourseRepository courseRepository,
            IResourceFactory resourceFactory,
            IStarRepository starRepository,
            IUserRepository userRepository,
            IKnowledgeRepository knowledgeRepository,
            IDecorationRepository decorationRepository,
            ICategoryRepository categoryRepository,
            IImageRepository imageRepository,
            IStorage storage
        )
        {
            ResourceFactory = resourceFactory;
            ResponseResource = resourceFactory.GetResponseResource();
            EntityResource = resourceFactory.GetEntityResource();
            Storage = storage;

            CourseRepository = courseRepository;
            StarRepository = starRepository;
            CategoryRepository = categoryRepository;
            ImageRepository = imageRepository;
            UserRepository = userRepository;
            KnowledgeRepository = knowledgeRepository;
            DecorationRepository = decorationRepository;

            CourseResource = EntityResource.Course();
            NotExistedCourse = ResponseResource.NotExist(CourseResource);
            ExistedCourse = ResponseResource.ExistName(CourseResource);
        }

        #region Functionality Methods

        //protected virtual async Task<IEnumerable<ResponseCourseModel>> DecorationRepository.DecorateResponseCourseModel(Guid? myUid, IEnumerable<ViewCourse> courses)
        //{
        //    List<Guid> courseIds = courses.Select(course => course.UserItemId).ToList();
        //    Dictionary<Guid, int?>? myStars = null;
        //    if (myUid != null)
        //    {
        //        // calculate myStar from myUid to all courses
        //        myStars = await StarRepository.CalculateUserStars(myUid.Value, courseIds);
        //    }
        //    // calculate total stars to all courses
        //    Dictionary<Guid, int> totalStars = await StarRepository.CalculateTotalStars(courseIds);

        //    // calculate average stars to all courses
        //    Dictionary<Guid, double?> averageStars = await StarRepository.CalculateAverageStars(courseIds);

        //    // Number comments & Top comments

        //    // is Mark

        //    return courses.Select(course =>
        //    {
        //        ResponseCourseModel crs = new();
        //        crs.Copy(course);
        //        crs.MyStars = myStars?[course.UserItemId];
        //        crs.TotalStars = totalStars[course.UserItemId];
        //        crs.AverageStars = averageStars[course.UserItemId];
        //        return crs;
        //    });
        //}

        protected virtual Course CreateCourse(ViewUser user, CreateCourseModel model, string? thumbnail = null)
        {
            int fee = model.Fee!.Value;
            Course course = new()
            {
                // Entity:
                CreatedBy = user.FullName,
                CreatedTime = DateTime.Now,
                // UserItem:
                UserId = user.UserId,
                UserItemId = Guid.NewGuid(),
                UserItemType = EUserItemType.Knowledge,
                // Knowledge:
                Title = model.Title!,
                Abstract = model.Abstract!,
                Thumbnail = thumbnail,
                Views = 0,
                KnowledgeType = EKnowledgeType.Course,
                Privacy = model.Privacy!.Value,
                IsBlockComment = false,
                // Member:
                Introduction = model.Introduction!,
                EstimateTimeInMinutes = model.EstimateTimeInMinutes!.Value,
                Fee = fee > 0 ? fee : 0,
                IsFree = fee > 0 ? false : true
            };
            return course;
        }

        #endregion

        #region Admin Apies
        public async Task<ServiceResult> AdminDeleteCourse(Guid courseId)
        {
            // Kiểm tra course tồn tại
            ViewCourse course = await CourseRepository.CheckExistedCourse(courseId, NotExistedCourse);

            // Thực hiện xóa course (ghi đè repo)
            int deleted = await CourseRepository.Delete(courseId);
            if (deleted <= 0) return ServiceResult.ServerError(ResponseResource.DeleteFailure(CourseResource));

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.DeleteSuccess(CourseResource));
        }

        public async Task<ServiceResult> AdminGetCourseDetail(Guid courseId)
        {
            // Kiểm tra tồn tại
            ViewCourse course = await CourseRepository.CheckExistedCourse(courseId, NotExistedCourse);

            // Trả về thành công
            ResponseCourseModel res = (await DecorationRepository.DecorateResponseCourseModel(null, [course])).First();
            return ServiceResult.Success(ResponseResource.GetSuccess(CourseResource), string.Empty, res);
        }

        public async Task<ServiceResult> AdminGetCourses(int? limit, int? offset)
        {
            // Get
            IEnumerable<ViewCourse> listCourses = 
                await CourseRepository.GetViewCourse(limit ?? DefaultLimit, offset ?? 0);

            // decorate:
            IEnumerable<ResponseCourseModel> res =
                await DecorationRepository.DecorateResponseCourseModel(null, listCourses.ToList());

            // Return success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(CourseResource), string.Empty, res);
        }

        public async Task<ServiceResult> AdminGetUserCourses(Guid userId, int? limit, int? offset)
        {
            // Kiểm tra user tồn tại
            _ = await UserRepository.CheckExistedUser(userId, ResponseResource.NotExistUser());

            // Lấy về danh sách course của user
            IEnumerable<ViewCourse> listCourses = await CourseRepository.GetViewCourseOfUser(userId);

            // Phân trang
            int total = listCourses.Count();
            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            listCourses = listCourses.Skip(offsetValue).Take(limitValue);

            // decorate
            IEnumerable<ResponseCourseModel> resLists = await DecorationRepository.DecorateResponseCourseModel(null, listCourses.ToList());
            PaginationResponseModel<ResponseCourseModel> res =
                new(total, limitValue, offsetValue, resLists);

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(CourseResource), string.Empty, res);
        }

        public async Task<ServiceResult> AdminGetUserRegisteredCourses(Guid userId, int? limit, int? offset)
        {
            // Check user existed
            _ = await UserRepository.CheckExistedUser(userId, ResponseResource.NotExistUser());

            // Lấy về
            IEnumerable<ViewCourseRegister> listCourses = await CourseRepository.GetRegistersOfUser(userId);

            // Phân trang
            int total = listCourses.Count();
            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            listCourses = listCourses.Skip(offsetValue).Take(limitValue);

            // Trả về thành công
            PaginationResponseModel<ViewCourseRegister> res =
                new(total, limitValue, offsetValue, listCourses);
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(), string.Empty, res);
        }

        public async Task<ServiceResult> AdminListCourseOfCategory(string catName, int? limit, int? offset)
        {
            // Get về
            IEnumerable<ViewCourse> listCourses = 
                await CourseRepository.GetViewCourseOfCategory(catName, limit ?? DefaultLimit, offset ?? 0);

            // decorate
            IEnumerable<ResponseCourseModel> res = await DecorationRepository.DecorateResponseCourseModel(null, listCourses.ToList());

            // Thành công
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(CourseResource), string.Empty, res);
        }
        #endregion

        #region Anonymous Apies

        public async Task<ServiceResult> AnonymousGetCourseDetail(Guid courseId)
        {
            // Kiểm tra course tồn tại và public
            ViewCourse course = await CourseRepository.CheckExistedCourse(courseId, NotExistedCourse);
            if (course.Privacy != EPrivacy.Public)
                return ServiceResult.Forbidden("Khóa học ở trạng thái riêng tư");

            // decorate và trả về thành công
            ResponseCourseModel res = (await DecorationRepository.DecorateResponseCourseModel(null, [course])).First();
            return ServiceResult.Success(ResponseResource.GetSuccess(CourseResource), string.Empty, res);
        }

        public async Task<ServiceResult> AnonymousGetCourses(int? limit, int? offset)
        {
            // Get public courses
            IEnumerable<ViewCourse> lsCourses = await CourseRepository.GetPublicViewCourse(limit ?? DefaultLimit, offset ?? 0);

            // decorate 
            IEnumerable<ResponseCourseModel> res = await DecorationRepository.DecorateResponseCourseModel(null, lsCourses.ToList());

            // return success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(CourseResource), string.Empty, res);
        }

        public async Task<ServiceResult> AnonymousGetListCourseOfCategory(string catName, int? limit, int? offset)
        {
            // Get list public course by category
            IEnumerable<ViewCourse> lsCourses = 
                await CourseRepository.GetPublicViewCourseOfCategory(catName, limit ?? DefaultLimit, offset ?? 0);

            // decorate
            IEnumerable<ResponseCourseModel> res = await DecorationRepository.DecorateResponseCourseModel(null, lsCourses.ToList());

            // return Success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(CourseResource), string.Empty, res);
        }

        public async Task<ServiceResult> AnonymousGetUserCourses(Guid userId, int? limit, int? offset)
        {
            // Kiểm tra user tồn tại
            _ = await UserRepository.CheckExistedUser(userId, ResponseResource.NotExistUser());

            // Lấy về public
            IEnumerable<ViewCourse> lsCourses = await CourseRepository.GetPublicViewCourseOfUser(userId);

            // Phân trang
            int total = lsCourses.Count();
            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            lsCourses = lsCourses.Skip(offsetValue).Take(limitValue);

            // decorate
            IEnumerable<ResponseCourseModel> lsRes = await DecorationRepository.DecorateResponseCourseModel(null, lsCourses.ToList());
            PaginationResponseModel<ResponseCourseModel> res =
                new(total, limitValue, offsetValue, lsRes);

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(CourseResource), string.Empty, res);
        }

        #endregion

        #region User operation

        public async Task<ServiceResult> ChangePrivacy(Guid myUid, ChangeKnowledgePrivacyModel model)
        {
            // Kiểm tra course tồn tại và owner
            Course course = await CourseRepository.CheckExisted(model.KnowledgeId ?? Guid.Empty, NotExistedCourse);
            if (course.UserId != myUid)
                return ServiceResult.Forbidden("Bạn không phải là chủ khóa học này");

            // Update privacy 
            course.Privacy = model.Privacy!.Value;
            await CourseRepository.Update(course.UserId, course);

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.UpdateSuccess(CourseResource));
        }

        public async Task<ServiceResult> UserCreateCourse(Guid myUid, CreateCourseModel model)
        {
            // Kiểm tra myUid tồn tại
            ViewUser user = await UserRepository.CheckExistedUser(myUid, ResponseResource.NotExistUser());

            // Kiểm tra chính sách tạo khóa học của myUid

            // Update thumbnail
            string? thumbnail = await Storage.SaveImage(model.Thumbnail);
            _ = ImageRepository.TryInsertImage(myUid, thumbnail);

            // Tạo khóa học mới
            Course course = CreateCourse(user, model, thumbnail);
            Guid? id = await CourseRepository.Insert(course.UserItemId, course);
            if (id == null) return ServiceResult.ServerError(
                ResponseResource.InsertFailure(CourseResource));

            // Insert categories nếu có:
            if (model.Categories != null && model.Categories.Any())
            {
                _ = await CategoryRepository.UpdateKnowledgeCategories(course.UserItemId, model.Categories.ToList());
            }

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.InsertSuccess(CourseResource), string.Empty, course);
        }

        public async Task<ServiceResult> UserDeleteCourse(Guid myUid, Guid courseId)
        {
            // Check course existed and owner
            ViewCourse course = await CourseRepository.CheckExistedCourse(courseId, NotExistedCourse);
            if (course.UserId != myUid)
                return ServiceResult.Forbidden("Đây không phải khóa học của bạn");

            // Kiểm tra không có registers nào
            IEnumerable<ViewCourseRegister> courseRegisters = await CourseRepository
                .GetRegistersOfCourse(courseId);
            if (courseRegisters.Any())
                return ServiceResult.Forbidden("Không thể xóa khóa học đang có người học");

            // Delete
            int deleted = await CourseRepository.Delete(courseId);
            if (deleted <= 0)
                return ServiceResult.ServerError(ResponseResource.DeleteFailure(CourseResource));

            // Return Success
            return ServiceResult.Success(ResponseResource.DeleteSuccess(CourseResource));
        }

        public async Task<ServiceResult> UserUpdateCourse(Guid myUid, Guid courseId, UpdateCourseModel model)
        {
            // Check course existed and owner
            Course course = await CourseRepository.CheckExisted(courseId, NotExistedCourse);
            if (course.UserId != myUid)
                return ServiceResult.Forbidden("Đây không phải khóa học của bạn");

            // update thumbnail:
            string? thumbnail = await Storage.SaveImage(model.Thumbnail);
            _ = ImageRepository.TryInsertImage(myUid, thumbnail);

            // Update
            Course courseToUpdate = new();
            courseToUpdate.Copy(course);
            courseToUpdate.Copy(model);
            if (thumbnail != null) courseToUpdate.Thumbnail = thumbnail;
            int effects1 = await CourseRepository.Update(courseId, courseToUpdate);
            int effects2 = 0;

            // Update categories nếu có:
            if (model.Categories != null && model.Categories.Any())
            {
                effects2 = await CategoryRepository.UpdateKnowledgeCategories(courseId, model.Categories.ToList());
            }

            if (effects1 + effects2 <= 0) return ServiceResult.ServerError(ResponseResource.UpdateFailure(CourseResource));
            // return success
            return ServiceResult.Success(ResponseResource.UpdateSuccess(CourseResource), string.Empty, course);
        }

        #endregion
        
        #region User get apies
        public async Task<ServiceResult> UserGetCourseDetail(Guid myUid, Guid courseId)
        {
            // Kiểm tra course tồn tại
            ViewCourse course = await CourseRepository.CheckExistedCourse(courseId, NotExistedCourse);
            if (course.UserId == myUid) return await UserGetMyCourseDetail(myUid, courseId);

            // Kiểm tra accessible
            bool isAccessible = await KnowledgeRepository.CheckCourseAccessible(myUid, courseId);
            if (!isAccessible)
                return ServiceResult.Forbidden("Bạn không có quyền truy cập khóa học này");

            // decorate và trả về thành công
            ResponseCourseModel res = (await DecorationRepository.DecorateResponseCourseModel(myUid, [course])).First();
            return ServiceResult.Success(ResponseResource.GetSuccess(CourseResource), string.Empty, res);
        }

        public async Task<ServiceResult> UserGetListCourseOfCategory(Guid myUid, string catName, int? limit, int? offset)
        {
            // Get về
            IEnumerable<ViewCourse> listCourses =
                await CourseRepository.GetViewCourseOfCategory(myUid, catName, limit ?? DefaultLimit, offset ?? 0);

            // decorate
            IEnumerable<ResponseCourseModel> res = await DecorationRepository.DecorateResponseCourseModel(myUid, listCourses.ToList());

            // Thành công
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(CourseResource), string.Empty, res);
        }

        public async Task<ServiceResult> UserGetListCourses(Guid myUid, int? limit, int? offset)
        {
            // Get
            IEnumerable<ViewCourse> listCourses =
                await CourseRepository.GetViewCourse(myUid, limit ?? DefaultLimit, offset ?? 0);

            // decorate
            IEnumerable<ResponseCourseModel> res =
                await DecorationRepository.DecorateResponseCourseModel(myUid, listCourses.ToList());

            // Return success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(CourseResource), string.Empty, res);
        }

        public async Task<ServiceResult> UserGetMarkedCourses(Guid myUid, int? limit, int? offset)
        {
            IEnumerable<ViewCourse> lsCourses = 
                await CourseRepository.GetMarkedCoursesOfUse(myUid, limit ?? DefaultLimit, offset ?? 0);

            IEnumerable<ResponseCourseModel> res = await DecorationRepository.DecorateResponseCourseModel(myUid, lsCourses.ToList());

            return ServiceResult.Success(ResponseResource.GetMultiSuccess(CourseResource), string.Empty, res);
        }

        public async Task<ServiceResult> UserGetMyCourseDetail(Guid myUid, Guid courseId)
        {
            // Check course existed and owner
            ViewCourse course = await CourseRepository.CheckExistedCourse(courseId, NotExistedCourse);
            if (course.UserId != myUid)
                return ServiceResult.Forbidden("Đây không phải khóa học của bạn");

            // decorate
            ResponseCourseModel res = (await DecorationRepository.DecorateResponseCourseModel(myUid, [course])).First();

            // return Success
            return ServiceResult.Success(ResponseResource.GetSuccess(CourseResource), string.Empty, res);
        }

        public async Task<ServiceResult> UserGetMyCourses(Guid myUid, int? limit, int? offset)
        {
            // Lấy về
            IEnumerable<ViewCourse> lsCourses = await CourseRepository.GetViewCourseOfUser(myUid);

            // Pagination
            int total = lsCourses.Count();
            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            lsCourses = lsCourses.Skip(offsetValue).Take(limitValue);

            // decorate
            IEnumerable<ResponseCourseModel> lsRes = await DecorationRepository.DecorateResponseCourseModel(myUid, lsCourses.ToList());
            PaginationResponseModel<ResponseCourseModel> res =
                new(total, limitValue, offsetValue, lsRes);

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(CourseResource), string.Empty, res);
        }

        public async Task<ServiceResult> UserGetMyRegisteredCourses(Guid myUid, int? limit, int? offset)
        {
            // Lấy về 
            IEnumerable<ViewCourseRegister> lsCourses = await CourseRepository.GetRegistersOfUser(myUid);

            // Phân trang
            int total = lsCourses.Count();
            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            lsCourses = lsCourses.Skip(offsetValue).Take(limitValue);
            PaginationResponseModel<ViewCourseRegister> res = new(total, limitValue, offsetValue, lsCourses);

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(), string.Empty, res);
        }

        public async Task<ServiceResult> UserGetUserCourses(Guid myUid, Guid userId, int? limit, int? offset)
        {
            // Check user existed
            ViewUser user = await UserRepository.CheckExistedUser(userId, ResponseResource.NotExistUser());

            // Get 
            IEnumerable<ViewCourse> lsCourses = await CourseRepository.GetPublicViewCourseOfUser(userId);

            // Pagination
            int total = lsCourses.Count();
            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            lsCourses = lsCourses.Skip(offsetValue).Take(limitValue);

            // decorate
            IEnumerable<ResponseCourseModel> lsres = await DecorationRepository.DecorateResponseCourseModel(myUid, lsCourses.ToList());
            PaginationResponseModel<ResponseCourseModel> res = new(total, limitValue, offsetValue, lsres);

            // return success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(CourseResource), string.Empty, res);
        }

        #endregion
    }
}
