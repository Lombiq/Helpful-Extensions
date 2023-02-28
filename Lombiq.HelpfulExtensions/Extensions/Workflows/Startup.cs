using Fluid;
using Lombiq.HelpfulExtensions.Extensions.Workflows.Activities;
using Lombiq.HelpfulExtensions.Extensions.Workflows.Drivers;
using Lombiq.HelpfulExtensions.Extensions.Workflows.Models;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.Workflows.Helpers;

namespace Lombiq.HelpfulExtensions.Extensions.Workflows;

[Feature(FeatureIds.ResetPasswordActivity)]
public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddActivity<GenerateResetPasswordTokenTask, GenerateResetPasswordTokenTaskDisplayDriver>();
        services.Configure<TemplateOptions>(option =>
            option.MemberAccessStrategy.Register<GenerateResetPasswordTokenResult>());
    }
}
