using Lombiq.HelpfulExtensions.Extensions.Widgets.Models;
using Lombiq.HelpfulLibraries.OrchardCore.Contents;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using static Lombiq.HelpfulExtensions.Extensions.Widgets.WidgetTypes;

namespace Lombiq.HelpfulExtensions.Extensions.Widgets;

public class Migrations : DataMigration
{
    private readonly IContentDefinitionManager _contentDefinitionManager;

    public Migrations(IContentDefinitionManager contentDefinitionManager) =>
        _contentDefinitionManager = contentDefinitionManager;

    public int Create()
    {
        _contentDefinitionManager.AlterTypeDefinition(ContainerWidget, builder => builder
            .Securable()
            .Stereotype(CommonStereotypes.Widget)
            .WithPart("TitlePart", part => part.WithPosition("0"))
            .WithPart("FlowPart", part => part.WithPosition("1"))
        );

        _contentDefinitionManager.AlterTypeDefinition(HtmlWidget, builder => builder
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

        _contentDefinitionManager.AlterTypeDefinition(LiquidWidget, builder => builder
            .Securable()
            .Stereotype(CommonStereotypes.Widget)
            .WithPart("LiquidPart", part => part
                .WithDisplayName("Liquid Part")
            )
        );

        _contentDefinitionManager.AlterTypeDefinition(MenuWidget, builder => builder
            .Securable()
            .Stereotype(CommonStereotypes.Widget)
        );

        _contentDefinitionManager.AlterTypeDefinition(MarkdownWidget, builder => builder
            .Securable()
            .Stereotype(CommonStereotypes.Widget)
            .WithPart("MarkdownBodyPart", part => part
                .WithDisplayName("Markdown Part")
            )
        );

        var contentItemWidgetPartName = _contentDefinitionManager.AlterPartDefinition<ContentItemWidget>(builder => builder
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

        _contentDefinitionManager.AlterTypeDefinition(WidgetTypes.ContentItemWidget, builder => builder
            .Securable()
            .Stereotype(CommonStereotypes.Widget)
            .WithPart(contentItemWidgetPartName)
        );

        return 5;
    }

    public int UpdateFrom1()
    {
        _contentDefinitionManager.AlterTypeDefinition(ContainerWidget, builder => builder
            .WithPart("TitlePart", part => part.WithPosition("0"))
            .WithPart("FlowPart", part => part.WithPosition("1"))
        );

        return 2;
    }

    public int UpdateFrom2()
    {
        _contentDefinitionManager.AlterTypeDefinition(MenuWidget, builder => builder
            .Securable()
            .Stereotype(CommonStereotypes.Widget)
        );

        return 3;
    }

    public int UpdateFrom3()
    {
        _contentDefinitionManager.AlterTypeDefinition(MarkdownWidget, builder => builder
            .Securable()
            .Stereotype(CommonStereotypes.Widget)
            .WithPart("MarkdownBodyPart", part => part
                .WithDisplayName("Markdown Part")
            )
        );

        return 4;
    }

    public int UpdateFrom4()
    {
        var contentItemWidgetPartName = _contentDefinitionManager.AlterPartDefinition<ContentItemWidget>(builder => builder
            .WithField(part => part.ContentToDisplay, field => field.WithDisplayName("Content to display"))
            .WithField(part => part.DisplayType, field => field.WithDisplayName("Display type"))
            .WithField(part => part.GroupId, field => field.WithDisplayName("Group ID"))
        );

        _contentDefinitionManager.AlterTypeDefinition(WidgetTypes.ContentItemWidget, builder => builder
            .Securable()
            .Stereotype(CommonStereotypes.Widget)
            .WithPart(contentItemWidgetPartName)
        );

        return 5;
    }
}
