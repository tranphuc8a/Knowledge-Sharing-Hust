﻿using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.UpdateUserItemModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Infrastructures.Encrypts;
using KnowledgeSharingApi.Repositories.Interfaces;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories;
using KnowledgeSharingApi.Services.Filters;
using KnowledgeSharingApi.Services.Interfaces.Knowledges;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KnowledgeSharingApi.Controllers.Knowledges
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoriesController(
        ICategoryService categoryService
    ) : BaseController
    {
        protected readonly ICategoryService CategoryService = categoryService;

        #region Common Apies

        /// <summary>
        /// Xử lý yêu cầu lấy về danh sách categories
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await CategoryService.GetListCategories(pagination);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu lấy về danh sách categories của một knowledge
        /// </summary>
        /// <param name="knowledgeId"> Id của phần tử kiến thức cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Knowledges/categories/{knowledgeId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOfKnowledge(Guid knowledgeId)
        {
            ServiceResult res = await CategoryService.GetListCategoryOfKnowledge(knowledgeId);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu lấy về cate cụ thể
        /// </summary>
        /// <param name="categoryId"> Id của category </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        [HttpGet("by-id/{categoryId}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid categoryId)
        {
            ServiceResult res = await CategoryService.GetCategory(categoryId);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu lấy về cate cụ thể
        /// </summary>
        /// <param name="categoryName"> Tên của category </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        [HttpGet("{category}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(string category)
        {
            ServiceResult res = await CategoryService.GetCategory(category);
            return StatusCode(res);
        }

        #endregion


        #region Other Apies of user

        /// <summary>
        /// Xử lý yêu cầu thêm mới categories
        /// </summary>
        /// <param name="category"> Tên của category </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        [HttpPost("{category}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> Create(string category)
        {
            ServiceResult res = await CategoryService.AddCategory(category);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu lấy sửa category
        /// </summary>
        /// <param name="categoryId"> Id của category </param>
        /// <param name="category"> Tên mới của category </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        [HttpPatch("{categoryId}/{category}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> Update(Guid categoryId, string category)
        {
            ServiceResult res = await CategoryService.UpdateCategory(categoryId, category);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu lấy xóa category theo id
        /// </summary>
        /// <param name="categoryId"> Id của category </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        [HttpDelete("by-id/{categoryId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> Delete(Guid categoryId)
        {
            ServiceResult res = await CategoryService.DeleteCategory(categoryId);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu xóa category
        /// </summary>
        /// <param name="category"> Tên của category </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        [HttpDelete("{category}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> Delete(string category)
        {
            ServiceResult res = await CategoryService.DeleteCategoryByName(category);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu cập nhật danh sách categories của một knowledge
        /// </summary>
        /// <param name="model"> Thông tin cập nhật danh sách categories </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        [HttpPatch("/api/v1/Knowledges/categories")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> Update([FromBody] UpdateKnowledgeCategoriesModel model)
        {
            ServiceResult res = await CategoryService.UpdateKnowledgeCategories(GetCurrentUserIdStrictly(), model);
            return StatusCode(res);
        }


        #endregion
    }
}
