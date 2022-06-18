using OrchardCore.ContentLocalization.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Builders;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Markdown.Models;
using OrchardCore.Markdown.Settings;
using static Lombiq.HelpfulExtensions.Extensions.SiteTexts.Constants.ContentTypes;

namespace Lombiq.HelpfulExtensions.Extensions.SiteTexts;

public class Migrations : DataMigration
{
    private readonly IContentDefinitionManager _contentDefinitionManager;

    public Migrations(IContentDefinitionManager contentDefinitionManager) =>
        _contentDefinitionManager = contentDefinitionManager;

    public int Create()
    {
        _contentDefinitionManager.AlterTypeDefinition(SiteText, builder => builder
            .SetAbilities(
                creatable: true,
                securable: true,
                draftable: false,
                listable: true,
                versionable: false)
            .WithPart(nameof(MarkdownBodyPart), part => part.WithEditor("Wysiwyg"))
            .WithPart(nameof(LocalizationPart)));

        return 1;
    }
}
