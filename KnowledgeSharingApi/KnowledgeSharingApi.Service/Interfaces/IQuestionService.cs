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
        Task<ServiceResult> ConfirmQuestion(string myUid, string questionId, bool isConfirm);
    }
}
