using Lombiq.HelpfulExtensions.Extensions.OrchardRecipeMigration.Controllers;
using Lombiq.HelpfulLibraries.OrchardCore.Navigation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;

namespace Lombiq.HelpfulExtensions.Extensions.OrchardRecipeMigration.Navigation;

public sealed class AdminMenu : AdminMenuNavigationProviderBase
{
    public AdminMenu(IHttpContextAccessor hca, IStringLocalizer<AdminMenu> stringLocalizer)
        : base(hca, stringLocalizer)
    {
    }

    protected override void Build(NavigationBuilder builder) =>
        builder.Add(T["Configuration"], config => config
            .Add(T["Import/Export"], section => section
                .Add(T["Orchard 1 Recipe Migration"], T["Orchard 1 Recipe Migration"], item => item
                    .Action<OrchardRecipeMigrationAdminController>(_hca.HttpContext, controller => controller.Index())
                    .LocalNav()
                )));
}
