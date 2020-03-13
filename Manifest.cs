using OrchardCore.Modules.Manifest;
using static Lombiq.HelpfulExtensions.FeatureIds;

[assembly: Module(
    Name = "Helpful Extensions",
    Author = "Lombiq",
    Version = "1.0"
)]

[assembly: Feature(
    Id = Lombiq_HelpfulExtensions_Flows,
    Name = "Flows Helpful Extensions",
    Category = "Content",
    Description = "Adds additional styling capabilities to Flows.",
    Dependencies = new[]
    {
        "OrchardCore.Flows"
    }
)]

[assembly: Feature(
    Id = Lombiq_HelpfulExtensions_Widgets,
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
    Id = Lombiq_HelpfulExtensions_ContentTypes,
    Name = "Helpful Content Types",
    Category = "Content",
    Description = "Adds helpful content types such as Page.",
    Dependencies = new[]
    {
        "OrchardCore.Autoroute",
        "OrchardCore.Flows"
    }
)]