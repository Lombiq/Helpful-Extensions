namespace Lombiq.HelpfulExtensions.Tests.UI.Constants;

internal static class GeneratedMigrationCodes
{
    // The funny indentation is necessary to match the indentations of the textarea.
    public static string Page = @"_contentDefinitionManager.AlterTypeDefinition(""Page"", type => type
    .DisplayedAs(""Page"")
    .Creatable()
    .Listable()
    .Draftable()
    .Versionable()
    .Securable()
    .WithPart(""TitlePart"", part => part
        .WithPosition(""0"")
    )
    .WithPart(""AutoroutePart"", part => part
        .WithPosition(""1"")
        .WithSettings(new AutoroutePartSettings
        {
            AllowCustomPath = true,
            ShowHomepageOption = true,
            Pattern = ""{{ Model.ContentItem | display_text | slugify }}"",
        })
    )
    .WithPart(""FlowPart"", part => part
        .WithPosition(""2"")
    )
    .WithPart(""Page"", part => part
        .WithPosition(""3"")
    )
);
";
}
