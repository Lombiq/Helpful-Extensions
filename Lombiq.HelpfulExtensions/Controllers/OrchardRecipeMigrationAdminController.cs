using Lombiq.HelpfulExtensions.Extensions.OrchardRecipeMigration.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.DisplayManagement.Notify;
using OrchardCore.Modules;
using OrchardCore.Recipes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lombiq.HelpfulExtensions.Extensions.OrchardRecipeMigration.Controllers;

[Admin]
[Feature(FeatureIds.OrchardRecipeMigration)]
public sealed class OrchardRecipeMigrationAdminController : Controller
{
    private readonly IEnumerable<IOrchardExportConverter> _exportConverters;
    private readonly INotifier _notifier;
    private readonly IOrchardExportToRecipeConverter _converter;
    private readonly IHtmlLocalizer<OrchardRecipeMigrationAdminController> H;

    public OrchardRecipeMigrationAdminController(
        IEnumerable<IOrchardExportConverter> exportConverters,
        INotifier notifier,
        IOrchardExportToRecipeConverter converter,
        IHtmlLocalizer<OrchardRecipeMigrationAdminController> localizer)
    {
        _exportConverters = exportConverters;
        _notifier = notifier;
        _converter = converter;
        H = localizer;
    }

    public IActionResult Index() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Convert(IFormFile file, int page = 1)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        Stream stream;

        try
        {
            stream = file.OpenReadStream();
        }
        catch (Exception)
        {
            await _notifier.ErrorAsync(H["Please add a file to import."]);
            return Redirect(nameof(Index));
        }

        await using (stream)
        {
            var result = await _converter.ConvertAsync(XDocument.Load(stream), page);
            return Json(result);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DownloadRecipe([FromForm] IFormFile file, [FromForm] string contentItemsJson)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        XDocument export;
        IList<ContentItem> contentItems;

        try
        {
            contentItems = JsonSerializer.Deserialize<List<ContentItem>>(contentItemsJson) ?? [];
            using var stream = file.OpenReadStream();
            export = XDocument.Load(stream);
        }
        catch (Exception)
        {
            await _notifier.ErrorAsync(H["Failed to read the uploaded file."]);
            return Redirect(nameof(Index));
        }

        foreach (var converter in _exportConverters)
        {
            await converter.UpdateContentItemsAsync(export, contentItems);
        }

        var recipe = JObject.FromObject(new RecipeDescriptor())!;
        recipe["steps"] = new JsonArray(JObject.FromObject(new { name = "content", data = contentItems }));
        var json = recipe.ToString();

        Response.Headers.Append("Content-Disposition", "attachment;filename=export.recipe.json");
        return Content(json, MediaTypeNames.Application.Json, Encoding.UTF8);
    }
}
