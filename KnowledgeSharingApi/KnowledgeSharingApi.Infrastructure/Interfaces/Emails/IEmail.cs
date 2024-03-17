using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.Emails
{
    public interface IEmail
    { 
        /// <summary>
        /// Hàm thực hiện gửi email (đồng bộ và bất đồng bộ) với đầy đủ thông tin
        /// </summary>
        /// <param name="toEmail"> địa chỉ email nhận thư </param>
        /// <param name="subject"> tiêu đề thư </param>
        /// <param name="content"> nội dung thư </param>
        /// Created: PhucTV (21/2/24)
        /// Modified: None
        void Send(string toEmail, string subject, string content);
        Task SendAsync(string toEmail, string subject, string content);
    }
}
