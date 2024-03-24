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
    public class CourseMySqlRepository(IDbContext dbContext)
        : BaseMySqlRepository<Course>(dbContext), ICourseRepository
    {
        public Task<IEnumerable<ViewCourseRegister>> GetCourseRegisterOfCourse(string courseId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ViewCourseRegister>> GetCourseRegisterOfUser(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ViewCourseRegister?> GetViewCourseRegister(string userId, string courseId)
        {
            throw new NotImplementedException();
        }
    }
}
