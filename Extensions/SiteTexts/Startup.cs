using Lombiq.HelpfulExtensions.Extensions.SiteTexts.Services;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;

namespace Lombiq.HelpfulExtensions.Extensions.SiteTexts;

[Feature(FeatureIds.SiteTexts)]
public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IDataMigration, Migrations>();
        services.AddScoped<ISiteTextService, SiteTextService>();
    }
}

[Feature("OrchardCore.ContentLocalization")]
public class ContentLocalizationStartup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.RemoveImplementations<ISiteTextService>();
        services.AddScoped<ISiteTextService, ContentLocalizationSiteTextService>();
    }
}
