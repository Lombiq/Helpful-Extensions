using Lombiq.HelpfulExtensions.Extensions.ContentSets.Models;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Services;
using Microsoft.AspNetCore.Mvc;
using OrchardCore;
using OrchardCore.Modules;
using System.Linq;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets.Controllers;

[Feature(FeatureIds.ContentSets)]
public class ContentSetController : Controller
{
    private readonly IContentSetManager _contentSetManager;
    private readonly IOrchardHelper _orchardHelper;

    public ContentSetController(IContentSetManager contentSetManager, IOrchardHelper orchardHelper)
    {
        _contentSetManager = contentSetManager;
        _orchardHelper = orchardHelper;
    }

    public async Task<IActionResult> Create(string fromContentItemId, string fromPartName, string newKey) =>
        await _contentSetManager.CloneContentItemAsync(fromContentItemId, fromPartName, newKey) is { } content
            ? Redirect(_orchardHelper.GetItemEditUrl(content))
            : NotFound();

    [Route("Lombiq.HelpfulExtensions/ContentSet/Display/{setId}/{key}")]
    public async Task<IActionResult> Display(string setId, string key, bool exactKey)
    {
        if (string.IsNullOrEmpty(setId)) return NotFound();
        if (string.IsNullOrEmpty(key)) return NotFound();

        var indexes = await _contentSetManager.GetIndexAsync(setId);
        var ids = indexes.ToDictionary(index => index.Key, index => index.ContentItemId);

        if (ids.TryGetValue(key, out var exactMatch)) return this.RedirectToContentDisplay(exactMatch);
        if (exactKey) return NotFound();

        return this.RedirectToContentDisplay(ids[ContentSetPart.Default]);
    }
}
