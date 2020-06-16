using Lombiq.HelpfulExtensions;
using Lombiq.HelpfulExtensions.Extensions.Flows;
using Lombiq.HelpfulExtensions.Extensions.Flows.Drivers;
using Lombiq.HelpfulExtensions.Extensions.Flows.Handlers;
using Lombiq.HelpfulExtensions.Extensions.Flows.Models;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.DisplayManagement.Descriptors;
using OrchardCore.Modules;

namespace Piedone.HelpfulExtensions.Extensions.Flows
{
    [Feature(FeatureIds.Flows)]
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IContentDisplayDriver, AdditionalStylingPartDisplay>();
            services.AddScoped<IContentHandler, AdditionalStylingPartHandler>();
            services.AddContentPart<AdditionalStylingPart>();
            services.AddScoped<IShapeTableProvider, FlowPartShapeTableProvider>();
        }
    }
}
