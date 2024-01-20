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

public class ContentSetManager(
    IContentDefinitionManager contentDefinitionManager,
    IContentManager contentManager,
    IEnumerable<IContentSetEventHandler> contentSetEventHandlers,
    ISession session) : IContentSetManager
{
    public Task<IEnumerable<ContentSetIndex>> GetIndexAsync(string setId) =>
        session.QueryIndex<ContentSetIndex>(index => index.ContentSet == setId).ListAsync();

    public async Task<IEnumerable<ContentItem>> GetContentItemsAsync(string setId) =>
        await contentManager.GetAsync((await GetIndexAsync(setId)).Select(index => index.ContentItemId));

    public async Task<ContentItem> CloneContentItemAsync(string fromContentItemId, string fromPartName, string newKey)
    {
        if (string.IsNullOrEmpty(fromPartName)) fromPartName = nameof(ContentSetPart);

        if (await contentManager.GetAsync(fromContentItemId) is not { } original ||
            original.Get<ContentSetPart>(fromPartName)?.ContentSet is not { } contentSet ||
            await contentManager.CloneAsync(original) is not { } content)
        {
            return null;
        }

        var exists = await session
            .QueryIndex<ContentSetIndex>(index => index.ContentSet == contentSet && index.Key == newKey)
            .FirstOrDefaultAsync() is not null;
        if (exists) throw new InvalidOperationException($"The key \"{newKey}\" already exists for the content set \"{contentSet}\".");

        content.Alter<ContentSetPart>(fromPartName, part =>
        {
            part.ContentSet = contentSet;
            part.Key = newKey;
        });

        var contentTypePartDefinition = (await contentDefinitionManager.GetTypeDefinitionAsync(content.ContentType))
            .Parts
            .Single(definition => definition.Name == fromPartName);

        foreach (var handler in contentSetEventHandlers)
        {
            await handler.CreatingAsync(content, contentTypePartDefinition, contentSet, newKey);
        }

        await contentManager.PublishAsync(content);
        return content;
    }
}
