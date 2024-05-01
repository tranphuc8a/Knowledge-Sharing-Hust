using KnowledgeSharingApi.Domains.Models.ApiRequestModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Infrastructures.Encrypts;
using KnowledgeSharingApi.Services.Filters;
using KnowledgeSharingApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KnowledgeSharingApi.Controllers
{
    [CustomAuthorization(Roles: "User, Admin")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ImagesController(IImageService imageService) : ControllerBase
    {
        protected readonly IImageService ImageService = imageService;

        protected Guid GetCurrentUserId()
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return Guid.Parse(myUid);
        }

        /// <summary>
        /// Yêu càu lấy về danh sách image của một user
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (1/5/24)
        /// Modified: None
        [HttpGet]
        public async Task<IActionResult> GetListImages()
        {
            ServiceResult res = await ImageService.GetListImage(GetCurrentUserId());
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }


        /// <summary>
        /// Yêu càu upload ảnh
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (1/5/24)
        /// Modified: None
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] UploadImageModel model)
        {
            ServiceResult res = await ImageService.UploadImage(GetCurrentUserId(), model);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }


        /// <summary>
        /// Yêu càu xóa ảnh
        /// </summary>
        /// <param name="imageId"> id của ảnh cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (1/5/24)
        /// Modified: None
        [HttpDelete("{imageId}")]
        public async Task<IActionResult> DeleteImage(Guid imageId)
        {
            ServiceResult res = await ImageService.DeleteImage(GetCurrentUserId(), imageId);
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }
    }
}
