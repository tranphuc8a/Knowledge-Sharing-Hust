using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Repositories.MySqlRepositories
{
    public class ImageMySqlRepository(IDbContext dbContext) : BaseMySqlRepository<Image>(dbContext), IImageRepository
    {
        public async Task<IEnumerable<Image>> GetByUserId(Guid userId)
        {
            return await DbContext.Images.Where(image => image.UserId == userId).ToListAsync();
        }
    }
}
