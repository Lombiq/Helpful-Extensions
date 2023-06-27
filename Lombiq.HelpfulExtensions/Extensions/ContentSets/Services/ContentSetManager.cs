using OrchardCore.ContentManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets.Services;

public class ContentSetManager : IContentSetManager
{
    private readonly IContentManager _contentManager;

    public ContentSetManager(IContentManager contentManager) =>
        _contentManager = contentManager;

    public Task<IEnumerable<string>> GetContentItemIdsAsync(string setId) =>
        throw new System.NotImplementedException();

    public async Task<IEnumerable<ContentItem>> GetContentItemsAsync(string setId) =>
        await _contentManager.GetAsync(await GetContentItemIdsAsync(setId));
}
