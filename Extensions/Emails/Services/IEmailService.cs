using System.Threading.Tasks;
using Lombiq.HelpfulExtensions.Extensions.Emails.Models;
using OrchardCore.Email;

namespace Lombiq.HelpfulExtensions.Extensions.Emails.Services
{
    /// <summary>
    /// Service for sending emails.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email using the given parameters.
        /// </summary>
        /// <param name="parameters">Parameters required for sending emails (e.g., recipients, subject, CC).</param>
        /// <returns>Result of the SMTP operation.</returns>
        Task<SmtpResult> SendEmailAsync(EmailParameters parameters);
    }

    public static class EmailServiceExtensions
    {
        public static Task<SmtpResult> SendEmailAsync(this IEmailService service, string to, string subject, string body) =>
            service.SendEmailAsync(new EmailParameters
            {
                To = new[] {to},
                Subject = subject,
                Body = body,
            });
    }
}
