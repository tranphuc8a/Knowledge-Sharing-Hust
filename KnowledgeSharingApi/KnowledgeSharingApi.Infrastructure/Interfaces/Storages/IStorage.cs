using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.Storages
{
    public interface IStorage
    {
        /// <summary>
        /// Hàm thực hiện lưu image vào kho lưu trữ và trả về url của file đã lưu
        /// </summary>
        /// <param name="image"> Ảnh cần lưu </param>
        /// <returns> url của ảnh </returns>
        /// Created: PhucTV (5/3/24)
        /// Modified: None
        public Task<string?> SaveImage(IFormFile image);

        /// <summary>
        /// Hàm thực hiện lấy Url của ảnh được lưu trữ khi chỉ biết tên ảnh
        /// </summary>
        /// <param name="filename"> Tên ảnh cần lấy url </param>
        /// <returns> Url của ảnh hoặc null nếu không tìm thấy </returns>
        /// Created: PhucTV (5/3/24)
        /// Modified: None
        public Task<string?> GetImageUrl(string filename);
    }
}
