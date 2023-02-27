using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using OrchardCore.Users;
using OrchardCore.Users.Models;
using OrchardCore.Users.Services;
using OrchardCore.Workflows.Abstractions.Models;
using OrchardCore.Workflows.Activities;
using OrchardCore.Workflows.Models;
using OrchardCore.Workflows.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.Workflows.Activities;

public class GenerateResetPasswordTokenTask : TaskActivity
{
    private readonly IStringLocalizer<GenerateResetPasswordTokenTask> T;
    private readonly LinkGenerator _linkGenerator;
    private readonly IHttpContextAccessor _hca;
    private readonly UserManager<IUser> _userManager;
    private readonly IUserService _userService;
    private readonly IWorkflowExpressionEvaluator _expressionEvaluator;

    public override string Name => nameof(GenerateResetPasswordTokenTask);
    public override LocalizedString DisplayText => T["Generate reset password token"];
    public override LocalizedString Category => T["User"];

    public WorkflowExpression<string> UserPropertyKey
    {
        get => GetProperty(() => new WorkflowExpression<string>());
        set => SetProperty(value);
    }

    public WorkflowExpression<string> ResetPasswordTokenPropertyKey
    {
        get => GetProperty(() => new WorkflowExpression<string>());
        set => SetProperty(value);
    }

    public WorkflowExpression<string> ResetPasswordUrlPropertyKey
    {
        get => GetProperty(() => new WorkflowExpression<string>());
        set => SetProperty(value);
    }

    public GenerateResetPasswordTokenTask(
        IStringLocalizer<GenerateResetPasswordTokenTask> localizer,
        LinkGenerator linkGenerator,
        IHttpContextAccessor hca,
        UserManager<IUser> userManager,
        IUserService userService,
        IWorkflowExpressionEvaluator expressionEvaluator)
    {
        T = localizer;
        _linkGenerator = linkGenerator;
        _hca = hca;
        _userManager = userManager;
        _userService = userService;
        _expressionEvaluator = expressionEvaluator;
    }

    public override IEnumerable<Outcome> GetPossibleOutcomes(
        WorkflowExecutionContext workflowContext,
        ActivityContext activityContext) =>
        Outcomes(T["Done"], T["Error"]);

    public override async Task<ActivityExecutionResult> ExecuteAsync(
        WorkflowExecutionContext workflowContext,
        ActivityContext activityContext)
    {
        var userPropertyKey = !string.IsNullOrEmpty(UserPropertyKey.Expression)
            ? await _expressionEvaluator.EvaluateAsync(UserPropertyKey, workflowContext, encoder: null)
            : null;

        var user = string.IsNullOrEmpty(userPropertyKey)
            ? workflowContext.Input.GetMaybe("User") as User
            : workflowContext.Properties.GetMaybe(userPropertyKey) as User ??
                await _userService.GetUserByUniqueIdAsync(workflowContext.Properties.GetMaybe(userPropertyKey) as string) as User;

        if (user == null) return Outcomes("Error");

        var resetPasswordTokenPropertyKey = !string.IsNullOrEmpty(ResetPasswordTokenPropertyKey.Expression)
            ? await _expressionEvaluator.EvaluateAsync(ResetPasswordTokenPropertyKey, workflowContext, encoder: null)
            : null;
        var resetPasswordUrlPropertyKey = !string.IsNullOrEmpty(ResetPasswordUrlPropertyKey.Expression)
            ? await _expressionEvaluator.EvaluateAsync(ResetPasswordUrlPropertyKey, workflowContext, encoder: null)
            : null;

        var generatedToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        user.ResetToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(generatedToken));
        if (!string.IsNullOrEmpty(resetPasswordTokenPropertyKey))
        {
            workflowContext.Properties[resetPasswordTokenPropertyKey] = user.ResetToken;
        }

        if (!string.IsNullOrEmpty(resetPasswordUrlPropertyKey))
        {
            var resetPasswordUrl = _linkGenerator.GetUriByAction(
                _hca.HttpContext,
                "ResetPassword",
                "ResetPassword",
                new { area = "OrchardCore.Users", code = user.ResetToken });
            workflowContext.Properties[resetPasswordUrlPropertyKey] = resetPasswordUrl;
        }

        return Outcomes("Done");
    }
}
