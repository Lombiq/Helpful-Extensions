using Lombiq.HelpfulExtensions.Extensions.OrchardRecipeMigration.Controllers;

namespace Lombiq.HelpfulExtensions.Extensions.OrchardRecipeMigration.Navigation;

public sealed class AdminMenu : AdminMenuNavigationProviderBase
{

    public AdminMenu(IHttpContextAccessor hca, IStringLocalizer<AdminMenu> stringLocalizer)
        : base(hca, stringLocalizer)
    {
    }
    protected override void Build(NavigationBuilder builder)
    {
        builder.Add(_t["Configuration"], config => config
            .Add(_t["Import/Export"], section => section
                .Add(_t["Orchard 1 Recipe Migration"], _t["Orchard 1 Recipe Migration"], item => item
                    .Action<OrchardRecipeMigrationAdminController>(_hca.HttpContext, controller => controller.Index())
                    .LocalNav())));
    }
}
