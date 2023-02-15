using Lombiq.HelpfulExtensions.Extensions.Workflows.Activities;
using Lombiq.HelpfulExtensions.Extensions.Workflows.Drivers;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.Workflows.Helpers;

namespace Lombiq.HelpfulExtensions.Extensions.Workflows;

[Feature(FeatureIds.Workflows)]
public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services) =>
        services.AddActivity<GenerateResetPasswordTokenTask, GenerateResetPasswordTokenTaskDisplayDriver>();
}
