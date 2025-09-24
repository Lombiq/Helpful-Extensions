using Fluid;
using Fluid.Ast;
using Fluid.Values;
using OrchardCore.Liquid;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.Liquid;

public class IsNotEmptyFilter : ILiquidFilter
{
    public async ValueTask<FluidValue> ProcessAsync(
        FluidValue input,
        FilterArguments arguments,
        LiquidTemplateContext context)
    {
        var argumentExpressions = new List<Expression>(capacity: arguments.Count + 1) { new LiteralExpression(input) };

        for (var i = 0; i < arguments.Count; i++)
        {
            argumentExpressions.Add(new LiteralExpression(arguments.At(i)));
        }

        return BooleanValue.Create(
            await IfNotEmptyParserBlock.IsAnyArgumentNotNullOrWhiteSpaceAsync(argumentExpressions, context));
    }
}
