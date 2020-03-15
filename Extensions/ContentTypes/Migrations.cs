using OrchardCore.Autoroute.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using static Lombiq.HelpfulExtensions.Extensions.ContentTypes.ContentTypes;

namespace Lombiq.HelpfulExtensions.Extensions.ContentTypes
{
    public class Migrations : DataMigration
    {
        IContentDefinitionManager _contentDefinitionManager;


        public Migrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }


        public int Create()
        {
            _contentDefinitionManager.AlterTypeDefinition(Page, builder => builder
                .Creatable()
                .Securable()
                .Draftable()
                .Listable()
                .Versionable()
                .WithPart("TitlePart")
                .WithPart("FlowPart")
                .WithPart("AutoroutePart", part => part
                    .WithSettings(new AutoroutePartSettings
                    {
                        ShowHomepageOption = true,
                        AllowCustomPath = true
                    })
                )
            );

            return 1;
        }
    }
}
