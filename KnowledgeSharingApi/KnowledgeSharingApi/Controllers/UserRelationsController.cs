using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Infrastructures.Encrypts;
using KnowledgeSharingApi.Services.Filters;
using KnowledgeSharingApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mysqlx.Crud;
using System.Collections.Generic;
using System.Security.Claims;

namespace KnowledgeSharingApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserRelationsController(
        IUserRelationService userRelationService
    ) : BaseController
    {
        protected readonly IUserRelationService UserRelationService = userRelationService;

        #region Get Others list relations
        /// <summary>
        /// Xử lý yêu cầu lấy về quan he hien tai voi mot user
        /// </summary>
        /// <param name="userId"> id của người dùng cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("relation-status/{userId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> GetUserFriends(Guid userId)
        {
            ServiceResult res = await UserRelationService.GetRelationState(GetCurrentUserIdStrictly(), userId);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu lấy về danh sách bạn bè của một người dùng
        /// </summary>
        /// <param name="userId"> id của người dùng cần lấy </param>
        /// <param name="limit"> Số lượng bản ghi </param>
        /// <param name="offset"> Độ lệch bản ghi </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("friends/{userId}")]
        [AllowAnonymous]
        public virtual async Task<IActionResult> GetUserFriends(Guid userId, int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetFriends(userId, limit, offset);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu lấy về danh sách một người dùng đang theo dõi
        /// </summary>
        /// <param name="userId"> id của người dùng cần lấy </param>
        /// <param name="limit"> Số lượng bản ghi </param>
        /// <param name="offset"> Độ lệch bản ghi </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("followers/{userId}")]
        [AllowAnonymous]
        public virtual async Task<IActionResult> GetUserFollowers(Guid userId, int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetRelations(userId, EUserRelationType.Follow, isActive: true, limit, offset);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu lấy về danh sách người dùng đang theo dõi một người dùng khác
        /// </summary>
        /// <param name="userId"> id của người dùng cần lấy </param>
        /// <param name="limit"> Số lượng bản ghi </param>
        /// <param name="offset"> Độ lệch bản ghi </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("followees/{userId}")]
        [AllowAnonymous]
        public virtual async Task<IActionResult> GetUserFollowees(Guid userId, int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetRelations(userId, EUserRelationType.Follow, isActive: false, limit, offset);
            return StatusCode(res);
        }
        #endregion

        #region Get My List Relations
        /// <summary>
        /// Xử lý yêu cầu lấy về danh sách bạn bè của chính mình
        /// </summary>
        /// <param name="limit"> Số lượng bản ghi </param>
        /// <param name="offset"> Độ lệch bản ghi </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("my/friends")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> GetMyFriends(int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetFriends(GetCurrentUserIdStrictly(), limit, offset);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu lấy về danh sách người mà mình đang theo dõi
        /// </summary>
        /// <param name="limit"> Số lượng bản ghi </param>
        /// <param name="offset"> Độ lệch bản ghi </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("my/followers")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> GetMyFollowers(int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetRelations(GetCurrentUserIdStrictly(), EUserRelationType.Follow, isActive: true, limit, offset);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu lấy về danh sách người đang theo dõi chính mình
        /// </summary>
        /// <param name="limit"> Số lượng bản ghi </param>
        /// <param name="offset"> Độ lệch bản ghi </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("my/followees")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> GetMyFollowees(int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetRelations(GetCurrentUserIdStrictly(), EUserRelationType.Follow, isActive: false, limit, offset);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu lấy về danh sách lời mời kết bạn mà mình đã gửi đi
        /// </summary>
        /// <param name="limit"> Số lượng bản ghi </param>
        /// <param name="offset"> Độ lệch bản ghi </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("my/requesters")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> GetMyRequesters(int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetRelations(GetCurrentUserIdStrictly(), EUserRelationType.FriendRequest, isActive: true, limit, offset);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu lấy về danh sách lời mời kết bạn gửi tới mình
        /// </summary>
        /// <param name="limit"> Số lượng bản ghi </param>
        /// <param name="offset"> Độ lệch bản ghi </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("my/requestees")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> GetMyRequestees(int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetRelations(GetCurrentUserIdStrictly(), EUserRelationType.FriendRequest, isActive: false, limit, offset);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu lấy về danh sách người dùng mà mình đang chặn
        /// </summary>
        /// <param name="limit"> Số lượng bản ghi </param>
        /// <param name="offset"> Độ lệch bản ghi </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("my/blockers")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> GetMyBlockers(int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetRelations(GetCurrentUserIdStrictly(), EUserRelationType.Block, isActive: true, limit, offset);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu lấy về danh sách người đang chặn mình
        /// </summary>
        /// <param name="limit"> Số lượng bản ghi </param>
        /// <param name="offset"> Độ lệch bản ghi </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("my/blockees")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> GetMyBlockees(int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetRelations(GetCurrentUserIdStrictly(), EUserRelationType.Block, isActive: false, limit, offset);
            return StatusCode(res);
        }
        #endregion

        #region Admin Get user relations
        /// <summary>
        /// Xử lý yêu cầu admin lấy về danh sách bạn bè của một người dùng
        /// </summary>
        /// <param name="userId"> id của người dùng cần lấy </param>
        /// <param name="limit"> Số lượng bản ghi </param>
        /// <param name="offset"> Độ lệch bản ghi </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("admin/friends/{userId}")]
        [CustomAuthorization(Roles: "Admin")]
        public virtual async Task<IActionResult> AdminGetFriends(Guid userId, int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetFriends(userId, limit, offset);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu admin lấy về danh sách người mình đang theo dõi của một người dùng
        /// </summary>
        /// <param name="userId"> id của người dùng cần lấy </param>
        /// <param name="limit"> Số lượng bản ghi </param>
        /// <param name="offset"> Độ lệch bản ghi </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("admin/followers/{userId}")]
        [CustomAuthorization(Roles: "Admin")]
        public virtual async Task<IActionResult> AdminGetFollowers(Guid userId, int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetRelations(userId, EUserRelationType.Follow, isActive: true, limit, offset);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu admin lấy về danh sách người đang theo dõi mình của một người dùng
        /// </summary>
        /// <param name="userId"> id của người dùng cần lấy </param>
        /// <param name="limit"> Số lượng bản ghi </param>
        /// <param name="offset"> Độ lệch bản ghi </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("admin/followees/{userId}")]
        [CustomAuthorization(Roles: "Admin")]
        public virtual async Task<IActionResult> AdminGetFollowees(Guid userId, int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetRelations(userId, EUserRelationType.Follow, isActive: false, limit, offset);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu admin lấy về danh sách yêu cầu kết bạn mình gửi đi của một người dùng
        /// </summary>
        /// <param name="userId"> id của người dùng cần lấy </param>
        /// <param name="limit"> Số lượng bản ghi </param>
        /// <param name="offset"> Độ lệch bản ghi </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("admin/requesters/{userId}")]
        [CustomAuthorization(Roles: "Admin")]
        public virtual async Task<IActionResult> AdminGetRequesters(Guid userId, int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetRelations(userId, EUserRelationType.FriendRequest, isActive: true, limit, offset);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu admin lấy về danh sách yêu cầu kết bạn mình được nhận của một người dùng
        /// </summary>
        /// <param name="userId"> id của người dùng cần lấy </param>
        /// <param name="limit"> Số lượng bản ghi </param>
        /// <param name="offset"> Độ lệch bản ghi </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("admin/requestees/{userId}")]
        [CustomAuthorization(Roles: "Admin")]
        public virtual async Task<IActionResult> AdminGetRequestees(Guid userId, int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetRelations(userId, EUserRelationType.FriendRequest, isActive: false, limit, offset);
            return StatusCode(res);
        }


        /// <summary>
        /// Xử lý yêu cầu admin lấy về danh sách người dùng mà mình đă chặn của một người dùng
        /// </summary>
        /// <param name="userId"> id của người dùng cần lấy </param>
        /// <param name="limit"> Số lượng bản ghi </param>
        /// <param name="offset"> Độ lệch bản ghi </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("admin/blockers/{userId}")]
        public virtual async Task<IActionResult> AdminGetBlockers(Guid userId, int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetRelations(userId, EUserRelationType.Block, isActive: true, limit, offset);
            return StatusCode(res);
        }


        /// <summary>
        /// Xử lý yêu cầu admin lấy về danh sách ngừi dùng đã chặn mình của một người dùng
        /// </summary>
        /// <param name="userId"> id của người dùng cần lấy </param>
        /// <param name="limit"> Số lượng bản ghi </param>
        /// <param name="offset"> Độ lệch bản ghi </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("admin/blockees/{userId}")]
        public virtual async Task<IActionResult> AdminGetBlockees(Guid userId, int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetRelations(userId, EUserRelationType.Block, isActive: false, limit, offset);
            return StatusCode(res);
        }
        #endregion

        #region Follow and Unfollow
        /// <summary>
        /// Xử lý yêu cầu theo dõi một người dùng
        /// </summary>
        /// <param name="userId"> id của người dùng được follow </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpPost("follow/{userId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> Follow(Guid userId)
        {
            ServiceResult res = await UserRelationService.Follow(GetCurrentUserIdStrictly(), userId);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu hủy theo dõi một người dùng
        /// </summary>
        /// <param name="userId"> id của người dùng được follow </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpPost("unfollow/{userId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> Unfollow(Guid userId)
        {
            ServiceResult res = await UserRelationService.Unfollow(GetCurrentUserIdStrictly(), userId);
            return StatusCode(res);
        }
        #endregion


        #region Friends
        /// <summary>
        /// Xử lý yêu cầu gửi lời mời kết bạn
        /// </summary>
        /// <param name="userId"> id của người dùng </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpPost("add-friend/{userId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> AddFriend(Guid userId)
        {
            ServiceResult res = await UserRelationService.AddFriend(GetCurrentUserIdStrictly(), userId);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu xóa bạn bè
        /// </summary>
        /// <param name="userId"> id của người dùng </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpDelete("delete-friend/{userId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> DeleteFriend(Guid userId)
        {
            ServiceResult res = await UserRelationService.DeleteFriend(GetCurrentUserIdStrictly(), userId);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu phê duyệt lời mời kết bạn
        /// </summary>
        /// <param name="requestId"> id của lời mời </param>
        /// <param name="isAccept"> đồng ý / không đồng ý </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpPost("confirm-friend/{requestId}/{isAccept}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> ConfirmFriend(Guid requestId, bool isAccept)
        {
            ServiceResult res = await UserRelationService.ConfirmFriend(GetCurrentUserIdStrictly(), requestId, isAccept);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu xóa yêu cầu
        /// </summary>
        /// <param name="requestId"> id của lời mời </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpDelete("delete-request/{requestId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> DeleteRequest(Guid requestId)
        {
            ServiceResult res = await UserRelationService.DeleteRequest(GetCurrentUserIdStrictly(), requestId);
            return StatusCode(res);
        }

        #endregion

        #region Block and Unblock

        /// <summary>
        /// Xử lý yêu chặn một người dùng
        /// </summary>
        /// <param name="userId"> id của user cần chặn </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpPost("block/{userId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> BlockUser(Guid userId)
        {
            ServiceResult res = await UserRelationService.Block(GetCurrentUserIdStrictly(), userId);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu phê duyệt lời mời kết bạn
        /// </summary>
        /// <param name="userId"> id của user được mở chặn </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpDelete("unblock/{userId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> UnblockUser(Guid userId)
        {
            ServiceResult res = await UserRelationService.Unblock(GetCurrentUserIdStrictly(), userId);
            return StatusCode(res);
        }


        #endregion
    }
}
