using Lombiq.HelpfulExtensions.Extensions.ContentSets.Drivers;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Models;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.Modules;
using System;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets;

[Feature(FeatureIds.ContentSets)]
public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services
            .AddContentPart<ContentSetPart>()
            .UseDisplayDriver<ContentSetPartDisplayDriver>();

        services.AddScoped<IContentSetManager, ContentSetManager>();
    }

    public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
    {
        // No need for anything here yet.
    }
}
