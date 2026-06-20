using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.Extensions;
using OrchardCore.DisplayManagement.Liquid;
using OrchardCore.Liquid;
using OrchardCore.Modules;
using OrchardCore.Workflows.Models;
using OrchardCore.Workflows.Services;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using TimeZoneConverter;

namespace Lombiq.HelpfulExtensions.Extensions.Liquid.Services;

/// <summary>
/// A workaround until the next Orchard Core release that contains https://github.com/OrchardCMS/OrchardCore/pull/19400/.
/// </summary>
public class LiquidViewTemplateWorkflowExecutionContextHandler : WorkflowExecutionContextHandlerBase
{
    public override async Task EvaluatingExpressionAsync(WorkflowExecutionExpressionContext context)
    {
        if (context.TemplateContext is LiquidTemplateContext liquidTemplateContext)
        {
            var viewContext = liquidTemplateContext.Services.GetRequiredService<ViewContextAccessor>()?.ViewContext;
            await InitializeAsync(liquidTemplateContext, viewContext);
        }
    }

#pragma warning disable S103 // Lines should not be too long; but this is one big link.
    // Copied from https://github.com/Lombiq/OrchardCore/blob/ab3c76ca68382d3adcf0f5ca54dc2af96986ac47/src/OrchardCore/OrchardCore.DisplayManagement.Liquid/LiquidViewTemplate.cs with no changes that need to be tracked.
#pragma warning restore S103 // Lines should not be too long; but this is one big link.
    private static async ValueTask InitializeAsync(LiquidTemplateContext context, ViewContext viewContext)
    {
        if (!context.IsInitialized)
        {
            // Try to create fallback view context if none exists. This only works if an HTTP context is available.
            if (viewContext == null &&
                context.Services.GetRequiredService<IHttpContextAccessor>().HttpContext is { } httpContext &&
                await httpContext.GetActionContextAsync() is { } actionContext)
            {
                viewContext = GetViewContext(actionContext);
            }

            var localClock = context.Services.GetRequiredService<ILocalClock>();

            // Configure Fluid with the time zone to represent local date and times
            var localTimeZone = await localClock.GetLocalTimeZoneAsync();

            if (TZConvert.TryGetTimeZoneInfo(localTimeZone.TimeZoneId, out var timeZoneInfo))
            {
                context.TimeZone = timeZoneInfo;
            }

            // Configure Fluid with the local date and time
            var now = await localClock.GetLocalNowAsync();

            context.Now = () => now;

            context.ViewContext = viewContext;

            context.CultureInfo = CultureInfo.CurrentUICulture;

            context.IsInitialized = true;
        }
    }

    private static ViewContext GetViewContext(ActionContext actionContext)
    {
        var services = actionContext.HttpContext.RequestServices;

        var options = services.GetService<IOptions<MvcViewOptions>>();
        var viewEngine = options.Value.ViewEngines[0];

        var viewResult = viewEngine.GetView(
            executingFilePath: null,
            LiquidViewsFeatureProvider.DefaultRazorViewPath,
            isMainPage: true);

        var tempDataProvider = services.GetService<ITempDataProvider>();

        var viewContext = new ViewContext(
            actionContext,
            viewResult.View,
            new ViewDataDictionary(
                metadataProvider: new EmptyModelMetadataProvider(),
                modelState: new ModelStateDictionary()),
            new TempDataDictionary(
                actionContext.HttpContext,
                tempDataProvider),
            TextWriter.Null,
            new HtmlHelperOptions());

        if (viewContext.View is RazorView razorView)
        {
            razorView.RazorPage.ViewContext = viewContext;
        }

        return viewContext;
    }
}
