using System;

namespace Lombiq.HelpfulExtensions.Tests.UI.Constants;

internal static class GeneratedMigrationCodes
{
    // Environment.NewLine is used for this to work regardless of the mismatch of line ending style between the code
    // file, the platform where it was compiled, and where it was executed.
    public static string Page =
        string.Join(
            Environment.NewLine,
            new[]
            {
                "_contentDefinitionManager.AlterTypeDefinition(\"Page\", type => type",
                "    .DisplayedAs(\"Page\")",
                "    .Creatable()",
                "    .Listable()",
                "    .Draftable()",
                "    .Versionable()",
                "    .Securable()",
                "    .WithPart(\"TitlePart\", part => part",
                "        .WithPosition(\"0\")",
                "    )",
                "    .WithPart(\"AutoroutePart\", part => part",
                "        .WithPosition(\"1\")",
                "        .WithSettings(new AutoroutePartSettings",
                "        {",
                "            AllowCustomPath = true,",
                "            ShowHomepageOption = true,",
                "            Pattern = \"{{ Model.ContentItem | display_text | slugify }}\",",
                "        })",
                "    )",
                "    .WithPart(\"FlowPart\", part => part",
                "        .WithPosition(\"2\")",
                "    )",
                "    .WithPart(\"Page\", part => part",
                "        .WithPosition(\"3\")",
                "    )",
                ");",
                string.Empty,
            });
}
