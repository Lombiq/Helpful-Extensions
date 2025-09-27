using Fluid;
using Fluid.Values;
using Microsoft.AspNetCore.Html;
using OrchardCore.DisplayManagement.Liquid.Filters;
using OrchardCore.Liquid;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.Liquid.Services;

public class ShapesRenderFilter : ArrayFilterBase
{
    public ShapesRenderFilter(ShapeRenderFilter shapeRenderFilter)
        : base(shapeRenderFilter)
    {
    }

    protected override ValueTask<FluidValue> ThenAsync(
        IList<FluidValue> results,
        FluidValue input,
        FilterArguments arguments,
        LiquidTemplateContext context)
    {
        var combined = new HtmlContentBuilder();

        foreach (var result in results)
        {
            if (result.ToObjectValue() is IHtmlContent htmlContent)
            {
                combined.AppendHtml(htmlContent);
            }
            else
            {
                combined.Append(result.ToStringValue());
            }

            combined.AppendHtml("\n");
        }

        return ValueTask.FromResult(FluidValue.Create(combined, context.Options));
    }
}
