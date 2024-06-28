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
        /// <returns> ViewUserProfile </returns>
        /// Created: PhucTV (16/3/24)
        /// Modified: None
        Task<ViewUserProfile?> GetByUserId(Guid userId);

        /// <summary>
        /// Lấy về profile theo username
        /// </summary>
        /// <param name="username"> userId cần lấy </param>
        /// <returns> ViewUserProfile </returns>
        /// Created: PhucTV (16/3/24)
        /// Modified: None
        Task<ViewUserProfile?> GetByUsername(string username);

        /// <summary>
        /// Lấy về profile theo userId
        /// </summary>
        /// <param name="unOruid"> userId cần lấy </param>
        /// <returns> ViewUserProfile </returns>
        /// Created: PhucTV (16/3/24)
        /// Modified: None
        Task<ViewUserProfile?> GetByUsernameOrUserId(string unOruid);

        /// <summary>
        /// Lấy về profile theo email
        /// </summary>
        /// <param name="email"> userId cần lấy </param>
        /// <returns> ViewUserProfile </returns>
        /// Created: PhucTV (16/3/24)
        /// Modified: None
        Task<ViewUserProfile?> GetByEmail(string email);

        ///// <summary>
        ///// Tìm kiếm danh sách ViewUserProfile theo search Key
        ///// </summary>
        ///// <param name="uid"> id của n </param>
        ///// <param name="searchKey"></param>
        ///// <param name="limit"></param>
        ///// <param name="offset"></param>
        ///// <returns> ViewUserProfile </returns>
        ///// Created: PhucTV (16/3/24)
        ///// Modified: None
        //Task<PaginationResponseModel<ViewUserProfile>> Search(string uid, string searchKey, int? limit, int? offset);
    }
}
