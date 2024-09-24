using AngleSharp.Io;
using Lombiq.HelpfulExtensions.Extensions.OrchardRecipeMigration.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.Admin;
using OrchardCore.DisplayManagement.Notify;
using OrchardCore.Modules;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lombiq.HelpfulExtensions.Extensions.OrchardRecipeMigration.Controllers;

[Admin]
[Feature(FeatureIds.OrchardRecipeMigration)]
public sealed class OrchardRecipeMigrationAdminController : Controller
{
    private readonly INotifier _notifier;
    private readonly IOrchardExportToRecipeConverter _converter;
    private readonly IHtmlLocalizer<OrchardRecipeMigrationAdminController> H;
    public OrchardRecipeMigrationAdminController(
        INotifier notifier,
        IOrchardExportToRecipeConverter converter,
        IHtmlLocalizer<OrchardRecipeMigrationAdminController> localizer)
    {
        _notifier = notifier;
        _converter = converter;
        H = localizer;
    }

    public IActionResult Index() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Convert(IFormFile file)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        Stream stream;
        string json;

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
            json = await _converter.ConvertAsync(XDocument.Load(stream));
        }

        Response.Headers.Append("Content-Disposition", "attachment;filename=export.recipe.json");
        return Content(json, MimeTypeNames.ApplicationJson, Encoding.UTF8);
    }
}
