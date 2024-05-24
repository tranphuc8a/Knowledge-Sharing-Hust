using KnowledgeSharingApi.Domains.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Interfaces
{
    public interface IQuestionService : IBasePostService
    {
        /// <summary>
        /// Chủ câu hỏi đánh dấu câu hỏi đã được giải quyết hay chưa
        /// </summary>
        /// <param name="myUid"> id của chủ câu hỏi </param>
        /// <param name="questionId"> id của câu hỏi </param>
        /// <param name="isConfirm"> true - đã được giải quyết xong, false - chưa được giải quyết </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        Task<ServiceResult> ConfirmQuestion(Guid myUid, Guid questionId, bool isConfirm);

        /// <summary>
        /// User tim kiem bai thao luan trong khoa hoc
        /// </summary>
        /// <param name="myUid"> id cua user thuc hien  </param>
        /// <param name="courseId"> id cua khoa hoc can lay </param>
        /// <param name="search"> Tu khoa tim kiem </param>
        /// <param name="pagination"> thuoc tinh phan trang </param>
        /// <returns></returns>
        /// Created PhucTV (23/5/24)
        /// Modified None
        Task<ServiceResult> UserSearchQuestionOfCourse(Guid myUid, Guid courseId, string? search, PaginationDto pagination);
    }
}
