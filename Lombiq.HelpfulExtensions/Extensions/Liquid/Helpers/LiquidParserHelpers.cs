using Fluid;
using Fluid.Ast;
using Fluid.Values;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.Liquid.Helpers;

public static class LiquidParserHelpers
{
    /// <summary>
    /// Returns <see langword="true"/> if any <see cref="Expression"/> in <paramref name="arguments"/>
    /// evaluates to a truthy value (according to Liquid parser rules) that's also not empty or whitespace when
    /// converted to <see langword="string"/>.
    /// </summary>
    public static async Task<bool> IsAnyNotNullOrWhiteSpaceAsync(
        IEnumerable<Expression> arguments,
        TemplateContext context)
    {
        foreach (var expression in arguments)
        {
            var result = await expression.EvaluateAsync(context);
            if (result.ToBooleanValue() && !string.IsNullOrWhiteSpace(result.ToStringValue())) return true;
        }

        return false;
    }

    /// <inheritdoc cref="IsAnyNotNullOrWhiteSpaceAsync(IEnumerable{Expression}, TemplateContext)"/>
    public static Task<bool> IsAnyNotNullOrWhiteSpaceAsync(
        IEnumerable<FilterArgument> arguments,
        TemplateContext context) =>
        IsAnyNotNullOrWhiteSpaceAsync(arguments.Select(item => item.Expression), context);

    /// <summary>
    /// Evaluates the provided <paramref name="expressions"/>.
    /// </summary>
    public static Task<IList<FluidValue>> EvaluateAsync(
        IEnumerable<Expression> expressions,
        TemplateContext context) =>
        expressions.AwaitEachAsync(async expression => await expression.EvaluateAsync(context));

    /// <summary>
    /// Evaluates the provided <paramref name="arguments"/>.
    /// </summary>
    public static Task<IList<FluidValue>> EvaluateAsync(
        IEnumerable<FilterArgument> arguments,
        TemplateContext context) =>
        EvaluateAsync(arguments.Select(item => item.Expression), context);
}
