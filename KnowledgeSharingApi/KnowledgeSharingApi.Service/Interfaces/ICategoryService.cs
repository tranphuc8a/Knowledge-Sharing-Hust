using KnowledgeSharingApi.Domains.Models.ApiRequestModels.UpdateUserItemModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Interfaces
{
    public interface ICategoryService
    {
        #region Common get categories
        /// <summary>
        /// Lấy về danh sách categories trong hệ thống
        /// </summary>
        /// <param name="pagination"> TT phan trang </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<ServiceResult> GetListCategories(PaginationDto pagination);

        /// <summary>
        /// Lấy về chi tiết một category theo id
        /// </summary>
        /// <param name="catId"> id của catgory</param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<ServiceResult> GetCategory(Guid catId);

        /// <summary>
        /// Lấy về một category theo tên
        /// </summary>
        /// <param name="catName"> Tên của category</param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<ServiceResult> GetCategory(string catName);

        /// <summary>
        /// Lấy về danh sách categories của một phần tử kiến thức
        /// </summary>
        /// <param name="knowledgeId"> id của phần tử kiến thức</param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<ServiceResult> GetListCategoryOfKnowledge(Guid knowledgeId);
        #endregion

        #region Only user apies

        /// <summary>
        /// Yêu cầu thêm mới một category
        /// </summary>
        /// <param name="catName"> Tên của category </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<ServiceResult> AddCategory(string catName);

        /// <summary>
        /// Yêu cầu chỉnh sửa tên của một category
        /// </summary>
        /// <param name="catId"> Id của category </param>
        /// <param name="catName"> Tên mới của category </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<ServiceResult> UpdateCategory(Guid catId, string catName);

        /// <summary>
        /// Yêu cầu xóa một category theo tên
        /// </summary>
        /// <param name="catName"> Tên của category </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<ServiceResult> DeleteCategoryByName(string catName);

        /// <summary>
        /// Yêu cầu xóa một category theo id
        /// </summary>
        /// <param name="catId"> id của category </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<ServiceResult> DeleteCategory(Guid catId);

        /// <summary>
        /// Yêu cầu cập nhật danh sách category của một phần tử kiển thức
        /// </summary>
        /// <param name="myUid"> Id của người dùng yêu cầu </param>
        /// <param name="model"> Thông tin yêu cầu cập nhật </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<ServiceResult> UpdateKnowledgeCategories(Guid myUid, UpdateKnowledgeCategoriesModel model);

        #endregion



    }
}
