using OrchardCore.ContentLocalization.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;
using System.Threading.Tasks;
using static Lombiq.HelpfulExtensions.Extensions.SiteTexts.Constants.ContentTypes;

namespace Lombiq.HelpfulExtensions.Extensions.SiteTexts;

public sealed class LocalizationMigrations : DataMigration
{
    private readonly IContentDefinitionManager _contentDefinitionManager;

    public LocalizationMigrations(IContentDefinitionManager contentDefinitionManager) =>
        _contentDefinitionManager = contentDefinitionManager;

    public async Task<int> CreateAsync()
    {
        await _contentDefinitionManager.AlterTypeDefinitionAsync(SiteText, builder => builder
            .WithPart(nameof(LocalizationPart)));

        return 1;
    }
}
