using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        /// <summary>
        /// Lấy về category theo tên
        /// </summary>
        /// <param name="catName"> tên của category </param>
        /// <returns> Cate | null - không tìm thấy </returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<Category?> GetByName(string catName);

        /// <summary>
        /// Lấy về danh sách knowledge của một category
        /// </summary>
        /// <param name="catId"> id của category </param>
        /// <param name="catName"> name của category </param>
        /// <returns> Danh sách view knowledge cate </returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<IEnumerable<ViewKnowledgeCategory>> GetKnowledgesByCategory(Guid catId);
        Task<IEnumerable<ViewKnowledgeCategory>> GetKnowledgesByCategory(string catName);

        /// <summary>
        /// Lấy về danh sách categories của một knowledge
        /// </summary>
        /// <param name="knowledgeId"> id của knowledge </param>
        /// <returns> Danh sách cate </returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<IEnumerable<Category>> GetByKnowledgeId(Guid knowledgeId);

        /// <summary>
        /// Lấy về danh sách categories của một danh sách knowledge
        /// </summary>
        /// <param name="knowledgeIds"> danh sách id của knowledge </param>
        /// <returns> Danh sách cate </returns>
        /// Created: PhucTV (4/5/24)
        /// Modified: None
        Task<Dictionary<Guid, IEnumerable<Category>?>> GetByKnowledgeId(IEnumerable<Guid> knowledgeIds);

        /// <summary>
        /// Cập nhật danh sách category của một knowledge
        /// </summary>
        /// <param name="knowledgeId"> id của knowledge cần cập nhật </param>
        /// <param name="catNames"> Danh sách catname mới của knowledge </param>
        /// <returns> Số bản ghi ảnh hưởng </returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<int> UpdateKnowledgeCategories(Guid knowledgeId, List<string> catNames);
    }
}
