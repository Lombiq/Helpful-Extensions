using OrchardCore.Navigation;
using System.Collections.Generic;

namespace Lombiq.HelpfulExtensions.Extensions.Widgets.ViewModels
{
    public class PortalMenuWidgetViewModel
    {
        public bool NoWrapper { get; set; }
        public IEnumerable<MenuItem> MenuItems { get; set; }
    }
}
