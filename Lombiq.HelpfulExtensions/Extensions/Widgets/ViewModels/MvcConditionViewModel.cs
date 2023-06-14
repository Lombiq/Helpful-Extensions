using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Lombiq.HelpfulExtensions.Extensions.Widgets.ViewModels;

public class MvcConditionViewModel
{
    public string Area { get; set; }
    public string Controller { get; set; }
    public string Action { get; set; }

    public IList<string> OtherRouteNames { get; } = new List<string>();
    public IList<string> OtherRouteValues { get; } = new List<string>();
}
