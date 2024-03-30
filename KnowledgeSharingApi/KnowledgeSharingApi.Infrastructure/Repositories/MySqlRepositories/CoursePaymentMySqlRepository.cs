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
    public class CoursePaymentMySqlRepository(IDbContext dbContext)
        : BaseMySqlRepository<CoursePayment>(dbContext), ICoursePaymentRepository
    {
        public Task<IEnumerable<ViewCoursePayment>> GetByCourse(Guid courseId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ViewCoursePayment>> GetByUser(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<ViewCoursePayment?> GetCoursePayment(Guid paymentId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CoursePayment>> GetCoursePayment(Guid userId, Guid courseId)
        {
            throw new NotImplementedException();
        }

        public Task<int> UserPayCourse(Guid userId, Guid courseId)
        {
            throw new NotImplementedException();
        }
    }
}
