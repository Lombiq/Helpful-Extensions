using Lombiq.HelpfulExtensions.Extensions.Widgets.ViewModels;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Navigation;
using System.Threading.Tasks;
using static Lombiq.HelpfulExtensions.Extensions.Widgets.WidgetTypes;
using static Lombiq.HelpfulLibraries.Libraries.Navigation.PortalNavigationProviderBase;

namespace Lombiq.HelpfulExtensions.Extensions.Widgets
{
    public class PortalMenuWidgetDisplayDriver : ContentDisplayDriver
    {
        private readonly INavigationManager _navigationManager;
        private readonly IActionContextAccessor _actionContextAccessor;

        public PortalMenuWidgetDisplayDriver(
            INavigationManager navigationManager,
            IActionContextAccessor actionContextAccessor)
        {
            _navigationManager = navigationManager;
            _actionContextAccessor = actionContextAccessor;
        }

        public override async Task<IDisplayResult> DisplayAsync(ContentItem model, BuildDisplayContext context)
        {
            if (model.ContentType != PortalMenuWidget) return null;

            var menuItems = await _navigationManager.BuildMenuAsync(
                PortalNavigationName,
                _actionContextAccessor.ActionContext);
            return Initialize<PortalMenuWidgetViewModel>(PortalMenuWidget, viewModel => viewModel.MenuItems = menuItems);
        }
    }
}
