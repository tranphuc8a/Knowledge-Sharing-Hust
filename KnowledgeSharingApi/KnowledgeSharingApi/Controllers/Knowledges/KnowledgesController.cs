using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Infrastructures.Encrypts;
using KnowledgeSharingApi.Services.Filters;
using KnowledgeSharingApi.Services.Interfaces.Knowledges;
using KnowledgeSharingApi.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KnowledgeSharingApi.Controllers.Knowledges
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class KnowledgesController(
        IKnowledgeService knowledgeService
    ) : BaseController
    {
        protected readonly IKnowledgeService KnowledgeService = knowledgeService;

        #region Mark APIes

        /// <summary>
        /// Xử lý yêu cầu đánh dấu phần tử kiến thức
        /// </summary>
        /// <param name="knowledgeId"> Id phần tử kiến thức </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        [HttpPost("mark/{knowledgeId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> Mark(Guid knowledgeId)
        {
            ServiceResult res = await KnowledgeService.Mark(GetCurrentUserIdStrictly(), knowledgeId, isMark: true);
            return StatusCode(res);
        }


        /// <summary>
        /// Xử lý yêu cầu hủy đánh dấu phần tử kiến thức
        /// </summary>
        /// <param name="knowledgeId"> Id phần tử kiến thức </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        [HttpPost("unmark/{knowledgeId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> Unmark(Guid knowledgeId)
        {
            ServiceResult res = await KnowledgeService.Mark(GetCurrentUserIdStrictly(), knowledgeId, isMark: false);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu lấy về danh sách user đánh dấu phần tử kiến thức
        /// </summary>
        /// <param name="knowledgeId"> Id phần tử kiến thức </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        [HttpPost("users/{knowledgeId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> GetUserMarkedKnowledge(Guid knowledgeId, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await KnowledgeService.GetListUserMarkKnowledge(GetCurrentUserIdStrictly(), knowledgeId, pagination);
            return StatusCode(res);
        }

        #endregion
    }
}
