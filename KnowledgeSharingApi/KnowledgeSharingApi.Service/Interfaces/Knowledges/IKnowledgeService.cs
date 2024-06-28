using KnowledgeSharingApi.Domains.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Interfaces.Knowledges
{
    public interface IKnowledgeService
    {
        #region Mark apies

        /// <summary>
        /// Đánh dấu mark/unmark một knowledge
        /// </summary>
        /// <param name="myuid"></param>
        /// <param name="knowledgeId"></param>
        /// <param name="isMark"></param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<ServiceResult> Mark(Guid myuid, Guid knowledgeId, bool isMark);

        /// <summary>
        /// Lấy về danh sách user đã mark một knowledge
        /// </summary>
        /// <param name="myuid"> id của người lấy </param>
        /// <param name="knowledgeId"> id của knowledge </param>
        /// <param name="page"> TT phan trang </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<ServiceResult> GetListUserMarkKnowledge(Guid myuid, Guid knowledgeId, PaginationDto page);

        // Nhóm API lấy danh sách knowledge được đánh dấu đã được phân bổ về từng post/course/quesion/lesson
        // IPostService và ICourseService
        #endregion
    }
}
