#nullable enable

using Lombiq.HelpfulLibraries.OrchardCore.Workflow;
using Microsoft.Extensions.Localization;

namespace Lombiq.HelpfulExtensions.Extensions.Workflows.Activities;

public class AuthorizationEvent : SimpleEventActivityBase
{
    public override LocalizedString DisplayText => T["Content Item Authorization"];
    public override LocalizedString Category => T["Security"];

    public AuthorizationEvent(IStringLocalizer<AuthorizationEvent> stringLocalizer)
        : base(stringLocalizer)
    {
    }
}
