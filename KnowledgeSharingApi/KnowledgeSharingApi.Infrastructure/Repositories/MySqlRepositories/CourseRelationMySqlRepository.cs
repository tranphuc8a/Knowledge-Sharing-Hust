using KnowledgeSharingApi.Domains.Enums;
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
    public class CourseRelationMySqlRepository(IDbContext dbContext)
        : BaseMySqlRepository<CourseRelation>(dbContext), ICourseRelationRepository
    {
        public Task<int> ConfirmCourseRelation(Guid userRelationId)
        {
            throw new NotImplementedException();
        }

        public Task<CourseRelation?> GetInvite(Guid receiverId, Guid courseId)
        {
            throw new NotImplementedException();
        }

        public Task<CourseRelation?> GetRelation(Guid courseRelationId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CourseRelation>> GetRelationsOfCourse(Guid courseId, ECourseRelationType relationType)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CourseRelation>> GetRelationsOfUser(Guid userId, ECourseRelationType relationType)
        {
            throw new NotImplementedException();
        }

        public Task<CourseRelation?> GetRequest(Guid senderId, Guid courseId)
        {
            throw new NotImplementedException();
        }

        public Task<ECourseRoleType> GetRole(Guid userId, Guid courseId)
        {
            throw new NotImplementedException();
        }
    }
}
