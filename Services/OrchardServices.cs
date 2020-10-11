using Microsoft.AspNetCore.Http;
using OrchardCore.ContentManagement;
using OrchardCore.DisplayManagement;
using OrchardCore.Modules;
using OrchardCore.Settings;
using System;
using YesSql;

namespace Lombiq.HelpfulExtensions.Services
{
    public class OrchardServices : IOrchardServices
    {
        public Lazy<IClock> Clock { get; }
        public Lazy<IContentAliasManager> ContentAliasManager { get; }
        public Lazy<IContentManager> ContentManager { get; }
        public Lazy<IDisplayHelper> DisplayHelper { get; }
        public Lazy<ISiteService> SiteService { get; }
        public Lazy<Session> Session { get; }
        public Lazy<IHttpContextAccessor> HttpContextAccessor { get; }

        public OrchardServices(
            Lazy<IClock> clock,
            Lazy<IContentAliasManager> contentAliasManager,
            Lazy<IContentManager> contentManager,
            Lazy<IDisplayHelper> displayHelper,
            Lazy<ISiteService> siteService,
            Lazy<Session> session,
            Lazy<IHttpContextAccessor> httpContextAccessor)
        {
            Clock = clock;
            ContentAliasManager = contentAliasManager;
            ContentManager = contentManager;
            DisplayHelper = displayHelper;
            SiteService = siteService;
            Session = session;
            HttpContextAccessor = httpContextAccessor;
        }
    }
}
