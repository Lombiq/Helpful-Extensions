using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OrchardCore.ResourceManagement;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.TargetBlank.Filters;
public class TargetBlankFilter : IAsyncResultFilter
{
    private readonly IResourceManager _resourceManager;

    public TargetBlankFilter(
        IResourceManager resourceManager) => _resourceManager = resourceManager;

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is not ViewResult)
        {
            await next();

            return;
        }

        // Until NodeExtensions is not being used this solution is for to replace the usage of GULP
        // When NE will come alive this script should be refactored and store as a .js file.
        _resourceManager.RegisterFootScript(new HtmlString(
            "<script>function targetBlank(){const x=document.querySelectorAll('a');" +
            "for(let i=0;i<x.length;i++){if(!x[i].href.match(/^mailto:/)&&(x[i].hostname!==location.hostname))" +
            "{x[i].setAttribute('target','_blank')}}}window.addEventListener('load',function()" +
            "{window.setTimeout(targetBlank,100)},false);</script>"));

        await next();
    }
}
