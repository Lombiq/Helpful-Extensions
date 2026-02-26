using Fluid;
using Fluid.Values;
using OrchardCore.Liquid;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.Liquid.Services;

public abstract class ArrayFilterBase : ILiquidFilter
{
    private readonly ILiquidFilter _filter;

    protected ArrayFilterBase(ILiquidFilter filter) =>
        _filter = filter;

    public async ValueTask<FluidValue> ProcessAsync(
        FluidValue input,
        FilterArguments arguments,
        LiquidTemplateContext context)
    {
        var results = new List<FluidValue>();

        if (input.Type == FluidValues.Array && input is ArrayValue arrayValue)
        {
            foreach (var value in arrayValue.Values)
            {
                await AddAsync(results, value, arguments, context);
            }
        }
        else if (input.Type == FluidValues.Object)
        {
            await AddAsync(results, input, arguments, context);
        }

        return await ThenAsync(results, input, arguments, context);
    }

    protected virtual ValueTask<FluidValue> ThenAsync(
        IList<FluidValue> results,
        FluidValue input,
        FilterArguments arguments,
        LiquidTemplateContext context) =>
        ValueTask.FromResult(FluidValue.Create(results.ToArray(), context.Options));

    private async ValueTask AddAsync(
        List<FluidValue> results,
        FluidValue value,
        FilterArguments arguments,
        LiquidTemplateContext context)
    {
        if (!value.ToBooleanValue()) return;

        var result = await _filter.ProcessAsync(value, arguments, context);
        if (result.ToBooleanValue()) results.Add(result);
    }
}
