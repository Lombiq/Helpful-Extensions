using Lombiq.HelpfulExtensions.Services;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace Lombiq.HelpfulExtensions
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services) =>
            services.AddScoped<IOrchardServices, OrchardServices>();
    }
}
