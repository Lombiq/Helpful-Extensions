using Lombiq.HelpfulExtensions.Extensions.ContentSets.Models;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.ViewModels;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets.Events;

public interface IContentSetEventHandler
{
    Task<IEnumerable<ContentSetLinkViewModel>> GetSupportedOptionsAsync(
        ContentSetPart part,
        ContentTypePartDefinition definition) =>
        Task.FromResult<IEnumerable<ContentSetLinkViewModel>>(null);

    Task CreatingAsync(
        ContentItem content,
        ContentTypePartDefinition definition,
        string contentSet,
        string newKey) =>
        Task.CompletedTask;
}
