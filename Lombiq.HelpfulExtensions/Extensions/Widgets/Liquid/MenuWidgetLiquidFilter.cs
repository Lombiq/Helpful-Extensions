using Fluid;
using Fluid.Values;
using Lombiq.HelpfulExtensions.Extensions.Widgets.ViewModels;
using Lombiq.HelpfulLibraries.OrchardCore.Liquid;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OrchardCore.Liquid;
using OrchardCore.Navigation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.Widgets.Liquid;

public class MenuWidgetLiquidFilter : ILiquidFilter
{
    private readonly ILiquidContentDisplayService _liquidContentDisplayService;

    public MenuWidgetLiquidFilter(ILiquidContentDisplayService liquidContentDisplayService) =>
        _liquidContentDisplayService = liquidContentDisplayService;

    public ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments arguments, LiquidTemplateContext context)
    {
        bool noWrapper;
        noWrapper = arguments[nameof(noWrapper)].ToBooleanValue();

        var menuItems = input?.Type switch
        {
            FluidValues.String => JsonConvert.DeserializeObject<IEnumerable<MenuItem>>(input!.ToStringValue()),
            FluidValues.Object => input!.ToObjectValue() switch
            {
                IEnumerable<MenuItem> enumerable => enumerable,
                MenuItem single => new[] { single },
                JArray jArray => jArray.ToObject<IEnumerable<MenuItem>>(),
                JObject jObject => new[] { jObject.ToObject<MenuItem>() },
                _ => null,
            },
            _ => null,
        };

        return _liquidContentDisplayService.DisplayNewAsync<MenuWidgetViewModel>(
            WidgetTypes.MenuWidget,
            model =>
            {
                model.NoWrapper = noWrapper;
                model.MenuItems = menuItems ?? Enumerable.Empty<MenuItem>();
            });
    }
}
