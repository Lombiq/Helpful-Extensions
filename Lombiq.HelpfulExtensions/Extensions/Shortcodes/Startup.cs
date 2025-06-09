using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace Lombiq.HelpfulExtensions.Extensions.Shortcodes;

[Feature(FeatureIds.Shortcodes)]
public sealed class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
    }
}
