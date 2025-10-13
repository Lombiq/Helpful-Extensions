using OrchardCore.Contents.Liquid;

namespace Lombiq.HelpfulExtensions.Extensions.Liquid.Services;

public class ShapesBuildDisplayFilter : ArrayFilterBase
{
    public ShapesBuildDisplayFilter(BuildDisplayFilter buildDisplayFilter)
        : base(buildDisplayFilter)
    {
    }
}
