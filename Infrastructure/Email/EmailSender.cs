using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailSender> _logger;
        public EmailSender(IConfiguration configuration, ILogger<EmailSender> logger)
        {
            _logger = logger;
            _configuration = configuration;

        }

        public async Task SendEmailAsync(string userEmail, string emailSubject, string msg)
        {
            var client = new SendGridClient(_configuration["SendGrid:Key"]);
            var message = new SendGridMessage
            {
                From = new EmailAddress("stuart.murray.dev@outlook.com", _configuration["SendGrid:User"]),
                Subject = emailSubject,
                PlainTextContent = msg,
                HtmlContent = msg,
            };

            message.AddTo(new EmailAddress(userEmail));

            _logger.LogInformation($"Sending Email... {message.PlainTextContent}");

            var response = await client.SendEmailAsync(message);
            if (!response.IsSuccessStatusCode) _logger.LogError(response.ToString());
        }
    }
}