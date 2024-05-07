using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Models.Dtos;
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
        Task<bool> CheckBlock(Guid blockerId, Guid blockeeId);
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
        Task<bool> CheckBlockEachOther(Guid user1Id, Guid user2Id);
        Task<bool> CheckBlockByUsernameEachOther(string username1, string username2);


        /// <summary>
        /// Lấy về danh sách quan hệ theo UserId
        /// </summary>
        /// <param name="userId"> Id của user cần lấy </param>
        /// <param name="isActive"> Có phải là sender hay không </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        Task<IEnumerable<ViewUserRelation>> GetByUserId(Guid userId, bool isActive);

        /// <summary>
        /// Lấy về danh sách quan hệ theo UserId và loại quan hệ
        /// </summary>
        /// <param name="userId"> Id của user cần lấy </param>
        /// <param name="isActive"> Có phải là sender hay không </param>
        /// <param name="type"> Loại quan hệ cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        Task<IEnumerable<ViewUserRelation>> GetByUserIdAndType(Guid userId, bool isActive, EUserRelationType type);

        /// <summary>
        /// Lấy về mối quan hệ giữa hai user
        /// </summary>
        /// <param name="user1"> id của user 1</param>
        /// <param name="user2"> id của user 2</param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<EUserRelationType> GetUserRelationType(Guid user1, Guid user2);
        /// <summary>
        /// Lấy về mối quan hệ giữa hai một user với danh sách user kia
        /// </summary>
        /// <param name="user1"> id của user 1</param>
        /// <param name="users2"> Danh sách id của user 2</param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<Dictionary<Guid, EUserRelationType>> GetUserRelationType(Guid user1, List<Guid> users2);
        /// <summary>
        /// Lấy về mối quan hệ giữa hai một user với danh sách user kia
        /// </summary>
        /// <param name="user1"> id của user 1</param>
        /// <param name="users2"> Danh sách id của user 2</param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<Dictionary<Guid, UserRelationTypeDto>> GetDetailUserRelationType(Guid user1, List<Guid> users2);


        /// <summary>
        /// Them quan he block giua blocker va blockee
        /// </summary>
        /// <param name="blockerId"> id nguoi chan </param>
        /// <param name="blockeeId"> id nguoi bi chan </param>
        /// <returns></returns>
        /// Created: PhucTV (7/5/24)
        /// Modified: None
        Task<Guid?> AddBlock(Guid blockerId, Guid blockeeId);

        /// <summary>
        /// Them quan he ban be giua hai user
        /// </summary>
        /// <param name="idUser1"> id user 1 </param>
        /// <param name="idUser2"> id user 2 </param>
        /// <returns></returns>
        /// Created: PhucTV (7/5/24)
        /// Modified: None
        Task<Guid?> AddFriend(Guid idUser1, Guid idUser2);

        /// <summary>
        /// Xoa quan he ban be giua hai user ve quan he NotRelation
        /// </summary>
        /// <param name="idUser1"> id user 1 </param>
        /// <param name="idUser2"> id user 2 </param>
        /// <returns></returns>
        /// Created: PhucTV (7/5/24)
        /// Modified: None
        Task<int> DeleteFriend(Guid idUser1, Guid idUser2);
    }
}
