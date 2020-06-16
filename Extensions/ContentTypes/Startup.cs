using Lombiq.HelpfulExtensions;
using Lombiq.HelpfulExtensions.Extensions.ContentTypes;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;

namespace Piedone.HelpfulExtensions.Extensions.ContentTypes
{
    [Feature(FeatureIds.ContentTypes)]
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services) => services.AddScoped<IDataMigration, Migrations>();
    }
}
