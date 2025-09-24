using Fluid;
using Fluid.Ast;
using Lombiq.HelpfulLibraries.OrchardCore.Liquid;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.Liquid;

public class IfNotEmptyParserBlock : ILiquidParserBlock
{
    public async ValueTask<Completion> WriteToAsync(
        IReadOnlyList<FilterArgument> argumentsList,
        IReadOnlyList<Statement> statements,
        TextWriter writer,
        TextEncoder encoder,
        TemplateContext context)
    {
        if (await IsAnyArgumentNotNullOrWhiteSpaceAsync(argumentsList.Select(item => item.Expression), context))
        {
            foreach (var statement in statements)
            {
                var completion = await statement.WriteToAsync(writer, encoder, context);

                if (completion != Completion.Normal) return completion;
            }
        }

        return Completion.Normal;
    }

    internal static async Task<bool> IsAnyArgumentNotNullOrWhiteSpaceAsync(
        IEnumerable<Expression> argumentExpressions,
        TemplateContext context)
    {
        foreach (var expression in argumentExpressions)
        {
            var result = await expression.EvaluateAsync(context);
            if (result.ToBooleanValue() && !string.IsNullOrWhiteSpace(result.ToStringValue())) return true;
        }

        return false;
    }
}
