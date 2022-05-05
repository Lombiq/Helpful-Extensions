using OrchardCore.Modules.Manifest;
using static Lombiq.HelpfulExtensions.FeatureIds;

[assembly: Module(
    Name = "Lombiq Helpful Extensions",
    Author = "Lombiq Technologies",
    Version = "3.2.0",
    Website = "https://github.com/Lombiq/Helpful-Extensions"
)]

[assembly: Feature(
    Id = CodeGeneration,
    Name = "Lombiq Helpful Extensions - Code Generation Helpful Extensions",
    Category = "Development",
    Description = "Generates migrations from content type definitions.",
    Dependencies = new[]
    {
        "OrchardCore.Resources",
    }
)]

[assembly: Feature(
    Id = Flows,
    Name = "Lombiq Helpful Extensions - Flows Helpful Extensions",
    Category = "Content",
    Description = "Adds additional styling capabilities to Flow Part.",
    Dependencies = new[]
    {
        "OrchardCore.Flows",
    }
)]

[assembly: Feature(
    Id = Widgets,
    Name = "Lombiq Helpful Extensions - Helpful Widgets",
    Category = "Content",
    Description = "Adds helpful widgets such as Container or Liquid widgets.",
    Dependencies = new[]
    {
        "OrchardCore.Html",
        "OrchardCore.Liquid",
        "OrchardCore.Title",
    }
)]

[assembly: Feature(
    Id = ContentTypes,
    Name = "Lombiq Helpful Extensions - Helpful Content Types",
    Category = "Content",
    Description = "Adds helpful content types such as Page.",
    Dependencies = new[]
    {
        "OrchardCore.Autoroute",
        "OrchardCore.Flows",
        "OrchardCore.Title",
    }
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
    Dependencies = new[]
    {
        "OrchardCore.Email",
    }
)]
