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