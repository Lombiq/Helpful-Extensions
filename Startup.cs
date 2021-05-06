using Finitive.Common.Services;
using Lombiq.HelpfulExtensions.Services;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using static Lombiq.HelpfulExtensions.FeatureIds;

namespace Lombiq.HelpfulExtensions
{
    [Feature(Testing)]
    public class TestingStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services) =>
            services.AddScoped<ITestingMarkerService, TestingMarkerService>();
    }
}
