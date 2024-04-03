
using KnowledgeSharingApi.Domains.Algorithms;
using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.CourseLessonModels;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.DecorationRepositories;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Repositories.DecorationRepositories;
using KnowledgeSharingApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Services
{
    public class CourseLessonService : ICourseLessonService
    {
        protected readonly IResourceFactory ResourceFactory;
        protected readonly IResponseResource ResponseResource;
        protected readonly IEntityResource EntityResource;

        protected readonly ILessonRepository LessonRepository;
        protected readonly ICourseLessonRepository CourseLessonRepository;
        protected readonly ICourseRepository CourseRepository;
        protected readonly ICourseRelationRepository CourseRelationRepository;
        protected readonly IDecorationRepository DecorationRepository;

        protected readonly string CourseResource, ParticipantResource;
        protected readonly string NotExistedCourse, NotExistedParticipant;
        protected readonly string
            NotBeCourseOwner = "Bạn không phải là chủ khóa học này",
            NotBeLessonOwner = "Bạn không phải là chủ bài giảng này";
        protected readonly int DefaultLimit = 20;

        public CourseLessonService(
            IResourceFactory insertedLessonourceFactory,
            ICourseLessonRepository courseLessonRepository,
            ICourseRepository courseRepository,
            ICourseRelationRepository courseRelationRepository,
            ILessonRepository lessonRepository,
            IDecorationRepository decorationRepository
        )
        {
            CourseLessonRepository = courseLessonRepository;
            CourseRepository = courseRepository;
            CourseRelationRepository = courseRelationRepository;
            LessonRepository = lessonRepository;
            DecorationRepository = decorationRepository;

            ResourceFactory = insertedLessonourceFactory;
            ResponseResource = insertedLessonourceFactory.GetResponseResource();
            EntityResource = insertedLessonourceFactory.GetEntityResource();

            CourseResource = EntityResource.Course();
            ParticipantResource = EntityResource.CourseLesson();
            NotExistedCourse = ResponseResource.NotExist(CourseResource);
            NotExistedParticipant = ResponseResource.NotExist(ParticipantResource);
        }
        #region Create apies

        public async Task<ServiceResult> AddLessonToCourse(Guid myUid, AddLessonToCourseModel model)
        {
            // Kiem tra khoa hoc ton tai
            ViewCourse course = await CourseRepository.CheckExistedCourse(model.CourseId ?? Guid.Empty, NotExistedCourse);

            // Kiem tra la chu khoa hoc
            if (course.UserId != myUid)
                return ServiceResult.Forbidden(NotBeCourseOwner);

            // Kiem tra bai giang ton tai
            ViewLesson lesson = await LessonRepository.CheckExistedLesson(model.LessonId ?? Guid.Empty,
                ResponseResource.NotExist(EntityResource.Lesson()));

            // Kiem tra la chu bai giang
            if (lesson.UserId != myUid)
                return ServiceResult.Forbidden(NotBeLessonOwner);

            // OK them bai hoc
            CourseLesson? insertedLesson = await CourseLessonRepository.AddLessonToCourse(model);
            if (insertedLesson == null) return ServiceResult.ServerError(ResponseResource.InsertFailure(ParticipantResource));

            // Decorate:
            ResponseCourseLessonModel res = (await DecorationRepository
                .DecorateResponseCourseLessonModel(myUid, [insertedLesson], isDecorateLesson: true)).First();

            // Tra ve thanh cong
            return ServiceResult.Success(ResponseResource.InsertSuccess(ParticipantResource), string.Empty, res);
        }

        public async Task<ServiceResult> AddListLessonToCourse(Guid myUid, AddListLessonToCourseModel model)
        {
            // Kiem tra khoa hoc ton tai va la chu khoa hoc
            ViewCourse course = await CourseRepository.CheckExistedCourse(model.CourseId ?? Guid.Empty, NotExistedCourse);
            if (course.UserId != myUid)
                return ServiceResult.Forbidden(NotBeCourseOwner);

            // Kiem tra danh sach bai giang ton tai va la chu cua danh sach bai giang nay
            IEnumerable<Lesson?> listLesson = await LessonRepository.Get(
                model.ListLessonModel!.Select(lesson => lesson.LessonId ?? Guid.Empty).ToArray()
            );
            ServiceResult notExistedLesson = ServiceResult.BadRequest(ResponseResource.NotExist(EntityResource.Lesson()));
            ServiceResult notBeLessonOwner = ServiceResult.Forbidden(NotBeLessonOwner);
            foreach (Lesson? lesson in listLesson)
            {
                if (lesson == null) return notExistedLesson;
                if (lesson.UserId != myUid) return notBeLessonOwner;
            }

            // OK them danh sach bai giang vao khoa hoc
            IEnumerable<CourseLesson>? listLessonAdded = await CourseLessonRepository.AddListLessonToCourse(model);
            if (listLessonAdded == null)
                return ServiceResult.ServerError(ResponseResource.InsertMultiFalure(ParticipantResource));

            // Decorate:
            IEnumerable<ResponseCourseLessonModel> insertedLesson = await DecorationRepository
                .DecorateResponseCourseLessonModel(myUid, listLessonAdded, isDecorateLesson: true);

            // Tra ve thanh cong
            return ServiceResult.Success(ResponseResource.InsertMultiSuccess(ParticipantResource), string.Empty, insertedLesson);
        }

        #endregion

        #region Delete Apies

        public async Task<ServiceResult> DeleteLessonFromCourse(Guid myUid, Guid participantId)
        {
            // Kiem tra bai giang ton tai trong khoa hoc
            CourseLesson courseLesson = await CourseLessonRepository.CheckExisted(participantId, NotExistedParticipant);
            Course course = await CourseRepository.CheckExisted(courseLesson.CourseId, NotExistedCourse);

            // Kiem tra la chu khoa
            if (course.UserId != myUid)
                return ServiceResult.Forbidden(NotBeCourseOwner);

            // Xoa bai giang
            int deleted = await CourseLessonRepository.DeleteLessonFromCourse(participantId);
            if (deleted <= 0)
                return ServiceResult.ServerError(ResponseResource.DeleteFailure(ParticipantResource));

            // Tra ve thanh cong
            return ServiceResult.Success(ResponseResource.DeleteSuccess(ParticipantResource));
        }

        public async Task<ServiceResult> DeleteListLessonFromCourse(Guid myUid, IEnumerable<Guid> listParticipantIds)
        {
            // Kiểm tra toàn bộ participant phải tồn tại và chung 1 khóa học
            IEnumerable<CourseLesson?> listParticipants = await CourseLessonRepository.Get(listParticipantIds.ToArray());
            if (!listParticipants.Any() && listParticipants.Any(p => p == null))
                return ServiceResult.BadRequest(NotExistedParticipant);
            Guid courseId = listParticipants.First()!.CourseId;
            if (listParticipants.Any(p => p?.CourseId != courseId))
                return ServiceResult.BadRequest("Các bài giảng hiện Không cùng chung một khóa học");

            // Kiểm tra khóa học ấy tồn tại và phải do myUid làm owner
            Course course = await CourseRepository.CheckExisted(courseId, NotExistedCourse);
            if (course.UserId != myUid)
                return ServiceResult.Forbidden(NotBeCourseOwner);

            // Ok thực hiện xóa danh sách bài giảng khỏi khóa học
            int deleted = await CourseLessonRepository.DeleteListLessonFromCourse(listParticipantIds);
            if (deleted <= 0)
                return ServiceResult.ServerError(ResponseResource.DeleteMultiFailure(ParticipantResource));

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.DeleteMultiSuccess(ParticipantResource));
        }

        #endregion

        #region Update Apies

        public async Task<ServiceResult> UpdateLessonInCourse(Guid myUid, UpdateLessonInCourseModel model)
        {
            // Kiểm tra participant tồn tại trong course của myUId
            CourseLesson courseLesson = await CourseLessonRepository
                .CheckExisted(model.ParticipantId ?? Guid.Empty, NotExistedParticipant);
            Course course = await CourseRepository.CheckExisted(courseLesson.CourseId, NotExistedCourse);
            if (course.UserId != myUid)
                return ServiceResult.Forbidden(NotBeCourseOwner);

            // Update
            courseLesson.LessonTitle = model.LessonTitle!;
            courseLesson.ModifiedBy = myUid.ToString();
            courseLesson.ModifiedTime = DateTime.Now;
            int updated = await CourseLessonRepository.Update(courseLesson.CourseLessonId, courseLesson);
            if (updated <= 0)
                return ServiceResult.ServerError(ResponseResource.UpdateFailure(ParticipantResource));

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.UpdateSuccess(ParticipantResource));
        }

        public async Task<ServiceResult> UpdateListLessonInCourse(Guid myUid, IEnumerable<UpdateLessonInCourseModel> model)
        {
            // Kiểm tra toàn bộ participant phải tồn tại và chung 1 khóa học
            IEnumerable<CourseLesson?> listParticipants = await CourseLessonRepository.Get(
                model.Select(part => part.ParticipantId ?? Guid.Empty).ToArray());
            if (!listParticipants.Any() && listParticipants.Any(p => p == null))
                return ServiceResult.BadRequest(NotExistedParticipant);
            Guid courseId = listParticipants.First()!.CourseId;
            if (listParticipants.Any(p => p?.CourseId != courseId))
                return ServiceResult.BadRequest("Các bài giảng hiện Không cùng chung một khóa học");

            // Kiểm tra khóa học ấy tồn tại và phải do myUid làm owner
            Course course = await CourseRepository.CheckExisted(courseId, NotExistedCourse);
            if (course.UserId != myUid)
                return ServiceResult.Forbidden(NotBeCourseOwner);

            // Thực hiện cập nhật
            int updated = await CourseLessonRepository.UpdateListLessonInCourse(model);
            if (updated <= 0)
                return ServiceResult.ServerError(ResponseResource.UpdateMultiFailure(ParticipantResource));

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.UpdateMultiSuccess(ParticipantResource));
        }

        public async Task<ServiceResult> UpdateOffsetOfListLessonInCourse(Guid myUId, Guid[] listParticipantIds)
        {
            // Kiểm tra toàn bộ participant phải tồn tại và chung 1 khóa học
            IEnumerable<CourseLesson?> listParticipants = await CourseLessonRepository.Get(listParticipantIds);
            if (!listParticipants.Any() && listParticipants.Any(p => p == null))
                return ServiceResult.BadRequest(NotExistedParticipant);
            Guid courseId = listParticipants.First()!.CourseId;
            if (listParticipants.Any(p => p?.CourseId != courseId))
                return ServiceResult.BadRequest("Các bài giảng hiện Không cùng chung một khóa học");

            // Kiểm tra khóa học ấy tồn tại và phải do myUid làm owner
            Course course = await CourseRepository.CheckExisted(courseId, NotExistedCourse);
            if (course.UserId != myUId)
                return ServiceResult.Forbidden(NotBeCourseOwner);

            // Kiểm tra listParticipantIds phải là hoán vị của danh sách participant của course:
            IEnumerable<Guid> courseParticipantIds = (await CourseLessonRepository.GetCourseParticipant(courseId))
                .Select(cl => cl.CourseLessonId);
            if (!Algorithm.IsPermutation(courseParticipantIds, listParticipantIds.AsEnumerable()))
                return ServiceResult.BadRequest("Danh sách yêu cầu không phải là hoán vị của một danh sách bài giảng");

            // Thực hiện cập nhật
            int updated = await CourseLessonRepository.UpdateOffsetOfListLessonInCourse(listParticipantIds);
            if (updated <= 0)
                return ServiceResult.ServerError(ResponseResource.UpdateMultiFailure(ParticipantResource));

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.UpdateMultiSuccess(ParticipantResource));
        }

        #endregion

        #region Get Apies
        public async Task<ServiceResult> AdminGetListCourseParticipants(Guid courseId, int? limit, int? offset)
        {
            // Kiem tra khoa hoc ton tai
            Course course = await CourseRepository.CheckExisted(courseId, NotExistedCourse);

            // Get list vè
            IEnumerable<CourseLesson> listLesson = await CourseLessonRepository.GetCourseParticipant(courseId);

            // Phân trang
            int total = listLesson.Count();
            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            listLesson = listLesson.Skip(offsetValue).Take(limitValue);

            // Decorate
            IEnumerable<ResponseCourseLessonModel> listResponse = await
                DecorationRepository.DecorateResponseCourseLessonModel(null, listLesson, isDecorateLesson: true);
            PaginationResponseModel<ResponseCourseLessonModel> res = new(total, limitValue, offsetValue, listResponse);

            // Tra ve thanh cong
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(ParticipantResource), string.Empty, res);
        }

        public async Task<ServiceResult> UserGetListCourseParticipants(Guid myUid, Guid courseId, int? limit, int? offset)
        {
            // Kiem tra khoa hoc ton tai
            Course course = await CourseRepository.CheckExisted(courseId, NotExistedCourse);

            // Kiem tra role == user hoặc owner
            ECourseRoleType role = await CourseRelationRepository.GetRole(myUid, course.UserItemId);
            if (role != ECourseRoleType.Owner && role != ECourseRoleType.Member)
                return ServiceResult.Forbidden("Bạn không phải là thành viên của khóa học này");

            // Get list vè
            IEnumerable<CourseLesson> listLesson = await CourseLessonRepository.GetCourseParticipant(courseId);

            // Phân trang
            int total = listLesson.Count();
            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;
            listLesson = listLesson.Skip(offsetValue).Take(limitValue);

            // Decorate
            IEnumerable<ResponseCourseLessonModel> listResponse = await
                DecorationRepository.DecorateResponseCourseLessonModel(myUid, listLesson, isDecorateLesson: true);
            PaginationResponseModel<ResponseCourseLessonModel> res = new(total, limitValue, offsetValue, listResponse);

            // Tra ve thanh cong
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(ParticipantResource), string.Empty, res);
        }
        #endregion
    }
}
