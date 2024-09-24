using Lombiq.HelpfulExtensions.Extensions.Widgets.Models;
using Lombiq.HelpfulLibraries.OrchardCore.Contents;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using System.Threading.Tasks;
using static Lombiq.HelpfulExtensions.Extensions.Widgets.WidgetTypes;

namespace Lombiq.HelpfulExtensions.Extensions.Widgets;

public sealed class Migrations : DataMigration
{
    private readonly IContentDefinitionManager _contentDefinitionManager;

    public Migrations(IContentDefinitionManager contentDefinitionManager) =>
        _contentDefinitionManager = contentDefinitionManager;

    public async Task<int> CreateAsync()
    {
        await _contentDefinitionManager.AlterTypeDefinitionAsync(ContainerWidget, builder => builder
            .Securable()
            .Stereotype(CommonStereotypes.Widget)
            .WithPart("TitlePart", part => part.WithPosition("0"))
            .WithPart("FlowPart", part => part.WithPosition("1"))
        );

        await _contentDefinitionManager.AlterTypeDefinitionAsync(HtmlWidget, builder => builder
            .Securable()
            .Stereotype(CommonStereotypes.Widget)
            .WithPart("HtmlBodyPart", part => part
                .WithDisplayName("HTML Body")
                .WithSettings(new ContentTypePartSettings
                {
                    Editor = "Trumbowyg",
                })
            )
        );

        await _contentDefinitionManager.AlterTypeDefinitionAsync(LiquidWidget, builder => builder
            .Securable()
            .Stereotype(CommonStereotypes.Widget)
            .WithPart("LiquidPart", part => part
                .WithDisplayName("Liquid Part")
            )
        );

        await _contentDefinitionManager.AlterTypeDefinitionAsync(MenuWidget, builder => builder
            .Securable()
            .Stereotype(CommonStereotypes.Widget)
        );

        await _contentDefinitionManager.AlterTypeDefinitionAsync(MarkdownWidget, builder => builder
            .Securable()
            .Stereotype(CommonStereotypes.Widget)
            .WithPart("MarkdownBodyPart", part => part
                .WithDisplayName("Markdown Part")
            )
        );

        var contentItemWidgetPartName = await _contentDefinitionManager.AlterPartDefinitionAsync<ContentItemWidget>(builder => builder
            .WithField(part => part.ContentToDisplay, field => field
                .WithDisplayName("Content to display")
                .WithSettings(new ContentPickerFieldSettings
                {
                    DisplayAllContentTypes = true,
                    Multiple = true,
                }))
            .WithField(part => part.DisplayType, field => field.WithDisplayName("Display type"))
            .WithField(part => part.GroupId, field => field.WithDisplayName("Group ID"))
        );

        await _contentDefinitionManager.AlterTypeDefinitionAsync(WidgetTypes.ContentItemWidget, builder => builder
            .Securable()
            .Stereotype(CommonStereotypes.Widget)
            .WithPart(contentItemWidgetPartName)
        );

        return 5;
    }

    public async Task<int> UpdateFrom1Async()
    {
        await _contentDefinitionManager.AlterTypeDefinitionAsync(ContainerWidget, builder => builder
            .WithPart("TitlePart", part => part.WithPosition("0"))
            .WithPart("FlowPart", part => part.WithPosition("1"))
        );

        return 2;
    }

    public async Task<int> UpdateFrom2Async()
    {
        await _contentDefinitionManager.AlterTypeDefinitionAsync(MenuWidget, builder => builder
            .Securable()
            .Stereotype(CommonStereotypes.Widget)
        );

        return 3;
    }

    public async Task<int> UpdateFrom3Async()
    {
        await _contentDefinitionManager.AlterTypeDefinitionAsync(MarkdownWidget, builder => builder
            .Securable()
            .Stereotype(CommonStereotypes.Widget)
            .WithPart("MarkdownBodyPart", part => part
                .WithDisplayName("Markdown Part")
            )
        );

        return 4;
    }

    public async Task<int> UpdateFrom4Async()
    {
        var contentItemWidgetPartName = await _contentDefinitionManager.AlterPartDefinitionAsync<ContentItemWidget>(builder => builder
            .WithField(part => part.ContentToDisplay, field => field.WithDisplayName("Content to display"))
            .WithField(part => part.DisplayType, field => field.WithDisplayName("Display type"))
            .WithField(part => part.GroupId, field => field.WithDisplayName("Group ID"))
        );

        await _contentDefinitionManager.AlterTypeDefinitionAsync(WidgetTypes.ContentItemWidget, builder => builder
            .Securable()
            .Stereotype(CommonStereotypes.Widget)
            .WithPart(contentItemWidgetPartName)
        );

        return 5;
    }
}
