using OrchardCore.Navigation;
using System.Collections.Generic;
using System.Linq;

namespace Lombiq.HelpfulExtensions.Extensions.Widgets.ViewModels
{
    public class MainMenuWidgetViewModel
    {
        public bool NoWrapper { get; set; }
        public IEnumerable<MenuItem> MenuItems { get; set; }

        public MainMenuWidgetViewModel(bool noWrapper = false, IEnumerable<MenuItem> menuItems = null)
        {
            NoWrapper = noWrapper;
            MenuItems = menuItems ?? Enumerable.Empty<MenuItem>();
        }

        public MainMenuWidgetViewModel(dynamic model)
            : this(model.NoWrapper as bool? ?? false, model.MenuItems as IEnumerable<MenuItem>)
        {
        }
    }
}
