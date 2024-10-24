using Lombiq.HelpfulExtensions.Extensions.ContentSets.Indexes;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.Models;
using Lombiq.HelpfulLibraries.OrchardCore.Data;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using System.Threading.Tasks;
using YesSql.Sql;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets;

public sealed class Migrations : DataMigration
{
    private readonly IContentDefinitionManager _contentDefinitionManager;

    public Migrations(IContentDefinitionManager contentDefinitionManager) =>
        _contentDefinitionManager = contentDefinitionManager;

    public async Task<int> CreateAsync()
    {
        await _contentDefinitionManager.AlterPartDefinitionAsync(nameof(ContentSetPart), builder => builder
            .Attachable()
            .Reusable()
            .WithDisplayName("Content Set"));

        await SchemaBuilder.CreateMapIndexTableAsync<ContentSetIndex>(table => table
            .Column<string>(nameof(ContentSetIndex.ContentItemId), column => column.WithCommonUniqueIdLength())
            .Column<string>(nameof(ContentSetIndex.PartName))
            .Column<bool>(nameof(ContentSetIndex.IsPublished))
            .Column<string>(nameof(ContentSetIndex.ContentSet))
            .Column<string>(nameof(ContentSetIndex.Key)));

        return 1;
    }
}
