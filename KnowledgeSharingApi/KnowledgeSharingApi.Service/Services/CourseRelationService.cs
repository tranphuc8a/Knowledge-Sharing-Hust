using KnowledgeSharingApi.Domains.Algorithms;
using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.DecorationRepositories;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Win32;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Asn1.Ocsp;
using System;

namespace KnowledgeSharingApi.Services.Services
{
    public class CourseRelationService : ICourseRelationService
    {
        protected readonly IResourceFactory ResourceFactory;
        protected readonly IResponseResource ResponseResource;
        protected readonly IEntityResource EntityResource;

        protected readonly IUserRepository UserRepository;
        protected readonly IDecorationRepository DecorationRepository;
        protected readonly ICalculateKnowledgeSearchScore CalculateKnowledgeSearchScore;
        protected readonly ICourseRepository CourseRepository;
        protected readonly ICourseRelationRepository CourseRelationRepository;
        protected readonly ICoursePaymentRepository CoursePaymentRepository;
        protected readonly ICourseRegisterRepository CourseRegisterRepository;

        protected readonly string CourseResource, NotExistedCourse;
        protected readonly string MemberResource = "Thành viên khóa học", NotExistedMember = "Không tồn tại thành viên khóa học",
            NotBeCourseOnwner = "Bạn không phải là chủ khóa học",
            NotBeCourseMember = "Bạn không phải là thành viên khóa học",
            NotAccessibleCourse = "Bạn không có quyền truy cập khóa học này";
        protected readonly int DefaultLimit = 20;

        public CourseRelationService(
            IResourceFactory resourceFactory,
            IUserRepository userRepository,
            ICourseRelationRepository courseRelationRepository,
            ICalculateKnowledgeSearchScore calculateKnowledgeSearchScore,
            ICoursePaymentRepository coursePaymentRepository,
            ICourseRegisterRepository courseRegisterRepository,
            IDecorationRepository decorationRepository,
            ICourseRepository courseRepository
        )
        {
            ResourceFactory = resourceFactory;
            ResponseResource = ResourceFactory.GetResponseResource();
            EntityResource = ResourceFactory.GetEntityResource();

            UserRepository = userRepository;
            CourseRepository = courseRepository;
            CourseRelationRepository = courseRelationRepository;
            CalculateKnowledgeSearchScore = calculateKnowledgeSearchScore;
            CoursePaymentRepository = coursePaymentRepository;
            CourseRegisterRepository = courseRegisterRepository;
            DecorationRepository = decorationRepository;

            CourseResource = EntityResource.Course();
            NotExistedCourse = ResponseResource.NotExist(CourseResource);

        }


        #region Functionality methods

        /// <summary>
        /// Ket hop phan trang va decoration
        /// </summary>
        /// Created: PhucTV (30/3/24)
        /// Modified: None
        protected virtual async Task<PaginationResponseModel<ResponseCourseRelationModel>> PaginationAndDecoration(
            Guid? myUid,
            List<CourseRelation> relations,
            PaginationDto pagination,
            ECourseRelationType relationType,
            bool isDecorateUser = false,
            bool isDecorateCourse = false
        )
        {
            // Pagination
            int total = relations.Count;
            relations = CoursePaymentRepository.ApplyPagination(relations, pagination);

            // Decoration:
            List<ResponseCourseRelationModel> lsRes = await
                DecorationRepository.DecorateResponseCourseRelationModel(myUid, relations, relationType, isDecorateUser, isDecorateCourse);

            // Return:
            return new PaginationResponseModel<ResponseCourseRelationModel>(total, pagination.Limit, pagination.Offset, lsRes);
        }

        #endregion


        #region Admin apies
        public virtual async Task<ServiceResult> AdminDeleteUserFromCourse(Guid registerId)
        {
            // Check register is existed
            ViewCourseRegister? register = await CourseRegisterRepository.GetCourseRegister(registerId);
            if (register == null) return ServiceResult.BadRequest(NotExistedMember);

            // Check register has payment
            List<ViewCoursePayment> payments =
                await CoursePaymentRepository.GetCoursePayment(register.UserId, register.CourseId);
            if (payments.Count != 0)
                return ServiceResult.BadRequest("Không thể xóa thành viên đã thanh toán khóa học");

            // Delete register 
            int deleted = await CourseRegisterRepository.Delete(register.CourseRegisterId);
            if (deleted <= 0) return ServiceResult.ServerError(ResponseResource.DeleteFailure(MemberResource));

            // Return success
            return ServiceResult.Success(ResponseResource.DeleteSuccess(MemberResource));
        }

        public virtual async Task<ServiceResult> AdminGetCourseRegisters(Guid courseId, PaginationDto pagination)
        {
            // CHeck course Existed
            _ = await CourseRepository.CheckExisted(courseId, NotExistedCourse);

            // Get lisst register
            List<ViewCourseRegister> registers = await CourseRegisterRepository.GetCourseRegisters(courseId);

            // Pagination;
            int total = registers.Count;
            registers = CourseRegisterRepository.ApplyPagination(registers, pagination);

            // Return Success
            PaginationResponseModel<ResponseCourseRegisterModel> res = new(
                total, 
                pagination.Limit, 
                pagination.Offset,
                await DecorationRepository.DecorateResponseCourseRegisterModel(null, registers));
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(MemberResource), string.Empty, res);
        }
        #endregion

        #region User gets
        public virtual async Task<ServiceResult> UserGetCourseInvites(Guid myUid, Guid courseId, PaginationDto pagination)
        {
            // Check course exist
            Course course = await CourseRepository.CheckExisted(courseId, NotExistedCourse);

            // Check role is owner
            if (course.UserId != myUid)
                return ServiceResult.Forbidden(NotBeCourseOnwner);

            // Get list invite of course
            List<CourseRelation> relations = await CourseRelationRepository.GetRelationsOfCourse(courseId, ECourseRelationType.Invite);

            // Pagination and Decoration
            PaginationResponseModel<ResponseCourseRelationModel> res = await PaginationAndDecoration(
                myUid, relations, pagination, ECourseRelationType.Invite, isDecorateUser: true, isDecorateCourse: false
            );

            // Return success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(), string.Empty, res);
        }

        public virtual async Task<ServiceResult> UserGetCourseRequests(Guid myUid, Guid courseId, PaginationDto pagination)
        {
            // Check course is existed
            Course course = await CourseRepository.CheckExisted(courseId, NotExistedCourse);

            // Check owner
            if (course.UserId != myUid)
                return ServiceResult.Forbidden(NotBeCourseOnwner);

            // Get list requests
            List<CourseRelation> relations = await CourseRelationRepository.GetRelationsOfCourse(courseId, ECourseRelationType.Request);

            // Pagination and Decoration
            PaginationResponseModel<ResponseCourseRelationModel> res = await PaginationAndDecoration(
                myUid, relations, pagination, ECourseRelationType.Request, isDecorateUser: true, isDecorateCourse: false);

            // Return Success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(), string.Empty, res);
        }

        public virtual async Task<ServiceResult> UserGetMyCourseInvites(Guid myUid, PaginationDto pagination)
        {
            // Get list
            List<CourseRelation> lisRelations = await CourseRelationRepository.GetRelationsOfUser(myUid, ECourseRelationType.Invite);

            // Pagination and decoration
            PaginationResponseModel<ResponseCourseRelationModel> res = await PaginationAndDecoration(
                myUid, lisRelations, pagination, ECourseRelationType.Invite, isDecorateUser: false, isDecorateCourse: true);

            // return success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(), string.Empty, res);
        }

        public virtual async Task<ServiceResult> UserGetMyCourseRequests(Guid myUid, PaginationDto pagination)
        {
            // Get list
            List<CourseRelation> lisRelations = await CourseRelationRepository.GetRelationsOfUser(myUid, ECourseRelationType.Request);

            // Pagination and decoration
            PaginationResponseModel<ResponseCourseRelationModel> res = await PaginationAndDecoration(
                myUid, lisRelations, pagination, ECourseRelationType.Request, isDecorateUser: false, isDecorateCourse: true);

            // return success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(), string.Empty, res);
        }

        public virtual async Task<ServiceResult> UserGetRegisters(Guid? myUid, Guid courseId, PaginationDto pagination)
        {
            // Check course existed
            Course course = await CourseRepository.CheckExisted(courseId, NotExistedCourse);

            if (course.Privacy == EPrivacy.Private)
            {
                // Check role is owner, invited or member
                if (myUid == null) // anonymous --> not accessible
                    return ServiceResult.Forbidden(NotAccessibleCourse);

                ECourseRoleType roleType = await CourseRelationRepository.GetRole(myUid.Value, courseId);
                if (roleType != ECourseRoleType.Owner && roleType != ECourseRoleType.Invited && roleType != ECourseRoleType.Member)
                    return ServiceResult.Forbidden(NotAccessibleCourse);
            }

            // Get lisst register
            List<ViewCourseRegister> registers = await CourseRegisterRepository.GetCourseRegisters(courseId);

            // Pagination
            int total = registers.Count;
            registers = CourseRegisterRepository.ApplyPagination(registers, pagination);

            // Return Success
            PaginationResponseModel<ResponseCourseRegisterModel> res = new(
                total,
                pagination.Limit,
                pagination.Offset,
                await DecorationRepository.DecorateResponseCourseRegisterModel(myUid, registers));

            return ServiceResult.Success(ResponseResource.GetMultiSuccess(MemberResource), string.Empty, res);
        }

        public virtual async Task<ServiceResult> UserGetCourseRelationStatus(Guid myUid, Guid courseId, bool? isFocusCourse = true)
        {
            // check course exist
            ViewCourse course = await CourseRepository.CheckExistedCourse(courseId, NotExistedCourse);

            // get course role type type
            Dictionary<Guid, CourseRoleTypeDto> dictRoleType = await CourseRelationRepository.GetCourseRoleType(myUid, [courseId]);
            if (dictRoleType.TryGetValue(courseId, out CourseRoleTypeDto? value))
            {
                if (value.CourseRoleType == ECourseRoleType.NotAccessible)
                {
                    return ServiceResult.Forbidden("Bạn không có quyền truy cập khóa học này");
                }

                // Return ResponseUseCardModel if focus user
                // Return ResponseCourseCardModel if focus course
                // Both have CourseRoleType and CourseRelationId

                if (isFocusCourse == false)
                {
                    ViewUser user = await UserRepository.CheckExistedUser(myUid, ResponseResource.NotExistUser());
                    ResponseUserCardModel resUser = new();
                    resUser.Copy(user);
                    resUser.CourseRoleType = value.CourseRoleType;
                    resUser.CourseRelationId = value.CourseRelationId;
                    return ServiceResult.Success(ResponseResource.GetSuccess(), string.Empty, resUser);
                }
                else
                {
                    ResponseCourseCardModel res = new();
                    res.Copy(course);
                    res.CourseRoleType = value.CourseRoleType;
                    res.CourseRelationId = value.CourseRelationId;
                    return ServiceResult.Success(ResponseResource.GetSuccess(), string.Empty, res);
                }
            }

            // return server error
            return ServiceResult.ServerError(ResponseResource.GetFailure());
        }

        public virtual async Task<ServiceResult> UserGetCourseRelationStatus(Guid myUid, Guid userId, Guid courseId, bool? isFocusCourse = true)
        {
            // Check userid and courseid existed
            ViewCourse course = await CourseRepository.CheckExistedCourse(courseId, NotExistedCourse);
            ViewUser user = await UserRepository.CheckExistedUser(userId, ResponseResource.NotExistUser());

            // Check myUid is owner
            if (course.UserId != myUid)
            {
                return ServiceResult.Forbidden(NotBeCourseOnwner);
            }

            // Get course status (tuong tu o tren)
            Dictionary<Guid, CourseRoleTypeDto> dictRole = await CourseRelationRepository.GetCourseRoleType(userId, [courseId]);
            if (dictRole.TryGetValue(userId, out CourseRoleTypeDto? value))
            {
                if (isFocusCourse == false)
                {
                    ResponseUserCardModel resUser = new();
                    resUser.Copy(user);
                    resUser.CourseRoleType = value.CourseRoleType;
                    resUser.CourseRelationId = value.CourseRelationId;
                    return ServiceResult.Success(ResponseResource.GetSuccess(), string.Empty, resUser);
                }
                else
                {
                    ResponseCourseCardModel res = new();
                    res.Copy(course);
                    res.CourseRoleType = value.CourseRoleType;
                    res.CourseRelationId = value.CourseRelationId;
                    return ServiceResult.Success(ResponseResource.GetSuccess(), string.Empty, res);
                }
            }

            return ServiceResult.ServerError(ResponseResource.GetFailure());
        }

        #endregion


        #region User Operations apies
        #region Requestation
        public virtual async Task<ServiceResult> UserRequestCourse(Guid myUid, Guid courseId)
        {
            // Check course is existed
            Course course = await CourseRepository.CheckExisted(courseId, NotExistedCourse);

            // Kiem tra khoa hoc la co tinh phi:
            if (course.IsFree || course.Fee <= 0)
            {
                return ServiceResult.BadRequest("Đây là khóa học miễn phí, hãy đăng ký trực tiếp tham gia khóa học");
            }

            // Check Role is Guest, Accessible, (Not Invited, Not Requested, Not Owner and Not Member, NOt of NotAccessible)
            ECourseRoleType roleType = await CourseRelationRepository.GetRole(myUid, courseId);
            if (roleType == ECourseRoleType.NotAccessible)
                return ServiceResult.Forbidden(NotAccessibleCourse);
            if (roleType == ECourseRoleType.Member)
                return ServiceResult.BadRequest("Bạn đã tham gia khóa học này rồi");
            if (roleType == ECourseRoleType.Owner)
                return ServiceResult.BadRequest("Bạn là chủ của khóa học");
            if (roleType == ECourseRoleType.Requesting)
                return ServiceResult.BadRequest("Bạn đã gửi yêu cầu tham gia khóa học trước đó rồi");
            if (roleType == ECourseRoleType.Invited)
                return ServiceResult.BadRequest("Hãy xác nhận lời mời tham gia khóa học của chủ khóa học trước");

            // Get course owner
            User? courseOwner = await UserRepository.Get(course.UserId);
            if (courseOwner == null)
                return ServiceResult.ServerError("Không tìm thấy chủ của khóa học này");

            // Add Request
            CourseRelation request = new()
            {
                // Entity:
                CreatedTime = DateTime.UtcNow,
                CreatedBy = myUid.ToString(),
                // CourseRelation
                CourseRelationId = Guid.NewGuid(),
                SenderId = myUid,
                CourseId = courseId,
                ReceiverId = courseOwner.UserId,
                CourseRelationType = ECourseRelationType.Request
            };
            Guid? res = await CourseRelationRepository.Insert(request.CourseRelationId, request);
            if (res == null)
                return ServiceResult.ServerError(ResponseResource.ServerError());

            // Return success
            return ServiceResult.Success(ResponseResource.Success(), string.Empty, request);
        }

        public virtual async Task<ServiceResult> UserDeleteCourseRequest(Guid myUid, Guid requestId)
        {
            // Check course relation is existed
            CourseRelation? courseRelation = await CourseRelationRepository.GetRelation(requestId);
            if (courseRelation == null || courseRelation.CourseRelationType != ECourseRelationType.Request)
                return ServiceResult.BadRequest(ResponseResource.NotExist());

            // Check owner is myUid
            if (courseRelation.SenderId != myUid)
                return ServiceResult.Forbidden("Đây không phải yêu cầu của bạn");

            // OK Delete
            int deleted = await CourseRegisterRepository.Delete(requestId);
            if (deleted <= 0) return ServiceResult.ServerError(ResponseResource.DeleteFailure());

            // return Success
            return ServiceResult.Success(ResponseResource.DeleteSuccess());
        }

        public virtual async Task<ServiceResult> UserConfirmCourseRequest(Guid myUid, Guid requestId, bool isAccept)
        {
            // Check course relation is existed
            CourseRelation? courseRelation = await CourseRelationRepository.GetRelation(requestId);
            if (courseRelation == null || courseRelation.CourseRelationType != ECourseRelationType.Request)
                return ServiceResult.BadRequest(ResponseResource.NotExist());

            // Check owner receiver
            if (courseRelation.ReceiverId != myUid)
                return ServiceResult.Forbidden("Đây không phải yêu cầu dành cho bạn");


            // OK thuc hienj
            if (isAccept)
            {
                int effects = await CourseRelationRepository.ConfirmCourseRelation(requestId);
                if (effects <= 0)
                    return ServiceResult.ServerError(ResponseResource.Failure());
            }
            else
            { // tu choi: delete request
                int deleted = await CourseRelationRepository.Delete(requestId);
                if (deleted <= 0) return ServiceResult.ServerError(ResponseResource.Failure());
            }

            return ServiceResult.Success(ResponseResource.Success());
        }

        #endregion

        #region Invitation

        public virtual async Task<ServiceResult> UserInviteUserToCourse(Guid myUid, Guid courseId, Guid userId)
        {
            // CHeck two id is different
            if (myUid == userId)
                return ServiceResult.BadRequest("Không thể mời chính mình");

            // Check course exist
            Course course = await CourseRepository.CheckExisted(courseId, NotExistedCourse);

            // CHeck owner of course
            if (course.UserId != myUid)
                return ServiceResult.Forbidden(NotBeCourseOnwner);

            // CHeck userId is existed
            _ = await UserRepository.CheckExisted(userId, ResponseResource.NotExistUser());

            // Check user role is guest or inAccessible (Not be owner, not be member, not be requesting, not be invited)
            ECourseRoleType userRoles = await CourseRelationRepository.GetRole(userId, courseId);
            if (userRoles == ECourseRoleType.Member)
                return ServiceResult.BadRequest("Người dùng đã tham gia khóa học này rồi");
            if (userRoles == ECourseRoleType.Owner)
                return ServiceResult.BadRequest("Không thể mời chủ khóa học");
            if (userRoles == ECourseRoleType.Requesting)
                return ServiceResult.BadRequest("Hãy phê duyệt yêu cầu tham gia khóa học của user này trước");
            if (userRoles == ECourseRoleType.Invited)
                return ServiceResult.BadRequest("Người dùng đã được mời tham gia khóa học trước đó");

            // OK create new invite and insert
            CourseRelation invite = new()
            {
                // Entity:
                CreatedBy = myUid.ToString(),
                CreatedTime = DateTime.UtcNow,
                // CourseRelation:
                CourseRelationId = Guid.NewGuid(),
                SenderId = myUid,
                ReceiverId = userId,
                CourseId = courseId,
                CourseRelationType = ECourseRelationType.Invite
            };
            Guid? res = await CourseRelationRepository.Insert(invite.CourseRelationId, invite);
            if (res == null) return ServiceResult.ServerError(ResponseResource.ServerError());

            // Return success
            return ServiceResult.Success(ResponseResource.Success(), string.Empty, invite);
        }

        public virtual async Task<ServiceResult> UserInviteListUserToCourse(Guid myUid, Guid courseId, List<Guid> listUserIds)
        {
            // Check course exist
            Course course = await CourseRepository.CheckExisted(courseId, NotExistedCourse);

            // CHeck owner of course
            if (course.UserId != myUid)
                return ServiceResult.Forbidden(NotBeCourseOnwner);

            // Kiem tra danh sach user phai chua tham gia khoa hoc, chưa request khoa hoc, khong phai chu nhan khoa hoc

            // Bỏ không làm vì quá phức tạp

            return ServiceResult.ServerError("This api is not be supported");
        }

        public virtual async Task<ServiceResult> UserDeleteCourseInvite(Guid myUid, Guid inviteId)
        {
            // Kiem tra relation ton tai
            CourseRelation? invite = await CourseRelationRepository.Get(inviteId);
            if (invite == null) return ServiceResult.BadRequest(ResponseResource.NotExist());

            // Kiem tra owner cua invite
            if (invite.SenderId != myUid)
                return ServiceResult.Forbidden("Bạn không có quyền truy cập invite này");

            // Ok xoa invite
            int deleted = await CourseRelationRepository.Delete(inviteId);
            if (deleted <= 0) return ServiceResult.ServerError(ResponseResource.DeleteFailure());

            // Tra ve thanh cong
            return ServiceResult.Success(ResponseResource.DeleteSuccess());
        }

        public virtual async Task<ServiceResult> UserConfirmCourseInvite(Guid myUid, Guid inviteId, bool isAccept)
        {
            // Check course relation is existed
            CourseRelation? courseRelation = await CourseRelationRepository.GetRelation(inviteId);
            if (courseRelation == null || courseRelation.CourseRelationType != ECourseRelationType.Invite)
                return ServiceResult.BadRequest(ResponseResource.NotExist());

            // Check owner receiver
            if (courseRelation.ReceiverId != myUid)
                return ServiceResult.Forbidden("Đây không phải lời mời gửi tới bạn");


            // OK thuc hienj
            if (isAccept)
            {
                int effects = await CourseRelationRepository.ConfirmCourseRelation(inviteId);
                if (effects <= 0)
                    return ServiceResult.ServerError(ResponseResource.Failure());
            }
            else
            { // delete request
                int deleted = await CourseRelationRepository.Delete(courseRelation.CourseRelationId);
                if (deleted <= 0) return ServiceResult.ServerError(ResponseResource.Failure());
            }

            return ServiceResult.Success(ResponseResource.Success());
        }
        #endregion

        #region Register
        public virtual async Task<ServiceResult> UserRegisterCourse(Guid myUid, Guid courseId)
        {
            // Kiem tra khoa hoc ton tai
            Course course = await CourseRepository.CheckExisted(courseId, NotExistedCourse);

            // Kiem tra khoa hoc mien phi, cong khai
            if (!course.IsFree || course.Fee > 0)
                return ServiceResult.Forbidden("Đây là khóa học có tính phí, hãy thanh toán để tham gia khóa học này");
            if (course.Privacy != EPrivacy.Public)
                return ServiceResult.Forbidden(NotAccessibleCourse);

            // Kiem tra role la Guest
            ECourseRoleType roleType = await CourseRelationRepository.GetRole(myUid, courseId);
            if (roleType == ECourseRoleType.NotAccessible)
                return ServiceResult.Forbidden(NotAccessibleCourse);
            if (roleType == ECourseRoleType.Member)
                return ServiceResult.BadRequest("Bạn đã tham gia khóa học này rồi");
            if (roleType == ECourseRoleType.Owner)
                return ServiceResult.BadRequest("Bạn là chủ của khóa học");
            if (roleType == ECourseRoleType.Invited)
                return ServiceResult.BadRequest("Hãy xác nhận lời mời tham gia khóa học này trước");
            if (roleType == ECourseRoleType.Requesting)
                return ServiceResult.BadRequest("Bạn đã gửi yêu cầu tham gia khóa học này trước đó, hãy hủy yêu cầu trước khi đăng ký tham gia lại");

            // OK, dang ky user
            CourseRegister courseRegister = new()
            {
                // Entity:
                CreatedTime = DateTime.UtcNow,
                CreatedBy = myUid.ToString(),
                // Course Register:
                CourseRegisterId = Guid.NewGuid(),
                CourseId = courseId,
                UserId = myUid
            };
            Guid? res = await CourseRegisterRepository.Insert(courseRegister.CourseRegisterId, courseRegister);
            if (res == null) return ServiceResult.ServerError(ResponseResource.ServerError());

            // Tra ve thanh cong
            return ServiceResult.Success(ResponseResource.Success(), string.Empty, courseRegister);
        }

        public virtual async Task<ServiceResult> UserDeleteRegister(Guid myUid, Guid registerId)
        {
            // Check register is existed
            ViewCourseRegister? register = await CourseRegisterRepository.GetCourseRegister(registerId);
            if (register == null) return ServiceResult.BadRequest(NotExistedMember);

            // Check is owner of course
            Course? course = await CourseRepository.Get(register.CourseId);
            if (course == null || course.UserId != myUid)
                return ServiceResult.Forbidden(NotBeCourseOnwner);

            // Check register has payment
            List<ViewCoursePayment> payments =
                await CoursePaymentRepository.GetCoursePayment(register.UserId, register.CourseId);
            if (payments.Count != 0)
                return ServiceResult.BadRequest("Không thể xóa thành viên đã thanh toán khóa học");

            // Delete register 
            int deleted = await CourseRegisterRepository.Delete(register.CourseRegisterId);
            if (deleted <= 0) return ServiceResult.ServerError(ResponseResource.DeleteFailure(MemberResource));

            // Return success
            return ServiceResult.Success(ResponseResource.DeleteSuccess(MemberResource));
        }

        public virtual async Task<ServiceResult> UserUnregisterCourse(Guid myUid, Guid courseId)
        {
            // Kiem tra khoa hoc ton tai
            _ = await CourseRepository.CheckExisted(courseId, NotExistedCourse);

            // Kiem tra role phai la member (lay ve register)
            ViewCourseRegister? courseRegister = await CourseRepository.GetViewCourseRegister(myUid, courseId);
            if (courseRegister == null)
                return ServiceResult.Forbidden(NotBeCourseMember);

            // Delete
            int deleted = await CourseRegisterRepository.Delete(courseRegister.CourseRegisterId);
            if (deleted <= 0) return ServiceResult.ServerError(ResponseResource.DeleteFailure());

            // Tra ve thanh cong
            return ServiceResult.Success(ResponseResource.Success());
        }

        #endregion

        #endregion


        #region Search APIs

        public virtual async Task<ServiceResult> UserSearchRegisters(Guid? myUid, Guid courseId, string? search, PaginationDto page)
        {
            if (string.IsNullOrEmpty(search)) return ServiceResult.BadRequest("Từ khóa tìm kiếm rỗng");

            // check course existed
            Course course = await CourseRepository.CheckExisted(courseId, NotExistedCourse);

            // check if course is private then Uid must join or be owner of course
            if (course.Privacy == EPrivacy.Private)
            {
                // Check role is owner, invited or member
                if (myUid == null) // anonymous --> not accessible
                    return ServiceResult.Forbidden(NotAccessibleCourse);

                ECourseRoleType roleType = await CourseRelationRepository.GetRole(myUid.Value, courseId);
                if (roleType != ECourseRoleType.Owner && roleType != ECourseRoleType.Invited && roleType != ECourseRoleType.Member)
                    return ServiceResult.Forbidden(NotAccessibleCourse);
            }

            // Get list register 
            List<ViewCourseRegister> registers = await CourseRegisterRepository.GetCourseRegisters(courseId);
            registers = registers
                .GroupBy(item => item.CourseRegisterId)
                .Select(group => group.First())
                .ToList();

            // Calculate score --> search by user: FullName.6, Username.4
            search = search.ToLower();
            double fullnameWeight = 0.6, usernameWeight = 0.4;
            Dictionary<string, double> fullnameScore = Algorithm.SimilarityList(search, registers.Select(r => r.FullName).ToList());
            Dictionary<string, double> usernameScore = Algorithm.SimilarityList(search, registers.Select(r => r.Username).ToList());
            Dictionary<Guid, double> scored = registers.ToDictionary(
                r => r.CourseRegisterId,
                r => fullnameWeight * fullnameScore[r.FullName] + usernameWeight * usernameScore[r.Username]
            );

            // Order and apply pagination
            registers = [.. registers.OrderByDescending(r => scored[r.CourseRegisterId])];
            if (page.Filters != null)
            {
                registers = CourseRegisterRepository.ApplyFilter(registers, page.Filters);
            }
            registers = registers.Skip(page.Offset ?? 0).Take(page.Limit ?? 15).ToList();

            // return success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(), string.Empty, 
                await DecorationRepository.DecorateResponseCourseRegisterModel(myUid, registers)
                );
        }

        public virtual async Task<ServiceResult> UserSearchCourseInvites(Guid myUid, Guid courseId, string? search, PaginationDto page)
        {
            if (string.IsNullOrWhiteSpace(search)) return ServiceResult.BadRequest("Từ khóa tìm kiếm rỗng");
            search = search.ToLower();

            // check course existed and user must be owner of course
            Course course = await CourseRepository.CheckExisted(courseId, NotExistedCourse);
            if (course.UserId != myUid)
                return ServiceResult.Forbidden(NotBeCourseOnwner);

            // get list invite of course
            List<CourseRelation> relations = await CourseRelationRepository.GetRelationsOfCourse(courseId, ECourseRelationType.Invite);
            relations = relations.GroupBy(r => r.CourseRelationId).Select(g => g.First()).ToList();
            List<ResponseCourseRelationModel> responseCourseRelationModels = await DecorationRepository
                .DecorateResponseCourseRelationModel(myUid, relations, ECourseRelationType.Invite, isDecorateCourse: false, isDecorateUser: true);

            // calculate score --> search by user
            double fullnameWeight = 0.6, usernameWeight = 0.4;
            Dictionary<string, double> fullnameScore = Algorithm.SimilarityList(search, responseCourseRelationModels
                .Select(r => r.User?.FullName ?? string.Empty).ToList());
            Dictionary<string, double> usernameScore = Algorithm.SimilarityList(search, responseCourseRelationModels
                .Select(r => r.User?.Username ?? string.Empty).ToList());
            Dictionary<Guid, double> scored = responseCourseRelationModels.ToDictionary(
                r => r.CourseRelationId,
                r => fullnameWeight * fullnameScore[r.User?.FullName ?? string.Empty]
                    + usernameWeight * usernameScore[r.User?.Username ?? string.Empty]
            );

            // order and apply pagination
            responseCourseRelationModels = [.. responseCourseRelationModels.OrderByDescending(r => scored[r.CourseRelationId])];
            if (page.Filters != null)
            {
                responseCourseRelationModels = CourseRegisterRepository.ApplyFilter(responseCourseRelationModels, page.Filters);
            }
            responseCourseRelationModels = responseCourseRelationModels.Skip(page.Offset ?? 0).Take(page.Limit ?? 15).ToList();

            // success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(), string.Empty, responseCourseRelationModels);
        }

        public virtual async Task<ServiceResult> UserSearchCourseRequests(Guid myUid, Guid courseId, string? search, PaginationDto page)
        {
            if (string.IsNullOrWhiteSpace(search)) return ServiceResult.BadRequest("Từ khóa tìm kiếm rỗng");
            search = search.ToLower();

            // check course existed and user must be owner of course
            Course course = await CourseRepository.CheckExisted(courseId, NotExistedCourse);
            if (course.UserId != myUid)
                return ServiceResult.Forbidden(NotBeCourseOnwner);

            // get list request of course 
            List<CourseRelation> relations = await CourseRelationRepository.GetRelationsOfCourse(courseId, ECourseRelationType.Request);
            relations = relations.GroupBy(r => r.CourseRelationId).Select(g => g.First()).ToList();
            List<ResponseCourseRelationModel> responseCourseRelationModels = await DecorationRepository
                .DecorateResponseCourseRelationModel(myUid, relations, ECourseRelationType.Request, isDecorateCourse: false, isDecorateUser: true);

            // calculate score --> search by user
            double fullnameWeight = 0.6, usernameWeight = 0.4;
            Dictionary<string, double> fullnameScore = Algorithm.SimilarityList(search, responseCourseRelationModels
                .Select(r => r.User?.FullName ?? string.Empty).ToList());
            Dictionary<string, double> usernameScore = Algorithm.SimilarityList(search, responseCourseRelationModels
                .Select(r => r.User?.Username ?? string.Empty).ToList());
            Dictionary<Guid, double> scored = responseCourseRelationModels.ToDictionary(
                r => r.CourseRelationId,
                r => fullnameWeight * fullnameScore[r.User?.FullName ?? string.Empty]
                    + usernameWeight * usernameScore[r.User?.Username ?? string.Empty]
            );

            // order and apply pagination
            responseCourseRelationModels = [.. responseCourseRelationModels.OrderByDescending(r => scored[r.CourseRelationId])];
            if (page.Filters != null)
            {
                responseCourseRelationModels = CourseRegisterRepository.ApplyFilter(responseCourseRelationModels, page.Filters);
            }
            responseCourseRelationModels = responseCourseRelationModels.Skip(page.Offset ?? 0).Take(page.Limit ?? 15).ToList();

            // success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(), string.Empty, responseCourseRelationModels);
        }

        public virtual async Task<ServiceResult> UserSearchMyCourseInvites(Guid myUid, string? search, PaginationDto page)
        {
            if (string.IsNullOrWhiteSpace(search)) return ServiceResult.BadRequest("Từ khóa tìm kiếm rỗng");
            search = search.ToLower();

            // get list invite to myUid
            List<CourseRelation> lisRelations = await CourseRelationRepository.GetRelationsOfUser(myUid, ECourseRelationType.Invite);
            List<ResponseCourseRelationModel> models = await DecorationRepository
                .DecorateResponseCourseRelationModel(myUid, lisRelations, ECourseRelationType.Invite, isDecorateCourse: true, isDecorateUser: false);

            // calculate score --> search by course (title .4, owner fullname .3, content .2, abstract .1)
            List<(Guid, string, string, string, string?)> shortKnowledge = models.Select(
                    model => (model.CourseRelationId,
                            model.Course?.Title ?? string.Empty,
                            model.Course?.FullName ?? string.Empty,
                            model.Course?.Introduction ?? string.Empty,
                            model.Course?.Abstract)
                ).ToList();
            Dictionary<Guid, double> scored = CalculateKnowledgeSearchScore.Calculate(search, shortKnowledge);

            // order and apply pagination
            models = [.. models.OrderByDescending(m => scored[m.CourseRelationId])];
            if (page.Filters != null)
            {
                models = CourseRegisterRepository.ApplyFilter(models, page.Filters);
            }
            models = models.Skip(page.Offset ?? 0).Take(page.Limit ?? 15).ToList();

            // success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(), string.Empty, models);
        }

        public virtual async Task<ServiceResult> UserSearchMyCourseRequests(Guid myUid, string? search, PaginationDto page)
        {
            if (string.IsNullOrWhiteSpace(search)) return ServiceResult.BadRequest("Từ khóa tìm kiếm rỗng");
            search = search.ToLower();

            // get list request of myUid
            List<CourseRelation> lisRelations = await CourseRelationRepository.GetRelationsOfUser(myUid, ECourseRelationType.Request);
            List<ResponseCourseRelationModel> models = await DecorationRepository
                .DecorateResponseCourseRelationModel(myUid, lisRelations, ECourseRelationType.Request, isDecorateCourse: true, isDecorateUser: false);

            // calculate score --> search by course
            List<(Guid, string, string, string, string?)> shortKnowledge = models.Select(
                    model => (model.CourseRelationId,
                            model.Course?.Title ?? string.Empty,
                            model.Course?.FullName ?? string.Empty,
                            model.Course?.Introduction ?? string.Empty,
                            model.Course?.Abstract)
                ).ToList();
            Dictionary<Guid, double> scored = CalculateKnowledgeSearchScore.Calculate(search, shortKnowledge);

            models = [.. models.OrderByDescending(m => scored[m.CourseRelationId])];
            if (page.Filters != null)
            {
                models = CourseRegisterRepository.ApplyFilter(models, page.Filters);
            }
            models = models.Skip(page.Offset ?? 0).Take(page.Limit ?? 15).ToList();

            // success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(), string.Empty, models);
        }

        #endregion
    }
}
