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
        /// Hàm thực hiện gửi email (đồng bộ và bất đồng bộ) sau khi cấu hình xong các thông tin
        /// </summary>
        /// Created: PhucTV (21/2/24)
        /// Modified: None
        void Send();
        Task SendAsync();


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


        /// <summary>
        /// 4 hàm cấu hình từng thông tin cần gửi email
        /// </summary>
        /// <param name="toEmail"> địa chỉ email nhận thư </param>
        /// <param name="subject"> tiêu đề thư </param>
        /// <param name="content"> nội dung thư </param>
        /// <returns></returns>
        /// Created: PhucTV (21/2/24)
        /// Modified: None
        IEmail SetToEmail(string toEmail);
        IEmail SetSubject(string subject);
        IEmail SetContent(string content);

    }
}
