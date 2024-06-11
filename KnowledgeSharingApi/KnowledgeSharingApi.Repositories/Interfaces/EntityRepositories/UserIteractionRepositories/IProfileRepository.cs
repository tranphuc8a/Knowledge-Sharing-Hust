using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Repositories.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.UserIteractionRepositories
{
    public interface IProfileRepository : IRepository<Profile>
    {
        /// <summary>
        /// Lấy về profile theo userId
        /// </summary>
        /// <param name="userId"> userId cần lấy </param>
        /// <returns> ViewUser </returns>
        /// Created: PhucTV (16/3/24)
        /// Modified: None
        Task<ViewUser?> GetByUserId(Guid userId);

        /// <summary>
        /// Lấy về profile theo username
        /// </summary>
        /// <param name="username"> userId cần lấy </param>
        /// <returns> ViewUser </returns>
        /// Created: PhucTV (16/3/24)
        /// Modified: None
        Task<ViewUser?> GetByUsername(string username);

        /// <summary>
        /// Lấy về profile theo userId
        /// </summary>
        /// <param name="unOruid"> userId cần lấy </param>
        /// <returns> ViewUser </returns>
        /// Created: PhucTV (16/3/24)
        /// Modified: None
        Task<ViewUser?> GetByUsernameOrUserId(string unOruid);

        /// <summary>
        /// Lấy về profile theo email
        /// </summary>
        /// <param name="email"> userId cần lấy </param>
        /// <returns> ViewUser </returns>
        /// Created: PhucTV (16/3/24)
        /// Modified: None
        Task<ViewUser?> GetByEmail(string email);

        ///// <summary>
        ///// Tìm kiếm danh sách ViewUser theo search Key
        ///// </summary>
        ///// <param name="uid"> id của n </param>
        ///// <param name="searchKey"></param>
        ///// <param name="limit"></param>
        ///// <param name="offset"></param>
        ///// <returns> ViewUser </returns>
        ///// Created: PhucTV (16/3/24)
        ///// Modified: None
        //Task<PaginationResponseModel<ViewUser>> Search(string uid, string searchKey, int? limit, int? offset);
    }
}
