using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Treks.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
    public class EmailSender : IEmailSender
    {
        private readonly SmtpClient _smtpClient;
        private readonly SmtpSettings _settings;

        public EmailSender(SmtpClient smtpClient, IOptions<SmtpSettings> smtpOptions)
        {
            _smtpClient = smtpClient;
            _settings = smtpOptions.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            if (string.IsNullOrWhiteSpace(_settings.FromEmail))
                throw new InvalidOperationException("SmtpSettings:FromEmail is required.");

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_settings.FromEmail, _settings.FromName),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);

            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
