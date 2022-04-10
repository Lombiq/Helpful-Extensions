using Lombiq.HelpfulExtensions.Extensions.Emails.Services;
using Lombiq.HelpfulLibraries.OrchardCore.Shapes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using System;

namespace Lombiq.HelpfulExtensions.Extensions.Emails;

[Feature(FeatureIds.Emails)]
public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddShapeRenderer();
        services.AddScoped<IEmailTemplateService, ShapeBasedEmailTemplateService>();
    }

    public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
    {
        // No need for anything here yet.
    }
}
