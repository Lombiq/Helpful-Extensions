using Lombiq.HelpfulExtensions.Extensions.ContentSets.Events;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Models;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.ViewModels;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Workflows.Activities;
using Lombiq.HelpfulLibraries.OrchardCore.Workflow;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.Workflows.Models;
using OrchardCore.Workflows.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets.Workflows.Handlers;

public class WorkflowContentSetEventHandler : IContentSetEventHandler
{
    private readonly IWorkflowManager _workflowManager;
    private readonly IWorkflowTypeStore _workflowTypeStore;

    public WorkflowContentSetEventHandler(
        IWorkflowManager workflowManager,
        IWorkflowTypeStore workflowTypeStore)
    {
        _workflowManager = workflowManager;
        _workflowTypeStore = workflowTypeStore;
    }

    public async Task<IEnumerable<ContentSetLinkViewModel>> GetSupportedOptionsAsync(
        ContentSetPart part,
        ContentTypePartDefinition definition)
    {
        var links = new List<ContentSetLinkViewModel>();

        var values = new GetSupportedOptionsContext(definition, part).ToDictionary();

        var workflowContexts = await _workflowManager
            .TriggerEventAndGetContextsAsync<ContentSetGetSupportedOptionsEvent>(_workflowTypeStore, values);

        foreach (var context in workflowContexts)
        {
            if (context.Status is WorkflowStatus.Faulted or WorkflowStatus.Halted or WorkflowStatus.Aborted) continue;
            if (!context.Output.TryGetValue(nameof(ContentSetPartViewModel.MemberLinks), out var memberLinks)) continue;

            switch (memberLinks)
            {
                case IEnumerable<ContentSetLinkViewModel> viewModels:
                    links.AddRange(viewModels);
                    break;
                case ContentSetLinkViewModel viewModel:
                    links.Add(viewModel);
                    break;
                case JArray jsonArray:
                    links.AddRange(jsonArray.ToObject<IEnumerable<ContentSetLinkViewModel>>());
                    break;
                case JObject jsonObject:
                    links.Add(jsonObject.ToObject<ContentSetLinkViewModel>());
                    break;
                case string json when !string.IsNullOrWhiteSpace(json):
                    links.AddRange(JsonConvert.DeserializeObject<List<ContentSetLinkViewModel>>(json));
                    break;
                default: continue;
            }
        }

        return links;
    }

    public Task CreatingAsync(
        ContentItem content,
        ContentTypePartDefinition definition,
        string contentSet,
        string newKey) =>
        _workflowManager.TriggerEventAsync<ContentSetCreatingEvent>(
            new CreatingContext(content, definition, contentSet, newKey),
            $"{nameof(WorkflowContentSetEventHandler)}.{nameof(CreatingAsync)}" +
            $"({content.ContentItemId}, {definition.Name}, {contentSet}, {newKey})");
}
