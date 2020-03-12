using Lombiq.HelpfulExtensions.Extensions.Flows;
using Lombiq.HelpfulExtensions.Flows.Drivers;
using Lombiq.HelpfulExtensions.Flows.Handlers;
using Lombiq.HelpfulExtensions.Flows.Models;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.DisplayManagement.Descriptors;
using OrchardCore.Modules;
using static Lombiq.HelpfulExtensions.FeatureIds;

namespace Piedone.HelpfulExtensions.Extensions.Flows
{
    [Feature(Lombiq_HelpfulExtensions_Flows)]
    public class Startup : StartupBase
    {
        override public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IContentDisplayDriver, AdditionalStylingPartDisplay>();
            services.AddScoped<IContentHandler, AdditionalStylingPartHandler>();
            services.AddContentPart<AdditionalStylingPart>();
            services.AddScoped<IShapeTableProvider, FlowPartShapeTableProvider>();
        }
    }
}
