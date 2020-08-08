using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OrchardCore.DisplayManagement.Implementation;
using OrchardCore.DisplayManagement.Shapes;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.ShapeTracing
{
    internal class ShapeTracingShapeEvents : IShapeDisplayEvents
    {
        public Task DisplayedAsync(ShapeDisplayContext context)
        {
            // We could also use _orchardHelper.ConsoleLog(context.Shape) here but that causes an OutOfMemoryException.

            var builder = new HtmlContentBuilder(6);
            var shapeMetadata = context.Shape.Metadata;

            builder.AppendLine();
            builder.AppendHtmlLine("<!-- ");
            builder.AppendHtmlLine(shapeMetadata.Type);
            builder.AppendLine();

            void AddIfNotNullOrEmpty(string name, string value)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    builder.AppendHtml(name);
                    builder.AppendHtml(": ");
                    builder.AppendHtmlLine(value);
                }
            }

            if (shapeMetadata.Alternates.Any())
            {
                builder.AppendHtml("Alternates: ");
                builder.AppendHtmlLine(string.Join(", ", shapeMetadata.Alternates));
            }

            if (shapeMetadata.BindingSources.Any())
            {
                builder.AppendHtml("Binding sources: ");
                builder.AppendHtmlLine(string.Join(", ", shapeMetadata.BindingSources));
            }

            if (shapeMetadata.Wrappers.Any())
            {
                builder.AppendHtml("Wrappers: ");
                builder.AppendHtmlLine(string.Join(", ", shapeMetadata.Wrappers));
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

            builder.AppendHtmlLine("-->");

            builder.AppendHtml(context.ChildContent);

            context.ChildContent = builder;

            return Task.CompletedTask;
        }

        public Task DisplayingAsync(ShapeDisplayContext context) => Task.CompletedTask;

        public Task DisplayingFinalizedAsync(ShapeDisplayContext context) => Task.CompletedTask;


        // Taken from: https://stackoverflow.com/a/37954657/220230
        private class JsonIgnoreAttributeIgnorerContractResolver : DefaultContractResolver
        {
            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                var property = base.CreateProperty(member, memberSerialization);
                property.Ignored = false; // Here is the magic
                return property;
            }
        }
    }
}
