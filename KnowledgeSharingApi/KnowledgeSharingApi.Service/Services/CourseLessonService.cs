using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.CourseLessonModels;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
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

        protected readonly string CourseResource, ParticipantResource;
        protected readonly string NotExistedCourse, NotExistedParticipant;
        protected readonly string
            NotBeCourseOwner = "Bạn không phải là chủ khóa học này",
            NotBeLessonOwner = "Bạn không phải là chủ bài giảng này";

        public CourseLessonService(
            IResourceFactory resourceFactory,
            ICourseLessonRepository courseLessonRepository,
            ICourseRepository courseRepository,
            ILessonRepository lessonRepository
        )
        {
            CourseLessonRepository = courseLessonRepository;
            CourseRepository = courseRepository;
            LessonRepository = lessonRepository;

            ResourceFactory = resourceFactory;
            ResponseResource = resourceFactory.GetResponseResource();
            EntityResource = resourceFactory.GetEntityResource();

            CourseResource = EntityResource.Course();
            ParticipantResource = EntityResource.CourseLesson();
            NotExistedCourse = ResponseResource.NotExist(CourseResource);
            NotExistedParticipant = ResponseResource.NotExist(ParticipantResource);
        }

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
            CourseLesson? res = await CourseLessonRepository.AddLessonToCourse(model);
            if (res == null) return ServiceResult.ServerError(ResponseResource.InsertFailure(ParticipantResource));

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
            IEnumerable<ResponseCourseLessonModel>

            // Tra ve thanh cong
        }

        public Task<ServiceResult> DeleteLessonFromCourse(Guid myUid, Guid participantId)
        {
            // Kiem tra bai giang ton tai trong khoa hoc

            // Kiem tra la chu khoa hoc

            // Xoa bai giang

            // Tra ve thanh cong
        }

        public Task<ServiceResult> DeleteListLessonFromCourse(Guid myUid, IEnumerable<Guid> listParticipantIds)
        {
            // Kiểm tra toàn bộ participant phải tồn tại và chung 1 khóa học

            // Kiểm tra khóa học ấy tồn tại và phải do myUid làm owner

            // Ok thực hiện xóa danh sách bài giảng khỏi khóa học

            // Trả về thành công
        }

        public Task<ServiceResult> UpdateLessonInCourse(Guid myUid, UpdateLessonInCourseModel model)
        {
            // Kiểm tra participant tồn tại trong course của myUId

            // Update

            // Trả về thành công
        }

        public Task<ServiceResult> UpdateListLessonInCourse(Guid myUid, IEnumerable<UpdateLessonInCourseModel> model)
        {
            // Kiểm tra danh sách participant tồn tại trong course của myUid

            // Thực hiện cập nhật

            // Trả về thành công
        }

        public Task<ServiceResult> UpdateOffsetOfListLessonInCourse(Guid myUId, Guid[] listParticipantIds)
        {
            // Kiểm tra danh sách participant tồn tại trong course của myUid

            // Thực hiện cập nhật

            // Trả về thành công
        }
    }
}
