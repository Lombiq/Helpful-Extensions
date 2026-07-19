using Fluid;
using Fluid.Values;
using Lombiq.HelpfulExtensions.Extensions.Widgets.ViewModels;
using Lombiq.HelpfulLibraries.OrchardCore.Liquid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OrchardCore;
using OrchardCore.Liquid;
using OrchardCore.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.Widgets.Liquid;

public class MenuWidgetLiquidFilter : ILiquidFilter
{
    private readonly ILiquidContentDisplayService _liquidContentDisplayService;
    private readonly IOrchardHelper _orchardHelper;
    private readonly IStringLocalizer<MenuWidgetLiquidFilter> T;

    public MenuWidgetLiquidFilter(
        ILiquidContentDisplayService liquidContentDisplayService,
        IOrchardHelper orchardHelper,
        IStringLocalizer<MenuWidgetLiquidFilter> stringLocalizer)
    {
        _liquidContentDisplayService = liquidContentDisplayService;
        _orchardHelper = orchardHelper;

        T = stringLocalizer;
    }

    public async ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments arguments, LiquidTemplateContext context)
    {
        bool noWrapper, localNav;
        string classes;
        noWrapper = arguments[nameof(noWrapper)].ToBooleanValue();
        localNav = arguments[nameof(localNav)].ToBooleanValue();
        classes = arguments[nameof(classes)].ToStringValue();

        var serializerOptions = LocalizedStringJsonConverter.Add(T);
        var menuItems = input?.Type switch
        {
            FluidValues.String => JsonSerializer.Deserialize<IList<MenuItem>>(
                input!.ToStringValue(),
                serializerOptions),
            FluidValues.Object => input!.ToObjectValue() switch
            {
                IEnumerable<MenuItem> enumerable => enumerable.AsList(),
                MenuItem single => [single],
                JsonArray jsonArray => jsonArray.ToObject<IList<MenuItem>>(serializerOptions),
                JsonObject jsonObject => [jsonObject.ToObject<MenuItem>(serializerOptions)],
                _ => null,
            },
            _ => null,
        };

        var urlHelper = await _orchardHelper.GetUrlHelperAsync();
        UpdateMenuItems(menuItems, localNav, urlHelper);

        return await _liquidContentDisplayService.DisplayNewAsync<MenuWidgetViewModel>(
            WidgetTypes.MenuWidget,
            model =>
            {
                model.NoWrapper = noWrapper;
                model.MenuItems = menuItems ?? [];
                model.HtmlClasses = classes;
            });
    }

    private static void UpdateMenuItems(IEnumerable<MenuItem> menuItems, bool localNav, IUrlHelper urlHelper)
    {
        if (menuItems == null) return;

        foreach (var item in menuItems)
        {
            if (!string.IsNullOrEmpty(item.Url))
            {
                var finalUrl = urlHelper.Content(item.Url);
                item.Url = finalUrl;
                item.Href = finalUrl;
            }

            item.LocalNav = localNav || item.LocalNav;

            UpdateMenuItems(item.Items, localNav, urlHelper);
        }
    }

    public class LocalizedStringJsonConverter : JsonConverter<LocalizedString>
    {
        private readonly IStringLocalizer T;

        private LocalizedStringJsonConverter(IStringLocalizer stringLocalizer) =>
            T = stringLocalizer;

        public override void Write(Utf8JsonWriter writer, LocalizedString value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value?.Value);

        [SuppressMessage("Style", "IDE0010:Add missing cases", Justification = "We don't want to handle other token types.")]
        public override LocalizedString Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.String:
                    return JsonSerializer.Deserialize<string>(ref reader, options) is { } text ? T[text] : null;
                case JsonTokenType.StartObject:
                    var data = new Dictionary<string, string>(
                        JsonSerializer.Deserialize<Dictionary<string, string>>(ref reader, options),
                        StringComparer.OrdinalIgnoreCase);
                    return new LocalizedString(data[nameof(LocalizedString.Name)], data[nameof(LocalizedString.Value)]);
                default:
                    throw new InvalidOperationException("Unable to parse JSON!");
            }
        }

        public static JsonSerializerOptions Add(IStringLocalizer stringLocalizer, JsonSerializerOptions options = null)
        {
            options ??= new JsonSerializerOptions(JsonSerializerOptions.Default);
            options.Converters.RemoveAll(converter => converter is JsonConverter<LocalizedString>);
            options.Converters.Add(new LocalizedStringJsonConverter(stringLocalizer));

            return options;
        }
    }
}
