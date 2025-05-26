using System.Net;
using System.Net.Mail;
using _8bitstore_be.Interfaces;

namespace _8bitstore_be.Services
{
    public class EmailService: IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string body, string subject)
        {
            var smtpHost = _configuration["EmailSetting:SmtpHost"];
            var smtpPort = int.Parse(_configuration["EmailSetting:SmtpPort"]);
            var smtpUser = _configuration["EmailSetting:SmtpUser"];
            var smtpPass = _configuration["EmailSetting:SmtpPass"];
            var fromEmail = _configuration["EmailSetting:FromEmail"];

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Timeout = 10000
            };

            var message = new MailMessage(fromEmail, toEmail, subject, body)
            {
                IsBodyHtml = true,
            };

            await client.SendMailAsync(message);
        }
    }
}
