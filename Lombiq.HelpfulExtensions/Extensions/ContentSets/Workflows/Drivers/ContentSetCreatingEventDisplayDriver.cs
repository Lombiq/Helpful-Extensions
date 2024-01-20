using Lombiq.HelpfulExtensions.Extensions.ContentSets.Workflows.Activities;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Workflows.Models;
using Lombiq.HelpfulLibraries.OrchardCore.Workflow;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.DisplayManagement.Notify;
using System.Collections.Generic;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets.Workflows.Drivers;

public class ContentSetCreatingEventDisplayDriver(
    INotifier notifier,
    IStringLocalizer<DocumentedEventActivityDisplayDriver> baseLocalizer,
    IHtmlLocalizer<ContentSetCreatingEventDisplayDriver> htmlLocalizer)
    : DocumentedEventActivityDisplayDriverBase<ContentSetCreatingEvent>(notifier, baseLocalizer)
{
    public override string IconClass => "fa-circle-half-stroke";
    public override LocalizedHtmlString Description => htmlLocalizer["Executes when a new content item is created in the content set."];
    public override IDictionary<string, string> AvailableInputs { get; } = new Dictionary<string, string>
    {
        [nameof(CreatingContext.ContentItem)] = nameof(ContentItem),
        [nameof(CreatingContext.Definition)] = nameof(ContentTypePartDefinition),
        [nameof(CreatingContext.ContentSet)] = "string",
        [nameof(CreatingContext.NewKey)] = "string",
    };
}
