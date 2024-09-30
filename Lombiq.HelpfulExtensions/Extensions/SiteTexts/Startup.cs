using Lombiq.HelpfulExtensions.Extensions.SiteTexts.Services;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;

namespace Lombiq.HelpfulExtensions.Extensions.SiteTexts;

[Feature(FeatureIds.SiteTexts)]
public sealed class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddDataMigration<SiteTextMigrations>();
        services.AddDataMigration<LocalizationMigrations>();
        services.AddScoped<ISiteTextService, SiteTextService>();
    }
}

[RequireFeatures("OrchardCore.ContentLocalization")]
public sealed class ContentLocalizationStartup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.RemoveImplementationsOf<ISiteTextService>();
        services.AddScoped<ISiteTextService, ContentLocalizationSiteTextService>();
    }
}
