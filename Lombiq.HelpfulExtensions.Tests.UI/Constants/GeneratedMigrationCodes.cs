using Shouldly;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Lombiq.HelpfulExtensions.Tests.UI.Constants;

internal static class GeneratedMigrationCodes
{
    [SuppressMessage(
        "Usage",
        "MA0136:Raw String contains an implicit end of line character",
        Justification = "It's wrapped to prevent issues related to that.")]
    private const string Page =
        """
        _contentDefinitionManager.AlterTypeDefinition("Page", type => type
            .DisplayedAs("Page")
            .Creatable()
            .Listable()
            .Draftable()
            .Versionable()
            .Securable()
            .WithPart("TitlePart", part => part
                .WithPosition("0")
            )
            .WithPart("AutoroutePart", part => part
                .WithPosition("1")
                .WithSettings(new AutoroutePartSettings
                {
                    AllowCustomPath = true,
                    Pattern = {{ Model.ContentItem | display_text | slugify }},
                    ShowHomepageOption = true,
                    AllowUpdatePath = false,
                    AllowDisabled = false,
                    AllowRouteContainedItems = false,
                    ManageContainedItemRoutes = false,
                    AllowAbsolutePath = false,
                })
            )
            .WithPart("FlowPart", part => part
                .WithPosition("2")
            )
            .WithPart("Page", part => part
                .WithPosition("3")
            )
        );
        """;

    public static void ShouldBePage(string actual) => actual.ShouldBeByLine(Page);
}
