using Lombiq.HelpfulExtensions.Extensions.ContentSets.Services;
using Microsoft.AspNetCore.Mvc;
using OrchardCore;
using OrchardCore.Modules;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets.Controllers;

[Feature(FeatureIds.ContentSets)]
public class ContentSetController(IContentSetManager contentSetManager, IOrchardHelper orchardHelper) : Controller
{
    public async Task<IActionResult> Create(string fromContentItemId, string fromPartName, string newKey) =>
        await contentSetManager.CloneContentItemAsync(fromContentItemId, fromPartName, newKey) is { } content
            ? Redirect(orchardHelper.GetItemEditUrl(content))
            : NotFound();
}
