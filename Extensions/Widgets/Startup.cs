using Lombiq.HelpfulExtensions.Extensions.Widgets;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using Lombiq.HelpfulExtensions;

namespace Piedone.HelpfulExtensions.Extensions.Widgets
{
    [Feature(FeatureIds.Widgets)]
    public class Startup : StartupBase
    {
        override public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, Migrations>();
        }
    }
}
