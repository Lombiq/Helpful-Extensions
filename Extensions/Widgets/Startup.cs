using Lombiq.HelpfulExtensions.Extensions.Widgets;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using static Lombiq.HelpfulExtensions.FeatureIds;

namespace Piedone.HelpfulExtensions.Extensions.Widgets
{
    [Feature(Lombiq_HelpfulExtensions_Widgets)]
    public class Startup : StartupBase
    {
        override public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, Migrations>();
        }
    }
}
