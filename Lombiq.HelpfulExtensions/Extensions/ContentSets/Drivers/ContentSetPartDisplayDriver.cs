using Lombiq.HelpfulExtensions.Extensions.ContentSets.Events;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Models;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Services;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.ViewModels;
using Lombiq.HelpfulLibraries.OrchardCore.Contents;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets.Drivers;

public class ContentSetPartDisplayDriver : ContentPartDisplayDriver<ContentSetPart>
{
    private const string ShapeType = $"{nameof(ContentSetPart)}_{CommonContentDisplayTypes.SummaryAdmin}";

    private readonly IContentSetManager _contentSetManager;
    private readonly IIdGenerator _idGenerator;
    private readonly IEnumerable<IContentSetEventHandler> _contentSetEventHandlers;
    private readonly IStringLocalizer<ContentSetPart> T;

    public ContentSetPartDisplayDriver(
        IContentSetManager contentSetManager,
        IIdGenerator idGenerator,
        IEnumerable<IContentSetEventHandler> contentSetEventHandlers,
        IStringLocalizer<ContentSetPart> stringLocalizer)
    {
        _contentSetManager = contentSetManager;
        _idGenerator = idGenerator;
        _contentSetEventHandlers = contentSetEventHandlers;
        T = stringLocalizer;
    }

    public override IDisplayResult Display(ContentSetPart part, BuildPartDisplayContext context)
    {
        ValueTask InitializeAsync(ContentSetPartViewModel model) =>
            model.InitializeAsync(
                _contentSetManager,
                _contentSetEventHandlers,
                T,
                part,
                context.TypePartDefinition,
                isNew: false);

        return Combine(
            Initialize<ContentSetPartViewModel>($"{ShapeType}_Tags", InitializeAsync)
                .Location(CommonContentDisplayTypes.SummaryAdmin, "Tags:11"),
            Initialize<ContentSetPartViewModel>($"{ShapeType}_Links", InitializeAsync)
                .Location(CommonContentDisplayTypes.SummaryAdmin, "Actions:5")
        );
    }

    public override IDisplayResult Edit(ContentSetPart part, BuildPartEditorContext context) =>
        Initialize<ContentSetPartViewModel>($"{nameof(ContentSetPart)}_Edit", model => model.InitializeAsync(
                _contentSetManager,
                _contentSetEventHandlers,
                T,
                part,
                context.TypePartDefinition,
                context.IsNew))
            .Location($"Parts:0%{context.TypePartDefinition.Name};0");

    public override async Task<IDisplayResult> UpdateAsync(
        ContentSetPart part,
        IUpdateModel updater,
        UpdatePartEditorContext context)
    {
        var viewModel = new ContentSetPartViewModel();

        if (await updater.TryUpdateModelAsync(viewModel, Prefix))
        {
            part.Key = viewModel.Key;

            // Need to do this here to support displaying the message to save before adding when the
            // item has not been saved yet.
            if (string.IsNullOrEmpty(part.ContentSet))
            {
                part.ContentSet = _idGenerator.GenerateUniqueId();
            }
        }

        return await EditAsync(part, context);
    }
}
