using Fluid;
using Fluid.Ast;
using Fluid.Values;
using Lombiq.HelpfulLibraries.OrchardCore.Liquid;
using OrchardCore.DisplayManagement.Liquid.Tags;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.GoogleTag;

public class GoogleTagLiquidParserTag : ILiquidParserTag
{
    public async ValueTask<Completion> WriteToAsync(
        IReadOnlyList<FilterArgument> argumentsList,
        TextWriter writer,
        TextEncoder encoder,
        TemplateContext context)
    {
        var arguments = new List<FilterArgument>
        {
            new(null, new LiteralExpression(new StringValue(GoogleTagViewModel.ShapeType))),
        };

        foreach (var argument in argumentsList)
        {
            if (argument.Name == "property_id")
            {
                await AddStringAsync(arguments, nameof(GoogleTagViewModel.GoogleTagPropertyId), argument, context);
            }
            else if (argument.Name == "cookie_domain")
            {
                await AddStringAsync(arguments, nameof(GoogleTagViewModel.CookieDomain), argument, context);
            }
        }

        return await ShapeTag.WriteToAsync(arguments, writer, encoder, context);
    }

    private static async Task AddStringAsync(
        List<FilterArgument> arguments,
        string newName,
        FilterArgument argument,
        TemplateContext context)
    {
        var newValue = await argument.Expression.EvaluateAsync(context);
        arguments.Add(new(newName, new LiteralExpression(newValue)));
    }
}
