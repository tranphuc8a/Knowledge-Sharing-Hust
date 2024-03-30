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
    public class CourseRegisterMySqlRepository(IDbContext dbContext)
        : BaseMySqlRepository<CourseRegister>(dbContext), ICourseRegisterRepository
    {
        public Task<CourseRegister?> GetCourseRegister(Guid registerId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ViewCourseRegister>> GetCourseRegisters(Guid courseId)
        {
            throw new NotImplementedException();
        }
    }
}
