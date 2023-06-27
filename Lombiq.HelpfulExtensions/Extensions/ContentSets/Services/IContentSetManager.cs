using OrchardCore.ContentManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets.Services;

public interface IContentSetManager
{
    Task<IEnumerable<string>> GetContentItemIdsAsync(string setId);
    Task<IEnumerable<ContentItem>> GetContentItemsAsync(string setId);
}
