using Lombiq.HelpfulExtensions.Extensions.Widgets.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Navigation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Lombiq.HelpfulExtensions.Extensions.Widgets.WidgetTypes;
using static Lombiq.HelpfulLibraries.Libraries.Navigation.PortalNavigationProviderBase;

namespace Lombiq.HelpfulExtensions.Extensions.Widgets
{
    public class PortalMenuWidgetDisplayDriver : ContentDisplayDriver
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IEnumerable<INavigationProvider> _navigationProviders;
        private readonly IHttpContextAccessor _hca;

        public PortalMenuWidgetDisplayDriver(
            IAuthorizationService authorizationService,
            IEnumerable<INavigationProvider> navigationProviders,
            IHttpContextAccessor hca)
        {
            _authorizationService = authorizationService;
            _navigationProviders = navigationProviders;
            _hca = hca;
        }

        public override async Task<IDisplayResult> DisplayAsync(ContentItem model, BuildDisplayContext context)
        {
            if (model.ContentType != PortalMenuWidget) return null;

            var menuItems = await GetMenuItemsAsync(_authorizationService, _navigationProviders, _hca);
            return Initialize<PortalMenuWidgetViewModel>(PortalMenuWidget, viewModel => viewModel.MenuItems = menuItems);
        }

        public static async Task<IEnumerable<MenuItem>> GetMenuItemsAsync(
            IAuthorizationService authorizationService,
            IEnumerable<INavigationProvider> navigationProviders,
            IHttpContextAccessor hca)
        {
            var builder = new NavigationBuilder();
            foreach (var provider in navigationProviders) await provider.BuildNavigationAsync(NavigationName, builder);

            var menuItems = new List<MenuItem>();
            foreach (var menuItem in builder.Build())
            {
                if (hca.HttpContext?.User is { } user &&
                    await menuItem.Permissions.AwaitWhileAsync(
                        permission => authorizationService.AuthorizeAsync(user, permission, menuItem.Resource)))
                {
                    menuItems.Add(menuItem);
                }
            }

            return menuItems;
        }
    }
}
