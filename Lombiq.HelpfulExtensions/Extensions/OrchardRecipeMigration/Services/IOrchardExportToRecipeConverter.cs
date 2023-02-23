using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LombiqDotCom.Services;

public interface IOrchardExportToRecipeConverter
{
    Task<string> ConvertAsync(XDocument export);
}

public static class OrchardExportToRecipeConverterExtensions
{
    public static async Task<string> ConvertAsync(this IOrchardExportToRecipeConverter converter, IFormFile file)
    {
        await using var stream = file.OpenReadStream();
        return await converter.ConvertAsync(XDocument.Load(stream));
    }
}
