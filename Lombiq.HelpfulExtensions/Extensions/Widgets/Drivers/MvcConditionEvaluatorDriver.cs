using Lombiq.HelpfulExtensions.Extensions.Widgets.Models;
using Microsoft.AspNetCore.Http;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.Modules;
using OrchardCore.Rules;
using System.Linq;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.Widgets.Drivers;

public class MvcConditionEvaluatorDriver(IHttpContextAccessor hca) : ContentDisplayDriver, IConditionEvaluator
{
    public ValueTask<bool> EvaluateAsync(Condition condition) => new(Evaluate((MvcCondition)condition));

    private bool Evaluate(MvcCondition condition) =>
        MatchRouteValue("area", condition.Area) &&
        MatchRouteValue("controller", condition.Controller) &&
        MatchRouteValue("action", condition.Action) &&
        (!condition.OtherRouteValues.Any() || condition.OtherRouteValues.All(pair => MatchRouteValue(pair.Key, pair.Value)));

    private bool MatchRouteValue(string name, string value)
    {
        // Ignore this match operation if the target value is not set.
        if (string.IsNullOrWhiteSpace(value)) return true;

        return hca.HttpContext?.Request.RouteValues.TryGetValue(name, out var routeValue) == true &&
               value.EqualsOrdinalIgnoreCase(routeValue?.ToString());
    }
}
