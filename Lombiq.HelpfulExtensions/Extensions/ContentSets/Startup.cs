using Lombiq.HelpfulExtensions.Extensions.Activities;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Drivers;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Events;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Indexes;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Models;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.Modules;
using OrchardCore.Workflows.Helpers;
using System;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets;

[Feature(FeatureIds.ContentSets)]
public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services
            .AddContentPart<ContentSetPart>()
            .UseDisplayDriver<ContentSetPartDisplayDriver>()
            .WithIndex<ContentSetIndexProvider>()
            .WithMigration<Migrations>();

        services.AddScoped<IContentSetManager, ContentSetManager>();
    }

    public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
    {
        // No need for anything here yet.
    }
}

[RequireFeatures("OrchardCore.Workflows")]
public class WorkflowStartup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddActivity<ContentSetGetSupportedOptionsEvent, ContentSetGetSupportedOptionsEventDisplayDriver>();
        services.AddActivity<ContentSetCreatingEvent, ContentSetCreatingEventDisplayDriver>();
        services.AddScoped<IContentSetEventHandler, WorkflowContentSetEventHandler>();
    }
}
