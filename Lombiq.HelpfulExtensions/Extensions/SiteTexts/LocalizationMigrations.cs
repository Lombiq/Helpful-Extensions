using OrchardCore.ContentLocalization.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;
using System.Threading.Tasks;
using static Lombiq.HelpfulExtensions.Extensions.SiteTexts.Constants.ContentTypes;

namespace Lombiq.HelpfulExtensions.Extensions.SiteTexts;

public class LocalizationMigrations(IContentDefinitionManager contentDefinitionManager) : DataMigration
{
    public async Task<int> CreateAsync()
    {
        await contentDefinitionManager.AlterTypeDefinitionAsync(SiteText, builder => builder
            .WithPart(nameof(LocalizationPart)));

        return 1;
    }
}
