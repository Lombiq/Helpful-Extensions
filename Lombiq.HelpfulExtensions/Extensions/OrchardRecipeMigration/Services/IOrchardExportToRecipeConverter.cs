using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lombiq.HelpfulExtensions.Extensions.OrchardRecipeMigration.Services;

/// <summary>
/// Converts old Orchard 1 export files into Orchard Core recipe files. Besides the built-in content conversion for some
/// common parts, its functionality can be expanded by registering additional services that implement <see
/// cref="IOrchardContentConverter"/> or <see cref="IOrchardExportConverter"/> recipes.
/// </summary>
public interface IOrchardExportToRecipeConverter
{
    /// <summary>
    /// Returns a JSON string that contains an Orchard Core recipe file based on the provided Orchard 1 <paramref
    /// name="export"/> XML.
    /// </summary>
    Task<string> ConvertAsync(XDocument export);
}
