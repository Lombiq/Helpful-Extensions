using Lombiq.HelpfulExtensions.Extensions.Workflows.Activities;
using Lombiq.HelpfulExtensions.Extensions.Workflows.ViewModels;
using Microsoft.Extensions.Localization;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Mvc.ModelBinding;
using OrchardCore.Workflows.Display;
using OrchardCore.Workflows.Models;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.Workflows.Drivers;

public class GenerateResetPasswordTokenTaskDisplayDriver : ActivityDisplayDriver<
    GenerateResetPasswordTokenTask,
    GenerateResetPasswordTokenTaskViewModel>
{
    private readonly IStringLocalizer T;

    public GenerateResetPasswordTokenTaskDisplayDriver(IStringLocalizer<GenerateResetPasswordTokenTaskDisplayDriver> localizer) =>
        T = localizer;

    protected override void EditActivity(GenerateResetPasswordTokenTask activity, GenerateResetPasswordTokenTaskViewModel model)
    {
        model.UserPropertyKey = activity.UserPropertyKey.Expression;
        model.ResetPasswordTokenPropertyKey = activity.ResetPasswordTokenPropertyKey.Expression;
        model.ResetPasswordUrlPropertyKey = activity.ResetPasswordUrlPropertyKey.Expression;
    }

    public override async Task<IDisplayResult> UpdateAsync(GenerateResetPasswordTokenTask model, IUpdateModel updater)
    {
        var viewModel = new GenerateResetPasswordTokenTaskViewModel();
        if (await updater.TryUpdateModelAsync(viewModel, Prefix))
        {
            model.UserPropertyKey = new WorkflowExpression<string>(viewModel.UserPropertyKey);
            model.ResetPasswordTokenPropertyKey = new WorkflowExpression<string>(viewModel.ResetPasswordTokenPropertyKey);
            model.ResetPasswordUrlPropertyKey = new WorkflowExpression<string>(viewModel.ResetPasswordUrlPropertyKey);

            if (string.IsNullOrEmpty(viewModel.ResetPasswordTokenPropertyKey) &&
                string.IsNullOrEmpty(viewModel.ResetPasswordUrlPropertyKey))
            {
                updater.ModelState.AddModelError(
                    Prefix,
                    "TokenOrUrlIsRequired",
                    T["A value is required for either the token or the URL property key."]);
            }
        }

        return Edit(model);
    }
}
