using OrchardCore.ContentLocalization.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Builders;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Markdown.Models;
using System.Threading.Tasks;
using static Lombiq.HelpfulExtensions.Extensions.SiteTexts.Constants.ContentTypes;
using static Lombiq.HelpfulLibraries.OrchardCore.Contents.ContentFieldEditorEnums;

namespace Lombiq.HelpfulExtensions.Extensions.SiteTexts;

public class SiteTextMigrations(IContentDefinitionManager contentDefinitionManager) : DataMigration
{
    public async Task<int> CreateAsync()
    {
        await contentDefinitionManager.AlterTypeDefinitionAsync(SiteText, builder => builder
            .SetAbilities(
                creatable: true,
                securable: true,
                draftable: false,
                listable: true,
                versionable: false)
            .WithPart(nameof(MarkdownBodyPart), part => part.WithEditor(MarkdownFieldEditors.Wysiwyg))
            .WithPart(nameof(LocalizationPart)));

        return 1;
    }
}
