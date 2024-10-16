using OrchardCore.Autoroute.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using System.Threading.Tasks;
using static Lombiq.HelpfulExtensions.Extensions.ContentTypes.ContentTypes;

namespace Lombiq.HelpfulExtensions.Extensions.ContentTypes;

public sealed class Migrations : DataMigration
{
    private readonly IContentDefinitionManager _contentDefinitionManager;

    public Migrations(IContentDefinitionManager contentDefinitionManager) =>
        _contentDefinitionManager = contentDefinitionManager;

    public async Task<int> CreateAsync()
    {
        await _contentDefinitionManager.AlterTypeDefinitionAsync(Page, builder => builder
            .Creatable()
            .Securable()
            .Draftable()
            .Listable()
            .Versionable()
            .WithPart("TitlePart", part => part.WithPosition("0"))
            .WithPart("AutoroutePart", part => part
                .WithPosition("1")
                .WithSettings(new AutoroutePartSettings
                {
                    ShowHomepageOption = true,
                    AllowCustomPath = true,
                })
            )
            .WithPart("FlowPart", part => part.WithPosition("2"))
        );

        await _contentDefinitionManager.AlterTypeDefinitionAsync(Empty, builder => builder
            .WithDescription("A base content type for ad-hoc welding parts or fields on.")
        );

        return 3;
    }

    public async Task<int> UpdateFrom1Async()
    {
        await _contentDefinitionManager.AlterTypeDefinitionAsync(Page, builder => builder
            .WithPart("TitlePart", part => part.WithPosition("0"))
            .WithPart("AutoroutePart", part => part.WithPosition("1"))
            .WithPart("FlowPart", part => part.WithPosition("2"))
        );

        return 2;
    }

    public async Task<int> UpdateFrom2Async()
    {
        await _contentDefinitionManager.AlterTypeDefinitionAsync(Empty, builder => builder
            .WithDescription("A base content type for ad-hoc welding parts or fields on.")
        );

        return 3;
    }
}
