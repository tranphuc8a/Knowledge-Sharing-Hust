using KnowledgeSharingApi.Infrastructures.Interfaces.Emails;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Emails
{
    public abstract class BaseEmail : IEmail
    {
        protected readonly string MISACUKCUK_NAME = "MisaCukcuk";
        protected readonly IConfiguration configuration;
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string FromPassword { get; set; }
        public string? ToEmail { get; set; }
        public string? Subject { get; set; }
        public string? Content { get; set; }
        public BaseEmail(IConfiguration _configuration)
        {
            configuration = _configuration;
            FromEmail = configuration["Email:email"] ?? String.Empty;
            FromName = configuration["Email:name"] ?? MISACUKCUK_NAME;
            FromPassword = configuration["Email:password"] ?? String.Empty;
        }

        public abstract void Send();
        public abstract Task SendAsync();

        public virtual void Send(string toEmail, string subject, string content)
        {
            SetToEmail(toEmail).SetSubject(subject).SetContent(content).Send();
        }

        public async Task SendAsync(string toEmail, string subject, string content)
        {
            await SetToEmail(toEmail).SetSubject(subject).SetContent(content).SendAsync();
        }

        public IEmail SetContent(string content)
        {
            Content = content;
            return this;
        }

        public IEmail SetSubject(string subject)
        {
            Subject = subject;
            return this;
        }

        public IEmail SetToEmail(string toEmail)
        {
            ToEmail = toEmail;
            return this;
        }
    }
}
