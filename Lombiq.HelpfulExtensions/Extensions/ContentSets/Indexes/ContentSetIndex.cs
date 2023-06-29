using Lombiq.HelpfulExtensions.Extensions.ContentSets.Models;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using System;
using System.Linq;
using YesSql.Indexes;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets.Indexes;

public class ContentSetIndex : MapIndex
{
    public string ContentItemId { get; set; }
    public string PartName { get; set; }
    public bool IsPublished { get; set; }
    public string ContentSet { get; set; }
    public string Key { get; set; }

    public static ContentSetIndex FromPart(ContentSetPart part, string partName) =>
        new()
        {
            ContentItemId = part.ContentItem.ContentItemId,
            PartName = partName,
            IsPublished = part.ContentItem.Published,
            ContentSet = part.ContentSet,
            Key = part.Key,
        };
}

public class ContentSetIndexProvider : IndexProvider<ContentItem>
{
    private readonly Lazy<IContentDefinitionManager> _contentDefinitionManager;

    public ContentSetIndexProvider(Lazy<IContentDefinitionManager> contentDefinitionManager) =>
        _contentDefinitionManager = contentDefinitionManager;

    public override void Describe(DescribeContext<ContentItem> context) =>
        context.For<ContentSetIndex>().Map(contentItem => !contentItem.Latest
            ? Enumerable.Empty<ContentSetIndex>()
            : _contentDefinitionManager
                .Value
                .GetTypeDefinition(contentItem.ContentType)
                .Parts
                .Where(part => part.PartDefinition.Name == nameof(ContentSetPart))
                .Select(part => new { Part = contentItem.Get<ContentSetPart>(part.Name), part.Name })
                .Where(info => info.Part != null)
                .Select(info => ContentSetIndex.FromPart(info.Part, info.Name)));
}
