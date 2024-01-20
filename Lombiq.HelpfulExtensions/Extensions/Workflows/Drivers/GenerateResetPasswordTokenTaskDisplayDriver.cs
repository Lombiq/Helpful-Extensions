using Lombiq.HelpfulExtensions.Extensions.Workflows.Activities;
using Lombiq.HelpfulExtensions.Extensions.Workflows.ViewModels;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Users.Models;
using OrchardCore.Workflows.Display;
using OrchardCore.Workflows.Models;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.Workflows.Drivers;

public class GenerateResetPasswordTokenTaskDisplayDriver
    : ActivityDisplayDriver<GenerateResetPasswordTokenTask, GenerateResetPasswordTokenTaskViewModel>
{
    protected override void EditActivity(GenerateResetPasswordTokenTask activity, GenerateResetPasswordTokenTaskViewModel model)
    {
        model.UserExpression = activity.User.Expression;
        model.ResetPasswordTokenPropertyKey = activity.ResetPasswordTokenPropertyKey;
        model.ResetPasswordUrlPropertyKey = activity.ResetPasswordUrlPropertyKey;
    }

    public override async Task<IDisplayResult> UpdateAsync(GenerateResetPasswordTokenTask model, IUpdateModel updater)
    {
        var viewModel = new GenerateResetPasswordTokenTaskViewModel();
        if (await updater.TryUpdateModelAsync(viewModel, Prefix))
        {
            model.User = new WorkflowExpression<User>(viewModel.UserExpression);
            model.ResetPasswordTokenPropertyKey = viewModel.ResetPasswordTokenPropertyKey;
            model.ResetPasswordUrlPropertyKey = viewModel.ResetPasswordUrlPropertyKey;
        }

        return Edit(model);
    }
}
