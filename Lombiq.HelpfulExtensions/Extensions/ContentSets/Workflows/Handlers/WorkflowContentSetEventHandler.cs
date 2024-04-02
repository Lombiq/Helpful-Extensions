using Lombiq.HelpfulExtensions.Extensions.ContentSets.Events;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Models;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.ViewModels;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Workflows.Activities;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Workflows.Models;
using Lombiq.HelpfulLibraries.OrchardCore.Workflow;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.Workflows.Models;
using OrchardCore.Workflows.Services;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
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
                case ExpandoObject expandoObject:
                    links.Add(SerializeAndDeserialize<ContentSetLinkViewModel>(expandoObject));
                    break;
                case IEnumerable<object> collection when
                    collection.CastWhere<ExpandoObject>().ToList() is { } objects &&
                    objects.Count != 0:
                    links.AddRange(SerializeAndDeserialize<IEnumerable<ContentSetLinkViewModel>>(objects));
                    break;
                case string json when !string.IsNullOrWhiteSpace(json):
                    links.AddRange(JsonSerializer.Deserialize<List<ContentSetLinkViewModel>>(json));
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

    private static T SerializeAndDeserialize<T>(object source) =>
        JObject.FromObject(source).ToObject<T>();
}
