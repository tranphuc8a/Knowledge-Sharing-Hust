using KnowledgeSharingApi.Domains.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.Repositories
{
    public interface IRepository<T> where T : Entity
    {
        IDbConnection Connection { get; set; }
        IDbTransaction? Transaction { get; set; }


        /// <summary>
        /// Hàm thực hiện lấy tất cả bản ghi của bảng trong CSDL
        /// </summary>
        /// <returns>Danh sách bản ghi được lấy</returns>
        /// Created: PhucTV (26/12/23)
        /// Modified: None
        Task<IEnumerable<T>> Get();


        /// <summary>
        /// Hàm thực hiện lấy tất cả bản ghi của bảng trong CSDL có phân trang
        /// </summary>
        /// <param name="limit"> Số lượng bản ghi cần lấy </param>
        /// <param name="offset"> Độ lệch bản ghi đầu tiên </param>
        /// <returns> Danh sách bản ghi </returns>
        /// Created: PhucTV (26/12/23)
        /// Modified: None
        Task<IEnumerable<T>> Get(int limit, int offset);



        /// <summary>
        /// Thực hiện lấy một bản ghi trong CSDL dựa vào trường ID
        /// </summary>
        /// <param name="id"> id của bản ghi cần lấy </param>
        /// <returns> Bản ghi cần lấy </returns>
        /// Created: PhucTV (5/1/24)
        /// Modified: None
        Task<T?> Get(string id);

        /**
         * Hàm thực hiện thêm mới một bản ghi
         * Params {*} entity - bản ghi cần thêm mới
         * Returns: Số lượng bản ghi bị ảnh hưởng
         * Created: PhucTV (26/12/23)
         * Modified: None
         */
        Task<string?> Insert(T entity);


        /// <summary>
        /// Thực hiện cập nhật thông tin bản ghi
        /// </summary>
        /// <param name="entity"> Thông tin cần cập nhật </param>
        /// <param name="id"> id của bản ghi cần cập nhật </param>
        /// <returns> Số lượng bản ghi bị ảnh hưởng </returns>
        /// Created: PhucTV (5/1/24)
        /// Modified: None
        Task<int> Update(string id, T entity);


        /// <summary>
        /// Thực hiện xóa một bản ghi dựa vào id
        /// </summary>
        /// <param name="id">Id của bản ghi cần xóa</param>
        /// <returns> số bản ghi bị xóa </returns>
        /// Created: PhucTV (3/1/24)
        /// Modified: None
        Task<int> Delete(string id);


        /// <summary>
        /// Thực hiện xóa nhiều bản ghi dựa vào trường ids
        /// </summary>
        /// <param name="ids"> Mảng các id cần xóa </param>
        /// <returns> Số lượng bản ghi bị xóa </returns>
        /// Created: PhucTV (8/1/24)
        /// Modified: None
        Task<int> Delete(string[] ids);


        /// <summary>
        /// Thực hiện lọc ra những thực thể match với từ khóa
        /// </summary>
        /// <param name="text"> từ khóa cần lọc </param>
        /// <returns> Danh sách thực thể lọc được </returns>
        /// Created: PhucTV (5/1/24)
        /// Modified: PhucTV (10/1/24) move up to IBaseRepo
        Task<IEnumerable<T>> Filter(string text);


        /// <summary>
        /// Thực hiện lọc ra những thực thể bởi từ khóa
        /// Có phân trang
        /// </summary>
        /// <param name="text"> Từ khóa cần lọc </param>
        /// <param name="limit"> Thuộc tính phân trang - số bản ghi cần lấy </param>
        /// <param name="offset"> Thuộc tính phân trang - độ lệch bản ghi ban đấu </param>
        /// <returns> Danh sách bản ghi </returns>
        /// Created: PhucTV (5/1/24)
        /// Modified: PhucTV (10/1/24) move up to IBaseRepo
        Task<IEnumerable<T>> Filter(string text, int limit, int offset);


        /// <summary>
        /// Đăng ký transaction
        /// </summary>
        /// <param name="transaction"> transaction cần đăng ký </param>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        public void RegisterTransaction(IDbTransaction transaction);
    }
}
