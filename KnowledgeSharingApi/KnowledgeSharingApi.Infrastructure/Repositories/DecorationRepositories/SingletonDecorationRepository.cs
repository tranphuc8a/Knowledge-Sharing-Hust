using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Encrypts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.DecorationRepositories;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Repositories.BaseRepositories;
using KnowledgeSharingApi.Infrastructures.Repositories.MySqlRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Repositories.DecorationRepositories
{
    public abstract class SingletonDecorationRepository(IDbContext dbContext) : BaseMySqlUserItemRepository<UserItem>(dbContext), IDecorationRepository
    {
        public Task<List<IResponseCommentModel>> DecorateResponseCommentModel(Guid? myUid, List<ViewComment> viewComments, bool isDecorateReplies = true)
        {
            throw new NotImplementedException();
        }

        public Task<List<ResponseCourseLessonModel>> DecorateResponseCourseLessonModel(Guid? myUid, List<CourseLesson> participants, bool isDecorateLesson = false, bool isDecorateCourse = false)
        {
            throw new NotImplementedException();
        }

        public Task<List<IResponseCourseModel>> DecorateResponseCourseModel(Guid? myUid, List<ViewCourse> courses)
        {
            throw new NotImplementedException();
        }

        public Task<List<ResponseCourseRegisterModel>> DecorateResponseCourseRegisterModel(Guid? myUid, List<ViewCourseRegister> registers)
        {
            throw new NotImplementedException();
        }

        public Task<List<ResponseCourseRelationModel>> DecorateResponseCourseRelationModel(Guid? myUid, List<CourseRelation> relations, ECourseRelationType relationType, bool isDecorateUser = false, bool isDecorateCourse = false)
        {
            throw new NotImplementedException();
        }

        public Task<List<ResponseFriendCardModel>> DecorateResponseFriendCardModel(Guid? myUid, List<ResponseFriendCardModel> responseFriendCardModel)
        {
            throw new NotImplementedException();
        }

        public Task<List<IResponseLessonModel>> DecorateResponseLessonModel(Guid? myUid, List<ViewLesson> lessons)
        {
            throw new NotImplementedException();
        }

        public Task<List<IResponsePostModel>> DecorateResponsePostModel(Guid? myUId, List<ViewPost> posts)
        {
            throw new NotImplementedException();
        }

        public Task<List<IResponseQuestionModel>> DecorateResponseQuestionModel(Guid? myUid, List<ViewQuestion> questions)
        {
            throw new NotImplementedException();
        }

        public Task<List<ResponseStarModel>> DecorateResponseStarModel(Guid? myUid, List<Star> listStars, bool isDecorateUser = false, bool isDecorateItem = false)
        {
            throw new NotImplementedException();
        }

        public Task<List<ResponseUserCardModel>> DecorateResponseUserCardModel(Guid? myUid, List<ResponseUserCardModel> responseUserCardModels)
        {
            throw new NotImplementedException();
        }
    }
}
