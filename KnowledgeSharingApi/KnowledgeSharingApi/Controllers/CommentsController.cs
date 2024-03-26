﻿using KnowledgeSharingApi.Domains.Models.ApiRequestModels.CreateUserItemModels;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.UpdateUserItemModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Infrastructures.Encrypts;
using KnowledgeSharingApi.Services.Filters;
using KnowledgeSharingApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KnowledgeSharingApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CommentsController(
        ICommentService commentService
    ) : ControllerBase
    {
        protected readonly ICommentService CommentService = commentService;

        #region Common APIes

        /// <summary>
        /// Yêu cầu lấy về danh sách comment của một knowledge
        /// </summary>
        /// <param name="knowledgeId"> id của phần tử kiến thức </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        [HttpGet("anonymous/{knowledgeId}")]
        [AllowAnonymous]
        public async Task<IActionResult> AnonymousGetCommentsOfKnowledge(Guid knowledgeId, int? limit, int? offset)
        {
            ServiceResult res = await CommentService.AnonymousGetListKnowledgeComments(knowledgeId, limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Yêu cầu lấy về danh sách reply của một comment
        /// </summary>
        /// <param name="commentId"> id của comment </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        [HttpGet("replies/{commentId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCommentReplies(Guid commentId, int? limit, int? offset)
        {
            ServiceResult res = await CommentService.GetListCommentReplies(commentId, limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Yêu cầu lấy về chi tiết một comment
        /// </summary>
        /// <param name="commentId"> id của comment cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        [HttpGet("detail/{commentId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCommentDetail(Guid commentId)
        {
            ServiceResult res = await CommentService.GetComment(commentId);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }
        #endregion


        #region Admin Apies

        /// <summary>
        /// Yêu cầu admin lấy về danh sách comment của một knowledge
        /// </summary>
        /// <param name="knowledgeId"> id của knowledge </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        [HttpGet("admin/{knowledgeId}")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminGetListComments(Guid knowledgeId, int? limit, int? offset)
        {
            ServiceResult res = await CommentService.AdminGetListKnowledgeComments(knowledgeId, limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Yêu cầu admin xóa một comment
        /// </summary>
        /// <param name="commentId"> id của comment cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        [HttpDelete("admin/{commentId}")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminDeleteComment(Guid commentId)
        {
            ServiceResult res = await CommentService.AdminDeleteComment(commentId);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Yêu cầu Admin khóa comment một knowledge
        /// </summary>
        /// <param name="knowledgeId"> id của knowledge cần khóa </param>
        /// <returns></returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        [HttpGet("admin/block/{knowledgeId}")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminBlockComment(Guid knowledgeId)
        {
            ServiceResult res = await CommentService.AdminBlockCommentOfKnowledge(knowledgeId, isBlock: true);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Yêu cầu Admin mở khóa comment một knowledge
        /// </summary>
        /// <param name="knowledgeId"> id của knowledge cần mở khóa </param>
        /// <returns></returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        [HttpGet("admin/unblock/{knowledgeId}")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminUnBlockComment(Guid knowledgeId)
        {
            ServiceResult res = await CommentService.AdminBlockCommentOfKnowledge(knowledgeId, isBlock: false);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }



        #endregion


        #region User Get Comments Apies

        /// <summary>
        /// Yêu cầu User lấy danh sách comment một knowledge
        /// </summary>
        /// <param name="knowledgeId"> id của knowledge cần lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        [HttpGet("{knowledgeId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserGetListComments(Guid knowledgeId, int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await CommentService.UserGetListKnowledgeComments(Guid.Parse(myUid), knowledgeId, limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Yêu cầu User lấy về danh sách comment của mình trong một knowledge
        /// </summary>
        /// <param name="knowledgeId"> id của knowledge cần lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        [HttpGet("my/{knowledgeId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserGetListMyComments(Guid knowledgeId, int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await CommentService.UserGetMyCommentsOfKnowledge(Guid.Parse(myUid), knowledgeId, limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Yêu cầu User lấy về danh sách comment của một user khác trong một knowledge
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <param name="knowledgeId"> id của knowledge cần lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        [HttpGet("user/{userId}/{knowledgeId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserGetListUserComments(Guid userId, Guid knowledgeId, int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await CommentService.UserGetUserCommentsOfKnowledge(Guid.Parse(myUid), userId, knowledgeId, limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Yêu cầu User tìm kiếm danh sách comment trong một knowledge
        /// </summary>
        /// <param name="knowledgeId"> id của knowledge cần lấy </param>
        /// <param name="search"> Từ khóa tìm kiếm </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        [HttpGet("search/{knowledgeId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserSearchListComments(Guid knowledgeId, string search, int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await CommentService.UserSearchCommentsOfKnowledge(Guid.Parse(myUid), knowledgeId, search, limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        #endregion


        #region User Apies do operations

        /// <summary>
        /// Yêu cầu User thêm một comment trong một knowledge
        /// </summary>
        /// <param name="commentModel"> Nội dung bình luận </param>
        /// <returns></returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        [HttpPost]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserComment([FromBody] CreateCommentModel commentModel)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await CommentService.UserAddComment(Guid.Parse(myUid), commentModel);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Yêu cầu User chỉnh sửa một comment
        /// </summary>
        /// <param name="commentId"> id của comment cần update </param>
        /// <param name="updateModel"> Nội dung bình luận cần cập nhật </param>
        /// <returns></returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        [HttpPatch("{commentId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserUpdateComment(Guid commentId, [FromBody] UpdateCommentModel updateModel)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await CommentService.UserUpdateComment(Guid.Parse(myUid), commentId, updateModel);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Yêu cầu User xóa bình luận
        /// </summary>
        /// <param name="commentId"> id của comment cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        [HttpDelete("{commentId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserGetListMyComments(Guid commentId)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await CommentService.UserDeleteComment(Guid.Parse(myUid), commentId);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Yêu cầu User reply lại một comment khác
        /// </summary>
        /// <param name="commentId"> id của comment được reply </param>
        /// <param name="replyModel"> Nội dung bình luận phản hồi </param>
        /// <returns></returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        [HttpPost("reply")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserReplyComment([FromBody] ReplyCommentModel replyModel)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await CommentService.UserReplyComment(Guid.Parse(myUid), replyModel);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Yêu cầu User khóa bình luận của một knowledge
        /// </summary>
        /// <param name="knowledgeId"> id của knowledge cần khóa </param>
        /// <returns></returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        [HttpGet("block/{knowledgeId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserBlockKnowledge(Guid knowledgeId)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await CommentService.UserBlockKnowledgeComments(Guid.Parse(myUid), knowledgeId, isBlock: true);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Yêu cầu User mở khóa bình luận của một knowledge
        /// </summary>
        /// <param name="knowledgeId"> id của knowledge cần mở khóa </param>
        /// <returns></returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        [HttpGet("unblock/{knowledgeId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserUnBlockKnowledge(Guid knowledgeId)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await CommentService.UserBlockKnowledgeComments(Guid.Parse(myUid), knowledgeId, isBlock: false);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        #endregion

    }
}