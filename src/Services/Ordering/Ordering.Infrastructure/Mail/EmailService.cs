using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;

namespace Ordering.Infrastructure.Mail
{
    public class EmailService : IEmailService
    {
        public EmailSettings _emailSettings { get; }
        public ILogger<EmailService> _logger { get; }

        public EmailService(IOptions<EmailSettings> mailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = mailSettings.Value;
            _logger = logger;
        }

        public async Task<bool> SendEmail(Email email)
        {


            try
            {
                var emailObj = new MimeMessage();
                emailObj.Sender = MailboxAddress.Parse(_emailSettings.SMTPServer);
                emailObj.To.Add(MailboxAddress.Parse(email.To));
                emailObj.Subject = email.Subject;
                var builder = new BodyBuilder();
                
                builder.HtmlBody = email.Body;
                emailObj.Body = builder.ToMessageBody();
                
                using var smtp = new SmtpClient();

                if (_emailSettings.Ssl)
                {
                    await smtp.ConnectAsync(_emailSettings.SMTPServer, _emailSettings.Port, SecureSocketOptions.SslOnConnect);
                }
                else 
                {
                    await smtp.ConnectAsync(_emailSettings.SMTPServer, _emailSettings.Port, SecureSocketOptions.StartTls);
                }

                await smtp.AuthenticateAsync(_emailSettings.EmailUsername, _emailSettings.EmailPassword);
                await smtp.SendAsync(emailObj);

                await smtp.DisconnectAsync(true);
                _logger.LogInformation("Email sent.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Email sending failed : {ex.Message}");
                return false;

            }
            
        }
    }
}
