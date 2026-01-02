#nullable enable

using Lombiq.HelpfulLibraries.OrchardCore.Workflow;
using Microsoft.Extensions.Localization;
using OrchardCore.Workflows.Models;
using System.Collections.Generic;

namespace Lombiq.HelpfulExtensions.Extensions.Workflows.Activities;

public class AuthorizationEvent : SimpleEventActivityBase
{
    public override LocalizedString DisplayText => T["Content Item Authorization"];
    public override LocalizedString Category => T["Security"];

    public IEnumerable<string> ContentTypes
    {
        get => GetProperty<IEnumerable<string>>();
        set => SetProperty(value);
    }

    public IEnumerable<string> Permissions
    {
        get => GetProperty<IEnumerable<string>>();
        set => SetProperty(value);
    }

    public AuthorizationEvent(IStringLocalizer<AuthorizationEvent> stringLocalizer)
        : base(stringLocalizer)
    {
    }

    public static AuthorizationEvent FromActivityRecord(
        ActivityRecord record,
        IStringLocalizer<AuthorizationEvent> localizer) =>
        new(localizer)
        {
            Properties = record.Properties,
        };
}
