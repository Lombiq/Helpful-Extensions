using Lombiq.HelpfulExtensions.Extensions.OrchardRecipeMigration.Navigation;
using Lombiq.HelpfulExtensions.Extensions.OrchardRecipeMigration.Services;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.Navigation;

namespace Lombiq.HelpfulExtensions.Extensions.OrchardRecipeMigration;

[Feature(FeatureIds.OrchardRecipeMigration)]
public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IOrchardExportToRecipeConverter, OrchardExportToRecipeConverter>();
        services.AddScoped<IOrchardContentConverter, CommonOrchardContentConverter>();
        services.AddScoped<IOrchardContentConverter, GraphMetadataOrchardContentConverter>();
        services.AddScoped<IOrchardExportConverter, ListPartOrchardExportConverter>();
        services.AddScoped<IOrchardUserConverter, UserOrchardContentConverter>();

        services.AddNavigationProvider<AdminMenu>();
    }
}
