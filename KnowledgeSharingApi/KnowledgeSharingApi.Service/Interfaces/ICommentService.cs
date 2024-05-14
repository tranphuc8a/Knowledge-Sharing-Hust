using KnowledgeSharingApi.Domains.Models.ApiRequestModels.CreateUserItemModels;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.UpdateUserItemModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Interfaces
{
    public interface ICommentService
    {
        #region Anonymous Apies

        /// <summary>
        /// Anonymous Lấy về danh sách comment của một một knowledge
        /// </summary>
        /// <param name="knowledgeId"> id của knowledge </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<ServiceResult> AnonymousGetListKnowledgeComments(Guid knowledgeId, int? limit, int? offset);

        /// <summary>
        /// Lấy về danh sách comment đã reply một comment khác
        /// </summary>
        /// <param name="myUid"> id cua user can lay</param>
        /// <param name="commentId"> id của comment </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: PhucTV (13/5/24)
        Task<ServiceResult> GetListCommentReplies(Guid? myUid, Guid commentId, int? limit, int? offset);

        /// <summary>
        /// Lấy về chi tiết một comment
        /// </summary>
        /// <param name="myUid"> id cua user can lay </param>
        /// <param name="commentId"> id của comment </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<ServiceResult> GetComment(Guid? myUid, Guid commentId);

        #endregion


        #region Admin Apies

        /// <summary>
        /// Admin Lấy về danh sách comment của một một knowledge
        /// </summary>
        /// <param name="knowledgeId"> id của knowledge </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<ServiceResult> AdminGetListKnowledgeComments(Guid knowledgeId, int? limit, int? offset);

        /// <summary>
        /// Admin xóa một comment
        /// </summary>
        /// <param name="commentId"> id của comment </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<ServiceResult> AdminDeleteComment(Guid commentId);

        /// <summary>
        /// Admin khóa/mở khóa bình luận cho một knowledge
        /// Chức năng này vẫn đang phát triển... (cần điều chỉnh db và các api liên quan)
        /// </summary>
        /// <param name="isBlock"> true - khóa, false - mở khóa </param>
        /// <param name="knowledgeId"> id của knowledge </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<ServiceResult> AdminBlockCommentOfKnowledge(Guid knowledgeId, bool isBlock);


        #endregion


        #region User Get Comments Apies

        /// <summary>
        /// Anonymous Lấy về danh sách comment của một một knowledge
        /// </summary>
        /// <param name="myuid"> id của người lấy </param>
        /// <param name="knowledgeId"> id của knowledge </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetListKnowledgeComments(Guid myuid, Guid knowledgeId, int? limit, int? offset);

        /// <summary>
        /// User lấy về danh sách bình luận của mình trong một knowledge
        /// </summary>
        /// <param name="myuid"> id của người lấy </param>
        /// <param name="knowledgeId"> id của knowledge </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetMyCommentsOfKnowledge(Guid myuid, Guid knowledgeId, int? limit, int? offset);

        /// <summary>
        /// User lấy về danh sách bình luận của một user khác trong một knowledge
        /// </summary>
        /// <param name="myuid"> id của người đi lấy </param>
        /// <param name="myuid"> id của người được lấy </param>
        /// <param name="knowledgeId"> id của knowledge </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetUserCommentsOfKnowledge(Guid myuid, Guid userId, Guid knowledgeId, int? limit, int? offset);

        /// <summary>
        /// User tìm kiếm bình luận của một knowledge theo từ khóa 
        /// </summary>
        /// <param name="myuid"> id của người lấy </param>
        /// <param name="knowledgeId"> id của knowledge </param>
        /// <param name="search"> Từ khóa tìm kiếm </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<ServiceResult> UserSearchCommentsOfKnowledge(Guid myuid, Guid knowledgeId, string search, int? limit, int? offset, List<(string Field, bool IsAscending)>? orders);
        #endregion


        #region User operate with comments 

        /// <summary>
        /// User thêm bình luận vào một knowledge
        /// </summary>
        /// <param name="myuid"> id của người thực hiện </param>
        /// <param name="commentModel"> nội dung bình luận được thêm </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<ServiceResult> UserAddComment(Guid myuid, CreateCommentModel commentModel);

        /// <summary>
        /// User reply một bình luận
        /// </summary>
        /// <param name="myuid"> id của người thực hiện </param>
        /// <param name="replyModel"> nội dung bình luận phản hồi </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<ServiceResult> UserReplyComment(Guid myuid, ReplyCommentModel replyModel);

        /// <summary>
        /// User chỉnh sửa một bình luận
        /// </summary>
        /// <param name="myuid"> id của người chỉnh sửa </param>
        /// <param name="commentId"> Id của comment cần update </param>
        /// <param name="commentModel"> Nội dung bình luận chỉnh sửa </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<ServiceResult> UserUpdateComment(Guid myuid, Guid commentId, UpdateCommentModel commentModel);

        /// <summary>
        /// User xóa bình luận của mình hoặc bình luận khác trong bài đăng của mình
        /// Lưu ý cập nhật những comment khác đang reply comment của mình về reply null
        /// </summary>
        /// <param name="myuid"> id của người lấy </param>
        /// <param name="commentId"> id của comment cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<ServiceResult> UserDeleteComment(Guid myuid, Guid commentId);


        /// <summary>
        /// User bật/tắt chức năng khóa bình luận cho knowledge của mình (đang phát triển...)
        /// </summary>
        /// <param name="myuid"> id của người thực hiện </param>
        /// <param name="knowledgeId"> id của knowledge </param>
        /// <param name="isBlock"> true - khóa, false - mở khóa </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<ServiceResult> UserBlockKnowledgeComments(Guid myuid, Guid knowledgeId, bool isBlock);


        #endregion
    }
}
