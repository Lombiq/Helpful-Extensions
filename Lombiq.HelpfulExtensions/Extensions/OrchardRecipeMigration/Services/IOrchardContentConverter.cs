using OrchardCore.ContentManagement;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LombiqDotCom.Services;

/// <summary>
/// A converter used to port information from a &lt;Content&gt; item from an Orchard 1 XML export into an Orchard Core
/// <see cref="ContentItem"/>.
/// </summary>
public interface IOrchardContentConverter
{
    /// <summary>
    /// Gets the sorting order number, lower numbers are used first.
    /// </summary>
    int Order => 10;

    /// <summary>
    /// Returns <see langword="true"/> if this converter can use the given <paramref name="element"/>.
    /// </summary>
    bool IsApplicable(XElement element);

    /// <summary>
    /// Returns a new <see cref="ContentItem"/> created for this element, or <see langword="null"/> if this converter
    /// can't create a content item for the given <paramref name="element"/>. If all converters returned <see
    /// langword="null"/>, <see cref="IContentManager.NewAsync"/> is used with the <paramref name="element"/>'s <see
    /// cref="XElement.Name"/> as the content type.
    /// </summary>
    Task<ContentItem> CreateContentItemAsync(XElement element) => Task.FromResult<ContentItem>(null);

    /// <summary>
    /// Processes further content in the <paramref name="element"/> to fill the <paramref name="contentItem"/>.
    /// </summary>
    Task ImportAsync(XElement element, ContentItem contentItem);
}
