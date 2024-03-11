using KnowledgeSharingApi.Infrastructures.Interfaces.Emails;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MimeKit;

namespace KnowledgeSharingApi.Infrastructures.Emails
{
    public class Email : BaseEmail
    {
        readonly string MISACUKCUK_CLIENT = "MisaCukcuk Client";
        readonly string HostAddress = "smtp.gmail.com";
        readonly int HostPort = 587;
        readonly bool isEnableSsl = true;

        // Smtp Sync
        protected SmtpClient SmtpClient { get; set; }
        protected MailAddress From { get; set; }
        protected MailAddress? To { get; set; }


        // Smtp Async
        protected MimeMessage Message;
        protected MailKit.Net.Smtp.SmtpClient SmtpClientAsync;

        public Email(IConfiguration _configuration) : base(_configuration)
        {
            // For Smtp Sync
            From = new MailAddress(FromEmail, FromName);
            SmtpClient = new SmtpClient
            {
                Host = HostAddress,
                Port = HostPort,
                EnableSsl = isEnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(FromEmail, FromPassword)
            };

            // For Smtp Async
            Message = new MimeMessage();
            Message.From.Add(new MailboxAddress(FromName, FromEmail));
            SmtpClientAsync = new MailKit.Net.Smtp.SmtpClient();
            SmtpClientAsync.Connect(HostAddress, HostPort, !isEnableSsl);
            SmtpClientAsync.Authenticate(FromEmail, FromPassword);
        }

        public override void Send()
        {
            To = new MailAddress(ToEmail ?? String.Empty, MISACUKCUK_CLIENT);
            using var message = new MailMessage(From, To)
            {
                Subject = Subject,
                Body = Content,
                IsBodyHtml = true
            };
            SmtpClient.Send(message);
        }

        public override async Task SendAsync()
        {
            Message.Subject = Subject;
            Message.To.Add(new MailboxAddress(MISACUKCUK_CLIENT, ToEmail));
            Message.Body = new TextPart("html")
            {
                Text = Content
            };
            await SmtpClientAsync.SendAsync(Message);
        }
    }
}
