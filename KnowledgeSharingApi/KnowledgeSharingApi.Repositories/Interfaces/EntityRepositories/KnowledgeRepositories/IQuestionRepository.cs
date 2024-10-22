﻿using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
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
    public interface IQuestionRepository : IRepository<Question>, IBasePostRepository<ViewQuestion>
    {
        /// <summary>
        /// Hàm kiểm tra question có tồn tại hay không
        /// CÓ: trả về question tương ứng
        /// KHÔNG: throw Not Existed Entity Exception
        /// </summary>
        /// <param name="questionId"> id của question cần kiểm tra </param>
        /// <param name="errorMessage"> message báo lỗi </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        Task<ViewQuestion> CheckExistedQuestion(Guid questionId, string errorMessage);


        /// <summary>
        /// Lấy về danh sách question trong một course
        /// </summary>
        /// <param name="courseid"> id của course cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        Task<List<ViewQuestion>> GetQuestionInCourse(Guid courseid);


        /// <summary>
        /// Lấy về thông tin chi tiết một bài thảo luận
        /// </summary>
        /// <param name="questionId"> id của bài thảo luận cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        Task<ViewQuestion?> GetQuestionDetail(Guid questionId);
    }
}
