using Lombiq.HelpfulExtensions.Extensions.ContentTypes;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using static Lombiq.HelpfulExtensions.FeatureIds;

namespace Piedone.HelpfulExtensions.Extensions.ContentTypes
{
    [Feature(Lombiq_HelpfulExtensions_ContentTypes)]
    public class Startup : StartupBase
    {
        override public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, Migrations>();
        }
    }
}
