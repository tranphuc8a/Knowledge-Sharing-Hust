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
using KnowledgeSharingApi.Domains.Models.Entities;

namespace KnowledgeSharingApi.Infrastructures.Emails
{
    public class Email : IEmail
    {
        #region Email config attributes
        // Config constant
        protected readonly string SenderName = "Knowledge Sharing";
        protected readonly string ReceiverName = "Client of Knowledge Sharing";
        protected readonly string HostAddress = "smtp.gmail.com";
        protected readonly int HostPort = 587;
        protected readonly bool isEnableSsl = true;

        // Get values from configuration
        protected readonly IConfiguration configuration;
        protected readonly string FromEmail;
        protected readonly string FromName;
        protected readonly string FromPassword;


        // Smtp Sync
        protected readonly SmtpClient SmtpClient;
        protected readonly MailAddress From;

        // Smtp Async
        protected readonly MailKit.Net.Smtp.SmtpClient SmtpClientAsync;
        protected readonly MailboxAddress FromMailboxAddress;
        #endregion

        public Email(IConfiguration _configuration)
        {
            // Configuration
            configuration = _configuration;
            FromEmail = configuration["Email:email"] ?? string.Empty;
            FromName = configuration["Email:name"] ?? SenderName;
            FromPassword = configuration["Email:password"] ?? string.Empty;

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
            FromMailboxAddress = new(FromName, FromEmail);
            SmtpClientAsync = new MailKit.Net.Smtp.SmtpClient();
            SmtpClientAsync.Connect(HostAddress, HostPort, !isEnableSsl);
            SmtpClientAsync.Authenticate(FromEmail, FromPassword);
        }

        public virtual void Send(string toEmail, string subject, string content)
        {
            MailAddress To = new(toEmail ?? string.Empty, ReceiverName);
            using var message = new MailMessage(From, To)
            {
                Subject = subject,
                Body = content,
                IsBodyHtml = true
            };
            SmtpClient.Send(message);
        }

        public virtual async Task SendAsync(string toEmail, string subject, string content)
        {
            MimeMessage message = new(
                from: [FromMailboxAddress],
                to: [new MailboxAddress(ReceiverName, toEmail)],
                subject: subject,
                body: new TextPart("html")
                {
                    Text = content
                }
            );
            await SmtpClientAsync.SendAsync(message);
        }
    }
}
