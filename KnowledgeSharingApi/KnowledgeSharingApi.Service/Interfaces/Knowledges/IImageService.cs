using KnowledgeSharingApi.Domains.Models.ApiRequestModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Interfaces.Knowledges
{
    public interface IImageService
    {
        /// <summary>
        /// Thực hiện lấy về danh sách các image của một user
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (1/5/24)
        /// Modified: None
        Task<ServiceResult> GetListImage(Guid userId);


        /// <summary>
        /// Thực hiện upload một image
        /// </summary>
        /// <param name="myUid"> id user thực hiện upload </param>
        /// <param name="image"> Nội dung ảnh upload </param>
        /// <returns></returns>
        /// Created: PhucTV (1/5/24)
        /// Modified: None
        Task<ServiceResult> UploadImage(Guid myUid, UploadImageModel image);


        /// <summary>
        /// Thực hiện xóa ảnh
        /// </summary>
        /// <param name="myUid"> id user thực hiện</param>
        /// <param name="imageId"> id image cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (1/5/24)
        /// Modified: None
        Task<ServiceResult> DeleteImage(Guid myUid, Guid imageId);
    }
}
