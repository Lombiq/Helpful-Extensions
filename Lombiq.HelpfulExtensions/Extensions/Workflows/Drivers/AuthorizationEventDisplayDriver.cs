#nullable enable

using Lombiq.HelpfulExtensions.Extensions.Workflows.Activities;
using Lombiq.HelpfulExtensions.Extensions.Workflows.Services;
using Lombiq.HelpfulLibraries.OrchardCore.Workflow;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.Workflows.Models;
using System.Diagnostics.CodeAnalysis;

namespace Lombiq.HelpfulExtensions.Extensions.Workflows.Drivers;

public class AuthorizationEventDisplayDriver : SimpleEventActivityDisplayDriverBase<AuthorizationEvent>
{
    protected readonly IHtmlLocalizer H;

    public override string IconClass => "fa-key";

    [SuppressMessage("Major Code Smell", "S103:Lines should not be too long", Justification = "Can't split localization strings.")]
    public override LocalizedHtmlString Description =>
        H["Executed by the matching the authorization handler. If the workflow status is <code>{0}</code>, <code>{1}</code>, or <code>{2}</code> then the authorization is failed. Also, if the workflow contains the output <code>{3}</code>, then authorization is succeeded if it's <code>{4}</code> (case-insensitive) or failed if not.", WorkflowStatus.Aborted, WorkflowStatus.Faulted, WorkflowStatus.Halted, WorkflowAuthorizationHandler.ExpectedOutputName, "true"];

    public AuthorizationEventDisplayDriver(IHtmlLocalizer<AuthorizationEventDisplayDriver> htmlLocalizer) =>
        H = htmlLocalizer;
}
