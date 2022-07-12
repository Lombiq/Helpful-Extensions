using Atata;
using Lombiq.HelpfulExtensions.Tests.UI.Constants;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Shouldly;
using System.Threading.Tasks;
using static Lombiq.HelpfulExtensions.Tests.UI.Constants.TextInputValues;
using static Lombiq.HelpfulExtensions.Tests.UI.Constants.XPathSelectors;

namespace Lombiq.HelpfulExtensions.Tests.UI.Extensions;

public static class TestCaseUITestContextExtensions
{
    public static async Task TestFeatureWidgetsAsync(this UITestContext context)
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
            await context.TestWidgetAsync(widget);
        }
    }

    public static async Task TestFlowAdditionalStylingPartNotActivatingGh76Async(this UITestContext context)
    {
        await context.SignInDirectlyAsync();
        await context.EnableFeatureDirectlyAsync(FeatureIds.Flows);
        await context.EnableFeatureDirectlyAsync(FeatureIds.ContentTypes);
        await context.EnableFeatureDirectlyAsync(FeatureIds.Widgets);

        await context.GoToCreatePageAsync();

        // Adding 'Html' to flow.
        context.AddWidgetToPageFlow(WidgetTypes.Html);

        // To show toolbar.
        new Actions(context.Driver)
            .MoveToElement(context.Get(By.XPath(WidgetEditorHeader)))
            .Build()
            .Perform();

        var flowSettingsButtonSelector = By.XPath(FlowSettingsButton);

        // Check that the settings button exists and is visible.
        context.Exists(flowSettingsButtonSelector);

        // To show 'AdditionalStylingPart' view.
        new Actions(context.Driver)
            .Click(context.Get(flowSettingsButtonSelector))
            .Build()
            .Perform();

        // Check that the 'AdditionalStylingPart' view exists and is visible.
        context.Exists(By.XPath(FlowSettingsDropdown));

        // Check CustomClasses input.
        var customClassesInputSelector = By.XPath(CustomClassesInput);
        context.Get(customClassesInputSelector)
            .SendKeys(TestClass);

        context.Get(customClassesInputSelector)
            .GetValue()
            .ShouldBe(TestClass);
    }

    private static async Task TestWidgetAsync(this UITestContext context, string widget)
    {
        await context.GoToCreatePageAsync();
        context.AddWidgetToPageFlow(widget);

        context.Get(By.XPath(WidgetEditorHeaderText))
            .GetAttribute("data-content-type-display-text")
            .ShouldBe($"{widget} Widget");
    }

    private static void AddWidgetToPageFlow(this UITestContext context, string widget) =>
        new Actions(context.Driver)
            .Click(context.Get(By.XPath(AddWidgetButton)))
            .Click(context.Get(By.XPath($"{WidgetList}/a[text()='{widget} Widget']").OfAnyVisibility()))
            .Build()
            .Perform();
}
