using Atata;
using Lombiq.HelpfulExtensions.Tests.UI.Constants;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
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
        await context.RetryIfStaleOrFailAsync(async () =>
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

                return true;
            }
            catch (StaleElementReferenceException)
            {
                context.Refresh();
                throw;
            }
        });
    }

    /// <summary>
    /// Tests the Lombiq Helpful Extensions - Content Sets feature.
    /// </summary>
    public static async Task TestFeatureContentSetsAsync(this UITestContext context)
    {
        const string contentId0 = "contentsetexample000000000";
        const string contentId2 = "contentsetexample000000002";

        var byLink = By.CssSelector(".field-name-content-set-example-content-set-type .value a");

        void VerifyDisplay(string title, string body, params string[] linkTexts)
        {
            var contentItem = context.Get(By.ClassName("content-set-example"));

            contentItem.Get(By.TagName("h1")).Text.Trim().ShouldBe(title);
            contentItem.Get(By.ClassName("content-set-example-body")).Text.Trim().ShouldBe(body);

            contentItem
                .GetAll(byLink)
                .Select(link => link.Text.Trim())
                .ToArray()
                .ShouldBe(linkTexts);
        }

        // Verify the default item.
        await context.SignInDirectlyAsync();
        await context.GoToRelativeUrlAsync($"/Contents/ContentItems/{contentId0}");
        VerifyDisplay("Default Content Set Example", "Some generic text.", "Other Example", "Some Example");

        // Verify the first item both by content set content picker link and direct access.
        await context.ClickReliablyOnAsync(byLink);
        VerifyDisplay("Second Content Set Variant", "Some generic text v2.", "Default content item", "Some Example");
        await context.GoToRelativeUrlAsync($"/Contents/ContentItems/{contentId2}");
        VerifyDisplay("Second Content Set Variant", "Some generic text v2.", "Default content item", "Some Example");

        // Create the final variant.
        await context.GoToContentItemListAsync("ContentSetExample");
        await context.SelectFromBootstrapDropdownReliablyAsync(
            context.Get(By.XPath("//li[contains(@class, 'list-group-item')][3]//div[@title='Content Set Type']//button")),
            By.XPath("//a[@title='Create Final Example']"));
        await context.ClickAndFillInWithRetriesAsync(By.Id("TitlePart_Title"), "Test Title");
        await context.ClickPublishAsync();
        context.ShouldBeSuccess();

        // Verify changes.
        await context.GoToRelativeUrlAsync($"/Contents/ContentItems/{contentId0}");
        VerifyDisplay("Default Content Set Example", "Some generic text.", "Final Example", "Other Example", "Some Example");
        await context.ClickReliablyOnAsync(byLink);
        VerifyDisplay("Test Title", "Some generic text v1.", "Default content item", "Other Example", "Some Example");
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
