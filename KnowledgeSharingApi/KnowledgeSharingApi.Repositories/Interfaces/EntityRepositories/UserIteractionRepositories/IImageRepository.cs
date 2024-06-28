using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Repositories.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.UserIteractionRepositories
{
    public interface IImageRepository : IRepository<Image>
    {

        /// <summary>
        /// Lấy về danh sách ảnh của một userId
        /// </summary>
        /// <param name="userId"> id user cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (1/5/24)
        /// Modified: None
        Task<List<Image>> GetByUserId(Guid userId);


        /// <summary>
        /// Try to insert imageUrl for user 
        /// </summary>
        /// <param name="userId"> id of user want to do </param>
        /// <param name="imageUrl"> imageUrl of image </param>
        /// <returns></returns>
        /// Created: PhucTV (5/5/24)
        /// Modified: None
        Task<int> TryInsertImage(Guid userId, string? imageUrl);
    }
}
