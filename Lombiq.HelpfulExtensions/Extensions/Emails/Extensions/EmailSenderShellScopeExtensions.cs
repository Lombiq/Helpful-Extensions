using Lombiq.HelpfulExtensions.Extensions.Emails.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrchardCore.Email;
using OrchardCore.Environment.Shell.Scope;
using System.Collections.Generic;
using System.Linq;

namespace Lombiq.HelpfulExtensions.Extensions.Emails.Extensions;

public static class EmailSenderShellScopeExtensions
{
    /// <summary>
    /// Sends an HTML email after the current shell scope has ended. If any errors occur during the process they will be
    /// logged.
    /// </summary>
    /// <param name="parameters">Parameters required for sending emails (e.g., recipients, subject, CC).</param>
    public static void SendEmailDeferred(this ShellScope shellScope, EmailParameters parameters) =>
        shellScope.AddDeferredTask(async scope =>
        {
            var smtpService = scope.ServiceProvider.GetRequiredService<ISmtpService>();
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
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<ShellScope>>();
                logger.LogError("Email sending was unsuccessful: {Error}", result.Errors.Select(error => error.ToString()).Join());
            }
        });
}
