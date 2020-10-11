using Microsoft.AspNetCore.Http;
using OrchardCore.ContentManagement;
using OrchardCore.DisplayManagement;
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
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:Elements should be documented",
        Justification = "There is nothing to add past what's already on the individual services' documentations.")]
    public interface IOrchardServices
    {
        Lazy<IClock> Clock { get; }
        Lazy<IContentAliasManager> ContentAliasManager { get; }
        Lazy<IContentManager> ContentManager { get; }
        Lazy<IDisplayHelper> DisplayHelper { get; }
        Lazy<ISiteService> SiteService { get; }
        Lazy<Session> Session { get; }
        Lazy<IHttpContextAccessor> HttpContextAccessor { get; }
    }
}
