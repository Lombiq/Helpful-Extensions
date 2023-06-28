using Lombiq.HelpfulExtensions.Extensions.ContentSets.Services;
using Microsoft.AspNetCore.Mvc;
using OrchardCore;
using OrchardCore.Modules;
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
            ? Redirect(await _orchardHelper.GetItemEditUrlAsync(content))
            : NotFound();
}
