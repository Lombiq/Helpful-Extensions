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

public class GenerateResetPasswordTokenTask(
    IStringLocalizer<GenerateResetPasswordTokenTask> localizer,
    LinkGenerator linkGenerator,
    IHttpContextAccessor hca,
    UserManager<IUser> userManager,
    IWorkflowScriptEvaluator workflowScriptEvaluator) : TaskActivity
{
    private readonly IStringLocalizer T = localizer;

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

    public override IEnumerable<Outcome> GetPossibleOutcomes(
        WorkflowExecutionContext workflowContext,
        ActivityContext activityContext) =>
        Outcomes(T["Done"], T["Error"]);

    public override async Task<ActivityExecutionResult> ExecuteAsync(
        WorkflowExecutionContext workflowContext,
        ActivityContext activityContext)
    {
        var user = await workflowScriptEvaluator.EvaluateAsync(User, workflowContext);

        if (user == null) return Outcomes("Error");

        var generatedToken = await userManager.GeneratePasswordResetTokenAsync(user);
        user.ResetToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(generatedToken));
        var resetPasswordUrl = linkGenerator.GetUriByAction(
            hca.HttpContext,
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
