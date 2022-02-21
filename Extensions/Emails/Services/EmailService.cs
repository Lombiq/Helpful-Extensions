using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lombiq.HelpfulExtensions.Extensions.Emails.Models;
using Microsoft.Extensions.Logging;
using OrchardCore.Email;
using OrchardCore.Environment.Shell.Scope;
using Microsoft.Extensions.DependencyInjection;

namespace Lombiq.HelpfulExtensions.Extensions.Emails.Services
{
    public class EmailService : IEmailService
    {
        public void SendEmailDeferred(EmailParameters parameters) =>
            ShellScope.AddDeferredTask(async scope =>
            {
                var smtpService = scope.ServiceProvider.GetService<ISmtpService>();
                var logger = scope.ServiceProvider.GetService<ILogger<EmailService>>();
                var result = await smtpService.SendAsync(new MailMessage
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
                    logger.LogError("Email sending was unsuccessful: {0}", result.Errors.Select(error => error.ToString()).Join());
                }
            });
    }
}
