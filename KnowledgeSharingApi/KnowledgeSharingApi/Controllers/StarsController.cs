using KnowledgeSharingApi.Domains.Models.ApiRequestModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Infrastructures.Encrypts;
using KnowledgeSharingApi.Services.Filters;
using KnowledgeSharingApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Tnef;
using System.Security.Claims;

namespace KnowledgeSharingApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StarsController(
        IStarService starService    
    ): ControllerBase
    {
        protected IStarService StarService = starService;


        #region Get stars of an user item

        /// <summary>
        /// Yêu cầu lấy về danh sách star của một item
        /// </summary>
        /// <param name="itemId"> id cần lấy </param>
        /// <param name="limit"> Số lượng phần tử trang </param>
        /// <param name="offset"> Độ lệch trang </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpGet("anonymous/{itemId}")]
        [AllowAnonymous]
        public virtual async Task<IActionResult> AnonymousGetUserItemStars(Guid itemId, int? limit, int? offset)
        {
            ServiceResult res = await StarService.AnonymousGetUserItemStars(itemId, limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Yêu cầu user lấy về danh sách star của một item
        /// </summary>
        /// <param name="itemId"> id cần lấy </param>
        /// <param name="limit"> Số lượng phần tử trang </param>
        /// <param name="offset"> Độ lệch trang </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpGet("{itemId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> UserGetUserItemStars(Guid itemId, int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await StarService.UserGetUserItemStars(Guid.Parse(myUid), itemId, limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Yêu cầu Admin lấy về danh sách star của một item
        /// </summary>
        /// <param name="itemId"> id cần lấy </param>
        /// <param name="limit"> Số lượng phần tử trang </param>
        /// <param name="offset"> Độ lệch trang </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpGet("admin/{itemId}")]
        [CustomAuthorization(Roles: "Admin")]
        public virtual async Task<IActionResult> AdminGetUserItemStars(Guid itemId, int? limit, int? offset)
        {
            ServiceResult res = await StarService.AdminGetUserItemStars(itemId, limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        #endregion


        #region Get my stars

        /// <summary>
        /// Yêu cầu lấy về danh sách item mà mình đã đánh giá sao
        /// </summary>
        /// <param name="limit"> Số lượng phần tử trang </param>
        /// <param name="offset"> Độ lệch trang </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpGet("useritems")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> GetMyStaredUserItems(int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await StarService.UserGetMyScoredUserItems(Guid.Parse(myUid), limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Yêu cầu lấy về danh sách comment mà mình đã đánh giá sao
        /// </summary>
        /// <param name="itemId"> id cần lấy </param>
        /// <param name="limit"> Số lượng phần tử trang </param>
        /// <param name="offset"> Độ lệch trang </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpGet("comments")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> GetMyStaredComments(int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await StarService.UserGetMyScoredComments(Guid.Parse(myUid), limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Yêu cầu lấy về danh sách item mà mình đã đánh giá sao
        /// </summary>
        /// <param name="limit"> Số lượng phần tử trang </param>
        /// <param name="offset"> Độ lệch trang </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpGet("courses")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> GetMyStaredCourse(int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await StarService.UserGetMyScoredCourses(Guid.Parse(myUid), limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }


        /// <summary>
        /// Yêu cầu lấy về danh sách item mà mình đã đánh giá sao
        /// </summary>
        /// <param name="limit"> Số lượng phần tử trang </param>
        /// <param name="offset"> Độ lệch trang </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpGet("posts")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> GetMyStaredPosts(int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await StarService.UserGetMyScoredPosts(Guid.Parse(myUid), limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }


        /// <summary>
        /// Yêu cầu lấy về danh sách item mà mình đã đánh giá sao
        /// </summary>
        /// <param name="limit"> Số lượng phần tử trang </param>
        /// <param name="offset"> Độ lệch trang </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpGet("questions")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> GetMyStaredQuestions(int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await StarService.UserGetMyScoredQuestions(Guid.Parse(myUid), limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }


        /// <summary>
        /// Yêu cầu lấy về danh sách item mà mình đã đánh giá sao
        /// </summary>
        /// <param name="limit"> Số lượng phần tử trang </param>
        /// <param name="offset"> Độ lệch trang </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpGet("lessons")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> GetMyStaredLessons(int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await StarService.UserGetMyScoredLessons(Guid.Parse(myUid), limit, offset);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        #endregion

        #region Put Stars

        /// <summary>
        /// Yêu cầu gửi đánh giá sao cho một item của user
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpPut]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> StarAnUserItem(PutScoreModel starModel)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await StarService.UserPutScores(Guid.Parse(myUid), starModel);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }

        #endregion
    }
}
