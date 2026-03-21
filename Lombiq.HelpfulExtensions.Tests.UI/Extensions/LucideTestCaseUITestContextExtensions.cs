using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using Shouldly;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Tests.UI.Extensions;

public static class LucideTestCaseUITestContextExtensions
{
    /// <summary>
    /// Tests the Lombiq Helpful Extensions - Lucide feature.
    /// </summary>
    public static async Task TestLucideFeatureAsync(this UITestContext context)
    {
        const string iconName = "camera";

        await context.SignInDirectlyAsync();

        await context.EnableFeatureDirectlyAsync(FeatureIds.Lucide);
        await context.ExecuteRecipeDirectlyAsync("Lombiq.HelpfulExtensions.Tests.UI.Lucide.Tests");
        await context.CreateNewContentItemAsync("LucidePickerTest", onlyIfNotAlreadyThere: false);

        await context.ClickReliablyOnAsync(By.CssSelector("[data-lucide-toggle]"));
        await context.ClickAndFillInWithRetriesAsync(By.CssSelector("[data-lucide-search]"), iconName);
        await context.ClickReliablyOnAsync(By.CssSelector($"[data-lucide-icon='{iconName}']"));

        var selectedIcon = context.ExecuteScript("return document.querySelector(arguments[0])?.dataset.lucideIcon ?? '';", $"[data-lucide-icon='{iconName}'].active") as string;
        selectedIcon.ShouldBe(iconName);
        context.Get(By.CssSelector($"[data-lucide-preview] [data-lucide='{iconName}']"));
    }
}
