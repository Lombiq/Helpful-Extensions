using Lombiq.HelpfulLibraries.OrchardCore.Workflow;
using Microsoft.Extensions.Localization;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets.Workflows.Activities;

public class ContentSetCreatingEvent(IStringLocalizer<ContentSetCreatingEvent> stringLocalizer) : SimpleEventActivityBase(stringLocalizer)
{
    public override LocalizedString DisplayText => T["Creating Content Set"];
    public override LocalizedString Category => T["Content Sets"];
}
