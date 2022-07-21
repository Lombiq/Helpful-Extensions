using Lombiq.HelpfulExtensions.Extensions.Security.Driver;
using Lombiq.HelpfulExtensions.Extensions.Security.Services;
using Lombiq.HelpfulLibraries.Common.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.Modules;

namespace Lombiq.HelpfulExtensions.Extensions.Security;

[Feature(FeatureIds.Security)]
public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddLazyInjectionSupport();
        services.AddScoped<IAuthorizationHandler, StrictSecurityPermissionAuthorizationHandler>();
        services.AddScoped<IContentTypeDefinitionDisplayDriver, StrictSecuritySettingsDisplayDriver>();
    }
}
