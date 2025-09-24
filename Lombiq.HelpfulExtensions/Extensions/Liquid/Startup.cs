using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Liquid;
using OrchardCore.Modules;

namespace Lombiq.HelpfulExtensions.Extensions.Liquid;

[Feature(FeatureIds.Liquid)]
public sealed class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddLiquidParserBlock<IfNotEmptyParserBlock>("ifnotempty");
        services.AddLiquidFilter<IsNotEmptyFilter>("isnotempty");
    }
}
