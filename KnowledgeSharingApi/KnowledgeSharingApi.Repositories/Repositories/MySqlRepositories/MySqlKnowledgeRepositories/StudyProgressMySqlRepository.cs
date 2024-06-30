using KnowledgeSharingApi.Domains.Models.Entities.Tables;
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
    public class StudyProgressMySqlRepository(IDbContext dbContext)
        : BaseMySqlRepository<StudyProgress>(dbContext), IStudyProgressRepository
    {
        protected override DbSet<StudyProgress> GetDbSet()
        {
            return DbContext.StudyProgresses;
        }
    }
}
