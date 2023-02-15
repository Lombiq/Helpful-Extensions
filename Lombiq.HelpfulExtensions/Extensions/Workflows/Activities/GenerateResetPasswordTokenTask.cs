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

    public override string Name => nameof(GenerateResetPasswordTokenTask);
    public override LocalizedString DisplayText => T["Generate reset password token"];
    public override LocalizedString Category => T["User"];

    public GenerateResetPasswordTokenTask(
        IStringLocalizer<GenerateResetPasswordTokenTask> localizer,
        LinkGenerator linkGenerator,
        IHttpContextAccessor hca,
        UserManager<IUser> userManager,
        IUserService userService)
    {
        T = localizer;
        _linkGenerator = linkGenerator;
        _hca = hca;
        _userManager = userManager;
        _userService = userService;
    }

    public override IEnumerable<Outcome> GetPossibleOutcomes(
        WorkflowExecutionContext workflowContext,
        ActivityContext activityContext) =>
        Outcomes(T["Done"]);

    public override async Task<ActivityExecutionResult> ExecuteAsync(
        WorkflowExecutionContext workflowContext,
        ActivityContext activityContext)
    {
        var user = workflowContext.Input["User"] as User ?? workflowContext.Properties["User"] as User;
        if (user == null && _hca.HttpContext.User.Identity.IsAuthenticated)
        {
            user = await _userService.GetAuthenticatedUserAsync(_hca.HttpContext.User) as User;
        }

        if (user == null) return Outcomes("Done", "Done");

        var generatedToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        user.ResetToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(generatedToken));
        workflowContext.Properties["ResetPasswordToken"] = user.ResetToken;

        var resetPasswordUrl = _linkGenerator.GetUriByAction(
            _hca.HttpContext,
            "ResetPassword",
            "ResetPassword",
            new { area = "OrchardCore.Users", code = user.ResetToken });
        workflowContext.Properties["ResetPasswordUrl"] = resetPasswordUrl;

        return Outcomes("Done", "Done");
    }
}
