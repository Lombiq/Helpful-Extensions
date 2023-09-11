using Lombiq.HelpfulExtensions.Extensions.ContentSets.Events;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Models;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Services;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.ViewModels;
using Lombiq.HelpfulLibraries.OrchardCore.Contents;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Linq;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.DisplayManagement.Views;
using System.Collections.Generic;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets.Drivers;

public class ContentSetContentPickerFieldDisplayDriver : ContentFieldDisplayDriver<ContentSetContentPickerField>
{
    private readonly IContentDefinitionManager _contentDefinitionManager;
    private readonly IContentSetManager _contentSetManager;
    private readonly IEnumerable<IContentSetEventHandler> _contentSetEventHandlers;
    private readonly IStringLocalizer<ContentSetPart> T;

    public ContentSetContentPickerFieldDisplayDriver(
        IContentDefinitionManager contentDefinitionManager,
        IContentSetManager contentSetManager,
        IEnumerable<IContentSetEventHandler> contentSetEventHandlers,
        IStringLocalizer<ContentSetPart> stringLocalizer)
    {
        _contentDefinitionManager = contentDefinitionManager;
        _contentSetManager = contentSetManager;
        _contentSetEventHandlers = contentSetEventHandlers;
        T = stringLocalizer;
    }

    public override IDisplayResult Display(
        ContentSetContentPickerField field,
        BuildFieldDisplayContext fieldDisplayContext)
    {
        var name = fieldDisplayContext.PartFieldDefinition.Name;
        if (field.ContentItem.Get<ContentSetPart>(name) is not { } part) return null;

        return Initialize<ContentSetContentPickerFieldViewModel>(GetDisplayShapeType(fieldDisplayContext), model =>
            {
                model.PartFieldDefinition = fieldDisplayContext.PartFieldDefinition;
                return model.InitializeAsync(
                    _contentSetManager,
                    _contentSetEventHandlers,
                    T,
                    part,
                    new ContentTypePartDefinition(
                        name,
                        _contentDefinitionManager.GetPartDefinition(nameof(ContentSetPart)),
                        new JObject()),
                    isNew: false);
            })
            .Location(CommonContentDisplayTypes.Detail, CommonLocationNames.Content)
            .Location(CommonContentDisplayTypes.Summary, CommonLocationNames.Content);
    }
}
