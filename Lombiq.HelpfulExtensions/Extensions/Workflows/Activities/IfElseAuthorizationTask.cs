#nullable enable

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement;
using OrchardCore.Security.Permissions;
using OrchardCore.Workflows.Abstractions.Models;
using OrchardCore.Workflows.Activities;
using OrchardCore.Workflows.Models;
using OrchardCore.Workflows.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.Workflows.Activities;

public class IfElseAuthorizationTask : TaskActivity<IfElseAuthorizationTask>
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IContentManager _contentManager;
    private readonly IHttpContextAccessor _hca;
    private readonly IEnumerable<IPermissionProvider> _permissionProviders;
    private readonly IWorkflowScriptEvaluator _scriptEvaluator;
    private readonly IStringLocalizer<IfElseAuthorizationTask> T;

    private readonly Dictionary<bool, LocalizedString> _outcomes;

    public override LocalizedString DisplayText => T["If Else Authorization Task"];
    public override LocalizedString Category => T["Control Flow"];

    public string? Permission
    {
        get => GetProperty<string?>();
        set => SetProperty(value);
    }

    public WorkflowExpression<string?> ContentItemId
    {
        get => GetProperty(() => new WorkflowExpression<string?>());
        set => SetProperty(value);
    }

    public IfElseAuthorizationTask(
        IAuthorizationService authorizationService,
        IContentManager contentManager,
        IHttpContextAccessor hca,
        IEnumerable<IPermissionProvider> permissionProviders,
        IWorkflowScriptEvaluator scriptEvaluator,
        IStringLocalizer<IfElseAuthorizationTask> localizer)
    {
        _authorizationService = authorizationService;
        _contentManager = contentManager;
        _hca = hca;
        _permissionProviders = permissionProviders;
        _scriptEvaluator = scriptEvaluator;
        T = localizer;

#pragma warning disable MA0003 // Add parameter name to improve readability, but it's not possible for dictionary keys.
        _outcomes = new()
        {
            [true] = T["Authorized"],
            [false] = T["Rejected"],
        };
#pragma warning restore MA0003 // Add parameter name to improve readability, but it's not possible for dictionary keys.
    }

    public override IEnumerable<Outcome> GetPossibleOutcomes(
        WorkflowExecutionContext workflowContext,
        ActivityContext activityContext) =>
        Outcomes(_outcomes[true], _outcomes[false]);

    public override async Task<ActivityExecutionResult> ExecuteAsync(
        WorkflowExecutionContext workflowContext,
        ActivityContext activityContext)
    {
        var user = _hca.HttpContext?.User;
        var permission = await _permissionProviders.GetPermissionAsync(Permission, _hca.GetCancellation());
        var contentItemId = await _scriptEvaluator.EvaluateAsync(ContentItemId, workflowContext);
        var resource = string.IsNullOrWhiteSpace(contentItemId) ? null : await _contentManager.GetAsync(contentItemId);

        var result = await _authorizationService.AuthorizeAsync(user, permission, resource);
        return Outcomes(_outcomes[result].Name);
    }
}
