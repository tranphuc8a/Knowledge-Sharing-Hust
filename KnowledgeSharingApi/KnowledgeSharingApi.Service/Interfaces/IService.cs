using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Interfaces
{
    public interface IService<T> where T : Entity
    {
        /// <summary>
        /// Dịch vụ lấy về dữ liệu theo trường id
        /// </summary>
        /// <param name="id"> id của đối tượng cần lấy dữ liệu </param>
        /// <returns></returns>
        /// Created: PhucTV (27/12/23)
        /// Modified: None
        Task<ServiceResult> GetService(string id);

        /// <summary>
        /// Dịch vụ lấy về tất cả bản ghi 
        /// </summary>
        /// <param name="limit"> thuộc tính phân trang - số lượng bản ghi cần lấy</param>
        /// <param name="offset"> thuộc tính phân trang - độ lệch bản ghi đầu tiên</param>
        /// <default> Mặc định lấy tất cả bản ghi </default>
        /// <returns></returns>
        /// Created: PhucTV (27/12/23)
        /// Modified: None
        Task<ServiceResult> GetService(int? limit = null, int? offset = null);



        /// <summary>
        /// Dịch vụ lấy về danh sách dữ liệu theo danh sách id
        /// </summary>
        /// <param name="ids"> mảng id của các đối tượng cần lấy dữ liệu </param>
        /// <returns></returns>
        /// Created: PhucTV (24/1/24)
        /// Modified: None
        Task<ServiceResult> GetService(string[] ids);

        /// <summary>
        /// Dịch vụ thêm mới một bản ghi
        /// </summary>
        /// <param name="entity"> Giá trị cần thêm mới </param>
        /// <returns></returns>
        /// Created: PhucTV (27/12/23)
        /// Modified: None
        Task<ServiceResult> InsertService(T entity);

        /// <summary>
        /// Dịch vụ cập nhật một bàn ghi
        /// </summary>
        /// <param name="id"> id của đối tượng cần cập nhật </param>
        /// <param name="entity"> giá trị cần cập nhật </param>
        /// <returns></returns>
        /// Created: PhucTV (27/12/23)
        /// Modified: None
        Task<ServiceResult> UpdateService(string id, T entity);

        /// <summary>
        /// Dịch vụ xóa một bản ghi
        /// </summary>
        /// <param name="id"> id của bàn ghi cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (27/12/23)
        /// Modified: None
        Task<ServiceResult> DeleteService(string id);

        /// <summary>
        /// Dịch vụ xóa nhiều bản ghi
        /// </summary>
        /// <param name="ids"> Mảng các id cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (8/1/24)
        /// Modififed: None
        Task<ServiceResult> DeleteService(string[] ids);



        /// <summary>
        /// Dịch vụ lọc danh sách thực thể theo từ khóa tìm kiếm
        /// </summary>
        /// <param name="search"> từ khóa tìm kiếm </param>
        /// <param name="limit"> thuộc tính phân trang - số lượng bản ghi cần lấy </param>
        /// <param name="offset"> thuộc tính phân trang - độ lệch bản ghi đầu tiên </param>
        /// <returns></returns>
        /// Created: PhucTV (27/12/23)
        /// Modified: PhucTV (10/1/24) Move up to IBaseService
        Task<ServiceResult> FilterService(string search, int? limit = null, int? offset = null);

    }
}
