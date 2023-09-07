using Lombiq.HelpfulLibraries.OrchardCore.Workflow;
using Microsoft.Extensions.Localization;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets.Workflows.Activities;

public class ContentSetGetSupportedOptionsEvent : SimpleEventActivityBase
{
    public const string OutputName = "MemberLinks";

    public override LocalizedString DisplayText => T["Get Supported Content Set Options"];
    public override LocalizedString Category => T["Content Sets"];

    public ContentSetGetSupportedOptionsEvent(IStringLocalizer stringLocalizer)
        : base(stringLocalizer)
    {
    }
}
