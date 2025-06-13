using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.Shortcodes;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.Shortcodes;

[Feature(FeatureIds.Shortcodes)]
public sealed class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services) =>
        services.AddShortcode(
            "em",
            (_, content, _) => ValueTask.FromResult($"<em>{content}</em>"),
            describe =>
            {
                describe.DefaultValue = "[em] [/em]";
                describe.Hint = "Add emphasis with a shortcode.";
                describe.Usage = "[em]your content here[/em]";
                describe.Categories = ["HTML Content"];
            });
}
