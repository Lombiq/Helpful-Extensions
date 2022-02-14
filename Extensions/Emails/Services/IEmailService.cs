using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OrchardCore.Email;

namespace Lombiq.HelpfulExtensions.Extensions.Emails.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailParameters parameters);
    }

    public class EmailService : IEmailService
    {
        private readonly ISmtpService _smtpService;
        private readonly ILogger<EmailService> _logger;

        public EmailService(ISmtpService smtpService, ILogger<EmailService> logger)
        {
            _smtpService = smtpService;
            _logger = logger;
        }

        public async Task SendEmailAsync(EmailParameters parameters)
        {
            var result = await _smtpService.SendAsync(new MailMessage
            {
                Sender = parameters.Sender,
                To = parameters.To?.Join(","),
                Cc = parameters.Cc?.Join(","),
                Bcc = parameters.Bcc?.Join(","),
                Subject = parameters.Subject,
                ReplyTo = parameters.ReplyTo,
                Body = parameters.Body,
                IsBodyHtml = true,
            });

            if (!result.Succeeded)
            {
                _logger.LogError("Email sending was unsuccessful: {0}", result.Errors.Select(error => error.ToString()).Join());
            }
        }
    }

    public class EmailParameters
    {
        public string Sender { get; set; }
        public IEnumerable<string> To { get; set; }
        public IEnumerable<string> Cc { get; set; }
        public IEnumerable<string> Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string ReplyTo { get; set; }
    }
}
