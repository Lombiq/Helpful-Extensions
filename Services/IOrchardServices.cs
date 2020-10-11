using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;
using OrchardCore.Modules;
using OrchardCore.Settings;
using System;
using System.Diagnostics.CodeAnalysis;
using YesSql;

namespace Lombiq.HelpfulExtensions.Services
{
    /// <summary>
    /// A convenience bundle of services that are common dependencies of other CMS services in Orchard Core.
    /// </summary>
    /// <typeparam name="T">The type of the dependant service, used for logger.</typeparam>
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:Elements should be documented",
        Justification = "There is nothing to add past what's already on the individual services' documentations.")]
    public interface IOrchardServices<T>
    {
        Lazy<IClock> Clock { get; }
        Lazy<IContentAliasManager> ContentAliasManager { get; }
        Lazy<IContentManager> ContentManager { get; }
        Lazy<IHttpContextAccessor> HttpContextAccessor { get; }
        Lazy<ILogger<T>> Logger { get; }
        Lazy<Session> Session { get; }
        Lazy<ISiteService> SiteService { get; }
    }
}
