using OrchardCore.Users.Models;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lombiq.HelpfulExtensions.Extensions.OrchardRecipeMigration.Services;

/// <summary>
/// A converter used to create Users from an Orchard 1 XML export into an Orchard Core
/// <see cref="User"/>.
/// </summary>
public interface IOrchardUserConverter
{
    /// <summary>
    /// Gets a value indicating whether the default user converter should be executed.
    /// </summary>
    bool IgnoreDefaultConverter => true;

    /// <summary>
    /// Returns <see langword="true"/> if this converter can use the given <paramref name="element"/>.
    /// </summary>
    bool IsApplicable(XElement element);

    /// <summary>
    /// Processes further content in the <paramref name="element"/>.
    /// </summary>
    Task ImportAsync(XElement element);
}
