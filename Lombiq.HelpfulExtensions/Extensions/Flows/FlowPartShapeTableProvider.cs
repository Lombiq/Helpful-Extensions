using OrchardCore.DisplayManagement.Descriptors;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.Flows;

internal sealed class FlowPartShapeTableProvider : IShapeTableProvider
{
    public ValueTask DiscoverAsync(ShapeTableBuilder builder)
    {
        builder
            .Describe("FlowPart")
            .OnDisplaying(displaying => displaying.Shape.Metadata.Alternates.Add("Lombiq_HelpfulExtensions_Flows_FlowPart"));

        return ValueTask.CompletedTask;
    }
}
