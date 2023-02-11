using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Data.Migration;

namespace Lombiq.HelpfulExtensions.Extensions;

/// We need to use this until the Orchard Core 1.6 upgrade so we don't have to upgrade submodules to
/// to nightly versions. This is the same method as what is in the 1.6 nightly version of Orchard Core.
/// During the upgrade, this can be removed and use the Orchard Core method for data migrations.
/// <summary>
/// Provides extension methods for <see cref="IServiceCollection"/> to add YesSql migration <see cref="IDataMigration"/>.
/// </summary>
public static class MigrationServiceCollectionExtensions
{
    public static IServiceCollection AddDataMigration<TDataMigration>(this IServiceCollection services)
        where TDataMigration : class, IDataMigration => services.AddScoped<IDataMigration, TDataMigration>();
}
