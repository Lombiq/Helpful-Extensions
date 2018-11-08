namespace Orchard.ContentManagement
{
    public static class ContentManagerExtensions
    {
        public static IContent NewDummyItem(this IContentManager contentManager) =>
            contentManager.New("Dummy");

        public static ContentItem GetContentItemByIdentity(this IContentManager contentManager, string identity) =>
            contentManager.ResolveIdentity(new ContentIdentity(identity));
    }
}