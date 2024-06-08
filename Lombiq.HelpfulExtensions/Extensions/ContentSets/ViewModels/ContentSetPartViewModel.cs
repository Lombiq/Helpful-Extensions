using Lombiq.HelpfulExtensions.Extensions.ContentSets.Events;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Models;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentManagement.Metadata.Settings;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets.ViewModels;

public class ContentSetPartViewModel
{
    public string ContentSet { get; set; }
    public string Key { get; set; }

    [BindNever]
    public ContentTypePartDefinition Definition { get; set; }

    [BindNever]
    public ContentSetPart ContentSetPart { get; set; }

    [BindNever]
    public IEnumerable<ContentSetLinkViewModel> MemberLinks { get; set; } = [];

    [BindNever]
    public bool IsNew { get; set; }

    [BindNever]
    public string DisplayName =>
        Definition?
            .Settings?
            .Property(nameof(ContentTypePartSettings))?
            .Value
            .ToObject<ContentTypePartSettings>()?
            .DisplayName ?? Definition?.Name;

    public async ValueTask InitializeAsync(
        IContentSetManager contentSetManager,
        IEnumerable<IContentSetEventHandler> contentSetEventHandlers,
        IStringLocalizer stringLocalizer,
        ContentSetPart part,
        ContentTypePartDefinition definition,
        bool isNew)
    {
        Key = part.Key;
        ContentSet = part.ContentSet;
        ContentSetPart = part;
        Definition = definition;
        IsNew = isNew;

        var existingContentItems = (await contentSetManager.GetContentItemsAsync(part.ContentSet))
            .ToDictionary(item => item.Get<ContentSetPart>(definition.Name)?.Key);

        var options = new Dictionary<string, ContentSetLinkViewModel>
        {
            [ContentSetPart.Default] = new(
                IsDeleted: false,
                stringLocalizer["Default content item"],
                existingContentItems.GetMaybe(ContentSetPart.Default)?.ContentItemId,
                ContentSetPart.Default),
        };

        var supportedOptions = (await contentSetEventHandlers.AwaitEachAsync(item => item.GetSupportedOptionsAsync(part, definition)))
            .Where(links => links != null)
            .SelectMany(links => links);
        options.AddRange(supportedOptions, link => link.Key);

        // Ensure the existing content item IDs are applied to the supported option links.
        existingContentItems
            .Where(pair => options.GetMaybe(pair.Key)?.ContentItemId != pair.Value.ContentItemId)
            .ForEach(pair => options[pair.Key] = options[pair.Key] with { ContentItemId = pair.Value.ContentItemId });

        // Content items that have been added to the set but no longer generate a valid option matching their key.
        var inapplicableSetMembers = existingContentItems
            .Where(pair => !options.ContainsKey(pair.Key))
            .Select(pair => new ContentSetLinkViewModel(
                IsDeleted: true,
                stringLocalizer["{0} (No longer applicable)", pair.Value.DisplayText].Value,
                pair.Value.ContentItemId,
                pair.Key));
        options.AddRange(inapplicableSetMembers, link => link.Key);

        MemberLinks = [.. options
            .Values
            .Where(link => link.Key != Key && link.ContentItemId != part.ContentItem.ContentItemId)
            .OrderBy(link => string.IsNullOrEmpty(link.ContentItemId) ? 1 : 0)
            .ThenBy(link => link.IsDeleted ? 1 : 0)
            .ThenBy(link => link.DisplayText)];
    }
}

public record ContentSetLinkViewModel(bool IsDeleted, string DisplayText, string ContentItemId, string Key);
