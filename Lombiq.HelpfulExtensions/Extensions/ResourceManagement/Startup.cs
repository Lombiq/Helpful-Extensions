using Lombiq.HelpfulLibraries.OrchardCore.ResourceManagement;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.ResourceManagement;

namespace Lombiq.HelpfulExtensions.Extensions.ResourceManagement;

[Feature(FeatureIds.ResourceManagement)]
public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services) =>
        services.Decorate<IResourceManager, ResourceManagerDecorator>();
}
