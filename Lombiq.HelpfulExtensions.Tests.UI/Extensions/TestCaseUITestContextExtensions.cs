using Atata;
using Lombiq.HelpfulExtensions.Tests.UI.Constants;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Shouldly;
using System.Threading.Tasks;
using Xunit.Abstractions;
using static Lombiq.HelpfulExtensions.Tests.UI.Constants.TextInputValues;
using static Lombiq.HelpfulExtensions.Tests.UI.Constants.XPathSelectors;

namespace Lombiq.HelpfulExtensions.Tests.UI.Extensions;

public static class TestCaseUITestContextExtensions
{
    /// <summary>
    /// Tests the Lombiq Helpful Extensions - Helpful Widgets feature.
    /// </summary>
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

    /// <summary>
    /// Tests the Lombiq Helpful Extensions - Flows Helpful Extensions feature.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Added originally to cover the fix for <see href="https://github.com/Lombiq/Helpful-Extensions/issues/76">
    /// Flow 'Additional Styling Part' flyout not activating</see>.
    /// </para>
    /// </remarks>
    public static async Task TestFlowAdditionalStylingPartAsync(this UITestContext context)
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

    /// <summary>
    /// Tests the Lombiq Helpful Extensions - Code Generation Helpful Extensions feature.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Added originally to cover the fix for <see href="https://github.com/Lombiq/Helpful-Extensions/issues/85">
    /// Fix content type code generation button not working</see>.
    /// </para>
    /// </remarks>
    public static async Task TestFeatureCodeGenerationsAsync(this UITestContext context)
    {
        await context.SignInDirectlyAsync();

        await context.EnableFeatureDirectlyAsync(FeatureIds.ContentTypes);
        await context.EnableFeatureDirectlyAsync(FeatureIds.CodeGeneration);

        await context.GoToContentTypeEditorAsync("Page");

        // These CodeMirror checks can throw StaleElementReferenceException even if the code section is there, opened,
        // and visible in the current viewport (though this shouldn't matter). So, it should just work, but it doesn't,
        // so we need to retry for these random issues.
        var succeeded = false;
        const int maxTries = 3;
        for (var currentTry = 1; !succeeded && currentTry <= maxTries; currentTry++)
        {
            try
            {
                await context.ClickReliablyOnAsync(By.ClassName("toggle-showing-generated-migration-code"));

                context.Get(By.Id("generated-migration-code").OfAnyVisibility()).GetValue().ShouldBe(GeneratedMigrationCodes.Page);

                // Making sure that the collapsible area is open.
                context.Get(By.CssSelector("#generated-migration-code-container.collapse.show"));

                // Checking the first line of the CodeMirror editor.
                context.Get(By.CssSelector(".CodeMirror-line .cm-variable")).Text.ShouldBe("_contentDefinitionManager");
                context.Get(By.CssSelector(".CodeMirror-line .cm-property")).Text.ShouldBe("AlterTypeDefinition");
                context.Get(By.CssSelector(".CodeMirror-line .cm-string")).Text.ShouldBe("\"Page\"");

                succeeded = true;
            }
            catch (StaleElementReferenceException) when (currentTry < maxTries)
            {
                context.Configuration.TestOutputHelper.WriteLineTimestampedAndDebug(
                    "The CodeMirror code block was checked {0} times but failed with StaleElementReferenceException. " +
                        "It'll be checked {1} times altogether.",
                    currentTry,
                    maxTries);
                context.Refresh();
            }
        }
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
