using OrchardCore.Navigation;
using System.Collections.Generic;
using System.Linq;

namespace Lombiq.HelpfulExtensions.Extensions.Widgets.ViewModels;

public class MenuWidgetViewModel
{
    public bool NoWrapper { get; set; }
    public IEnumerable<MenuItem> MenuItems { get; set; }

    public MenuWidgetViewModel(bool noWrapper = false, IEnumerable<MenuItem> menuItems = null)
    {
        NoWrapper = noWrapper;
        MenuItems = menuItems ?? Enumerable.Empty<MenuItem>();
    }

    public MenuWidgetViewModel(dynamic model)
        : this((model.NoWrapper as bool?) == true, model.MenuItems as IEnumerable<MenuItem>)
    {
    }
}
