namespace Orchard.ContentManagement
{
    public static class ContentManagerExtensions
    {
        public static IContent NewDummyItem(this IContentManager contentManager) =>
            contentManager.New("Dummy");
    }
}