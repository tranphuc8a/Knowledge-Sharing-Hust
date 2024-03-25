using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Repositories.MySqlRepositories
{
    public class LessonMySqlRepository(IDbContext dbContext)
        : BaseMySqlRepository<Lesson>(dbContext), ILessonRepository
    {
        public Task<ViewQuestion> CheckExistedLesson(Guid lessonId, string errorMessage)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ViewLesson>> GetByUserId(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ViewLesson>> GetPublicPosts(int limit, int offset)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ViewLesson>> GetPublicPostsByUserId(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResponseModel<ViewLesson>> GetQuestionInCourse(Guid courseid, int limit, int offset)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ViewLesson>> GetViewPost(int limit, int offset)
        {
            throw new NotImplementedException();
        }
    }
}
