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

        const string script = @"
            <script>
                    function targetBlank() {
                        const x=document.querySelectorAll('a');
                        for(let i=0;i<x.length;i++) {
                            if(!x[i].href.match(/^mailto:/)&&(x[i].hostname!==location.hostname&&(!x[i].href.match('javascript:'))))
                            {
                                x[i].setAttribute('target','_blank')
                            }
                        }
                    }
                    window.addEventListener(
                        'load',
                        function() {
                            window.setTimeout(targetBlank,100)
                        },
                        false);
            </script>";

        // Until Node Extensions is ready for use, this solution is here to replace the usage of Gulp. When Node
        // Extensions is ready, this script should be replaced with a JavaScript file.
        _resourceManager.RegisterFootScript(new HtmlString(script));

        await next();
    }
}
