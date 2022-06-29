using Lombiq.HelpfulExtensions.Extensions.TargetBlank.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using System;

namespace Lombiq.HelpfulExtensions.Extensions.TargetBlank;

[Feature(FeatureIds.TargetBlank)]
public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services) =>
        services.Configure<MvcOptions>(options => options.Filters.Add(typeof(TargetBlankFilter)));

    public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
    {
        // No need for anything here yet.
    }
}
