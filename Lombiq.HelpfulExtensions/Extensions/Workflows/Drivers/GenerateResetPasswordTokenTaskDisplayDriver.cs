using Lombiq.HelpfulExtensions.Extensions.Workflows.Activities;
using Lombiq.HelpfulExtensions.Extensions.Workflows.ViewModels;
using Microsoft.Extensions.Localization;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Workflows.Display;
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
        model.UserExpression = activity.User.Expression;
        model.ResetPasswordTokenPropertyKey = activity.ResetPasswordTokenPropertyKey;
        model.ResetPasswordUrlPropertyKey = activity.ResetPasswordUrlPropertyKey;
    }

    public override async Task<IDisplayResult> UpdateAsync(GenerateResetPasswordTokenTask activity, UpdateEditorContext context)
    {
        var viewModel = await context.CreateModelAsync<GenerateResetPasswordTokenTaskViewModel>(Prefix);

        activity.User = new(viewModel.UserExpression);
        activity.ResetPasswordTokenPropertyKey = viewModel.ResetPasswordTokenPropertyKey;
        activity.ResetPasswordUrlPropertyKey = viewModel.ResetPasswordUrlPropertyKey;

        return await EditAsync(activity, context);
    }
}
