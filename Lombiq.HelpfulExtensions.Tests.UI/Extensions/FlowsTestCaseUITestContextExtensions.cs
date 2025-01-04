using Atata;
using Lombiq.HelpfulExtensions.Tests.UI.Constants;
using Lombiq.HelpfulExtensions.Tests.UI.Helpers;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Shouldly;
using System.Threading.Tasks;
using static Lombiq.HelpfulExtensions.Tests.UI.Constants.TextInputValues;
using static Lombiq.HelpfulExtensions.Tests.UI.Constants.XPathSelectors;

namespace Lombiq.HelpfulExtensions.Tests.UI.Extensions;

public static class FlowsTestCaseUITestContextExtensions
{
    /// <summary>
    /// Tests the Lombiq Helpful Extensions - Flows Helpful Extensions feature.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Added originally to cover the fix for <see href="https://github.com/Lombiq/Helpful-Extensions/issues/76">
    /// Flow 'Additional Styling Part' flyout not activating</see>.
    /// </para>
    /// </remarks>
    public static async Task TestFlowsFeatureAsync(this UITestContext context)
    {
        await context.SignInDirectlyAsync();
        await context.EnableFeatureDirectlyAsync(FeatureIds.Flows);
        await context.EnableFeatureDirectlyAsync(FeatureIds.ContentTypes);
        await context.EnableFeatureDirectlyAsync(FeatureIds.Widgets);

        await context.GoToCreatePageAsync();

        // Adding 'Html' to flow.
        WidgetHelpers.AddWidgetToPageFlow(context, WidgetTypes.Html);

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
}
