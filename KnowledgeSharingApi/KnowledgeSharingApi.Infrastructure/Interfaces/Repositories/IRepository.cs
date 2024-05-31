using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.Repositories
{
    public interface IRepository<T> where T : Entity
    {
        IDbConnection Connection { get; set; }
        IDbTransaction? Transaction { get; set; }


        /// <summary>
        /// Kiểm tra xem entity có tồn tại không
        /// Trả về entity nếu tồn tại, bắn ra NotExistedEntityException nếu không tồn tại
        /// </summary>
        /// <param name="entityId"> Id của entity cần kiểm tra </param>
        /// <returns> Entity cần lấy </returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<T> CheckExisted(Guid entityId, string errorMessage);

        /// <summary>
        /// Hàm thực hiện lấy tất cả bản ghi của bảng trong CSDL
        /// </summary>
        /// <returns>Danh sách bản ghi được lấy</returns>
        /// Created: PhucTV (26/12/23)
        /// Modified: None
        Task<List<T>> Get();


        /// <summary>
        /// Hàm thực hiện lấy tất cả bản ghi của bảng trong CSDL có phân trang
        /// </summary>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <returns> Danh sách bản ghi </returns>
        /// Created: PhucTV (26/12/23)
        /// Modified: None
        Task<PaginationResponseModel<T>> Get(PaginationDto pagination);



        /// <summary>
        /// Thực hiện lấy một bản ghi trong CSDL dựa vào trường ID
        /// </summary>
        /// <param name="id"> id của bản ghi cần lấy </param>
        /// <returns> Bản ghi cần lấy </returns>
        /// Created: PhucTV (5/1/24)
        /// Modified: None
        Task<T?> Get(Guid id);
        

        /// <summary>
        /// Thực hiện lấy danh sách phần tử theo mảng id
        /// </summary>
        /// <param name="ids"> danh sách id của những bản ghi cần lấy </param>
        /// <returns> Bản ghi cần lấy </returns>
        /// Created: PhucTV (5/1/24)
        /// Modified: None
        Task<List<T?>> Get(Guid[] ids);

        /// <summary>
        /// Hàm thực hiện thêm mới một bản ghi
        /// Trường id tự động cài set
        /// </summary>
        /// <param name="entity"> bản ghi cần thêm mới </param>
        /// <returns> id của bản ghi đã thêm </returns>
        /// Created: PhucTV (26/12/23)
        /// Modified: None
        Task<Guid?> Insert(T entity);


        /// <summary>
        /// Hàm thực hiện thêm mới một bản ghi
        /// Trường id được custom từ tham số id
        /// </summary>
        /// <param name="id"> id của bản ghi </param>
        /// <param name="entity"> bản ghi cần thêm mới </param>
        /// <returns> id của bản ghi đã thêm </returns>
        /// Created: PhucTV (26/12/23)
        /// Modified: None
        Task<Guid?> Insert(Guid id, T entity);


        /// <summary>
        /// Thực hiện cập nhật thông tin bản ghi
        /// </summary>
        /// <param name="entity"> Thông tin cần cập nhật </param>
        /// <param name="id"> id của bản ghi cần cập nhật </param>
        /// <returns> Số lượng bản ghi bị ảnh hưởng </returns>
        /// Created: PhucTV (5/1/24)
        /// Modified: None
        Task<int> Update(Guid id, T entity);


        /// <summary>
        /// Thực hiện xóa một bản ghi dựa vào id
        /// </summary>
        /// <param name="id">Id của bản ghi cần xóa</param>
        /// <returns> số bản ghi bị xóa </returns>
        /// Created: PhucTV (3/1/24)
        /// Modified: None
        Task<int> Delete(Guid id);


        /// <summary>
        /// Thực hiện xóa nhiều bản ghi dựa vào trường ids
        /// </summary>
        /// <param name="ids"> Mảng các id cần xóa </param>
        /// <returns> Số lượng bản ghi bị xóa </returns>
        /// Created: PhucTV (8/1/24)
        /// Modified: None
        Task<int> Delete(Guid[] ids);


        /// <summary>
        /// Thực hiện lọc ra những thực thể match với từ khóa
        /// </summary>
        /// <param name="text"> từ khóa cần lọc </param>
        /// <returns> Danh sách thực thể lọc được </returns>
        /// Created: PhucTV (5/1/24)
        /// Modified: PhucTV (10/1/24) move up to IBaseRepo
        Task<List<T>> Filter(string text);


        /// <summary>
        /// Thực hiện lọc ra những thực thể bởi từ khóa
        /// Có phân trang
        /// </summary>
        /// <param name="text"> Từ khóa cần lọc </param>
        /// <param name="pagination"> Thuộc tính phân trang </param>
        /// <returns> Danh sách bản ghi </returns>
        /// Created: PhucTV (5/1/24)
        /// Modified: PhucTV (10/1/24) move up to IBaseRepo
        Task<PaginationResponseModel<T>> Filter(string text, PaginationDto pagination);


        /// <summary>
        /// Đăng ký transaction
        /// </summary>
        /// <param name="transaction"> transaction cần đăng ký </param>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        public void RegisterTransaction(IDbTransaction transaction);

        /// <summary>
        /// Sap xep danh sach theo orders
        /// </summary>
        /// <param name="beforeList"> danh sach truoc khi sap xep </param>
        /// <param name="orders"> Dieu kien sap xep </param>
        /// <returns></returns>
        /// Created: PhucTV (14/5/24)
        /// Modified: None
        List<T> ApplyOrder(List<T> beforeList, List<OrderDto> orders);
        List<Q> ApplyOrder<Q>(List<Q> beforeList, List<OrderDto> orders);

        /// <summary>
        /// Ap dung filter cho mot list
        /// </summary>
        /// <param name="beforeList"> list truoc khi filter </param>
        /// <param name="filters"> list cac filter </param>
        /// <returns></returns>
        /// Created: PhucTV (16/5/24)
        /// Modified: None
        List<T> ApplyFilter(List<T> beforeList, List<FilterDto> filters);
        List<T> ApplyFilter(List<T> beforeList, FilterDto filters);
        List<Q> ApplyFilter<Q>(List<Q> beforeList, List<FilterDto> filter);
        List<Q> ApplyFilter<Q>(List<Q> beforeList, FilterDto filter);


        /// <summary>
        /// Ap dung Limit offset
        /// </summary>
        /// <param name="beforeList"> Danh sach truoc khi ap dung</param>
        /// <param name="limit"> Gia tri so luong ban ghi </param>
        /// <param name="offset"> Gia tri do lech ban ghi </param>
        /// <returns></returns>
        /// Created: PhucTV (16/5/24)
        /// Modified: None
        List<T> ApplyLimitOffset(List<T> beforeList, int? limit, int? offset);
        List<Q> ApplyLimitOffset<Q>(List<Q> beforeList, int? limit, int? offset);


        /// <summary>
        /// Ap dung pagination
        /// </summary>
        /// <param name="beforeList"> Danh sach truoc khi ap dung</param>
        /// <param name="pagination"> Doi tuong pagination</param>
        /// <returns></returns>
        /// Created: PhucTV (16/5/24)
        /// Modified: None
        List<T> ApplyPagination(List<T> beforeList, PaginationDto? pagination);
        List<Q> ApplyPagination<Q>(List<Q> beforeList, PaginationDto? pagination);
    }

}
