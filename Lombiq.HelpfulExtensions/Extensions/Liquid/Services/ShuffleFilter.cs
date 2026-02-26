using Fluid;
using Fluid.Values;
using Lombiq.HelpfulLibraries.Common.Utilities;
using OrchardCore.Liquid;
using System.Linq;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.Liquid.Services;

public class ShuffleFilter : ILiquidFilter
{
    public ValueTask<FluidValue> ProcessAsync(
        FluidValue input,
        FilterArguments arguments,
        LiquidTemplateContext context)
    {
        if (input is not ArrayValue { Values: { Count: > 0 } values })
        {
            return ValueTask.FromResult(input);
        }

        var random = new NonSecurityRandomizer();
        var shuffled = values
            .Select(item => (Item: item, Sort: random.Get()))
            .OrderBy(pair => pair.Sort)
            .Select(pair => pair.Item)
            .ToArray();

        return FluidValue.Create(shuffled, context.Options);
    }
}
