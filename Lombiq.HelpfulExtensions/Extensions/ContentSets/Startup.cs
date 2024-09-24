using Lombiq.HelpfulExtensions.Extensions.ContentSets.Drivers;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Indexes;
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
public sealed class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services
            .AddContentPart<ContentSetPart>()
            .UseDisplayDriver<ContentSetPartDisplayDriver>()
            .WithIndex<ContentSetIndexProvider>()
            .WithMigration<Migrations>();

        services.AddScoped<IContentSetManager, ContentSetManager>();

        services.AddContentField<ContentSetContentPickerField>()
            .UseDisplayDriver<ContentSetContentPickerFieldDisplayDriver>();
    }

    public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
    {
        // No need for anything here yet.
    }
}
