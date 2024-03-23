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
    public interface ILessonRepository : IRepository<Lesson>, IBasePostRepository<ViewLesson>
    {
        /// <summary>
        /// Hàm kiểm tra lesson có tồn tại hay không
        /// CÓ: trả về lesson tương ứng
        /// KHÔNG: throw Not Existed Entity Exception
        /// </summary>
        /// <param name="lessonId"> id của lesson cần kiểm tra </param>
        /// <param name="errorMessage"> message báo lỗi </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        Task<ViewQuestion> CheckExistedLesson(string lessonId, string errorMessage);


        /// <summary>
        /// Lấy về danh sách lesson trong một course
        /// </summary>
        /// <param name="courseid"> id của course cần lấy </param>
        /// <param name="limit"> Số lượng lesson cần lấy </param>
        /// <param name="offset"> Độ lệch bản ghi đầu </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        Task<PaginationResponseModel<ViewLesson>> GetQuestionInCourse(string courseid, int limit, int offset);
    }
}
