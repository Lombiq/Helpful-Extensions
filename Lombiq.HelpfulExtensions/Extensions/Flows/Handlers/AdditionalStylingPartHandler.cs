using Lombiq.HelpfulExtensions.Extensions.Flows.Models;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.Flows.Handlers;

public class AdditionalStylingPartHandler(IContentDefinitionManager contentDefinitionManager) : ContentHandlerBase
{
    public override async Task ActivatedAsync(ActivatedContentContext context)
    {
        if (!context.ContentItem.Has<AdditionalStylingPart>() &&
            (await contentDefinitionManager.GetTypeDefinitionAsync(context.ContentItem.ContentType))
                .GetSettings<ContentTypeSettings>().Stereotype == "Widget")
        {
            context.ContentItem.Weld<AdditionalStylingPart>();
        }
    }
}
