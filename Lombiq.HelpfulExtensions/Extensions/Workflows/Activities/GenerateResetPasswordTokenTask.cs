using Lombiq.HelpfulExtensions.Extensions.Workflows.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using OrchardCore.Users;
using OrchardCore.Users.Models;
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
    private readonly IWorkflowScriptEvaluator _workflowScriptEvaluator;

    public override string Name => nameof(GenerateResetPasswordTokenTask);
    public override LocalizedString DisplayText => T["Generate reset password token"];
    public override LocalizedString Category => T["User"];

    public WorkflowExpression<User> User
    {
        get => GetProperty(() => new WorkflowExpression<User>());
        set => SetProperty(value);
    }

    public string ResetPasswordTokenPropertyKey
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public string ResetPasswordUrlPropertyKey
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public GenerateResetPasswordTokenTask(
        IStringLocalizer<GenerateResetPasswordTokenTask> localizer,
        LinkGenerator linkGenerator,
        IHttpContextAccessor hca,
        UserManager<IUser> userManager,
        IWorkflowScriptEvaluator workflowScriptEvaluator)
    {
        T = localizer;
        _linkGenerator = linkGenerator;
        _hca = hca;
        _userManager = userManager;
        _workflowScriptEvaluator = workflowScriptEvaluator;
    }

    public override IEnumerable<Outcome> GetPossibleOutcomes(
        WorkflowExecutionContext workflowContext,
        ActivityContext activityContext) =>
        Outcomes(T["Done"], T["Error"]);

    public override async Task<ActivityExecutionResult> ExecuteAsync(
        WorkflowExecutionContext workflowContext,
        ActivityContext activityContext)
    {
        var user = !string.IsNullOrEmpty(User.Expression)
            ? await _workflowScriptEvaluator.EvaluateAsync(User, workflowContext)
            : null;

        if (user == null) return Outcomes("Error");

        var generatedToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        user.ResetToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(generatedToken));
        var resetPasswordUrl = _linkGenerator.GetUriByAction(
            _hca.HttpContext,
            "ResetPassword",
            "ResetPassword",
            new { area = "OrchardCore.Users", code = user.ResetToken });

        workflowContext.LastResult = new GenerateResetPasswordTokenResult
        {
            ResetPasswordToken = user.ResetToken,
            ResetPasswordUrl = resetPasswordUrl,
        };

        if (!string.IsNullOrEmpty(ResetPasswordTokenPropertyKey))
        {
            workflowContext.Properties[ResetPasswordTokenPropertyKey] = user.ResetToken;
        }

        if (!string.IsNullOrEmpty(ResetPasswordUrlPropertyKey))
        {
            workflowContext.Properties[ResetPasswordUrlPropertyKey] = resetPasswordUrl;
        }

        return Outcomes("Done");
    }
}
