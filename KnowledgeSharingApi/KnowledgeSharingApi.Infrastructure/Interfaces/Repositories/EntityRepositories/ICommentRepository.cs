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
    public interface ICommentRepository : IRepository<Comment>
    {
        /// <summary>
        /// Kiểm tra xem comment có tồn tại không
        /// Trả về comment nếu tồn tại, bắn ra NotExistedEntityException nếu không tồn tại
        /// </summary>
        /// <param name="commentId"> Id của comment cần kiểm tra </param>
        /// <returns> Comment cần lấy </returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        Task<ViewComment> CheckExistedComment(Guid commentId, string errorMessage);


        /// <summary>
        /// Lấy về danh sách phản hồi của một comment
        /// </summary>
        /// <param name="commentId"> id của comment cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        Task<PaginationResponseModel<ViewComment>> GetRepliesOfComment(Guid commentId, int limit, int offset);

        /// <summary>
        /// Lấy về danh sách top phản hồi của một danh sách comment
        /// </summary>
        /// <param name="commentId"> id của danh sách comment cần lấy </param>
        /// <param name="limit"> giới hạn số lượng cần lấy cho mỗi comment </param>
        /// <returns></returns>
        /// Created: PhucTV (4/5/24)
        /// Modified: None
        Task<Dictionary<Guid, PaginationResponseModel<ViewComment>?>> GetRepliesOfComment(IEnumerable<Guid> commentId, int limit);

        /// <summary>
        /// Lấy về danh sách bình luận của một Phần tử kiển thức
        /// </summary>
        /// <param name="knowledgeId"> id của phần tử kiến thức </param>
        /// <param name="limit"> Số lượng bình luận muốn lấy </param>
        /// <param name="offset"> Độ lệch bản ghi </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<PaginationResponseModel<ViewComment>> GetListCommentsOfKnowledge(Guid knowledgeId, int limit, int offset);

        /// <summary>
        /// Lấy về danh sách bình luận của danh sách Phần tử kiển thức
        /// </summary>
        /// <param name="knowledgeId"> danh sách id của phần tử kiến thức </param>
        /// <param name="limit"> Số lượng bình luận muốn lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (4/5/24)
        /// Modified: None
        Task<Dictionary<Guid, PaginationResponseModel<ViewComment>?>> GetListCommentsOfKnowledge(IEnumerable<Guid> knowledgeId, int limit);

        /// <summary>
        /// Lấy về tổng số phản hồi của danh sách comment
        /// </summary>
        /// <param name="commentsId"> danh sách id của các comment cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        Task<Dictionary<Guid, int>> GetTotalReplies(List<Guid> commentsId);


        /// <summary>
        /// Lấy về danh sách bình luận của user trong một knowledge
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <param name="knowledgeId"> id của knowledge cần lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        Task<PaginationResponseModel<ViewComment>> GetCommentsOfUserInKnowledge(Guid userId, Guid knowledgeId, int limit, int offset);

       
    }
}
