using Lombiq.HelpfulExtensions.Extensions.ContentSets.Models;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.ViewModels;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Workflows.Activities;
using Lombiq.HelpfulLibraries.OrchardCore.Workflow;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.DisplayManagement.Notify;
using System.Collections.Generic;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets.Workflows.Drivers;

public class ContentSetGetSupportedOptionsEventDisplayDriver :
    DocumentedEventActivityDisplayDriverBase<ContentSetGetSupportedOptionsEvent>
{
    private readonly IHtmlLocalizer<ContentSetGetSupportedOptionsEventDisplayDriver> H;

    public override string IconClass => "fa-circle-half-stroke";

    public override LocalizedHtmlString Description =>
        H["Tries to get a list of links representing the supported options for this content set."];

    public override IDictionary<string, string> AvailableInputs { get; } = new Dictionary<string, string>
    {
        [nameof(GetSupportedOptionsContext.Definition)] = nameof(ContentTypePartDefinition),
        [nameof(GetSupportedOptionsContext.ContentSetPart)] = nameof(ContentSetPart),
        [nameof(GetSupportedOptionsContext.ContentItem)] = nameof(ContentItem),
    };

    public override IDictionary<string, string> ExpectedOutputs { get; } = new Dictionary<string, string>
    {
        [ContentSetGetSupportedOptionsEvent.OutputName] = JsonConvert.SerializeObject(new ContentSetLinkViewModel(
            IsDeleted: false,
            "string",
            "string",
            "string")) + "[]",
    };

    public ContentSetGetSupportedOptionsEventDisplayDriver(
        INotifier notifier,
        IStringLocalizer<DocumentedEventActivityDisplayDriver> baseLocalizer,
        IHtmlLocalizer<ContentSetGetSupportedOptionsEventDisplayDriver> htmlLocalizer)
        : base(notifier, baseLocalizer) =>
        H = htmlLocalizer;
}
