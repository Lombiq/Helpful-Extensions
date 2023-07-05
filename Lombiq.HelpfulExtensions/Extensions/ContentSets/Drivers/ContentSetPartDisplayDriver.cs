using Lombiq.HelpfulExtensions.Extensions.ContentSets.Events;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Models;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Services;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.ViewModels;
using Lombiq.HelpfulLibraries.OrchardCore.Contents;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets.Drivers;

public class ContentSetPartDisplayDriver : ContentPartDisplayDriver<ContentSetPart>
{
    private const string ShapeType = $"{nameof(ContentSetPart)}_{CommonContentDisplayTypes.SummaryAdmin}";

    private readonly IContentSetManager _contentSetManager;
    private readonly IIdGenerator _iidGenerator;
    private readonly IEnumerable<IContentSetEventHandler> _contentSetEventHandlers;
    private readonly IStringLocalizer<ContentSetPartDisplayDriver> T;

    public ContentSetPartDisplayDriver(
        IContentSetManager contentSetManager,
        IIdGenerator iidGenerator,
        IEnumerable<IContentSetEventHandler> contentSetEventHandlers,
        IStringLocalizer<ContentSetPartDisplayDriver> stringLocalizer)
    {
        _contentSetManager = contentSetManager;
        _iidGenerator = iidGenerator;
        _contentSetEventHandlers = contentSetEventHandlers;
        T = stringLocalizer;
    }

    public override IDisplayResult Display(ContentSetPart part, BuildPartDisplayContext context) =>
        Combine(
            Initialize<ContentSetPartViewModel>(
                    $"{ShapeType}_Tags",
                    model => BuildViewModelAsync(model, part, context.TypePartDefinition, isNew: false))
                .Location(CommonContentDisplayTypes.SummaryAdmin, "Tags:11"),
            Initialize<ContentSetPartViewModel>(
                    $"{ShapeType}_Links",
                    model => BuildViewModelAsync(model, part, context.TypePartDefinition, isNew: false))
                .Location(CommonContentDisplayTypes.SummaryAdmin, "Actions:5")
        );

    public override IDisplayResult Edit(ContentSetPart part, BuildPartEditorContext context) =>
        Initialize<ContentSetPartViewModel>(
                $"{nameof(ContentSetPart)}_Edit",
                model => BuildViewModelAsync(model, part, context.TypePartDefinition, context.IsNew))
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
                part.ContentSet = _iidGenerator.GenerateUniqueId();
            }
        }

        return await EditAsync(part, context);
    }

    public async ValueTask BuildViewModelAsync(
        ContentSetPartViewModel model,
        ContentSetPart part,
        ContentTypePartDefinition definition,
        bool isNew)
    {
        string GetKey(IContent content) => content?.ContentItem?.Get<ContentSetPart>(definition.Name)?.Key;

        model.Key = part.Key;
        model.ContentSet = part.ContentSet;
        model.ContentSetPart = part;
        model.Definition = definition;
        model.IsNew = isNew;

        var existingContentItems = (await _contentSetManager.GetContentItemsAsync(part.ContentSet)).AsList();
        var options = new Dictionary<string, ContentSetLinkViewModel>
        {
            [ContentSetPart.Default] = new(
                IsDeleted: false,
                T["Default content item"],
                existingContentItems.SingleOrDefault(item => GetKey(item) == ContentSetPart.Default)?.ContentItemId,
                ContentSetPart.Default),
        };

        var supportedOptions = (await _contentSetEventHandlers.AwaitEachAsync(item => item.GetSupportedOptionsAsync(part, definition)))
            .SelectMany(links => links ?? Enumerable.Empty<ContentSetLinkViewModel>());
        options.AddRange(supportedOptions, link => link.Key);

        // Content items that have been added to the set but no longer generate a valid option matching their key.
        var inapplicableSetMembers = existingContentItems
            .Where(item => !options.ContainsKey(item.ContentItemId))
            .Select(item => new ContentSetLinkViewModel(
                IsDeleted: true,
                T["{0} (No longer applicable)", item.DisplayText].Value,
                item.ContentItemId,
                GetKey(item)));
        options.AddRange(inapplicableSetMembers, link => link.Key);

        model.MemberLinks = options
            .Values
            .Where(link => link.Key != model.Key && link.ContentItemId != part.ContentItem.ContentItemId)
            .OrderBy(link => string.IsNullOrEmpty(link.ContentItemId) ? 1 : 0)
            .ThenBy(link => link.IsDeleted ? 1 : 0)
            .ThenBy(link => link.DisplayText)
            .ToList();
    }
}
