using OrchardCore.Modules.Manifest;
using static Lombiq.HelpfulExtensions.FeatureIds;

[assembly: Module(
    Name = "Helpful Extensions",
    Author = "Lombiq",
    Version = "1.0"
)]

[assembly: Feature(
    Id = Flows,
    Name = "Flows Helpful Extensions",
    Category = "Content",
    Description = "Adds additional styling capabilities to Flows.",
    Dependencies = new[]
    {
        "OrchardCore.Flows"
    }
)]

[assembly: Feature(
    Id = Widgets,
    Name = "Helpful Widgets",
    Category = "Content",
    Description = "Adds helpful widgets such as Container or Liquid widgets.",
    Dependencies = new[]
    {
        "OrchardCore.Html",
        "OrchardCore.Liquid"
    }
)]

[assembly: Feature(
    Id = ContentTypes,
    Name = "Helpful Content Types",
    Category = "Content",
    Description = "Adds helpful content types such as Page.",
    Dependencies = new[]
    {
        "OrchardCore.Autoroute",
        "OrchardCore.Flows",
        "OrchardCore.Title"
    }
)]
