using Fluid;
using Lombiq.HelpfulExtensions.Extensions.Workflows.Activities;
using Lombiq.HelpfulExtensions.Extensions.Workflows.Drivers;
using Lombiq.HelpfulExtensions.Extensions.Workflows.Models;
using Lombiq.HelpfulExtensions.Extensions.Workflows.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.Workflows.Helpers;

namespace Lombiq.HelpfulExtensions.Extensions.Workflows;

[Feature(FeatureIds.ResetPasswordActivity)]
public sealed class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddActivity<GenerateResetPasswordTokenTask, GenerateResetPasswordTokenTaskDisplayDriver>();
        services.Configure<TemplateOptions>(option =>
            option.MemberAccessStrategy.Register<GenerateResetPasswordTokenResult>());
    }
}

[Feature(FeatureIds.Authorize)]
public sealed class AuthorizeStartup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IAuthorizationHandler, WorkflowAuthorizationHandler>();
        services.AddActivity<AuthorizationEvent, AuthorizationEventDisplayDriver>();
        services.AddActivity<IfElseAuthorizationTask, IfElseAuthorizationTaskDisplayDriver>();
        services.AddScoped<IfElseAuthorizationTask>();
    }
}
