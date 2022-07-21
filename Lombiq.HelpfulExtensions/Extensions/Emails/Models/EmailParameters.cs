using System.Collections.Generic;

namespace Lombiq.HelpfulExtensions.Extensions.Emails.Models;

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
