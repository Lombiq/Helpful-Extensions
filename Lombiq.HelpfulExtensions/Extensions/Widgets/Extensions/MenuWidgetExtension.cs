using Lombiq.HelpfulExtensions.Extensions.Widgets.Constants;
using Lombiq.HelpfulExtensions.Extensions.Widgets.Models;
using System;
using System.Linq;

namespace OrchardCore.Navigation;

public static class MenuWidgetExtension
{
    public static MenuWidgetItemType GetMenuWidgetItemType(this MenuItem menuItem, Uri baseUri)
    {
        if (menuItem.Items.Count > 0) return MenuWidgetItemType.Parent;
        if (menuItem.IsDivider()) return MenuWidgetItemType.Divider;

        var uri = new Uri(baseUri, menuItem.Href);
        var isForm = uri
            .Query
            .TrimStart('?')
            .Split('&')
            .Contains("Lombiq.MenuWidget.Post=true", StringComparer.OrdinalIgnoreCase);

        return isForm ? MenuWidgetItemType.FormPost : MenuWidgetItemType.Link;
    }

    public static string GetMenuWidgetItemShapeName(this MenuItem menuItem, Uri baseUri) =>
        $"{ShapeTypes.MenuWidgetItem}__{menuItem.GetMenuWidgetItemType(baseUri)}";

    // We use LocalizedString.Name instead of Value intentionally, as this shouldn't be affected by localization.
    public static bool IsDivider(this MenuItem menuItem) =>
        menuItem.Text.Name.Length >= 3 && menuItem.Text.Name.All(character => character == '-');
}
