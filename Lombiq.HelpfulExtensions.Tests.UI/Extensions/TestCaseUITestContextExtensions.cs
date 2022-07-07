using Atata;
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
    public static async Task TestFlowAdditionalStylingPartNotActivatingGh76Async(this UITestContext context)
    {
        await context.SignInDirectlyAsync();
        await context.EnableFeatureDirectlyAsync(FeatureIds.Flows);
        await context.EnableFeatureDirectlyAsync(FeatureIds.ContentTypes);
        await context.EnableFeatureDirectlyAsync("OrchardCore.Forms");

        await context.GoToRelativeUrlAsync("/Admin/Contents/ContentTypes/Page/Create");

        // Adding 'Blockquote' to flow.
        new Actions(context.Driver)
            .Click(context.Get(By.XPath(AddWidgetButton)))
            .Click(
                context.Get(By.XPath(BlockquoteWidgetButton)
                    .OfAnyVisibility()))
            .Build()
            .Perform();

        // To show toolbar.
        new Actions(context.Driver)
            .MoveToElement(context.Get(By.XPath(WidgetEditorHeader)))
            .Build()
            .Perform();

        var flowSettingsButtonSelector = By.XPath(FlowSettingsButton);

        // Check that, the settings button is exists and visible.
        context.Exists(flowSettingsButtonSelector);

        // To show 'AdditionalStylingPart' view.
        new Actions(context.Driver)
            .Click(context.Get(flowSettingsButtonSelector))
            .Build()
            .Perform();

        var customClassesInputSelector = By.XPath(CustomClassesInput);

        // Check that, the custom class input is exists and visible.
        context.Exists(customClassesInputSelector);

        context.Get(customClassesInputSelector)
            .SendKeys(TestClass);

        context.Get(customClassesInputSelector)
            .GetValue()
            .ShouldBe(TestClass);
    }
}
