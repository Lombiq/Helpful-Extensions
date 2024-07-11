using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Logging;
using OrchardCore.DisplayManagement.Implementation;
using OrchardCore.DisplayManagement.Shapes;
using System.Linq;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.ShapeTracing;

internal sealed class ShapeTracingShapeEvents : IShapeDisplayEvents
{
    private readonly IHttpContextAccessor _hca;
    private readonly ILogger<ShapeTracingShapeEvents> _logger;

    public ShapeTracingShapeEvents(IHttpContextAccessor hca, ILogger<ShapeTracingShapeEvents> logger)
    {
        _hca = hca;
        _logger = logger;
    }

    public Task DisplayedAsync(ShapeDisplayContext context)
    {
        if (!_hca.HttpContext.IsDevelopment()) return Task.CompletedTask;

        // We could also use _orchardHelper.ConsoleLog(context.Shape) here but that causes an OutOfMemoryException.

        var builder = new HtmlContentBuilder(6);
        var builderShapeInfo = new HtmlContentBuilder(6);
        var shapeMetadata = context.Shape.Metadata;

        var isPageTitle = shapeMetadata.Type == nameof(PageTitleShapes.PageTitle);

        builder.AppendLine();
        builder.AppendHtmlLine("<!-- ");
        builderShapeInfo.AppendHtmlLine(shapeMetadata.Type);
        builderShapeInfo.AppendLine();

        void AddIfNotNullOrEmpty(string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                builderShapeInfo.AppendHtml(name);
                builderShapeInfo.AppendHtml(": ");
                builderShapeInfo.AppendHtmlLine(value);
            }
        }

        if (shapeMetadata.Alternates.Count != 0)
        {
            builderShapeInfo.AppendHtml("Alternates: ");
            builderShapeInfo.AppendHtmlLine(string.Join(", ", shapeMetadata.Alternates));
        }

        if (shapeMetadata.BindingSources.Any())
        {
            builderShapeInfo.AppendHtml("Binding sources: ");
            builderShapeInfo.AppendHtmlLine(string.Join(", ", shapeMetadata.BindingSources));
        }

        if (shapeMetadata.Wrappers.Count != 0)
        {
            builderShapeInfo.AppendHtml("Wrappers: ");
            builderShapeInfo.AppendHtmlLine(string.Join(", ", shapeMetadata.Wrappers));
        }

        AddIfNotNullOrEmpty(nameof(ShapeMetadata.Card), shapeMetadata.Card);
        AddIfNotNullOrEmpty(nameof(ShapeMetadata.Column), shapeMetadata.Column);
        AddIfNotNullOrEmpty(nameof(ShapeMetadata.Differentiator), shapeMetadata.Differentiator);
        AddIfNotNullOrEmpty(nameof(ShapeMetadata.DisplayType), shapeMetadata.DisplayType);
        AddIfNotNullOrEmpty(nameof(ShapeMetadata.IsCached), shapeMetadata.IsCached.ToString());
        AddIfNotNullOrEmpty(nameof(ShapeMetadata.Name), shapeMetadata.Name);
        AddIfNotNullOrEmpty(nameof(ShapeMetadata.PlacementSource), shapeMetadata.PlacementSource);
        AddIfNotNullOrEmpty(nameof(ShapeMetadata.Position), shapeMetadata.Position);
        AddIfNotNullOrEmpty(nameof(ShapeMetadata.Prefix), shapeMetadata.Prefix);
        AddIfNotNullOrEmpty(nameof(ShapeMetadata.Tab), shapeMetadata.Tab);

        _logger.LogInformation("Shape information:\n{ShapeInformation}", builderShapeInfo.Html());

        builder.AppendHtml(builderShapeInfo);

        builder.AppendHtmlLine("-->");

        // We are skipping the PageTitle shape, otherwise the shape information would be put in the title.
        if (!isPageTitle)
        {
            builder.AppendHtml(context.ChildContent);
            context.ChildContent = builder;
        }

        return Task.CompletedTask;
    }

    public Task DisplayingAsync(ShapeDisplayContext context) => Task.CompletedTask;

    public Task DisplayingFinalizedAsync(ShapeDisplayContext context) => Task.CompletedTask;
}
