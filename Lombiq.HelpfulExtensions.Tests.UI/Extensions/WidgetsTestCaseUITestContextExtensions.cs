using Atata;
using Lombiq.HelpfulExtensions.Tests.UI.Constants;
using Lombiq.HelpfulExtensions.Tests.UI.Helpers;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using Shouldly;
using System.Threading.Tasks;
using static Lombiq.HelpfulExtensions.Tests.UI.Constants.XPathSelectors;

namespace Lombiq.HelpfulExtensions.Tests.UI.Extensions;

public static class WidgetsTestCaseUITestContextExtensions
{
    /// <summary>
    /// Tests the Lombiq Helpful Extensions - Helpful Widgets feature.
    /// </summary>
    public static async Task TestWidgetsFeatureAsync(this UITestContext context)
    {
        await context.SignInDirectlyAsync();
        await context.EnableFeatureDirectlyAsync(FeatureIds.ContentTypes);
        await context.EnableFeatureDirectlyAsync(FeatureIds.Widgets);

        var widgets = new[]
        {
            WidgetTypes.Container,
            WidgetTypes.Html,
            WidgetTypes.Liquid,
            WidgetTypes.Markdown,
            WidgetTypes.Menu,
        };

        foreach (var widget in widgets)
        {
            await TestWidgetAsync(context, widget);
        }
    }

    private static async Task TestWidgetAsync(UITestContext context, string widget)
    {
        await context.GoToCreatePageAsync();
        WidgetHelpers.AddWidgetToPageFlow(context, widget);

        context.Get(By.XPath(WidgetEditorHeaderText))
            .GetAttribute("data-content-type-display-text")
            .ShouldBe($"{widget} Widget");
    }
}
