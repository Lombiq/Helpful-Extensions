using Lombiq.HelpfulExtensions.Extensions.Security.Driver;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.Modules;
using System;

namespace Lombiq.HelpfulExtensions.Extensions.Widgets
{
    [Feature(FeatureIds.Widgets)]
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services) =>
            services.AddScoped<IContentTypeDefinitionDisplayDriver, StrictSecuritySettingsDisplayDriver>();

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            // No need for anything here yet.
        }
    }
}
