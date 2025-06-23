using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace LevelUp.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendRegistrationEmailAsync(string toEmail, string username)
        {
            string subject = "Chào mừng bạn đến với LevelUp!";
            string body = $@"
                <h2>Xin chào {username},</h2>
                <p>Cảm ơn bạn đã đăng ký tài khoản tại <strong>LevelUp</strong>.</p>
                <p>Chúng tôi rất vui khi có bạn đồng hành!</p>
                <hr />
                <p>Trân trọng,<br/>Đội ngũ LevelUp</p>";

            await SendEmailAsync(toEmail, subject, body);
        }

        private async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpHost = _configuration["EmailSettings:SmtpHost"] ?? throw new InvalidOperationException("EmailSettings:SmtpHost not configured.");

            if (!int.TryParse(_configuration["EmailSettings:SmtpPort"], out int smtpPort))
            {
                smtpPort = 587;
                Console.WriteLine("Warning: EmailSettings:SmtpPort not found or invalid. Using default port 587.");
            }

            var senderEmail = _configuration["EmailSettings:SenderEmail"] ?? throw new InvalidOperationException("EmailSettings:SenderEmail not configured.");
            var senderPassword = _configuration["EmailSettings:SenderPassword"] ?? throw new InvalidOperationException("EmailSettings:SenderPassword not configured.");

            if (!bool.TryParse(_configuration["EmailSettings:EnableSsl"], out bool enableSsl))
            {
                enableSsl = true;
                Console.WriteLine("Warning: EmailSettings:EnableSsl not found or invalid. Using default value true.");
            }

            var senderName = _configuration["EmailSettings:SenderName"] ?? "Your Application Name"; // Giá trị mặc định cho SenderName

            var smtpClient = new SmtpClient(smtpHost)
            {
                Port = smtpPort,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = enableSsl
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail, senderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
