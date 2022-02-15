using Lombiq.HelpfulExtensions.Extensions.Security.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;

namespace Lombiq.HelpfulExtensions.Extensions.Security.Migrations
{
    public class StrictSecurityPartMigration : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public StrictSecurityPartMigration(IContentDefinitionManager contentDefinitionManager)
            => _contentDefinitionManager = contentDefinitionManager;

        public int Create()
        {
            _contentDefinitionManager.AlterPartDefinition<StrictSecurityPart>(part => part
                .Configure(builder => builder.Attachable()));

            return 1;
        }
    }
}
