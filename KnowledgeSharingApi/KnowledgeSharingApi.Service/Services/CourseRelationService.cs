using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
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
            ICoursePaymentRepository coursePaymentRepository,
            ICourseRegisterRepository courseRegisterRepository,
            ICourseRepository courseRepository
        )
        {
            ResourceFactory = resourceFactory;
            ResponseResource = ResourceFactory.GetResponseResource();
            EntityResource = ResourceFactory.GetEntityResource();

            UserRepository = userRepository;
            CourseRepository = courseRepository;
            CourseRelationRepository = courseRelationRepository;
            CoursePaymentRepository = coursePaymentRepository;
            CourseRegisterRepository = courseRegisterRepository;

            CourseResource = EntityResource.Course();
            NotExistedCourse = ResponseResource.NotExist(CourseResource);

        }


        #region Functionality methods

        /// <summary>
        /// Trang tri va bo sung them thong tin cho ResponseRelationModel tu COurseRelation
        /// </summary>
        /// <param name="myUid"> id cua user thuc hien </param>
        /// <param name="relations"> Danh sach relation can trang tri </param>
        /// <param name="relationType"> Loai relation </param>
        /// <param name="isDecorateUser"> Co trang tri user khong </param>
        /// <param name="isDecorateCourse"> Co trang tri course khong </param>
        /// <returns></returns>
        /// Created: PhucTV (30/3/24)
        /// Modified: None
        protected virtual async Task<IEnumerable<ResponseCourseRelationModel>> Decorate(
            Guid? myUid,
            IEnumerable<CourseRelation> relations,
            ECourseRelationType relationType,
            bool isDecorateUser = false,
            bool isDecorateCourse = false)
        {
            IEnumerable<ResponseCourseRelationModel> res = relations.Select(relation =>
            {
                return (ResponseCourseRelationModel)new ResponseCourseRelationModel().Copy(relation);
            });
            return await Task.FromResult(res);
        }

        /// <summary>
        /// Ket hop phan trang va decoration
        /// </summary>
        /// Created: PhucTV (30/3/24)
        /// Modified: None
        protected virtual async Task<PaginationResponseModel<ResponseCourseRelationModel>> PaginationAndDecoration(
            Guid? myUid,
            IEnumerable<CourseRelation> relations,
            int? limit, int? offset,
            ECourseRelationType relationType,
            bool isDecorateUser = false,
            bool isDecorateCourse = false
        )
        {
            // Pagination
            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            int total = relations.Count();
            relations = relations.Skip(offsetValue).Take(limitValue);

            // Decoration:
            IEnumerable<ResponseCourseRelationModel> lsRes = await Decorate(myUid, relations, relationType, isDecorateUser, isDecorateCourse);

            // Return:
            return new PaginationResponseModel<ResponseCourseRelationModel>(total, limitValue, offsetValue, lsRes);
        }

        #endregion


        #region Admin apies
        public async Task<ServiceResult> AdminDeleteUserFromCourse(Guid registerId)
        {
            // Check register is existed
            CourseRegister? register = await CourseRegisterRepository.GetCourseRegister(registerId);
            if (register == null) return ServiceResult.BadRequest(NotExistedMember);

            // Check register has payment
            IEnumerable<CoursePayment> payments =
                await CoursePaymentRepository.GetCoursePayment(register.UserId, register.CourseId);
            if (payments.Any())
                return ServiceResult.BadRequest("Không thể xóa thành viên đã thanh toán khóa học");

            // Delete register 
            int deleted = await CourseRegisterRepository.Delete(register.CourseRegisterId);
            if (deleted <= 0) return ServiceResult.ServerError(ResponseResource.DeleteFailure(MemberResource));

            // Return success
            return ServiceResult.Success(ResponseResource.DeleteSuccess(MemberResource));
        }

        public async Task<ServiceResult> AdminGetCourseRegisters(Guid courseId, int? limit, int? offset)
        {
            // CHeck course Existed
            _ = await CourseRepository.CheckExistedCourse(courseId, NotExistedCourse);

            // Get lisst register
            IEnumerable<ViewCourseRegister> registers = await CourseRegisterRepository.GetCourseRegisters(courseId);

            // Pagination
            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            int total = registers.Count();
            registers = registers.Skip(offsetValue).Take(limitValue);

            // Decorate (lam sau)

            // Return Success
            PaginationResponseModel<ViewCourseRegister> res = new(total, limitValue, offsetValue, registers);
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(MemberResource), string.Empty, res);
        }
        #endregion

        #region User gets
        public async Task<ServiceResult> UserGetCourseInvites(Guid myUid, Guid courseId, int? limit, int? offset)
        {
            // Check course exist
            ViewCourse course = await CourseRepository.CheckExistedCourse(courseId, NotExistedCourse);

            // Check role is owner
            if (course.UserId != myUid)
                return ServiceResult.Forbidden(NotBeCourseOnwner);

            // Get list invite of course
            IEnumerable<CourseRelation> relations = await CourseRelationRepository.GetRelationsOfCourse(courseId, ECourseRelationType.Invite);

            // Pagination and Decoration
            PaginationResponseModel<ResponseCourseRelationModel> res = await PaginationAndDecoration(
                myUid, relations, limit, offset, ECourseRelationType.Invite, isDecorateUser: true, isDecorateCourse: false
            );

            // Return success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(), string.Empty, res);
        }

        public async Task<ServiceResult> UserGetCoursePayments(Guid myUid, Guid courseId, int? limit, int? offset)
        {
            // Check course is existed
            ViewCourse course = await CourseRepository.CheckExistedCourse(courseId, NotExistedCourse);

            // Check owner
            if (course.UserId != myUid)
                return ServiceResult.Forbidden(NotBeCourseOnwner);

            // Get list payment
            IEnumerable<ViewCoursePayment> listPayments = await CoursePaymentRepository.GetByCourse(courseId);

            // pagination
            int total = listPayments.Count();
            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            listPayments = listPayments.Skip(offsetValue).Take(limitValue);

            // return success
            PaginationResponseModel<ViewCoursePayment> res = new(total, limitValue, offsetValue, listPayments);
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(), string.Empty, res);
        }

        public async Task<ServiceResult> UserGetCourseRequests(Guid myUid, Guid courseId, int? limit, int? offset)
        {
            // Check course is existed
            ViewCourse course = await CourseRepository.CheckExistedCourse(courseId, NotExistedCourse);

            // Check owner
            if (course.UserId != myUid)
                return ServiceResult.Forbidden(NotBeCourseOnwner);

            // Get list requests
            IEnumerable<CourseRelation> relations = await CourseRelationRepository.GetRelationsOfCourse(courseId, ECourseRelationType.Request);

            // Pagination and Decoration
            PaginationResponseModel<ResponseCourseRelationModel> res = await PaginationAndDecoration(
                myUid, relations, limit, offset, ECourseRelationType.Request, isDecorateUser: true, isDecorateCourse: false);

            // Return Success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(), string.Empty, res);
        }

        public async Task<ServiceResult> UserGetMyCourseInvites(Guid myUid, int? limit, int? offset)
        {
            // Get list
            IEnumerable<CourseRelation> lisRelations = await CourseRelationRepository.GetRelationsOfUser(myUid, ECourseRelationType.Invite);

            // Pagination and decoration
            PaginationResponseModel<ResponseCourseRelationModel> res = await PaginationAndDecoration(
                myUid, lisRelations, limit, offset, ECourseRelationType.Invite, isDecorateUser: false, isDecorateCourse: true);

            // return success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(), string.Empty, res);
        }

        public async Task<ServiceResult> UserGetMyCourseRequests(Guid myUid, int? limit, int? offset)
        {
            // Get list
            IEnumerable<CourseRelation> lisRelations = await CourseRelationRepository.GetRelationsOfUser(myUid, ECourseRelationType.Request);

            // Pagination and decoration
            PaginationResponseModel<ResponseCourseRelationModel> res = await PaginationAndDecoration(
                myUid, lisRelations, limit, offset, ECourseRelationType.Request, isDecorateUser: false, isDecorateCourse: true);

            // return success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(), string.Empty, res);
        }

        public async Task<ServiceResult> UserGetMyPayments(Guid myUid, int? limit, int? offset)
        {
            // Get list payment
            IEnumerable<ViewCoursePayment> listPayments = await CoursePaymentRepository.GetByUser(myUid);

            // pagination
            int total = listPayments.Count();
            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            listPayments = listPayments.Skip(offsetValue).Take(limitValue);

            // return success
            PaginationResponseModel<ViewCoursePayment> res = new(total, limitValue, offsetValue, listPayments);
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(), string.Empty, res);
        }

        public async Task<ServiceResult> UserGetPayment(Guid myUid, Guid paymentId)
        {
            // Check payment exist
            ViewCoursePayment? coursePayment = await CoursePaymentRepository.GetCoursePayment(paymentId);
            if (coursePayment == null)
                return ServiceResult.BadRequest(ResponseResource.NotExist(EntityResource.CoursePayment()));

            // Check accessible: owner or owner of course
            bool isAccessible = false;
            if (coursePayment.UserId == myUid)
            {
                isAccessible = true;
            }
            else
            {
                Course? course = await CourseRepository.Get(coursePayment.CourseId);
                if (course != null && course.UserId == myUid)
                {
                    isAccessible = true;
                }
            }
            if (!isAccessible) return ServiceResult.Forbidden("Bạn không có quyền truy cập vào hóa đơn thanh toán này");

            // return success
            return ServiceResult.Success(
                ResponseResource.GetSuccess(EntityResource.CoursePayment()), string.Empty, coursePayment);
        }

        public async Task<ServiceResult> UserGetRegisters(Guid myUid, Guid courseId, int? limit, int? offset)
        {
            // Check course existed
            _ = await CourseRepository.CheckExistedCourse(courseId, NotExistedCourse);

            // Check role is owner or member
            ECourseRoleType roleType = await CourseRelationRepository.GetRole(myUid, courseId);
            if (roleType != ECourseRoleType.Owner && roleType != ECourseRoleType.Member)
                return ServiceResult.Forbidden(NotAccessibleCourse);

            // Get lisst register
            IEnumerable<ViewCourseRegister> registers = await CourseRegisterRepository.GetCourseRegisters(courseId);

            // Pagination
            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            int total = registers.Count();
            registers = registers.Skip(offsetValue).Take(limitValue);

            // Decorate (lam sau)

            // Return Success
            PaginationResponseModel<ViewCourseRegister> res = new(total, limitValue, offsetValue, registers);
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(MemberResource), string.Empty, res);
        }

        #endregion

        #region Requestation
        public async Task<ServiceResult> UserRequestCourse(Guid myUid, Guid courseId)
        {
            // Check course is existed
            Course course = await CourseRepository.CheckExisted(courseId, NotExistedCourse);

            // Check Not request yet
            CourseRelation? beforerequest = await CourseRelationRepository.GetRequest(myUid, courseId);
            if (beforerequest != null)
                return ServiceResult.BadRequest("Duplicate request");

            // Check Role is Guest
            ECourseRoleType roleType = await CourseRelationRepository.GetRole(myUid, courseId);
            if (roleType == ECourseRoleType.InAccessible)
                return ServiceResult.Forbidden(NotAccessibleCourse);
            if (roleType == ECourseRoleType.Member)
                return ServiceResult.BadRequest("Bạn đã tham gia khóa học này rồi");
            if (roleType == ECourseRoleType.Owner)
                return ServiceResult.BadRequest("Bạn là chủ của khóa học");

            // Get course owner
            ViewUser? courseOwner = await UserRepository.GetDetail(course.UserId);
            if (courseOwner == null)
                return ServiceResult.ServerError("Course owner is null");

            // Add Request
            CourseRelation request = new()
            {
                // Entity:
                CreatedTime = DateTime.Now,
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
            return ServiceResult.Success(ResponseResource.Success());
        }

        public async Task<ServiceResult> UserDeleteCourseRequest(Guid myUid, Guid requestId)
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

        public async Task<ServiceResult> UserConfirmCourseRequest(Guid myUid, Guid requestId, bool isAccept)
        {
            // Check course relation is existed
            CourseRelation? courseRelation = await CourseRelationRepository.GetRelation(requestId);
            if (courseRelation == null || courseRelation.CourseRelationType != ECourseRelationType.Request)
                return ServiceResult.BadRequest(ResponseResource.NotExist());

            // Check owner receiver
            if (courseRelation.ReceiverId != myUid)
                return ServiceResult.Forbidden("Đây không phải yêu cầu của bạn");


            // OK thuc hienj
            if (isAccept)
            {
                int effects = await CourseRelationRepository.ConfirmCourseRelation(requestId);
                if (effects <= 0)
                    return ServiceResult.ServerError(ResponseResource.DeleteFailure());
            }
            else
            { // delete request
                int deleted = await CourseRelationRepository.Delete(courseRelation.CourseRelationId);
                if (deleted <= 0) return ServiceResult.ServerError(ResponseResource.DeleteFailure());
            }

            return ServiceResult.Success(ResponseResource.DeleteSuccess());
        }

        #endregion




        #region Invitation

        public async Task<ServiceResult> UserInviteUserToCourse(Guid myUid, Guid courseId, Guid userId)
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
            _ = await UserRepository.CheckExistedUser(userId, ResponseResource.NotExistUser());

            // CHeck not invite yet
            CourseRelation? beforeInvite = await CourseRelationRepository.GetInvite(userId, courseId);
            if (beforeInvite != null)
                return ServiceResult.BadRequest("Bạn đã gửi lời mời trước đó rồi");

            // Check user role is guest or inAccessible
            ECourseRoleType userRoles = await CourseRelationRepository.GetRole(userId, courseId);
            if (userRoles == ECourseRoleType.Member)
                return ServiceResult.BadRequest("Người dùng đã tham gia khóa học này rồi");
            if (userRoles == ECourseRoleType.Owner)
                return ServiceResult.BadRequest("Không thể mời chủ khóa học");

            // OK create new invite and insert
            CourseRelation invite = new()
            {
                // Entity:
                CreatedBy = myUid.ToString(),
                CreatedTime = DateTime.Now,
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
            return ServiceResult.Success(ResponseResource.Success());
        }

        public async Task<ServiceResult> UserInviteListUserToCourse(Guid myUid, Guid courseId, IEnumerable<Guid> listUserIds)
        {
            // Check course exist
            Course course = await CourseRepository.CheckExisted(courseId, NotExistedCourse);

            // CHeck owner of course
            if (course.UserId != myUid)
                return ServiceResult.Forbidden(NotBeCourseOnwner);

            // lam sau

            throw new NotImplementedException();
        }

        public async Task<ServiceResult> UserDeleteInvite(Guid myUid, Guid inviteId)
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

        public async Task<ServiceResult> UserConfirmInvite(Guid myUid, Guid inviteId, bool isAccept)
        {
            // Check course relation is existed
            CourseRelation? courseRelation = await CourseRelationRepository.GetRelation(inviteId);
            if (courseRelation == null || courseRelation.CourseRelationType != ECourseRelationType.Invite)
                return ServiceResult.BadRequest(ResponseResource.NotExist());

            // Check owner receiver
            if (courseRelation.ReceiverId != myUid)
                return ServiceResult.Forbidden("Đây không phải yêu cầu của bạn");


            // OK thuc hienj
            if (isAccept)
            {
                int effects = await CourseRelationRepository.ConfirmCourseRelation(inviteId);
                if (effects <= 0)
                    return ServiceResult.ServerError(ResponseResource.DeleteFailure());
            }
            else
            { // delete request
                int deleted = await CourseRelationRepository.Delete(courseRelation.CourseRelationId);
                if (deleted <= 0) return ServiceResult.ServerError(ResponseResource.DeleteFailure());
            }

            return ServiceResult.Success(ResponseResource.DeleteSuccess());
        }
        #endregion


        #region Register
        public async Task<ServiceResult> UserRegisterCourse(Guid myUid, Guid courseId)
        {
            // Kiem tra khoa hoc ton tai
            Course course = await CourseRepository.CheckExisted(courseId, NotExistedCourse);

            // Kiem tra khoa hoc mien phi, cong khai
            if (!course.IsFree || course.Fee > 0)
                return ServiceResult.Forbidden("Hãy thanh toán khóa học tính phí này");
            if (course.Privacy != EPrivacy.Public)
                return ServiceResult.Forbidden(NotAccessibleCourse);

            // Kiem tra role la Guest
            ECourseRoleType roleType = await CourseRelationRepository.GetRole(myUid, courseId);
            if (roleType == ECourseRoleType.InAccessible)
                return ServiceResult.Forbidden(NotAccessibleCourse);
            if (roleType == ECourseRoleType.Member)
                return ServiceResult.BadRequest("Bạn đã tham gia khóa học này rồi");
            if (roleType == ECourseRoleType.Owner)
                return ServiceResult.BadRequest("Bạn là chủ của khóa học");

            // OK, dang ky user
            CourseRegister courseRegister = new()
            {
                // Entity:
                CreatedTime = DateTime.Now,
                CreatedBy = myUid.ToString(),
                // Course Register:
                CourseRegisterId = Guid.NewGuid(),
                CourseId = courseId,
                UserId = myUid
            };
            Guid? res = await CourseRegisterRepository.Insert(courseRegister.CourseRegisterId, courseRegister);
            if (res == null) return ServiceResult.ServerError(ResponseResource.ServerError());

            // Tra ve thanh cong
            return ServiceResult.Success(ResponseResource.Success());
        }

        public async Task<ServiceResult> UserDeleteRegister(Guid myUid, Guid registerId)
        {
            // Check register is existed
            CourseRegister? register = await CourseRegisterRepository.GetCourseRegister(registerId);
            if (register == null) return ServiceResult.BadRequest(NotExistedMember);

            // Check is owner of course
            Course? course = await CourseRepository.Get(register.CourseId);
            if (course == null || course.UserId != myUid)
                return ServiceResult.Forbidden(NotBeCourseOnwner);

            // Check register has payment
            IEnumerable<CoursePayment> payments =
                await CoursePaymentRepository.GetCoursePayment(register.UserId, register.CourseId);
            if (payments.Any())
                return ServiceResult.BadRequest("Không thể xóa thành viên đã thanh toán khóa học");

            // Delete register 
            int deleted = await CourseRegisterRepository.Delete(register.CourseRegisterId);
            if (deleted <= 0) return ServiceResult.ServerError(ResponseResource.DeleteFailure(MemberResource));

            // Return success
            return ServiceResult.Success(ResponseResource.DeleteSuccess(MemberResource));
        }

        public async Task<ServiceResult> UserUnregisterCourse(Guid myUid, Guid courseId)
        {
            // Kiem tra khoa hoc ton tai
            Course course = await CourseRepository.CheckExisted(courseId, NotExistedCourse);

            // Kiem tra role phai la member (lay ve register)
            ViewCourseRegister? courseRegister = await CourseRepository.GetViewCourseRegister(myUid, courseId);
            if (courseRegister == null)
                return ServiceResult.Forbidden("Bạn không phải là thành viên của khóa học này");

            // Delete
            int deleted = await CourseRegisterRepository.Delete(courseRegister.CourseRegisterId);
            if (deleted <= 0) return ServiceResult.ServerError(ResponseResource.DeleteFailure());

            // Tra ve thanh cong
            return ServiceResult.Success(ResponseResource.DeleteSuccess());
        }
        #endregion
    }
}
