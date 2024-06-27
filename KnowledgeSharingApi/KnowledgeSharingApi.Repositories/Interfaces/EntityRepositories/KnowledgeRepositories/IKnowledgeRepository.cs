using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Repositories.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.KnowledgeRepositories
{
    public interface IKnowledgeRepository : IRepository<Knowledge>, IBaseUserItemRepository
    {

        /// <summary>
        /// Lấy về trung bình số sao của một knowledge
        /// </summary>
        /// <param name="knowledgeId"> id của knowledge </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<double?> GetAverageStar(Guid knowledgeId);


        /// <summary>
        /// Lấy về danh sách bình luận của một Phần tử kiển thức
        /// </summary>
        /// <param name="knowledgeId"> id của phần tử kiến thức </param>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<PaginationResponseModel<ViewComment>> GetListComments(Guid knowledgeId, PaginationDto pagination);

        /// <summary>
        /// Lấy về danh sách bình luận của một Phần tử kiển thức
        /// Lấy toàn bộ, không phân trang
        /// </summary>
        /// <param name="knowledgeId"> id của phần tử kiến thức </param>
        /// <returns></returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        Task<List<ViewComment>> GetListComments(Guid knowledgeId);


        /// <summary>
        /// Lấy về danh sách user đã đánh dấu knowledge
        /// </summary>
        /// <param name="knowledgeId"> id của phần tử kiến thức </param>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<PaginationResponseModel<ViewUserProfile>> GetListUserMarkedKnowledge(Guid knowledgeId, PaginationDto pagination);

        /// <summary>
        /// Lấy về phiên mark của user và knowledgeId nếu tồn tại
        /// </summary>
        /// <param name="userId"> id của user </param>
        /// <param name="knowledgeId"> id của knowledge </param>
        /// <returns> null nếu không tìm thấy </returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<Mark?> GetMark(Guid userId, Guid knowledgeId);

        #region Kiểm tra quyền truy cập
        /// <summary>
        /// Kiểm tra xem user có truy cập được vào tài nguyên knowledge không
        /// </summary>
        /// <param name="userId"> id của user cần kiểm tra </param>
        /// <param name="knowledgeId"> id phần tử kiến thức cần kiểm tra </param>
        /// <returns> true - có quyền truy cập, false - không có quyền truy cập </returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        Task<bool> CheckAccessible(Guid userId, Guid knowledgeId);

        /// <summary>
        /// Kiểm tra xem user có truy cập được vào khóa học course không
        /// </summary>
        /// <param name="userId"> id của user cần kiểm tra </param>
        /// <param name="courseId"> id khóa học cần kiểm tra </param>
        /// <returns> true - có quyền truy cập, false - không có quyền truy cập </returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        Task<bool> CheckCourseAccessible(Guid userId, Guid courseId);

        /// <summary>
        /// Kiểm tra xem user có truy cập được bài viết post không
        /// </summary>
        /// <param name="userId"> id của user cần kiểm tra </param>
        /// <param name="postId"> id bài đăng cần kiểm tra </param>
        /// <returns> true - có quyền truy cập, false - không có quyền truy cập </returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        Task<bool> CheckPostAccessible(Guid userId, Guid postId);

        /// <summary>
        /// Kiểm tra xem user có truy cập được vào bài thảo luận question không
        /// </summary>
        /// <param name="userId"> id của user cần kiểm tra </param>
        /// <param name="questionId"> id câu hỏi cần kiểm tra </param>
        /// <returns> true - có quyền truy cập, false - không có quyền truy cập </returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        Task<bool> CheckQuestionAccessible(Guid userId, Guid questionId);

        /// <summary>
        /// Kiểm tra xem user có truy cập được vào bài học lesson không
        /// </summary>
        /// <param name="userId"> id của user cần kiểm tra </param>
        /// <param name="lessonId"> id bài học cần kiểm tra </param>
        /// <returns> true - có quyền truy cập, false - không có quyền truy cập </returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        Task<bool> CheckLessonAccessible(Guid userId, Guid lessonId);
        #endregion
    }
}
