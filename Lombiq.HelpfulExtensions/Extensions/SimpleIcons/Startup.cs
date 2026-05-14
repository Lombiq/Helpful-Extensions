using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace Lombiq.HelpfulExtensions.Extensions.SimpleIcons;

[Feature(FeatureIds.SimpleIcons)]
public sealed class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services) =>
        services.AddTagHelpers<SimpleIconTagHelper>();
}
