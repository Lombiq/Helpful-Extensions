using System;

namespace Lombiq.HelpfulExtensions.Tests.UI.Constants;

internal static class GeneratedMigrationCodes
{
    // Environment.NewLine is used for this to work regardless of the mismatch of line ending style between the code
    // file, the platform where it was compiled, and where it was executed.
    public static string Page =
        "_contentDefinitionManager.AlterTypeDefinition(\"Page\", type => type" + Environment.NewLine +
        "    .DisplayedAs(\"Page\")" + Environment.NewLine +
        "    .Creatable()" + Environment.NewLine +
        "    .Listable()" + Environment.NewLine +
        "    .Draftable()" + Environment.NewLine +
        "    .Versionable()" + Environment.NewLine +
        "    .Securable()" + Environment.NewLine +
        "    .WithPart(\"TitlePart\", part => part" + Environment.NewLine +
        "        .WithPosition(\"0\")" + Environment.NewLine +
        "    )" + Environment.NewLine +
        "    .WithPart(\"AutoroutePart\", part => part" + Environment.NewLine +
        "        .WithPosition(\"1\")" + Environment.NewLine +
        "        .WithSettings(new AutoroutePartSettings" + Environment.NewLine +
        "        {" + Environment.NewLine +
        "            AllowCustomPath = true," + Environment.NewLine +
        "            ShowHomepageOption = true," + Environment.NewLine +
        "            Pattern = \"{{ Model.ContentItem | display_text | slugify }}\"," + Environment.NewLine +
        "        })" + Environment.NewLine +
        "    )" + Environment.NewLine +
        "    .WithPart(\"FlowPart\", part => part" + Environment.NewLine +
        "        .WithPosition(\"2\")" + Environment.NewLine +
        "    )" + Environment.NewLine +
        "    .WithPart(\"Page\", part => part" + Environment.NewLine +
        "        .WithPosition(\"3\")" + Environment.NewLine +
        "    )" + Environment.NewLine +
        ");" + Environment.NewLine;
}
