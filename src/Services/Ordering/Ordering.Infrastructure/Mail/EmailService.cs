using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;

namespace Ordering.Infrastructure.Mail
{
    public class EmailService : IEmailService
    {
        public EmailSettings EmailSettings { get; }
        public ILogger<EmailService> Logger { get; }

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            EmailSettings = emailSettings.Value;
            Logger = logger;
        }

        public async Task<bool> SendEmail(Email email)
        {
            var client = new SendGridClient(EmailSettings.ApiKey);
            
            var from = new EmailAddress
            {
                Email = EmailSettings.FromAddress,
                Name = EmailSettings.FromName
            };

            var sendGridMessage = MailHelper.CreateSingleEmail(from, new EmailAddress(email.To), email.Subject, email.Body, email.Body);
            var response = await client.SendEmailAsync(sendGridMessage);

            Logger.LogInformation("Email sent.");

            if (response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.OK)
                return true;

            Logger.LogError("Email sending failed.");
            return false;
        }
    }
}
