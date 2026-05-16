using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using Shouldly;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Tests.UI.Extensions;

public static class TrumbowygBlogPostsTestCaseUITestContextExtensions
{
    /// <summary>
    /// Tests the Lombiq Helpful Extensions - Trumbowyg code snippet - Blog Posts feature, and thus also the Lombiq Helpful
    /// Extensions - Trumbowyg code snippet one.
    /// </summary>
    public static async Task TestTrumbowygBlogPostsFeatureAsync(this UITestContext context)
    {
        void AssertCodeSnippetIsHighlighted() =>
            context.Get(By.CssSelector("code.language-clike")).Text.ShouldStartWith("Console");

        await context.EnableFeatureDirectlyAsync(FeatureIds.TrumbowygBlogPosts);
        await context.ExecuteRecipeDirectlyAsync("Lombiq.HelpfulExtensions.Tests.UI.TrumbowygBlogPosts.Tests");

        await context.SignInDirectlyAndGoToDashboardAsync();

        await context.GoToContentItemListAsync("Blog");
        await context.ClickReliablyOnAsync(By.ClassName("view"));
        context.SwitchToLastWindow();
        await context.ClickReliablyOnByLinkTextAsync("Man must explore, and this is exploration at its greatest");

        await context.ClickReliablyOnAsync(By.ClassName("trumbowyg-highlight-button"));
        await context.FillInWithRetriesAsync(By.CssSelector("textarea.trumbowyg-highlight-form-control"), "Console.WriteLine(\"Hello, world!\");");
        await context.ClickReliablyOnAsync(By.ClassName("trumbowyg-modal-submit"));
        AssertCodeSnippetIsHighlighted();

        await context.ClickPublishAsync();

        await context.GoToRelativeUrlAsync("/blog/post-1");
        AssertCodeSnippetIsHighlighted();
    }
}
