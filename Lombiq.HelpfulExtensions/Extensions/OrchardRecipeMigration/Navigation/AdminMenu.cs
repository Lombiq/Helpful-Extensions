using Lombiq.HelpfulExtensions.Extensions.OrchardRecipeMigration.Controllers;
using Lombiq.HelpfulLibraries.OrchardCore.Navigation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.OrchardRecipeMigration.Navigation;

public class AdminMenu(IHttpContextAccessor hca, IStringLocalizer<AdminMenu> stringLocalizer) : INavigationProvider
{
    private readonly IStringLocalizer T = stringLocalizer;

    public Task BuildNavigationAsync(string name, NavigationBuilder builder)
    {
        if (!name.EqualsOrdinalIgnoreCase("admin")) return Task.CompletedTask;

        builder.Add(T["Configuration"], configuration => configuration
            .Add(T["Import/Export"], importExport => importExport
                .Add(T["Orchard 1 Recipe Migration"], T["Orchard 1 Recipe Migration"], migration => migration
                    .Action<OrchardRecipeMigrationAdminController>(hca.HttpContext, controller => controller.Index())
                    .LocalNav()
                )));

        return Task.CompletedTask;
    }
}
