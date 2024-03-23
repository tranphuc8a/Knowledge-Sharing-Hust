using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories
{
    public interface IKnowledgeRepository : IRepository<Knowledge>
    {

        /// <summary>
        /// Lấy về trung bình số sao của một knowledge
        /// </summary>
        /// <param name="knowledgeId"> id của knowledge </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<double?> GetAverageStar(string knowledgeId);


        /// <summary>
        /// Lấy về danh sách bình luận của một Phần tử kiển thức
        /// </summary>
        /// <param name="knowledgeId"> id của phần tử kiến thức </param>
        /// <param name="limit"> Số lượng bình luận muốn lấy </param>
        /// <param name="offset"> Độ lệch bản ghi </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<PaginationResponseModel<ViewComment>> GetListComments(string knowledgeId, int limit, int offset);
    }
}
