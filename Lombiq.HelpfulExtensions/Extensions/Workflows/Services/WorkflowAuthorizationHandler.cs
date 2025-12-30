#nullable enable

using System;
using Lombiq.HelpfulExtensions.Extensions.Workflows.Activities;
using Microsoft.AspNetCore.Authorization;
using OrchardCore.ContentManagement;
using OrchardCore.Workflows.Models;
using OrchardCore.Workflows.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.Workflows.Services;

public class WorkflowAuthorizationHandler : IAuthorizationHandler
{
    public const string ExpectedOutputName = "Authorization";

    private readonly IWorkflowManager _workflowManager;
    private readonly IWorkflowTypeStore _workflowTypeStore;

    public WorkflowAuthorizationHandler(IWorkflowManager workflowManager, IWorkflowTypeStore workflowTypeStore)
    {
        _workflowManager = workflowManager;
        _workflowTypeStore = workflowTypeStore;
    }

    public async Task HandleAsync(AuthorizationHandlerContext context)
    {
        if (context.HasFailed ||
            context.Resource is not ContentItem contentItem ||
            context.Requirements.FirstOrDefault() is not { } requirement)
        {
            return;
        }

        var values = new Dictionary<string, object>
        {
            [nameof(ContentItem)] = contentItem,
            [nameof(context.Requirements)] = context.Requirements,
            [nameof(context.PendingRequirements)] = context.PendingRequirements,
            [nameof(context.User)] = new
            {
                Claims = context.User.Claims.Select(claim => new { claim.Type, claim.Value }),
                context.User.Identity,
            },
        };

        var workflowTypesToStart = await _workflowTypeStore.GetByStartActivityAsync(nameof(AuthorizationEvent));
        var contexts = await workflowTypesToStart
            .SelectMany(workflowType => workflowType
                .Activities
                .Where(activity => activity.IsStart)
                .Select(activity => (Type: workflowType, Activity: activity)))
            .AwaitEachAsync(pair => _workflowManager.StartWorkflowAsync(pair.Type, pair.Activity, values));

        var authorizationResult = contexts.Select(GetBoolean).FirstOrDefault(result => result.HasValue);
        if (authorizationResult == true)
        {
            context.Succeed(requirement);
        }
        else if (authorizationResult == false)
        {
            context.Fail();
        }
    }

    private bool? GetBoolean(WorkflowExecutionContext context)
    {
        if (context.Status is WorkflowStatus.Aborted or WorkflowStatus.Faulted or WorkflowStatus.Halted) return false;

        if (context.Output.TryGetValue(ExpectedOutputName, out var resultObject) &&
            resultObject.ToString()?.Trim() is { Length: > 0 } result)
        {
            return result.EqualsOrdinalIgnoreCase("true");
        }

        return null;
    }
}
