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
        #region User Relation Get and Search
        /// <summary>
        /// Hàm thực hiện lấy về danh sách bạn bè của uid
        /// </summary>
        /// <param name="myUid"> id của người dùng lấy </param>
        /// <param name="userId"> id cua user bi lay </param>
        /// <param name="page"> phan trang </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        Task<ServiceResult> GetFriends(Guid? myUid, Guid userId, PaginationDto page);

        /// <summary>
        /// Hàm thực hiện tim kiem trong danh sách bạn bè của uid
        /// </summary>
        /// <param name="myUid"> id của người dùng lấy </param>
        /// <param name="userId"> id cua user bi lay </param>
        /// <param name="search"> Tu khoa tim kiem </param>
        /// <param name="page"> phan trang </param>
        /// <returns></returns>
        /// Created: PhucTV (18/5/24)
        /// Modified: None
        Task<ServiceResult> SearchFriends(Guid? myUid, Guid userId, string? search, PaginationDto page);

        /// <summary>
        /// Hàm thực hiện lấy về danh sách một loại quan hệ của user
        /// Follow, Block, Request | Passive, Active
        /// </summary>
        /// <param name="userId"> id của người dùng bi lấy </param>
        /// <param name="myUid"> id cua user lay </param>
        /// <param name="relationType"> Loại quan hệ (Follow, Block, Request, Friend) </param>
        /// <param name="isActive"> true = Chủ động, false = bị động </param>
        /// <param name="page"> phan trang </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        Task<ServiceResult> GetRelations(Guid? myUid, Guid userId, EUserRelationType relationType, bool isActive, PaginationDto page);

        /// <summary>
        /// Hàm thực hiện tim kiem trong danh sách user relation của uid
        /// </summary>
        /// <param name="userId"> id của người dùng bi lấy </param>
        /// <param name="myUid"> id cua user lay </param>
        /// <param name="search"> Tu khoa tim kiem </param>
        /// <param name="isActive"> User co chu dong khong (true - co, false - khong)  </param>
        /// <param name="relationType"> Loai quan he la gi (friend, follow, request, block) </param>
        /// <param name="page"> phan trang </param>
        /// <returns></returns>
        /// Created: PhucTV (18/5/24)
        /// Modified: None
        Task<ServiceResult> SearchRelations(Guid? myUid, Guid userId, string? search, EUserRelationType relationType, bool isActive, PaginationDto page);


        /// <summary>
        /// Lay ve tinh trang quan he hien tai giua hai user
        /// </summary>
        /// <param name="myUid"> id user can lay </param>
        /// <param name="userId"> id doi phuong </param>
        /// <returns></returns>
        /// Created: PhucTV (7/5/24)
        /// Modified: None
        Task<ServiceResult> GetRelationState(Guid myUid, Guid userId); 
        #endregion



        #region User Relations actions

        /// <summary>
        /// Thực hiện theo dõi/bỏ theo dõi
        /// </summary>
        /// <param name="myuid"> id của User thực hiện </param>
        /// <param name="userId"> id của user được tác động </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        Task<ServiceResult> Follow(Guid myuid, Guid userId);
        Task<ServiceResult> Unfollow(Guid myuid, Guid userId);

        /// <summary>
        /// Thực hiện chặn/mở chặn
        /// </summary>
        /// <param name="myuid"> id của User thực hiện </param>
        /// <param name="userId"> id của user được tác động </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        Task<ServiceResult> Block(Guid myuid, Guid userId);
        Task<ServiceResult> Unblock(Guid myuid, Guid userId);

        /// <summary>
        /// Thực hiện Thêm bạn bè/Xóa bạn bè
        /// </summary>
        /// <param name="myuid"> id của User thực hiện </param>
        /// <param name="userId"> id của user được tác động </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        Task<ServiceResult> AddFriend(Guid myuid, Guid userId);
        Task<ServiceResult> DeleteFriend(Guid myuid, Guid userId);

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

        #endregion
    }
}
