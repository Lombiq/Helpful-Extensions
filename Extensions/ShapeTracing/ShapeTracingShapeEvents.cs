using Microsoft.AspNetCore.Html;
using Newtonsoft.Json.Linq;
using OrchardCore.DisplayManagement.Implementation;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.ShapeTracing
{
    internal class ShapeTracingShapeEvents : IShapeDisplayEvents
    {
        public Task DisplayedAsync(ShapeDisplayContext context)
        {
            // We could also use _orchardHelper.ConsoleLog(context.Shape) here but that causes an OutOfMemoryException.

            var builder = new HtmlContentBuilder(7);
            var shapeMetadata = context.Shape.Metadata;

            builder.AppendLine();
            builder.AppendHtmlLine("<!-- ");
            builder.AppendHtmlLine(shapeMetadata.Type);
            builder.AppendLine();

            builder.AppendHtmlLine(JObject.FromObject(shapeMetadata).ToString());

            builder.AppendHtmlLine("-->");

            builder.AppendHtml(context.ChildContent);

            context.ChildContent = builder;

            return Task.CompletedTask;
        }

        public Task DisplayingAsync(ShapeDisplayContext context) => Task.CompletedTask;

        public Task DisplayingFinalizedAsync(ShapeDisplayContext context) => Task.CompletedTask;
    }
}
