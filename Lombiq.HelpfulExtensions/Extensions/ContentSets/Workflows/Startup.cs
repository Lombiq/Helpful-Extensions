using Lombiq.HelpfulExtensions.Extensions.ContentSets.Events;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Workflows.Activities;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Workflows.Drivers;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Workflows.Handlers;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.Workflows.Helpers;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets.Workflows;

[RequireFeatures("OrchardCore.Workflows")]
public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddActivity<ContentSetGetSupportedOptionsEvent, ContentSetGetSupportedOptionsEventDisplayDriver>();
        services.AddActivity<ContentSetCreatingEvent, ContentSetCreatingEventDisplayDriver>();
        services.AddScoped<IContentSetEventHandler, WorkflowContentSetEventHandler>();
    }
}
