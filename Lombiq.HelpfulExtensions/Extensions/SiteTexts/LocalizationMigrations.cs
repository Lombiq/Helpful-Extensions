using OrchardCore.ContentLocalization.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;
using static Lombiq.HelpfulExtensions.Extensions.SiteTexts.Constants.ContentTypes;

namespace Lombiq.HelpfulExtensions.Extensions.SiteTexts;

public class LocalizationMigrations : DataMigration
{
    private readonly IContentDefinitionManager _contentDefinitionManager;

    public LocalizationMigrations(IContentDefinitionManager contentDefinitionManager) =>
        _contentDefinitionManager = contentDefinitionManager;

    public int Create()
    {
        _contentDefinitionManager.AlterTypeDefinition(SiteText, builder => builder
            .WithPart(nameof(LocalizationPart)));

        return 1;
    }
}
