using System.Threading.Tasks;
using System.Xml.Linq;

namespace LombiqDotCom.Services;

public interface IOrchardExportToRecipeConverter
{
    Task<string> ConvertAsync(XDocument export);
}
