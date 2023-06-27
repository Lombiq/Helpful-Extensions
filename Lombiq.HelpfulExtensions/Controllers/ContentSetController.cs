using Lombiq.HelpfulExtensions.Extensions.ContentSets.Events;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Models;
using Microsoft.AspNetCore.Mvc;
using OrchardCore;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Entities;
using OrchardCore.Modules;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.OrchardRecipeMigration.Controllers;

[Feature(FeatureIds.ContentSets)]
public class ContentSetController : Controller
{
    private readonly IContentDefinitionManager _contentDefinitionManager;
    private readonly IContentManager _contentManager;
    private readonly IEnumerable<IContentSetEventHandler> _contentSetEventHandlers;
    private readonly IIdGenerator _idGenerator;
    private readonly IOrchardHelper _orchardHelper;

    public ContentSetController(
        IContentDefinitionManager contentDefinitionManager,
        IContentManager contentManager,
        IEnumerable<IContentSetEventHandler> contentSetEventHandlers,
        IIdGenerator idGenerator,
        IOrchardHelper orchardHelper)
    {
        _contentDefinitionManager = contentDefinitionManager;
        _contentManager = contentManager;
        _idGenerator = idGenerator;
        _orchardHelper = orchardHelper;
        _contentSetEventHandlers = contentSetEventHandlers;
    }

    public async Task<IActionResult> Create(string fromContentItemId, string fromPartName, string newKey)
    {
        if (string.IsNullOrEmpty(fromPartName)) fromPartName = nameof(ContentSetPart);

        if (await _contentManager.GetAsync(fromContentItemId) is not { } content) return NotFound();
        if (content.Get<ContentSetPart>(fromPartName)?.ContentSet is not { } contentSet) return NotFound();

        content.ContentItemId = _idGenerator.GenerateUniqueId();
        content.ContentItemVersionId = _idGenerator.GenerateUniqueId();
        content.Alter<ContentSetPart>(fromPartName, part => part.Key = newKey);

        var contentTypePartDefinition = _contentDefinitionManager
            .GetTypeDefinition(content.ContentType)
            .Parts
            .Single(definition => definition.Name == fromPartName);

        foreach (var handler in _contentSetEventHandlers)
        {
            await handler.CreatingAsync(content, contentTypePartDefinition, contentSet, newKey);
        }

        await _contentManager.PublishAsync(content);
        return Redirect(await _orchardHelper.GetItemEditUrlAsync(content));
    }
}
