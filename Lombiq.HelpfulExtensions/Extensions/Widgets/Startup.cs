using Lombiq.HelpfulExtensions.Extensions.Widgets.Drivers;
using Lombiq.HelpfulExtensions.Extensions.Widgets.Models;
using Lombiq.HelpfulLibraries.OrchardCore.TagHelpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.Modules;
using OrchardCore.Rules;
using System;

namespace Lombiq.HelpfulExtensions.Extensions.Widgets;

[Feature(FeatureIds.Widgets)]
public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddDataMigration<Migrations>();

        services
            .AddScoped<IDisplayDriver<Condition>, MvcConditionDisplayDriver>()
            .AddCondition<MvcCondition, MvcConditionEvaluatorDriver, ConditionFactory<MvcCondition>>()
            .AddScoped(sp => (IContentDisplayDriver)sp.GetRequiredService<MvcConditionEvaluatorDriver>());

        services.AddTagHelpers<EditorFieldSetTagHelper>();

        services.AddContentPart<ContentItemWidget>()
            .UseDetailOnlyDriver();
    }

    public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
    {
        // No need for anything here yet.
    }
}
