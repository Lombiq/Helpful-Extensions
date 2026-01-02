#nullable enable

using System.Collections.Generic;

namespace Lombiq.HelpfulExtensions.Extensions.Workflows.ViewModels;

public class AuthorizationEventViewModel
{
    public IEnumerable<string> ContentTypes { get; set; } = [];
    public IEnumerable<string> Permissions { get; set; } = [];
}
