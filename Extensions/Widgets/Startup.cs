using Lombiq.HelpfulExtensions;
using Lombiq.HelpfulExtensions.Extensions.Widgets;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;

namespace Piedone.HelpfulExtensions.Extensions.Widgets
{
    [Feature(FeatureIds.Widgets)]
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services) => services.AddScoped<IDataMigration, Migrations>();
    }
}
