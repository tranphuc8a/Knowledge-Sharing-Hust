//using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
//using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
//using KnowledgeSharingApi.Domains.Models.Dtos;
//using KnowledgeSharingApi.Domains.Models.Entities;
//using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
//using KnowledgeSharingApi.Services.Interfaces;
//using Mysqlx.Crud;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace KnowledgeSharingApi.Services.Services
//{
//    public class UserForMeService(
//            IDbContext dbContext,
//            IResourceFactory resourceFactory
//        ) : IUserService
//    {
//        protected IDbContext DbContext = dbContext;
//        protected IResourceFactory ResourceFactory = resourceFactory;
//        protected IResponseResource ResponseResource = resourceFactory.GetResponseResource();

//        public virtual Task<ServiceResult> GetUserLessons(string userId, int? limit, int? offset)
//        {
//            IEnumerable<Lesson> lessons = DbContext.Lessons
//                .Where(lesson => lesson.UserId.ToString() == userId)
//                .OrderBy(lesson => lesson.CreatedTime);
////                .ToList(); // Kết thúc truy vấn và lấy kết quả dưới dạng danh sách

//            int totalCount = lessons.Count();

//            if (limit != null)
//            {
//                // Thực hiện phân trang
//                int offsetValue = offset ?? 0;
//                int limitValue = (int)limit;
//                lessons = lessons.Skip(offsetValue).Take(limitValue);
//            }

//            return Task.FromResult(ServiceResult.Success(
//                UserMessage: ResponseResource.Success(),
//                DevMessage: string.Empty,
//                Data: new PaginationResponseModel<Lesson>()
//                {
//                    Total = totalCount,
//                    Offset = offset ?? 0,
//                    Limit = limit ?? totalCount,
//                    Results = lessons
//                })
//            );
//        }

//        public Task<ServiceResult> GetMyUserProfile(string userid)
//        {
//            Profile? profile = DbContext.Profiles
//                .Where(profile => profile.UserId.ToString() == userid)
//                .FirstOrDefault();
            
//            if (profile == null) return Task.FromResult(ServiceResult.BadRequest(ResponseResource.Failure()));

//            return Task.FromResult(ServiceResult.Success(
//                UserMessage: ResponseResource.Success(),
//                DevMessage: string.Empty,
//                Data: profile
//            ));
//        }

//        public Task<ServiceResult> GetUserRegisteredCourses(string userId, int? limit, int? offset)
//        {
//            IEnumerable<Course> registeresCourses =
//                from course in DbContext.Courses
//                join courseRegister in DbContext.CourseRegisters
//                on course.UserItemId equals courseRegister.UserItemId
//                where courseRegister.UserId.ToString() == userId
//                select course;
//            //                .ToList(); // Kết thúc truy vấn và lấy kết quả dưới dạng danh sách

//            int totalCount = registeresCourses.Count();

//            if (limit != null)
//            {
//                // Thực hiện phân trang
//                int offsetValue = offset ?? 0;
//                int limitValue = (int)limit;
//                registeresCourses = registeresCourses.Skip(offsetValue).Take(limitValue);
//            }

//            return Task.FromResult(ServiceResult.Success(
//                UserMessage: ResponseResource.Success(),
//                DevMessage: string.Empty,
//                Data: new PaginationResponseModel<Course>()
//                {
//                    Total = totalCount,
//                    Offset = offset ?? 0,
//                    Limit = limit ?? totalCount,
//                    Results = registeresCourses
//                })
//            );
//        }
//    }
//}
