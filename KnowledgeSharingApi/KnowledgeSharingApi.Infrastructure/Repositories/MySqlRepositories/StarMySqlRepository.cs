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
    public class StarMySqlRepository(IDbContext dbContext)
        : BaseMySqlRepository<Star>(dbContext), IStarRepository
    {
        public async Task<Dictionary<Guid, double?>> CalculateAverageStars(List<Guid> userItemsId)
        {
            // Lấy ra từ điển của UserItemId và giá trị trung bình của Stars.
            var averages = await DbContext.Stars
                .Where(star => userItemsId.Contains(star.UserItemId))
                .GroupBy(star => star.UserItemId)
                .Select(group => new { StarItemId = group.Key, AverageStars = group.Average(star => star.Stars) })
                .ToListAsync();

            // Chuyển đổi danh sách trung bình thành từ điển, với giá trị mặc định là null cho mọi giá trị.
            var result = userItemsId.ToDictionary(id => id, id => (double?)null);

            // Đặt giá trị trung bình vào từ điển nếu có giá trị cho UserItemId.
            averages.ForEach(avg => result[avg.StarItemId] = avg.AverageStars);

            return result;
        }

        public async Task<Dictionary<Guid, int>> CalculateTotalStars(List<Guid> userItemsId)
        {
            // Lấy tổng số stars từ database đối với mỗi UserItemId
            var totalsQuery = DbContext.Stars
                .Where(star => userItemsId.Contains(star.UserItemId))
                .GroupBy(star => star.UserItemId)
                .Select(group => new
                {
                    UserItemId = group.Key,
                    TotalStars = group.Count()
                });

            // Chuyển đổi kết quả truy vấn thành dictionary và thực hiện truy vấn
            Dictionary<Guid, int> totals = await totalsQuery.ToDictionaryAsync(g => g.UserItemId, g => g.TotalStars);

            // Tạo dictionary kết quả, với mỗi UserItemId. Mặc định là 0 nếu không có giá trị tổng trong totals.
            Dictionary<Guid, int> result = userItemsId.ToDictionary(
                id => id,
                id => totals.TryGetValue(id, out int value) ? value : 0
            );

            return result;
        }

        public async Task<Dictionary<Guid, int?>> CalculateUserStars(Guid userId, List<Guid> userItemsId)
        {
            List<Star> userstars = await DbContext.Stars
                .Where(star => star.UserId == userId && userItemsId.Contains(star.UserItemId))
                .ToListAsync();

            // Chuyển đổi danh sách trung bình thành từ điển, với giá trị mặc định là null cho mọi giá trị.
            Dictionary<Guid, int?> result = userItemsId.ToDictionary(id => id, id => (int?)null);

            // Đặt giá trị star vào từ điển nếu có giá trị cho UserItemId.
            userstars.ForEach(ut => result[ut.UserItemId] = ut.Stars);

            return result;
        }
    }
}
