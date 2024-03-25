using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Interfaces
{
    public interface IUserRelationService
    {
        /// <summary>
        /// Hàm thực hiện lấy về danh sách bạn bè của uid
        /// </summary>
        /// <param name="userid"> id của người dùng cần lấy </param>
        /// <param name="limit"> Số lượng bản ghi </param>
        /// <param name="offset"> Độ lệch bản ghi đầu tiên </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        Task<ServiceResult> GetFriends(Guid userid, int? limit, int? offset);

        /// <summary>
        /// Hàm thực hiện lấy về danh sách một loại quan hệ của user
        /// Follow, Block, Request | Passive, Active
        /// </summary>
        /// <param name="userid"> id của người dùng cần lấy </param>
        /// <param name="relationType"> Loại quan hệ (Follow, Block, Request, Friend) </param>
        /// <param name="isActive"> true = Chủ động, false = bị động </param>
        /// <param name="limit"> Số lượng bản ghi </param>
        /// <param name="offset"> Độ lệch bản ghi đầu </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        Task<ServiceResult> GetRelations(Guid userid, EUserRelationType relationType, bool isActive, int? limit, int? offset);


        /// <summary>
        /// Thực hiện theo dõi/bỏ theo dõi
        /// </summary>
        /// <param name="myuid"> id của User thực hiện </param>
        /// <param name="uid"> id của user được tác động </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        Task<ServiceResult> Follow(Guid myuid, Guid uid);
        Task<ServiceResult> Unfollow(Guid myuid, Guid uid);

        /// <summary>
        /// Thực hiện chặn/mở chặn
        /// </summary>
        /// <param name="myuid"> id của User thực hiện </param>
        /// <param name="uid"> id của user được tác động </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        Task<ServiceResult> Block(Guid myuid, Guid uid);
        Task<ServiceResult> Unblock(Guid myuid, Guid uid);

        /// <summary>
        /// Thực hiện Thêm bạn bè/Xóa bạn bè
        /// </summary>
        /// <param name="myuid"> id của User thực hiện </param>
        /// <param name="uid"> id của user được tác động </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        Task<ServiceResult> AddFriend(Guid myuid, Guid uid);
        Task<ServiceResult> DeleteFriend(Guid myuid, Guid uid);

        /// <summary>
        /// Thực hiện xác nhận/hủy bỏ lời mời kết bạn
        /// </summary>
        /// <param name="myuid"> id của User thực hiện </param>
        /// <param name="requestId"> id của lời mời đã được gửi đi </param>
        /// <param name="isAccept"> Có đồng ý kết bạn không </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        Task<ServiceResult> ConfirmFriend(Guid myuid, Guid requestId, bool isAccept);
        Task<ServiceResult> DeleteRequest(Guid myuid, Guid requestId);

    }
}
