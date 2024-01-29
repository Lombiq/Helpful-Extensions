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
public class OrchardRecipeMigrationAdminController(
    INotifier notifier,
    IOrchardExportToRecipeConverter converter,
    IHtmlLocalizer<OrchardRecipeMigrationAdminController> localizer) : Controller
{
    private readonly IHtmlLocalizer H = localizer;

    public IActionResult Index() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Convert(IFormFile file)
    {
        Stream stream;
        string json;

        try
        {
            stream = file.OpenReadStream();
        }
        catch (Exception)
        {
            await notifier.ErrorAsync(H["Please add a file to import."]);
            return Redirect(nameof(Index));
        }

        await using (stream)
        {
            json = await converter.ConvertAsync(XDocument.Load(stream));
        }

        Response.Headers.Append("Content-Disposition", "attachment;filename=export.recipe.json");
        return Content(json, MimeTypeNames.ApplicationJson, Encoding.UTF8);
    }
}
