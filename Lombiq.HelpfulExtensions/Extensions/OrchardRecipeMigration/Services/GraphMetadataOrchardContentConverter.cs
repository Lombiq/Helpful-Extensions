using GraphQL.Execution;
using OrchardCore.ContentManagement;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LombiqDotCom.Services;

public class GraphMetadataOrchardContentConverter : IOrchardContentConverter
{
    public const string GraphMetadata = nameof(GraphMetadata);

    public bool IsApplicable(XElement element) => element.Element(GraphMetadata) != null;

    public Task ImportAsync(XElement element, ContentItem contentItem)
    {
        contentItem.ContentItemId = ToContentItemId(
            element.Element(GraphMetadata)!.Attribute("NodeId")?.Value,
            element);

        return Task.CompletedTask;
    }

    public static string ToContentItemId(string value, XElement element)
    {
        var id = value.ToTechnicalInt();
        if (id < 0) throw new InvalidOperationError($"Missing or corrupted {GraphMetadata} node ID.\n{element}");
        return "assoc" + id.PadZeroes(21);
    }
}
