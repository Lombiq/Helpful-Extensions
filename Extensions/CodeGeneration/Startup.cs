using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.Modules;

namespace Lombiq.HelpfulExtensions.Extensions.CodeGeneration
{
    [Feature(FeatureIds.CodeGeneration)]
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services) =>
            services.AddScoped<IContentTypeDefinitionDisplayDriver, CodeGenerationDisplayDriver>();
    }
}
