using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;

namespace Orchard.Security
{
    public static class UserExtensions
    {
        public static bool IsOwnerOf(this IUser user, IContent content) =>
            user == null || content == null ? false : user.Id == content.As<ICommonPart>()?.Owner?.Id;
    }
}