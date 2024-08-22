using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace Lombiq.HelpfulExtensions.Extensions.GoogleTag;

[Feature(FeatureIds.GoogleTag)]
public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddTagHelpers<GoogleTagTagHelper>();
        services.AddLiquidParserTag<GoogleTagLiquidParserTag>("google_tag");
    }
}
