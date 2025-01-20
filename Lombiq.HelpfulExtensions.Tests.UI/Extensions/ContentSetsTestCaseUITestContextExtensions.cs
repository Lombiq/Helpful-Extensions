using Atata;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Tests.UI.Extensions;

public static class ContentSetsTestCaseUITestContextExtensions
{
    /// <summary>
    /// Tests the Lombiq Helpful Extensions - Content Sets feature.
    /// </summary>
    public static async Task TestContentSetsFeatureAsync(this UITestContext context)
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
}
