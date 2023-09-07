using Lombiq.HelpfulExtensions.Extensions.ContentSets.Models;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.ViewModels;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Workflows.Activities;
using Lombiq.HelpfulLibraries.OrchardCore.Workflow;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Localization;
using Newtonsoft.Json;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets.Workflows.Drivers;

public class ContentSetGetSupportedOptionsEventDisplayDriver : SimpleEventActivityDisplayDriverBase<ContentSetGetSupportedOptionsEvent>
{
    private readonly IHtmlLocalizer<ContentSetGetSupportedOptionsEventDisplayDriver> H;

    public override string IconClass => "fa-circle-half-stroke";

    public override LocalizedHtmlString Description
    {
        get
        {
            var foreword = H["Tries to get a list of links representing the supported options for this content set."];

            var inputKeys = new GetSupportedOptionsContext(Definition: null, ContentSetPart: null).ToDictionary().Keys;
            var inputs = H["The available inputs are: {0}", string.Join(", ", inputKeys)];

            // Not using Newtonsoft.Json.Schema to generate a real JSON schema due to licensing issues. On the other hand
            // System.Text.Json doesn't officially support JSON schema generation yet, so that's not an option either.
            var schema = JsonConvert.SerializeObject(new ContentSetLinkViewModel(
                IsDeleted: false,
                "string",
                "string",
                "string")) + "[]";
            var expectation = H["Expects an output \"{0}\" in the format <code>{1}</code>.", ContentSetGetSupportedOptionsEvent.OutputName, schema];

            return new HtmlString("<br>").Join(foreword, inputs, expectation);
        }
    }

    public ContentSetGetSupportedOptionsEventDisplayDriver(
        IHtmlLocalizer<ContentSetGetSupportedOptionsEventDisplayDriver> htmlLocalizer) =>
        H = htmlLocalizer;
}
