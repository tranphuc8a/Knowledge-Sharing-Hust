using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.CreateUserItemModels;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.UpdateUserItemModels;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Repositories.Interfaces.DecorationRepositories;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Interfaces.Storages;
using KnowledgeSharingApi.Services.Interfaces;
using Microsoft.VisualBasic;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.KnowledgeRepositories;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.UserIteractionRepositories;
using KnowledgeSharingApi.Services.Interfaces.Knowledges;

namespace KnowledgeSharingApi.Services.Services.Knowledges
{
    public class CourseService : ICourseService
    {
        protected readonly ICourseRepository CourseRepository;
        protected readonly IStarRepository StarRepository;
        protected readonly IUserRepository UserRepository;
        protected readonly ICalculateSearchScoreService CalculateKnowledgeSearchScore;
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
            ICalculateSearchScoreService calculateKnowledgeSearchScore,
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
            CalculateKnowledgeSearchScore = calculateKnowledgeSearchScore;
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

        //protected virtual async Task<List<IResponseCourseModel>> DecorationRepository.DecorateResponseCourseModel(Guid? myUid, List<ViewCourse> courses)
        //{
        //    List<Guid> courseIds = courses.Select(course => course.UserItemId);
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

        protected virtual Course CreateCourse(ViewUserProfile user, CreateCourseModel model, string? thumbnail = null)
        {
            decimal fee = model.Fee!.Value;
            Course course = new()
            {
                // Entity:
                CreatedBy = user.FullName,
                CreatedTime = DateTime.UtcNow,
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
                IsFree = fee <= 0
            };
            return course;
        }

        #endregion

        #region Admin Apies
        public virtual async Task<ServiceResult> AdminDeleteCourse(Guid courseId)
        {
            // Kiểm tra course tồn tại
            _ = await CourseRepository.CheckExisted(courseId, NotExistedCourse);

            // Thực hiện xóa course (ghi đè repo)
            int deleted = await CourseRepository.Delete(courseId);
            if (deleted <= 0) return ServiceResult.ServerError(ResponseResource.DeleteFailure(CourseResource));

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.DeleteSuccess(CourseResource));
        }

        public virtual async Task<ServiceResult> AdminGetCourseDetail(Guid courseId)
        {
            // Kiểm tra tồn tại
            ViewCourse course = await CourseRepository.CheckExistedCourse(courseId, NotExistedCourse);

            // Trả về thành công
            IResponseCourseModel res = (await DecorationRepository.DecorateResponseCourseModel(null, [course])).First();
            return ServiceResult.Success(ResponseResource.GetSuccess(CourseResource), string.Empty, res);
        }

        public virtual async Task<ServiceResult> AdminGetCourses(PaginationDto pagination)
        {
            // Get
            List<ViewCourse> listCourses =
                await CourseRepository.GetViewCourse(pagination);

            // decorate:
            List<IResponseCourseModel> res =
                await DecorationRepository.DecorateResponseCourseModel(null, listCourses);

            // Return success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(CourseResource), string.Empty, res);
        }

        public virtual async Task<ServiceResult> AdminGetUserCourses(Guid userId, PaginationDto pagination)
        {
            // Kiểm tra user tồn tại
            _ = await UserRepository.CheckExisted(userId, ResponseResource.NotExistUser());

            // Lấy về danh sách course của user
            List<ViewCourse> listCourses = await CourseRepository.GetViewCourseOfUser(userId);

            // Phân trang
            int total = listCourses.Count;
            listCourses = CourseRepository.ApplyPagination(listCourses, pagination);

            // decorate
            List<IResponseCourseModel> resLists = await DecorationRepository.DecorateResponseCourseModel(null, listCourses);
            PaginationResponseModel<IResponseCourseModel> res =
                new(total, pagination.Limit, pagination.Offset, resLists);

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(CourseResource), string.Empty, res);
        }

        public virtual async Task<ServiceResult> AdminGetUserRegisteredCourses(Guid userId, PaginationDto pagination)
        {
            // Check user existed
            _ = await UserRepository.CheckExisted(userId, ResponseResource.NotExistUser());

            // Lấy về
            List<ViewCourseRegister> listCourses = await CourseRepository.GetRegistersOfUser(userId);

            // Phân trang
            int total = listCourses.Count;
            listCourses = CourseRepository.ApplyPagination(listCourses, pagination);

            // Trả về thành công
            PaginationResponseModel<ViewCourseRegister> res =
                new(total, pagination.Limit, pagination.Offset, listCourses);
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(), string.Empty, res);
        }

        public virtual async Task<ServiceResult> AdminListCourseOfCategory(string catName, PaginationDto pagination)
        {
            // Get về
            List<ViewCourse> listCourses =
                await CourseRepository.GetViewCourseOfCategory(catName, pagination);

            // decorate
            List<IResponseCourseModel> res = await DecorationRepository.DecorateResponseCourseModel(null, listCourses);

            // Thành công
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(CourseResource), string.Empty, res);
        }
        #endregion

        #region Anonymous Apies

        public virtual async Task<ServiceResult> AnonymousGetCourseDetail(Guid courseId)
        {
            // Kiểm tra course tồn tại và public
            ViewCourse course = await CourseRepository.CheckExistedCourse(courseId, NotExistedCourse);
            if (course.Privacy != EPrivacy.Public)
                return ServiceResult.Forbidden("Khóa học ở trạng thái riêng tư");

            // decorate và trả về thành công
            IResponseCourseModel res = (await DecorationRepository.DecorateResponseCourseModel(null, [course])).First();
            return ServiceResult.Success(ResponseResource.GetSuccess(CourseResource), string.Empty, res);
        }

        public virtual async Task<ServiceResult> AnonymousGetCourses(PaginationDto pagination)
        {
            // Get public courses
            List<ViewCourse> lsCourses = await CourseRepository.GetPublicViewCourse(pagination);

            // decorate 
            List<IResponseCourseModel> res = await DecorationRepository.DecorateResponseCourseModel(null, lsCourses);

            // return success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(CourseResource), string.Empty, res);
        }

        public virtual async Task<ServiceResult> AnonymousGetListCourseOfCategory(string catName, PaginationDto pagination)
        {
            // Get list public course by category
            List<ViewCourse> lsCourses =
                await CourseRepository.GetPublicViewCourseOfCategory(catName, pagination);

            // decorate
            List<IResponseCourseModel> res = await DecorationRepository.DecorateResponseCourseModel(null, lsCourses);

            // return Success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(CourseResource), string.Empty, res);
        }

        public virtual async Task<ServiceResult> AnonymousGetUserCourses(Guid userId, PaginationDto pagination)
        {
            // Kiểm tra user tồn tại
            _ = await UserRepository.CheckExisted(userId, ResponseResource.NotExistUser());

            // Lấy về public
            List<ViewCourse> lsCourses = await CourseRepository.GetPublicViewCourseOfUser(userId);

            // Phân trang
            int total = lsCourses.Count;
            lsCourses = CourseRepository.ApplyPagination(lsCourses, pagination);

            // decorate
            List<IResponseCourseModel> lsRes = await DecorationRepository.DecorateResponseCourseModel(null, lsCourses);
            PaginationResponseModel<IResponseCourseModel> res =
                new(total, pagination.Limit, pagination.Offset, lsRes);

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(CourseResource), string.Empty, res);
        }

        #endregion

        #region User operation

        public virtual async Task<ServiceResult> ChangePrivacy(Guid myUid, ChangeKnowledgePrivacyModel model)
        {
            // Kiểm tra course tồn tại và owner
            Course course = await CourseRepository.CheckExisted(model.KnowledgeId ?? Guid.Empty, NotExistedCourse);
            if (course.UserId != myUid)
                return ServiceResult.Forbidden("Bạn không phải là chủ khóa học này");

            // Update privacy 
            course.Privacy = model.Privacy!.Value;
            course.ModifiedBy = myUid.ToString();
            course.ModifiedTime = DateTime.UtcNow;
            await CourseRepository.Update(course.UserId, course);

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.UpdateSuccess(CourseResource));
        }

        public virtual async Task<ServiceResult> UserCreateCourse(Guid myUid, CreateCourseModel model)
        {
            // Kiểm tra myUid tồn tại
            ViewUserProfile user = await UserRepository.CheckExistedUser(myUid, ResponseResource.NotExistUser());

            // Kiểm tra chính sách tạo khóa học của myUid

            // Update thumbnail
            string? thumbnail = await Storage.SaveImage(model.Thumbnail);
            _ = await ImageRepository.TryInsertImage(myUid, thumbnail);

            // Tạo khóa học mới
            Course course = CreateCourse(user, model, thumbnail);
            Guid? id = await CourseRepository.Insert(course.UserItemId, course);
            if (id == null) return ServiceResult.ServerError(
                ResponseResource.InsertFailure(CourseResource));

            // Insert categories nếu có:
            if (model.Categories != null && model.Categories.Count != 0)
            {
                _ = await CategoryRepository.UpdateKnowledgeCategories(course.UserItemId, model.Categories);
            }

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.InsertSuccess(CourseResource), string.Empty, course);
        }

        public virtual async Task<ServiceResult> UserDeleteCourse(Guid myUid, Guid courseId)
        {
            // Check course existed and owner
            Course course = await CourseRepository.CheckExisted(courseId, NotExistedCourse);
            if (course.UserId != myUid)
                return ServiceResult.Forbidden("Đây không phải khóa học của bạn");

            // Kiểm tra không có registers nào
            List<ViewCourseRegister> courseRegisters = await CourseRepository
                .GetRegistersOfCourse(courseId);
            if (courseRegisters.Count != 0)
                return ServiceResult.Forbidden("Không thể xóa khóa học đang có người học");

            // Delete
            int deleted = await CourseRepository.Delete(courseId);
            if (deleted <= 0)
                return ServiceResult.ServerError(ResponseResource.DeleteFailure(CourseResource));

            // Return Success
            return ServiceResult.Success(ResponseResource.DeleteSuccess(CourseResource));
        }

        public virtual async Task<ServiceResult> UserUpdateCourse(Guid myUid, Guid courseId, UpdateCourseModel model)
        {
            // Check course existed and owner
            Course course = await CourseRepository.CheckExisted(courseId, NotExistedCourse);
            if (course.UserId != myUid)
                return ServiceResult.Forbidden("Đây không phải khóa học của bạn");

            // update thumbnail:
            string? thumbnail = await Storage.SaveImage(model.Thumbnail);
            _ = await ImageRepository.TryInsertImage(myUid, thumbnail);

            // Update
            Course courseToUpdate = new();
            courseToUpdate.Copy(course);
            courseToUpdate.Copy(model);
            if (model.Fee > 0)
            {
                courseToUpdate.IsFree = false;
            }
            courseToUpdate.ModifiedTime = DateTime.UtcNow;
            courseToUpdate.ModifiedBy = myUid.ToString();
            if (thumbnail != null) courseToUpdate.Thumbnail = thumbnail;
            int effects1 = await CourseRepository.Update(courseId, courseToUpdate);
            int effects2 = 0;

            // Update categories nếu có:
            if (model.Categories != null && model.Categories.Count != 0)
            {
                effects2 = await CategoryRepository.UpdateKnowledgeCategories(courseId, model.Categories);
            }

            if (effects1 + effects2 <= 0) return ServiceResult.ServerError(ResponseResource.UpdateFailure(CourseResource));
            // return success
            return ServiceResult.Success(ResponseResource.UpdateSuccess(CourseResource), string.Empty, courseToUpdate);
        }

        #endregion

        #region User get apies
        public virtual async Task<ServiceResult> UserGetCourseDetail(Guid myUid, Guid courseId)
        {
            // Kiểm tra course tồn tại
            ViewCourse course = await CourseRepository.CheckExistedCourse(courseId, NotExistedCourse);
            //if (course.UserId == myUid) return await UserGetMyCourseDetail(myUid, courseId);

            // Kiểm tra accessible
            CourseRoleTypeDto courseRoleTypeDto = (await CourseRepository.GetCourseRoleType(myUid, [courseId]))[courseId];
            if (courseRoleTypeDto.CourseRoleType == ECourseRoleType.NotAccessible)
                return ServiceResult.Forbidden("Bạn không có quyền truy cập khóa học này");

            // decorate và trả về thành công
            IResponseCourseModel res = (await DecorationRepository.DecorateResponseCourseModel(myUid, [course], isDecorateCourseRoleType: false)).First();
            res.CourseRoleType = courseRoleTypeDto.CourseRoleType;
            res.CourseRelationId = courseRoleTypeDto.CourseRelationId;
            return ServiceResult.Success(ResponseResource.GetSuccess(CourseResource), string.Empty, res);
        }

        public virtual async Task<ServiceResult> UserGetListCourseOfCategory(Guid myUid, string catName, PaginationDto pagination)
        {
            // Get về
            List<ViewCourse> listCourses =
                await CourseRepository.GetViewCourseOfCategory(myUid, catName, pagination);

            // decorate
            List<IResponseCourseModel> res = await DecorationRepository.DecorateResponseCourseModel(myUid, listCourses);

            // Thành công
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(CourseResource), string.Empty, res);
        }

        public virtual async Task<ServiceResult> UserGetListCourses(Guid myUid, PaginationDto pagination)
        {
            // Get
            List<ViewCourse> listCourses =
                await CourseRepository.GetViewCourse(myUid, pagination);

            // decorate
            List<IResponseCourseModel> res =
                await DecorationRepository.DecorateResponseCourseModel(myUid, listCourses);

            // Return success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(CourseResource), string.Empty, res);
        }

        public virtual async Task<ServiceResult> UserGetMarkedCourses(Guid myUid, PaginationDto pagination)
        {
            List<ViewCourse> lsCourses =
                await CourseRepository.GetMarkedCoursesOfUser(myUid, pagination);

            List<IResponseCourseModel> res = await DecorationRepository.DecorateResponseCourseModel(myUid, lsCourses);

            return ServiceResult.Success(ResponseResource.GetMultiSuccess(CourseResource), string.Empty, res);
        }

        public virtual async Task<ServiceResult> UserGetMyCourseDetail(Guid myUid, Guid courseId)
        {
            // Check course existed and owner
            ViewCourse course = await CourseRepository.CheckExistedCourse(courseId, NotExistedCourse);
            if (course.UserId != myUid)
                return ServiceResult.Forbidden("Đây không phải khóa học của bạn");

            // decorate
            IResponseCourseModel res = (await DecorationRepository.DecorateResponseCourseModel(myUid, [course])).First();
            res.CourseRoleType = ECourseRoleType.Owner;
            res.CourseRelationId = res.UserItemId;

            // return Success
            return ServiceResult.Success(ResponseResource.GetSuccess(CourseResource), string.Empty, res);
        }

        public virtual async Task<ServiceResult> UserGetMyCourses(Guid myUid, PaginationDto pagination)
        {
            // Lấy về
            List<ViewCourse> lsCourses = await CourseRepository.GetViewCourseOfUser(myUid);

            // Pagination
            int total = lsCourses.Count;
            lsCourses = CourseRepository.ApplyPagination(lsCourses, pagination);

            // decorate
            List<IResponseCourseModel> lsRes = await DecorationRepository.DecorateResponseCourseModel(myUid, lsCourses, isDecorateCourseRoleType: false);
            lsRes.ForEach(course =>
            {
                course.CourseRoleType = ECourseRoleType.Owner;
                course.CourseRelationId = course.UserItemId;
            });
            PaginationResponseModel<IResponseCourseModel> res =
                new(total, pagination.Limit, pagination.Offset, lsRes);

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(CourseResource), string.Empty, res);
        }

        public virtual async Task<ServiceResult> UserGetMyRegisteredCourses(Guid myUid, PaginationDto pagination)
        {
            // Lấy về 
            List<ViewCourseRegister> lsCourses = await CourseRepository.GetRegistersOfUser(myUid);

            // Phân trang
            int total = lsCourses.Count;
            lsCourses = CourseRepository.ApplyPagination(lsCourses, pagination);
            PaginationResponseModel<ViewCourseRegister> res = new(total, pagination.Limit, pagination.Offset, lsCourses);

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(), string.Empty, res);
        }

        public virtual async Task<ServiceResult> UserGetUserCourses(Guid myUid, Guid userId, PaginationDto pagination)
        {
            // Check user existed
            _ = await UserRepository.CheckExisted(userId, ResponseResource.NotExistUser());

            // Get 
            List<ViewCourse> lsCourses = await CourseRepository.GetPublicViewCourseOfUser(userId);

            // Pagination
            int total = lsCourses.Count;
            lsCourses = CourseRepository.ApplyPagination(lsCourses, pagination);

            // decorate
            List<IResponseCourseModel> lsres = await DecorationRepository.DecorateResponseCourseModel(myUid, lsCourses);
            PaginationResponseModel<IResponseCourseModel> res = new(total, pagination.Limit, pagination.Offset, lsres);

            // return success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(CourseResource), string.Empty, res);
        }

        #endregion



        #region Search APIs


        public virtual async Task<ServiceResult> UserSearchCourse(Guid? myUid, string? search, PaginationDto pagination)
        {
            // normalized search key
            if (string.IsNullOrWhiteSpace(search))
                return ServiceResult.BadRequest("Từ khóa rỗng");
            search = search.ToLower();

            // Get posts
            List<ViewCourse> listCourse = await CourseRepository.GetPublicViewCourse();

            // calculate score
            List<(Guid, string, string, string?, int?, int?)> listShortPost = listCourse
                .Select(p => (p.UserItemId, p.Title, p.FullName, p.Abstract, p.SumStar, p.TotalStar)).ToList();
            Dictionary<Guid, double> scored = CalculateKnowledgeSearchScore.CalculateKnowledgeScore(search, listShortPost);

            // order by score
            listCourse = [.. listCourse.OrderByDescending(p => scored[p.UserItemId])];

            // apply pagination
            if (pagination.Filters != null)
            {
                listCourse = CourseRepository.ApplyFilter(listCourse, pagination.Filters);
            }
            listCourse = listCourse.Skip(pagination.Offset ?? 0).Take(pagination.Limit ?? 15).ToList();

            // decoration
            List<IResponseCourseModel> res = await DecorationRepository.DecorateResponseCourseModel(myUid, listCourse);

            // return 
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(), string.Empty, res);
        }

        public virtual async Task<ServiceResult> UserSearchMyCourse(Guid myUid, string? search, PaginationDto pagination)
        {
            // normalized search key
            if (string.IsNullOrWhiteSpace(search))
                return ServiceResult.BadRequest("Từ khóa rỗng");
            search = search.ToLower();

            // Get posts
            List<ViewCourse> listCourse = await CourseRepository.GetViewCourseOfUser(myUid);

            // calculate score
            List<(Guid, string, string, string?, int?, int?)> listShortPost = listCourse
                .Select(p => (p.UserItemId, p.Title, p.FullName, p.Abstract, p.SumStar, p.TotalStar)).ToList();
            Dictionary<Guid, double> scored = CalculateKnowledgeSearchScore.CalculateKnowledgeScore(search, listShortPost);

            // order by score
            listCourse = [.. listCourse.OrderByDescending(p => scored[p.UserItemId])];

            // apply pagination
            if (pagination.Filters != null)
            {
                listCourse = CourseRepository.ApplyFilter(listCourse, pagination.Filters);
            }
            listCourse = listCourse.Skip(pagination.Offset ?? 0).Take(pagination.Limit ?? 15).ToList();

            // decoration
            List<IResponseCourseModel> res = await DecorationRepository.DecorateResponseCourseModel(myUid, listCourse);

            // return 
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(), string.Empty, res);
        }

        public virtual async Task<ServiceResult> UserSearchUserCourse(Guid myUid, Guid userId, string? search, PaginationDto pagination)
        {
            // normalized search key
            if (string.IsNullOrWhiteSpace(search))
                return ServiceResult.BadRequest("Từ khóa rỗng");
            search = search.ToLower();

            // Get posts
            List<ViewCourse> listCourse = await CourseRepository.GetPublicViewCourseOfUser(userId);

            // calculate score
            List<(Guid, string, string, string?, int?, int?)> listShortPost = listCourse
                .Select(p => (p.UserItemId, p.Title, p.FullName, p.Abstract, p.SumStar, p.TotalStar)).ToList();
            Dictionary<Guid, double> scored = CalculateKnowledgeSearchScore.CalculateKnowledgeScore(search, listShortPost);

            // order by score
            listCourse = [.. listCourse.OrderByDescending(p => scored[p.UserItemId])];

            // apply pagination
            if (pagination.Filters != null)
            {
                listCourse = CourseRepository.ApplyFilter(listCourse, pagination.Filters);
            }
            listCourse = listCourse.Skip(pagination.Offset ?? 0).Take(pagination.Limit ?? 15).ToList();

            // decoration
            List<IResponseCourseModel> res = await DecorationRepository.DecorateResponseCourseModel(myUid, listCourse);

            // return 
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(), string.Empty, res);
        }

        public virtual async Task<ServiceResult> AdminSearchUserCourse(Guid userId, string? search, PaginationDto pagination)
        {
            // normalized search key
            if (string.IsNullOrWhiteSpace(search))
                return ServiceResult.BadRequest("Từ khóa rỗng");
            search = search.ToLower();

            // Get posts
            List<ViewCourse> listCourse = await CourseRepository.GetViewCourseOfUser(userId);

            // calculate score
            List<(Guid, string, string, string?, int?, int?)> listShortPost = listCourse
                .Select(p => (p.UserItemId, p.Title, p.FullName, p.Abstract, p.SumStar, p.TotalStar)).ToList();
            Dictionary<Guid, double> scored = CalculateKnowledgeSearchScore.CalculateKnowledgeScore(search, listShortPost);

            // order by score
            listCourse = [.. listCourse.OrderByDescending(p => scored[p.UserItemId])];

            // apply pagination
            if (pagination.Filters != null)
            {
                listCourse = CourseRepository.ApplyFilter(listCourse, pagination.Filters);
            }
            listCourse = listCourse.Skip(pagination.Offset ?? 0).Take(pagination.Limit ?? 15).ToList();

            // decoration
            List<IResponseCourseModel> res = await DecorationRepository.DecorateResponseCourseModel(null, listCourse);

            // return 
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(), string.Empty, res);
        }

        #endregion
    }
}
