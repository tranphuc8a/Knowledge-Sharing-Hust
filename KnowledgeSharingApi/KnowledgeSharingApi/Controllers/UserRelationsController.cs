using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mysqlx.Crud;
using System.Collections.Generic;

namespace KnowledgeSharingApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserRelationsController(
        IUserRelationService userRelationService
    ) : ControllerBase
    {
        protected readonly IUserRelationService UserRelationService = userRelationService;

        #region Get Others list relations
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
        public virtual async Task<IActionResult> GetUserFriends(string userId, int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetFriends(userId, limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
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
        public virtual async Task<IActionResult> GetUserFollowers(string userId, int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetRelations(userId, EUserRelationType.Follow, isActive: true, limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
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
        public virtual async Task<IActionResult> GetUserFollowees(string userId, int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetRelations(userId, EUserRelationType.Follow, isActive: false, limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
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
        [HttpGet("me/friends")]
        public virtual async Task<IActionResult> GetMyFriends(string userId, int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetFriends(userId, limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Xử lý yêu cầu lấy về danh sách người mà mình đang theo dõi
        /// </summary>
        /// <param name="limit"> Số lượng bản ghi </param>
        /// <param name="offset"> Độ lệch bản ghi </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("me/followers")]
        public virtual async Task<IActionResult> GetMyFollowers(string userId, int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetRelations(userId, EUserRelationType.Follow, isActive: true, limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Xử lý yêu cầu lấy về danh sách người đang theo dõi chính mình
        /// </summary>
        /// <param name="limit"> Số lượng bản ghi </param>
        /// <param name="offset"> Độ lệch bản ghi </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("me/followees")]
        public virtual async Task<IActionResult> GetMyFollowees(string userId, int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetRelations(userId, EUserRelationType.Follow, isActive: false, limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Xử lý yêu cầu lấy về danh sách lời mời kết bạn mà mình đã gửi đi
        /// </summary>
        /// <param name="limit"> Số lượng bản ghi </param>
        /// <param name="offset"> Độ lệch bản ghi </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("me/requesters")]
        public virtual async Task<IActionResult> GetMyRequesters(string userId, int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetRelations(userId, EUserRelationType.FriendRequest, isActive: true, limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Xử lý yêu cầu lấy về danh sách lời mời kết bạn gửi tới mình
        /// </summary>
        /// <param name="limit"> Số lượng bản ghi </param>
        /// <param name="offset"> Độ lệch bản ghi </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("me/requestees")]
        public virtual async Task<IActionResult> GetMyRequestees(string userId, int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetRelations(userId, EUserRelationType.FriendRequest, isActive: false, limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Xử lý yêu cầu lấy về danh sách người dùng mà mình đang chặn
        /// </summary>
        /// <param name="limit"> Số lượng bản ghi </param>
        /// <param name="offset"> Độ lệch bản ghi </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("me/blockers")]
        public virtual async Task<IActionResult> GetMyBlockers(string userId, int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetRelations(userId, EUserRelationType.Block, isActive: true, limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Xử lý yêu cầu lấy về danh sách người đang chặn mình
        /// </summary>
        /// <param name="limit"> Số lượng bản ghi </param>
        /// <param name="offset"> Độ lệch bản ghi </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpGet("me/blockees")]
        public virtual async Task<IActionResult> GetMyBlockees(string userId, int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetRelations(userId, EUserRelationType.Block, isActive: false, limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
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
        [HttpGet("admin/friends")]
        public virtual async Task<IActionResult> AdminGetFriends(string userId, int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetFriends(userId, limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
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
        [HttpGet("admin/followers")]
        public virtual async Task<IActionResult> AdminGetFollowers(string userId, int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetRelations(userId, EUserRelationType.Follow, isActive: true, limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
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
        [HttpGet("admin/followees")]
        public virtual async Task<IActionResult> AdminGetFollowees(string userId, int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetRelations(userId, EUserRelationType.Follow, isActive: false, limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
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
        [HttpGet("admin/requesters")]
        public virtual async Task<IActionResult> AdminGetRequesters(string userId, int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetRelations(userId, EUserRelationType.FriendRequest, isActive: true, limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
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
        [HttpGet("admin/requestees")]
        public virtual async Task<IActionResult> AdminGetRequestees(string userId, int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetRelations(userId, EUserRelationType.FriendRequest, isActive: false, limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
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
        [HttpGet("admin/blockers")]
        public virtual async Task<IActionResult> AdminGetBlockers(string userId, int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetRelations(userId, EUserRelationType.Block, isActive: true, limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
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
        [HttpGet("admin/blockees")]
        public virtual async Task<IActionResult> AdminGetBlockees(string userId, int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetRelations(userId, EUserRelationType.Block, isActive: false, limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
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
        public virtual async Task<IActionResult> Follow(string myUid, string userId)
        {
            ServiceResult res = await UserRelationService.Follow(myUid, userId);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Xử lý yêu cầu hủy theo dõi một người dùng
        /// </summary>
        /// <param name="userId"> id của người dùng được follow </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpPost("unfollow/{userId}")]
        public virtual async Task<IActionResult> Unfollow(string myUid, string userId)
        {
            ServiceResult res = await UserRelationService.Unfollow(myUid, userId);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
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
        public virtual async Task<IActionResult> AddFriend(string myUid, string userId)
        {
            ServiceResult res = await UserRelationService.AddFriend(myUid, userId);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Xử lý yêu cầu xóa bạn bè
        /// </summary>
        /// <param name="userId"> id của người dùng </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpDelete("delete-friend/{userId}")]
        public virtual async Task<IActionResult> DeleteFriend(string myUid, string userId)
        {
            ServiceResult res = await UserRelationService.DeleteFriend(myUid, userId);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
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
        public virtual async Task<IActionResult> ConfirmFriend(string myUid, string requestId, bool isAccept)
        {
            ServiceResult res = await UserRelationService.ConfirmFriend(myUid, requestId, isAccept);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Xử lý yêu cầu xóa yêu cầu
        /// </summary>
        /// <param name="requestId"> id của lời mời </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpDelete("delete-request/{requestId}")]
        public virtual async Task<IActionResult> DeleteRequest(string myUid, string requestId)
        {
            ServiceResult res = await UserRelationService.DeleteRequest(myUid, requestId);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
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
        public virtual async Task<IActionResult> BlockUser(string myUid, string userId)
        {
            ServiceResult res = await UserRelationService.Block(myUid, userId);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Xử lý yêu cầu phê duyệt lời mời kết bạn
        /// </summary>
        /// <param name="userId"> id của user được mở chặn </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        [HttpDelete("unblock/{userId}")]
        public virtual async Task<IActionResult> UnblockUser(string myUid, string userId)
        {
            ServiceResult res = await UserRelationService.Unblock(myUid, userId);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }


        #endregion
    }
}
