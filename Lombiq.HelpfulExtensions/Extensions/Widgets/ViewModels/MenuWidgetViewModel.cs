using OrchardCore.Navigation;
using System.Collections.Generic;

namespace Lombiq.HelpfulExtensions.Extensions.Widgets.ViewModels;

public class MenuWidgetViewModel
{
    public bool NoWrapper { get; set; }

    public IEnumerable<MenuItem> MenuItems { get; set; }

    public string HtmlClasses { get; set; } = string.Empty;

    public MenuWidgetViewModel()
        : this(noWrapper: false, menuItems: [])
    {
    }

    public MenuWidgetViewModel(bool noWrapper, IEnumerable<MenuItem> menuItems)
    {
        NoWrapper = noWrapper;
        MenuItems = menuItems ?? [];
    }

    public MenuWidgetViewModel(dynamic model)
        : this((model.NoWrapper as bool?) == true, model.MenuItems as IEnumerable<MenuItem>) =>
        HtmlClasses = model.HtmlClasses as string ?? string.Empty;
}
