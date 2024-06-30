using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.UserIteractionRepositories;
using KnowledgeSharingApi.Repositories.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Repositories.Repositories.MySqlRepositories.MySqlUserIteractionRepositories
{
    public class ImageMySqlRepository(IDbContext dbContext) : BaseMySqlRepository<Image>(dbContext), IImageRepository
    {
        public virtual async Task<List<Image>> GetByUserId(Guid userId)
        {
            return await DbContext.Images.Where(image => image.UserId == userId).ToListAsync();
        }

        public virtual async Task<int> TryInsertImage(Guid userId, string? imageUrl)
        {
            if (imageUrl != null)
            {
                Image imageToAdd = new()
                {
                    CreatedBy = userId.ToString(),
                    CreatedTime = DateTime.UtcNow,
                    UserId = userId,
                    ImageId = Guid.NewGuid(),
                    ImageUrl = imageUrl
                };
                Guid? inserted = await Insert(imageToAdd.ImageId, imageToAdd);
                return inserted != null ? 1 : 0;
            }
            return 0;
        }

        protected override DbSet<Image> GetDbSet()
        {
            return DbContext.Images;
        }
    }
}
