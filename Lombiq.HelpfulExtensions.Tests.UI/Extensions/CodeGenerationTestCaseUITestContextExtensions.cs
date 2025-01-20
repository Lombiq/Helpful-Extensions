using Atata;
using Lombiq.HelpfulExtensions.Tests.UI.Constants;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using Shouldly;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Tests.UI.Extensions;

public static class CodeGenerationTestCaseUITestContextExtensions
{
    /// <summary>
    /// Tests the Lombiq Helpful Extensions - Code Generation Helpful Extensions feature.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Added originally to cover the fix for <see href="https://github.com/Lombiq/Helpful-Extensions/issues/85">
    /// Fix content type code generation button not working</see>.
    /// </para>
    /// </remarks>
    public static async Task TestCodeGenerationFeatureAsync(this UITestContext context)
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

                GeneratedMigrationCodes.ShouldBePage(
                    context.Get(By.Id("generated-migration-code").OfAnyVisibility()).GetValue());

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
}
