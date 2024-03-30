using KnowledgeSharingApi.Domains.Models.ApiRequestModels.AuthenticationModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Interfaces
{
    public interface IVerifyByEmailService
    {
        /// <summary>
        /// Hàm kiểm tra email phải hợp lệ với hành động muốn thực hiện
        /// Throw ValidatorException nếu không hợp lệ
        /// </summary>
        /// <param name="email"> email muốn thực hiện </param>
        /// <returns></returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        public Task CheckEmailIsValid(string? email);

        /// <summary>
        /// Thực hiện verify email, tạo Random code và gửi code tới email
        /// </summary>
        /// <param name="email"> email cần gửi code </param>
        /// <returns></returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        public Task<ServiceResult> SendVerifyCode(string? email);

        /// <summary>
        /// Hàm kiểm tra verify code có hợp lệ không,
        /// giảm 1 lần thử nếu không hợp lệ
        /// </summary>
        /// <param name="codeModel"> thông tin verify code cần kiểm tra </param>
        /// <returns></returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        public Task<ServiceResult> CheckVerifyCode(VerifyCodeModel codeModel);

        /// <summary>
        /// Hàm thực hiện hành động ứng với mục đich của xác minh email
        /// </summary>
        /// <param name="codeModel"> thông tin verify code và thông tin bổ sung </param>
        /// <returns></returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        public Task<ServiceResult> Action(ActiveCodeModel codeModel);
    }
}
