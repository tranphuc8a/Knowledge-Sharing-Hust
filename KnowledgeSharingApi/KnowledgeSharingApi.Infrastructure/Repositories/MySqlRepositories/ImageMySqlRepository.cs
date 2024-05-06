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
        public virtual async Task<IEnumerable<Image>> GetByUserId(Guid userId)
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
                    CreatedTime = DateTime.Now,
                    UserId = userId,
                    ImageId = Guid.NewGuid(),
                    ImageUrl = imageUrl
                };
                Guid? inserted = await Insert(imageToAdd.ImageId, imageToAdd);
                return inserted != null ? 1 : 0;
            }
            return 0;
        }
    }
}
