#nullable enable

using Lombiq.HelpfulExtensions.Extensions.Workflows.Activities;
using Lombiq.HelpfulExtensions.Extensions.Workflows.Services;
using Lombiq.HelpfulExtensions.Extensions.Workflows.ViewModels;
using Lombiq.HelpfulLibraries.OrchardCore.Workflow;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Notify;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Workflows.Models;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.Workflows.Drivers;

public class AuthorizationEventDisplayDriver : SimpleEventActivityDisplayDriverBase<AuthorizationEvent>
{
    private readonly INotifier _notifier;
    private readonly IHtmlLocalizer H;

    public override string IconClass => "fa-key";

    [SuppressMessage("Major Code Smell", "S103:Lines should not be too long", Justification = "Can't split localization strings.")]
    public override LocalizedHtmlString Description =>
        H["Executed by the matching the authorization handler. If the workflow status is <code>{0}</code>, <code>{1}</code>, or <code>{2}</code> then the authorization is failed. Also, if the workflow contains the output <code>{3}</code>, then authorization is succeeded if it's <code>{4}</code> (case-insensitive) or failed if not.", WorkflowStatus.Aborted, WorkflowStatus.Faulted, WorkflowStatus.Halted, WorkflowAuthorizationHandler.ExpectedOutputName, "true"];

    public AuthorizationEventDisplayDriver(INotifier notifier, IHtmlLocalizer<AuthorizationEventDisplayDriver> htmlLocalizer)
    {
        _notifier = notifier;
        H = htmlLocalizer;
    }

    public override IDisplayResult Edit(AuthorizationEvent activity, BuildEditorContext context) =>
        Initialize<AuthorizationEventViewModel>(
                nameof(AuthorizationEvent) + "_Edit",
                viewModel =>
                {
                    viewModel.ContentTypes = activity.ContentTypes;
                    viewModel.Permissions = activity.Permissions;
                })
            .PlaceInContent();

    public override async Task<IDisplayResult> UpdateAsync(AuthorizationEvent activity, UpdateEditorContext context)
    {
        var viewModel = new AuthorizationEventViewModel();
        await context.Updater.TryUpdateModelAsync(viewModel, Prefix);

        activity.ContentTypes = viewModel.ContentTypes ?? [];
        activity.Permissions = viewModel.Permissions ?? [];

        if (!activity.ContentTypes.Any())
        {
            var message = H["You must select at least one content type."];
            await _notifier.ErrorAsync(message);
            context.AddModelError(nameof(viewModel.ContentTypes), new(message.Name, message.Value));
        }

        return await EditAsync(activity, context);
    }
}
