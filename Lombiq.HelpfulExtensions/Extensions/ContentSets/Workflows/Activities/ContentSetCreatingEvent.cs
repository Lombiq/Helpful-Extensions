using Lombiq.HelpfulLibraries.OrchardCore.Workflow;
using Microsoft.Extensions.Localization;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets.Workflows.Activities;

public class ContentSetCreatingEvent : SimpleEventActivityBase
{
    public override LocalizedString DisplayText => T["Creating Content Set"];
    public override LocalizedString Category => T["Content Sets"];

    public ContentSetCreatingEvent(IStringLocalizer stringLocalizer)
        : base(stringLocalizer)
    {
    }
}
