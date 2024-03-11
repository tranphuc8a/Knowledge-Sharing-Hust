using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.Caches
{
    public interface ICache
    {
        /// <summary>
        /// Thực hiện trả về singleton của nhóm lớp
        /// Chuyển việc dùng biến static về dùng biến member của object
        /// Mục đích là chuyển bộ nhớ heap sang bộ nhớ stack
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (26/1/24)
        /// Modified: None
        public ICache GetInstance();

        /// <summary>
        /// Thực hiện chèn một cặp key-val vào cache
        /// </summary>
        /// <param name="key"> Khóa </param>
        /// <param name="value"> Dữ liệu </param>
        /// Created: PhucTV (26/1/24)
        /// Modified: None
        public void Set(string key, object value);

        /// <summary>
        /// Thực hiện Lấy giá trị theo key chỉ định cụ thể kiểu trả về
        /// </summary>
        /// <param name="key"> Khóa của dữ liệu cần lấy </param>
        /// <returns> object nếu có và null nếu không </returns>
        /// Created: PhucTV (26/1/24)
        /// Modified: None
        public T? Get<T>(string key);


        /// <summary>
        /// Thực hiện chèn một cặp key-val vào cache chỉ định cụ thể kiểu
        /// </summary>
        /// <param name="key"> Khóa </param>
        /// <param name="value"> Dữ liệu </param>
        /// Created: PhucTV (26/1/24)
        /// Modified: None
        public void Set<T>(string key, T value);

        /// <summary>
        /// Thực hiện Lấy giá trị theo key
        /// </summary>
        /// <param name="key"> Khóa của dữ liệu cần lấy </param>
        /// <returns> object nếu có và null nếu không </returns>
        /// Created: PhucTV (26/1/24)
        /// Modified: None
        public object? Get(string key);

        /// <summary>
        /// Thực hiện kiểm tra có tồn tại key trong cache không
        /// </summary>
        /// <param name="key"> Khóa cần kiểm tra </param>
        /// <returns> true - có tồn tại, false - không tồn tại </returns>
        /// Created: PhucTV (26/1/24)
        /// Modified: None
        public bool Contains(string key);

        /// <summary>
        /// Thực hiện xóa cache với key
        /// </summary>
        /// <param name="key"> Khóa cần xóa </param>
        /// Created: PhucTV (21/2/24)
        /// Modified: None
        public void Remove(string key);
    }
}
