using Lombiq.HelpfulExtensions.Extensions.ContentSets.Events;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Models;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Services;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.ViewModels;
using Lombiq.HelpfulLibraries.OrchardCore.Contents;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.DisplayManagement.Views;
using System.Collections.Generic;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets.Drivers;

public class ContentSetContentPickerFieldDisplayDriver(
    IContentDefinitionManager contentDefinitionManager,
    IContentSetManager contentSetManager,
    IEnumerable<IContentSetEventHandler> contentSetEventHandlers,
    IStringLocalizer<ContentSetPart> stringLocalizer) : ContentFieldDisplayDriver<ContentSetContentPickerField>
{
    private readonly IStringLocalizer T = stringLocalizer;

    public override IDisplayResult Display(
        ContentSetContentPickerField field,
        BuildFieldDisplayContext fieldDisplayContext)
    {
        var name = fieldDisplayContext.PartFieldDefinition.Name;
        if (field.ContentItem.Get<ContentSetPart>(name) is not { } part) return null;

        return Initialize<ContentSetContentPickerFieldViewModel>(GetDisplayShapeType(fieldDisplayContext), async model =>
            {
                model.PartFieldDefinition = fieldDisplayContext.PartFieldDefinition;
                await model.InitializeAsync(
                    contentSetManager,
                    contentSetEventHandlers,
                    T,
                    part,
                    new ContentTypePartDefinition(
                        name,
                        await contentDefinitionManager.GetPartDefinitionAsync(nameof(ContentSetPart)),
                        []),
                    isNew: false);
            })
            .Location(CommonContentDisplayTypes.Detail, CommonLocationNames.Content)
            .Location(CommonContentDisplayTypes.Summary, CommonLocationNames.Content);
    }
}
