using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.KnowledgeRepositories;
using KnowledgeSharingApi.Repositories.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Repositories.Repositories.MySqlRepositories.MySqlKnowledgeRepositories
{
    public class CourseRegisterMySqlRepository(IDbContext dbContext)
        : BaseMySqlRepository<CourseRegister>(dbContext), ICourseRegisterRepository
    {
        public async Task<ViewCourseRegister?> GetCourseRegister(Guid registerId)
        {
            return await DbContext.ViewCourseRegisters.FindAsync(registerId);
        }

        public async Task<List<ViewCourseRegister>> GetCourseRegisters(Guid courseId)
        {
            return await DbContext.ViewCourseRegisters
                .Where(cr => cr.CourseId == courseId)
                .OrderByDescending(cr => cr.CreatedTime)
                .ToListAsync();
        }
    }
}
