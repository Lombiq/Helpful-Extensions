extern alias OCCMA;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using Shouldly;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Tests.UI.Extensions;

public static class LiquidTestCaseUITestContextExtensions
{
    /// <summary>
    /// Tests the Lombiq Helpful Extensions - Liquid feature.
    /// </summary>
    public static async Task TestLiquidFeatureAsync(this UITestContext context)
    {
        // Initialize relevant content.
        await context.SignInDirectlyAsync();
        await context.ExecuteRecipeDirectlyAsync("Lombiq.HelpfulExtensions.Liquid.Sample");

        // Test the "ifnotempty" feature. If the evaluation works, we should see "HELLO![ FOO BAR true ]" with other
        // parts omitted (i.e. it shouldn't be "HELLO![ empty whitespace1 whitespace2 FOO BAR true false null empty ]").
        await context.GoToRelativeUrlAsync("/liquid-demo/ifnotempty");
        context.Get(By.Id("demo-value")).GetTextTrimmed().ShouldBe("HELLO![ FOO BAR true ]");
    }
}
