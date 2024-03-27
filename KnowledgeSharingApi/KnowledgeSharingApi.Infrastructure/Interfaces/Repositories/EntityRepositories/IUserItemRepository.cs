using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces;
using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories
{
    public interface IUserItemRepository : IRepository<UserItem>
    {

        /// <summary>
        /// Hai hàm lấy về chính xác kiểu và dữ liệu của userItem
        /// </summary>
        /// <param name="userItemId"> id của item cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        Task<IUserItemView?> GetExactlyUserItem(Guid userItemId);
        Task<IResponseUserItemModel?> GetExactlyResponseUserItemModel(Guid userItemId);
    }
}
