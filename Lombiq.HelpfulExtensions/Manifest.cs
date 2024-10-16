using OrchardCore.Modules.Manifest;
using static Lombiq.HelpfulExtensions.FeatureIds;

[assembly: Module(
    Name = "Lombiq Helpful Extensions",
    Author = "Lombiq Technologies",
    Version = "0.0.1",
    Website = "https://github.com/Lombiq/Helpful-Extensions"
)]

[assembly: Feature(
    Id = CodeGeneration,
    Name = "Lombiq Helpful Extensions - Code Generation Helpful Extensions",
    Category = "Development",
    Description = "Generates migrations from content type definitions.",
    Dependencies =
    [
        "OrchardCore.Resources",
    ]
)]

[assembly: Feature(
    Id = ContentSets,
    Name = "Lombiq Helpful Extensions - Content Sets",
    Category = "Development",
    Description = "Create arbitrary collections of content items.",
    Dependencies =
    [
        "OrchardCore.ContentManagement",
    ]
)]

[assembly: Feature(
    Id = Flows,
    Name = "Lombiq Helpful Extensions - Flows Helpful Extensions",
    Category = "Content",
    Description = "Adds additional styling capabilities to Flow Part.",
    Dependencies =
    [
        "OrchardCore.Flows",
    ]
)]

[assembly: Feature(
    Id = Widgets,
    Name = "Lombiq Helpful Extensions - Helpful Widgets",
    Category = "Content",
    Description = "Adds helpful widgets such as Container or Liquid widgets.",
    Dependencies =
    [
        "OrchardCore.Html",
        "OrchardCore.Liquid",
        "OrchardCore.Title",
    ]
)]

[assembly: Feature(
    Id = ContentTypes,
    Name = "Lombiq Helpful Extensions - Helpful Content Types",
    Category = "Content",
    Description = "Adds helpful content types such as Page.",
    Dependencies =
    [
        "OrchardCore.Autoroute",
        "OrchardCore.Flows",
        "OrchardCore.Title",
    ]
)]

[assembly: Feature(
    Id = ShapeTracing,
    Name = "Lombiq Helpful Extensions - Shape Tracing Helpful Extensions",
    Category = "Development",
    Description = "Adds a dump of metadata to the output about every shape."
)]

[assembly: Feature(
    Id = Security,
    Name = "Lombiq Helpful Extensions - Security Helpful Extensions",
    Category = "Security",
    Description = "Adds a content type definition setting and authorization handler for richer security options."
)]

[assembly: Feature(
    Id = Emails,
    Name = "Lombiq Helpful Extensions - Emails",
    Category = "Messaging",
    Description = "Adds shape-based email template rendering and helpful email sending services.",
    Dependencies =
    [
        "OrchardCore.Email",
        "OrchardCore.Email.Smtp",
    ]
)]

[assembly: Feature(
    Id = TargetBlank,
    Name = "Lombiq Helpful Extensions - Target Blank",
    Category = "Content",
    Description = "Turns all external links into target=\"_blank\"."
)]

[assembly: Feature(
    Id = SiteTexts,
    Name = "Lombiq Helpful Extensions - Site Texts",
    Category = "Content",
    Description =
        "Adds a content type that lets the users with admin dashboard access customize string/HTML resources on the " +
        "site via Markdown. If OrchardCore.ContentLocalization is enabled, it also tries to retrieve the localized " +
        "version if available.",
    Dependencies =
    [
        "OrchardCore.Markdown",
    ]
)]

[assembly: Feature(
    Id = OrchardRecipeMigration,
    Name = "Lombiq Helpful Extensions - Orchard 1 Recipe Migration",
    Category = "Development",
    Description =
        "Convert Orchard 1's export XML files into Orchard Core recipes. This feature contains the basics like " +
        "CommonPart and BodyPart (full list is in the Helpful Extensions repository readme), but can be extended " +
        "with additional converters that only have to handle more specialized export data.",
    Dependencies =
    [
        "OrchardCore.Contents",
    ]
)]

[assembly: Feature(
    Id = ResetPasswordActivity,
    Name = "Lombiq Helpful Extensions - Reset password workflow activity",
    Category = "Security",
    Description = "Adds generate reset password token activity.",
    Dependencies =
    [
        "OrchardCore.Users.ResetPassword",
        "OrchardCore.Workflows",
    ]
)]

[assembly: Feature(
    Id = Trumbowyg,
    Name = "Lombiq Helpful Extensions - Trumbowyg code-snippet",
    Category = "Content",
    Description = "Adds option for inserting code snippets in Trumbowyg editor."
)]

[assembly: Feature(
    Id = GoogleTag,
    Name = "Lombiq Helpful Extensions - Google Tag",
    Category = "Content",
    Description = "Adds a shape along with Razor and Liquid tag helpers for Google Analytics."
)]
