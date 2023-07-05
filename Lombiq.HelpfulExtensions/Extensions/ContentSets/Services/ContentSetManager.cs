using Lombiq.HelpfulExtensions.Extensions.ContentSets.Events;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Indexes;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Models;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YesSql;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets.Services;

public class ContentSetManager : IContentSetManager
{
    private readonly IContentDefinitionManager _contentDefinitionManager;
    private readonly IContentManager _contentManager;
    private readonly IEnumerable<IContentSetEventHandler> _contentSetEventHandlers;
    private readonly ISession _session;

    public ContentSetManager(
        IContentDefinitionManager contentDefinitionManager,
        IContentManager contentManager,
        IEnumerable<IContentSetEventHandler> contentSetEventHandlers,
        ISession session)
    {
        _contentDefinitionManager = contentDefinitionManager;
        _contentManager = contentManager;
        _contentSetEventHandlers = contentSetEventHandlers;
        _session = session;
    }

    public async Task<IEnumerable<string>> GetContentItemIdsAsync(string setId) =>
        (await _session.QueryIndex<ContentSetIndex>(index => index.ContentSet == setId).ListAsync())
            .Select(index => index.ContentItemId);

    public async Task<IEnumerable<ContentItem>> GetContentItemsAsync(string setId) =>
        await _contentManager.GetAsync(await GetContentItemIdsAsync(setId));

    public async Task<ContentItem> CloneContentItemAsync(string fromContentItemId, string fromPartName, string newKey)
    {
        if (string.IsNullOrEmpty(fromPartName)) fromPartName = nameof(ContentSetPart);

        if (await _contentManager.GetAsync(fromContentItemId) is not { } original ||
            original.Get<ContentSetPart>(fromPartName)?.ContentSet is not { } contentSet ||
            await _contentManager.CloneAsync(original) is not { } content)
        {
            return null;
        }

        var exists = await _session
            .QueryIndex<ContentSetIndex>(index => index.ContentSet == contentSet && index.Key == newKey)
            .FirstOrDefaultAsync() is not null;
        if (exists) throw new InvalidOperationException($"The key \"{newKey}\" already exists for the content set \"{contentSet}\".");

        content.Alter<ContentSetPart>(fromPartName, part =>
        {
            part.ContentSet = contentSet;
            part.Key = newKey;
        });

        var contentTypePartDefinition = _contentDefinitionManager
            .GetTypeDefinition(content.ContentType)
            .Parts
            .Single(definition => definition.Name == fromPartName);

        foreach (var handler in _contentSetEventHandlers)
        {
            await handler.CreatingAsync(content, contentTypePartDefinition, contentSet, newKey);
        }

        await _contentManager.PublishAsync(content);
        return content;
    }
}
