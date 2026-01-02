#nullable enable

using Lombiq.HelpfulExtensions.Extensions.Workflows.Activities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement;
using OrchardCore.Security;
using OrchardCore.Workflows.Models;
using OrchardCore.Workflows.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.Workflows.Services;

public class WorkflowAuthorizationHandler : IAuthorizationHandler
{
    public const string ExpectedOutputName = "Authorization";

    private readonly IWorkflowManager _workflowManager;
    private readonly IWorkflowTypeStore _workflowTypeStore;
    private readonly IStringLocalizer<AuthorizationEvent> _eventStringLocalizer;

    public WorkflowAuthorizationHandler(
        IWorkflowManager workflowManager,
        IWorkflowTypeStore workflowTypeStore,
        IStringLocalizer<AuthorizationEvent> eventStringLocalizer)
    {
        _workflowManager = workflowManager;
        _workflowTypeStore = workflowTypeStore;
        _eventStringLocalizer = eventStringLocalizer;
    }

    public async Task HandleAsync(AuthorizationHandlerContext context)
    {
        if (context.HasFailed ||
            context.Resource is not ContentItem contentItem ||
            context.Requirements.CastWhere<PermissionRequirement>().ToList() is not { Count: > 0 } requirements)
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
                .Where(activity => activity.IsStart && activity.Name == nameof(AuthorizationEvent))
                .Select(activity => new
                {
                    Type = workflowType,
                    Activity = activity,
                    Event = AuthorizationEvent.FromActivityRecord(activity, _eventStringLocalizer),
                }))
                .Where(workflow =>
                    workflow.Event.ContentTypes.Contains(contentItem.ContentType) &&
                    (!workflow.Event.Permissions.Any() ||
                     requirements.Any(requirement => workflow.Event.Permissions.Contains(requirement.Permission.Name))))
            .AwaitEachAsync(workflow => _workflowManager.StartWorkflowAsync(workflow.Type, workflow.Activity, values));

        var authorizationResult = contexts.Select(GetBoolean).FirstOrDefault(result => result.HasValue);
        if (authorizationResult == true)
        {
            requirements.ForEach(context.Succeed);
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
