#nullable enable

using Lombiq.HelpfulExtensions.Extensions.Workflows.Activities;
using Lombiq.HelpfulExtensions.Extensions.Workflows.ViewModels;
using OrchardCore.Workflows.Display;
using OrchardCore.Workflows.Models;

namespace Lombiq.HelpfulExtensions.Extensions.Workflows.Drivers;

public class IfElseAuthorizationTaskDisplayDriver : ActivityDisplayDriver<IfElseAuthorizationTask, IfElseAuthorizationTaskViewModel>
{
    protected override void EditActivity(IfElseAuthorizationTask activity, IfElseAuthorizationTaskViewModel model)
    {
        model.Permission = activity.Permission;
        model.ContentItemIdExpression = activity.ContentItemId.Expression;
    }

    protected override void UpdateActivity(IfElseAuthorizationTaskViewModel model, IfElseAuthorizationTask activity)
    {
        activity.Permission = model.Permission;
        activity.ContentItemId = new WorkflowExpression<string?>(model.ContentItemIdExpression);
    }
}
