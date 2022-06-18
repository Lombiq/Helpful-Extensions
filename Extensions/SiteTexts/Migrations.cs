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
            .WithTitlePart()
            .WithPart(nameof(MarkdownBodyPart), part => part
                // These resources shouldn't be made editable to untrusted parties so we don't need to sanitize HTML.
                .WithSettings(new MarkdownBodyPartSettings { SanitizeHtml = false })
                .WithEditor("Wysiwyg"))
            .WithPart(nameof(LocalizationPart)));

        return 1;
    }
}
