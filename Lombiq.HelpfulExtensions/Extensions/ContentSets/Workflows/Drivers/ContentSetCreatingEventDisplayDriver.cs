using Lombiq.HelpfulExtensions.Extensions.ContentSets.Workflows.Activities;
using Lombiq.HelpfulLibraries.OrchardCore.Workflow;
using Microsoft.AspNetCore.Mvc.Localization;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets.Workflows.Drivers;

public class ContentSetCreatingEventDisplayDriver : SimpleEventActivityDisplayDriverBase<ContentSetCreatingEvent>
{
    private readonly IHtmlLocalizer<ContentSetCreatingEventDisplayDriver> H;

    public override string IconClass => "fa-circle-half-stroke";
    public override LocalizedHtmlString Description => H["Executes when a new content item is created in the content set."];

    public ContentSetCreatingEventDisplayDriver(IHtmlLocalizer<ContentSetCreatingEventDisplayDriver> htmlLocalizer) =>
        H = htmlLocalizer;
}
