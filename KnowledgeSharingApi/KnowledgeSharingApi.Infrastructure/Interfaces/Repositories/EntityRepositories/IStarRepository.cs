using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories
{
    public interface IStarRepository : IRepository<Star>
    {
        /// <summary>
        /// Thực hiện lấy về danh sách số sao mà user đánh giá cho một danh sách userItem
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userItemsId"> id của useritem cần lấy </param>
        /// <returns> null nếu chưa đánh giá </returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        Task<Dictionary<Guid, int?>> CalculateUserStars(Guid userId, List<Guid> userItemsId);

        /// <summary>
        /// Thực hiện lấy về trung bình số sao mà userItem đã được nhận
        /// </summary>
        /// <param name="userItemsId"> id của useritem cần lấy </param>
        /// <returns> null nếu chưa có ai đánh giá </returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        Task<Dictionary<Guid, double?>> CalculateAverageStars(List<Guid> userItemsId);

        /// <summary>
        /// Thực hiện lấy về tổng số lượt đã đánh giá cho user Item
        /// </summary>
        /// <param name="userItemsId"> id của useritem cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        Task<Dictionary<Guid, int>> CalculateTotalStars(List<Guid> userItemsId);

    }
}
