using KnowledgeSharingApi.Domains.Models.ApiRequestModels.CourseLessonModels;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
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
    public class CourseLessonMySqlRepository(IDbContext dbContext)
        : BaseMySqlRepository<CourseLesson>(dbContext), ICourseLessonRepository
    {
        public Task<CourseLesson?> AddLessonToCourse(AddLessonToCourseModel model)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CourseLesson>?> AddListLessonToCourse(AddListLessonToCourseModel model)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteLessonFromCourse(Guid participantId)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteListLessonFromCourse(IEnumerable<Guid> listParticipantIds)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CourseLesson>> GetCourseParticipant(Guid courseId)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateLessonInCourse(UpdateLessonInCourseModel model)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateListLessonInCourse(IEnumerable<UpdateLessonInCourseModel> model)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateOffsetOfListLessonInCourse(Guid[] listParticipantIds)
        {
            throw new NotImplementedException();
        }
    }
}
