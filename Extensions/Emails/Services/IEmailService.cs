using Lombiq.HelpfulExtensions.Extensions.Emails.Models;

namespace Lombiq.HelpfulExtensions.Extensions.Emails.Services
{
    /// <summary>
    /// Service for sending emails.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email after the current shell scope is ended. Any error occurs during the process will be logged.
        /// </summary>
        /// <param name="parameters">Parameters required for sending emails (e.g., recipients, subject, CC).</param>
        void SendEmailDeferred(EmailParameters parameters);
    }

    public static class EmailServiceExtensions
    {
        public static void SendEmailDeferred(this IEmailService service, string to, string subject, string body) =>
            service.SendEmailDeferred(new EmailParameters
            {
                To = new[] { to },
                Subject = subject,
                Body = body,
            });
    }
}
