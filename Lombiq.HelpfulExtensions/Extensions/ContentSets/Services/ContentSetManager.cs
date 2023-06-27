using Lombiq.HelpfulExtensions.Extensions.ContentSets.Indexes;
using OrchardCore.ContentManagement;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YesSql;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets.Services;

public class ContentSetManager : IContentSetManager
{
    private readonly IContentManager _contentManager;
    private readonly ISession _session;

    public ContentSetManager(IContentManager contentManager, ISession session)
    {
        _contentManager = contentManager;
        _session = session;
    }

    public async Task<IEnumerable<string>> GetContentItemIdsAsync(string setId) =>
        (await _session.QueryIndex<ContentSetIndex>(index => index.ContentSet == setId).ListAsync())
            .Select(index => index.ContentItemId);

    public async Task<IEnumerable<ContentItem>> GetContentItemsAsync(string setId) =>
        await _contentManager.GetAsync(await GetContentItemIdsAsync(setId));
}
