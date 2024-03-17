using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories
{
    public interface IUserRelationRepository : IRepository<UserRelation>
    {

        /// <summary>
        /// Hai hàm kiểm tra xem blocker có đang block blockee hay không
        /// </summary>
        /// <param name="blockerId"> Id của người thực hiện </param>
        /// <param name="blockeeId"> Id của người bị tác động </param>
        /// <param name="blockerUsername"> username của người thực hiện </param>
        /// <param name="blockeeUsername"> username của người bị tác động </param>
        /// <returns> bool - có, false - không </returns>
        /// Created: PhucTV (15/3/24)
        /// Modified: None
        Task<bool> CheckBlock(string blockerId, string blockeeId);
        Task<bool> CheckBlockByUsername(string blockerUsername, string blockeeUsername);


        /// <summary>
        /// Hai hàm kiểm tra xem có user này có chặn hoặc bị user kia chặn hay không
        /// </summary>
        /// <param name="user1Id"> Id của người 1 </param>
        /// <param name="user2Id"> Id của người 2 </param>
        /// <param name="username1"> username của người 1 </param>
        /// <param name="username2"> username của người 2 </param>
        /// <returns> bool - có, false - không </returns>
        /// Created: PhucTV (15/3/24)
        /// Modified: None
        Task<bool> CheckBlockEachOther(string user1Id, string user2Id);
        Task<bool> CheckBlockByUsernameEachOther(string username1, string username2);


        /// <summary>
        /// Lấy về danh sách quan hệ theo UserId
        /// </summary>
        /// <param name="userId"> Id của user cần lấy </param>
        /// <param name="isActive"> Có phải là sender hay không </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        Task<IEnumerable<ViewUserRelation>> GetByUserId(string userId, bool isActive);

        /// <summary>
        /// Lấy về danh sách quan hệ theo UserId và loại quan hệ
        /// </summary>
        /// <param name="userId"> Id của user cần lấy </param>
        /// <param name="isActive"> Có phải là sender hay không </param>
        /// <param name="type"> Loại quan hệ cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        Task<IEnumerable<ViewUserRelation>> GetByUserIdAndType(string userId, bool isActive, EUserRelationType type);
    }
}
