using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OrchardCore.ResourceManagement;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.TargetBlank.Filters;

public class TargetBlankFilter : IAsyncResultFilter
{
    private readonly IResourceManager _resourceManager;

    public TargetBlankFilter(IResourceManager resourceManager) => _resourceManager = resourceManager;

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is not ViewResult)
        {
            await next();

            return;
        }

        _resourceManager.RegisterResource("script", "TargetBlank").AtFoot();

        await next();
    }
}
