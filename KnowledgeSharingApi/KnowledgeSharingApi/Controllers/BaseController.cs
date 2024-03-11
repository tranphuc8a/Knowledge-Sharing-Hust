using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories;
using KnowledgeSharingApi.Services.Filters;
using KnowledgeSharingApi.Services.Interfaces;
using KnowledgeSharingApi.Services.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace KnowledgeSharingApi.Controllers
{
    public abstract class BaseController<T>(IResourceFactory resourceFactory) : ControllerBase where T : Entity
    {
        #region Attributes and Controller
        protected readonly IResourceFactory _ResourceFactory = resourceFactory;
        protected readonly IResponseResource _ResponseResoucre = resourceFactory.GetResponseResource();
        protected readonly IEntityResource _EntityResource = resourceFactory.GetEntityResource();
        protected string TableName = typeof(T).Name;
        protected string? ResponseTableName;
        #endregion

        #region Template Methods Steps
        /// <summary>
        /// Lấy về Repository tương ứng
        /// </summary>
        /// <returns> Repository của controller </returns>
        /// Created: PhucTV (8/1/24)
        /// Modified: None
        protected abstract IRepository<T> GetRepository();

        /// <summary>
        /// Lấy về Service tương ứng
        /// </summary>
        /// <returns> Trả về Service của Controller </returns>
        /// Created: PhucTV (8/1/24)
        /// Modified: None
        protected abstract IService<T> GetService();
        #endregion




        #region Create
        /// <summary>
        /// Hàm xử lý yêu cầu thêm mới
        /// </summary>
        /// <param name="entity"> Thông tin entity cần thêm mới </param>
        /// <returns></returns>
        /// Created: PhucTV (8/1/24)
        /// Modified: None
        //[CustomAuthorization(Roles: UserRoles.User)]
        [HttpPost]
        public virtual async Task<IActionResult> Insert([FromBody] T entity)
        {
            ServiceResult res = await GetService().InsertService(entity);
            if (res.IsSuccess)
            {
                if (res.Data != null)
                {
                    if (Guid.TryParse(res.Data.ToString(), out Guid guid))
                    {
                        // Đặt giá trị guid cho trường id của entity
                        PropertyInfo? props = typeof(T).GetProperty($"{TableName}Id");
                        if (props != null)
                        {
                            bool isSameType = props.PropertyType == typeof(Guid) || props.PropertyType == typeof(Guid?);
                            if (isSameType)
                            {
                                props.SetValue(entity, guid);
                            }
                        }
                    }
                }
                string insertSuccess = _ResponseResoucre.InsertSuccess(ResponseTableName);
                return StatusCode((int)EStatusCode.Created, new ApiResponse
                {
                    StatusCode = EStatusCode.Created,
                    UserMessage = insertSuccess,
                    DevMessage = insertSuccess,
                    Body = entity
                });
            }
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }
        #endregion


        #region Read
        /// <summary>
        /// Xử lý truy vấn lấy nhiều entity
        /// </summary>
        /// <param name="limit"> Thuộc tính phân trang - số bản ghi muốn lấy </param>
        /// <param name="offset"> Thuộc tính phân trang - bản ghi ban đầu có độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (8/1/24)
        /// Modified: None
        //[CustomAuthorization(Roles: UserRoles.User)]
        [HttpGet]
        public virtual async Task<IActionResult> Get(int? limit, int? offset)
        {
            ServiceResult res = await GetService().GetService(limit, offset);
            if (res.IsSuccess)
            {
                return Ok(new ApiResponse(res));
            }
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }


        /// <summary>
        /// Hàm xử lý yêu cầu lấy ra một item
        /// </summary>
        /// <param name="id"> id của item cần lấy</param>
        /// <returns></returns>
        /// Created: PhucTV (10/1/24)
        /// Miodified: None
        //[CustomAuthorization(Roles: UserRoles.User)]
        [HttpGet("{id}")]
        public virtual async Task<IActionResult> Get(string id)
        {
            ServiceResult res = await GetService().GetService(id);
            if (res.IsSuccess)
            {
                return Ok(new ApiResponse(res));
            }
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }


        /// <summary>
        /// Xử lý lọc - tìm kiếm thực thể
        /// </summary>
        /// <param name="search"> Từ khóa tìm kiếm </param>
        /// <param name="limit"> Thuộc tính phân trang - Số lượng bản ghi cần lấy </param>
        /// <param name="offset"> Offset của bản ghi đầu tiên </param>
        /// <returns></returns>
        /// Created: PhucTV (10/1/24)
        /// Modified: None
        //[CustomAuthorization(Roles: UserRoles.User)]
        [HttpGet("Filter")]
        public async Task<IActionResult> Filter(string search, int? limit = null, int? offset = null)
        {
            if (String.IsNullOrEmpty(search))
            {
                return await Get(limit, offset);
            }
            ServiceResult res = await GetService().FilterService(search, limit, offset);
            if (res.IsSuccess)
            {
                string msg = _ResponseResoucre.FilterSuccess(ResponseTableName);
                return Ok(new ApiResponse
                {
                    StatusCode = EStatusCode.Success,
                    UserMessage = msg,
                    DevMessage = msg,
                    Body = res.Data
                });
            }
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }


        #endregion


        #region Update
        /// <summary>
        /// Hàm xử lý yêu cầu cập nhật thông tin entity
        /// </summary>
        /// <param name="entityId"> Id của thực thế muốn cập nhật </param>
        /// <param name="entity"> Giá trị mới </param>
        /// <returns></returns>
        /// Created: PhucTV (8/1/24)
        /// Modified: None
        //[CustomAuthorization(Roles: UserRoles.User)]
        [HttpPut("{entityId}")]
        public virtual async Task<IActionResult> Update(string entityId, [FromBody] T entity)
        {
            ServiceResult res = await GetService().UpdateService(entityId, entity);
            if (res.IsSuccess)
            {
                string msg = _ResponseResoucre.UpdateSuccess(ResponseTableName);
                return StatusCode((int)EStatusCode.Created, new ApiResponse
                {
                    StatusCode = EStatusCode.Success,
                    UserMessage = msg,
                    DevMessage = msg,
                    Body = res.RowEffect
                });
            }
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }
        #endregion


        #region Delete
        /// <summary>
        /// Xử lý yêu cầu xóa thực thể
        /// </summary>
        /// <param name="entityId"> Id của thực thể muốn xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (8/1/24)
        /// Modified: None
        //[CustomAuthorization(Roles: UserRoles.User)]
        [HttpDelete("{entityId}")]
        public virtual async Task<IActionResult> Delete(string entityId)
        {
            ServiceResult res = await GetService().DeleteService(entityId);
            if (res.IsSuccess)
            {
                string msg = _ResponseResoucre.DeleteSuccess(ResponseTableName);
                return StatusCode((int)EStatusCode.Success, new ApiResponse
                {
                    StatusCode = EStatusCode.Success,
                    UserMessage = msg,
                    DevMessage = msg,
                    Body = res.RowEffect
                });
            }
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }


        /// <summary>
        /// Xử lý yêu cầu xóa nhiều thực thể
        /// </summary>
        /// <param name="entityIds"> Mảng các id của các thực thể muốn xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (10/1/24)
        /// Modified: None
        //[CustomAuthorization(Roles: UserRoles.User)]
        [HttpDelete("delete-multi")]
        public virtual async Task<IActionResult> Delete([FromBody] string[] entityIds)
        {
            ServiceResult res = await GetService().DeleteService(entityIds);
            if (res.IsSuccess)
            {
                string msg = _ResponseResoucre.DeleteMultiSuccess(ResponseTableName);
                return Ok(new ApiResponse
                {
                    StatusCode = EStatusCode.Success,
                    UserMessage = msg,
                    DevMessage = msg,
                    Body = res.RowEffect
                });
            }
            return StatusCode((int)res.StatusCode, new ApiResponse(res));
        }


        #endregion

    }
}
