using OrchardCore.Navigation;
using System.Collections.Generic;
using System.Linq;

namespace Lombiq.HelpfulExtensions.Extensions.Widgets.ViewModels;

public class MenuWidgetViewModel(bool noWrapper, IEnumerable<MenuItem> menuItems)
{
    public bool NoWrapper { get; set; } = noWrapper;

    public IEnumerable<MenuItem> MenuItems { get; set; } = menuItems ?? Enumerable.Empty<MenuItem>();

    public string HtmlClasses { get; set; } = string.Empty;

    public MenuWidgetViewModel()
        : this(noWrapper: false, menuItems: Enumerable.Empty<MenuItem>())
    {
    }

    public MenuWidgetViewModel(dynamic model)
        : this((model.NoWrapper as bool?) == true, model.MenuItems as IEnumerable<MenuItem>) =>
        HtmlClasses = model.HtmlClasses as string ?? string.Empty;
}
