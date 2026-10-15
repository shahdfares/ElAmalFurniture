using ElAmal.Core.Contract.Services.Mail;
using ElAmal.Core.Entities.Mail;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ElAmal.Application.Services.Mail
{
    public class MailSettingsService : IMailSettingsService
    {
        
        private MailSettings _options;

        public MailSettingsService(IOptions<MailSettings> options)
        {
            _options = options.Value;
        }
        public async Task SendEmailAsync(Email email)
        {
            var mail = new MimeMessage
            {
                // Set sender email
                Sender = MailboxAddress.Parse(_options.Email),

                // Set email subject
                Subject = email.Subject
            };

            // Add display name and sender email
            mail.From.Add(new MailboxAddress(_options.DisplayName, _options.Email));

            // Add recipient email
            mail.To.Add(MailboxAddress.Parse(email.To));

            // Body 
            var builder = new BodyBuilder();

            builder.HtmlBody = email.Body;           // Set HTML body content

            mail.Body = builder.ToMessageBody();    // Attach body to the email

            using var smtp = new SmtpClient();

            // Open connection to SMTP server
            smtp.Connect(_options.Host, _options.Port, SecureSocketOptions.StartTls);

            // Authenticate with SMTP credentials
            await smtp.AuthenticateAsync(_options.Email, _options.Password);

            // Send the email
            smtp.Send(mail);

            // Disconnect from SMTP server
            await smtp.DisconnectAsync(true);
        }

    }
}
