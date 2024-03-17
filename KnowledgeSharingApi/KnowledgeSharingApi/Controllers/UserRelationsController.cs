using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeSharingApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserRelationsController(
        IUserRelationService userRelationService    
    ) : ControllerBase
    {
        protected readonly IUserRelationService UserRelationService = userRelationService;

        [HttpGet("friends/{userId}")]
        public virtual async Task<IActionResult> GetUserFriends(string userId, int? limit, int? offset)
        {
            ServiceResult res = await UserRelationService.GetFriends(userId, limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }
    }
}
