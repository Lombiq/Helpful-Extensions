using Fluid;
using Fluid.Values;
using Lombiq.HelpfulExtensions.Extensions.Widgets.ViewModels;
using Lombiq.HelpfulLibraries.OrchardCore.Liquid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OrchardCore.Liquid;
using OrchardCore.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.Widgets.Liquid;

public class MenuWidgetLiquidFilter(
    IActionContextAccessor actionContextAccessor,
    ILiquidContentDisplayService liquidContentDisplayService,
    IStringLocalizer<MenuWidgetLiquidFilter> stringLocalizer,
    IUrlHelperFactory urlHelperFactory) : ILiquidFilter
{
    private readonly Lazy<IUrlHelper> _urlHelperLazy = new(() =>
            urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext!));
    private readonly IStringLocalizer<MenuWidgetLiquidFilter> T = stringLocalizer;

    public ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments arguments, LiquidTemplateContext context)
    {
        bool noWrapper, localNav;
        string classes;
        noWrapper = arguments[nameof(noWrapper)].ToBooleanValue();
        localNav = arguments[nameof(localNav)].ToBooleanValue();
        classes = arguments[nameof(classes)].ToStringValue();

        var converter = new LocalizedStringJsonConverter(T);
        var serializer = new JsonSerializer();
        serializer.Converters.Add(converter);
        var serializerSettings = new JsonSerializerSettings();
        serializerSettings.Converters.Add(converter);

        var menuItems = input?.Type switch
        {
            FluidValues.String => JsonConvert.DeserializeObject<IList<MenuItem>>(
                input!.ToStringValue(),
                serializerSettings),
            FluidValues.Object => input!.ToObjectValue() switch
            {
                IEnumerable<MenuItem> enumerable => enumerable.AsList(),
                MenuItem single => new[] { single },
                JArray jArray => jArray.ToObject<IList<MenuItem>>(serializer),
                JObject jObject => new[] { jObject.ToObject<MenuItem>(serializer) },
                _ => null,
            },
            _ => null,
        };

        UpdateMenuItems(menuItems, localNav);

        return liquidContentDisplayService.DisplayNewAsync<MenuWidgetViewModel>(
            WidgetTypes.MenuWidget,
            model =>
            {
                model.NoWrapper = noWrapper;
                model.MenuItems = menuItems ?? Array.Empty<MenuItem>();
                model.HtmlClasses = classes;
            });
    }

    private void UpdateMenuItems(IEnumerable<MenuItem> menuItems, bool localNav)
    {
        if (menuItems == null) return;

        foreach (var item in menuItems)
        {
            if (!string.IsNullOrEmpty(item.Url))
            {
                var finalUrl = _urlHelperLazy.Value.Content(item.Url);
                item.Url = finalUrl;
                item.Href = finalUrl;
            }

            item.LocalNav = localNav || item.LocalNav;

            UpdateMenuItems(item.Items, localNav);
        }
    }

    public class LocalizedStringJsonConverter(IStringLocalizer stringLocalizer) : JsonConverter<LocalizedString>
    {
        private readonly IStringLocalizer T = stringLocalizer;

        public override void WriteJson(JsonWriter writer, LocalizedString value, JsonSerializer serializer) =>
            writer.WriteValue(value?.Value);

        [SuppressMessage("Style", "IDE0010:Add missing cases", Justification = "We don't want to handle other token types.")]
        public override LocalizedString ReadJson(
            JsonReader reader,
            Type objectType,
            LocalizedString existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.String:
                    return JToken.ReadFrom(reader).ToObject<string>() is { } text ? T[text] : null;
                case JsonToken.StartObject:
                    var data = new Dictionary<string, string>(
                        JToken.ReadFrom(reader).ToObject<Dictionary<string, string>>(),
                        StringComparer.OrdinalIgnoreCase);
                    return new LocalizedString(data[nameof(LocalizedString.Name)], data[nameof(LocalizedString.Value)]);
                default:
                    throw new InvalidOperationException("Unable to parse JSON!");
            }
        }
    }
}
