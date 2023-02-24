using LombiqDotCom.Models;
using OrchardCore.Alias.Models;
using OrchardCore.Autoroute.Models;
using OrchardCore.ContentManagement;
using OrchardCore.Html.Models;
using OrchardCore.Title.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LombiqDotCom.Services;

public class CommonOrchardContentConverter : IOrchardContentConverter
{
    public int Order => 0;

    public bool IsApplicable(XElement element) => true;

    public Task ImportAsync(XElement element, ContentItem contentItem)
    {
        bool HasElementAttribute(string elementName, string attributeName, out string value)
        {
            value = element.Element(elementName)?.Attribute(attributeName)?.Value.Trim();
            return !string.IsNullOrWhiteSpace(value);
        }

        var exportId = element.Attribute("Id")?.Value.Trim();
        contentItem.Alter<OrchardIds>(ids => ids.ExportId = exportId);
        if (exportId?.StartsWithOrdinal("/alias=") == true)
        {
            AlterIfPartExists<AliasPart>(contentItem, part => part.Alias = GetAlias(exportId));
        }

        if (element.Attribute("Status")?.Value == "Published")
        {
            contentItem.Published = true;
            contentItem.Latest = true;
        }

        ImportCommonPart(contentItem, element);

        if (element.Element("AutoroutePart") is { } autoroutePart &&
            autoroutePart.Attribute("Alias")?.Value is { } autoroutePartAlias)
        {
            AlterIfPartExists<AutoroutePart>(contentItem, part =>
            {
                part.Path = autoroutePartAlias;
                part.SetHomepage = autoroutePart.Attribute("PromoteToHomePage")?.Value == "true";
            });
        }

        if (HasElementAttribute("BodyPart", "Text", out var body))
        {
            AlterIfPartExists<HtmlBodyPart>(contentItem, part => part.Html = body);
        }

        if (HasElementAttribute("TitlePart", "Title", out var title))
        {
            contentItem.DisplayText = title;
            AlterIfPartExists<TitlePart>(contentItem, part => part.Title = title);
        }

        if (HasElementAttribute("IdentityPart", "Identifier", out var id) && id.Length <= 26)
        {
            contentItem.ContentItemId = id;
        }

        return Task.CompletedTask;
    }

    private static string GetAlias(string value) => value.Split("/alias=").Last().Trim().Replace("\\/", "/");

    private static void AlterIfPartExists<TPart>(ContentItem contentItem, Action<TPart> action)
        where TPart : ContentPart, new()
    {
        if (contentItem.Has<TPart>()) contentItem.Alter(action);
    }

    public static void ImportCommonPart(ContentItem contentItem, XElement parentElement)
    {
        if (parentElement.Element("CommonPart") is not { } commonPart) return;

        string Attribute(string name) => commonPart.Attribute(name)?.Value;

        DateTime? Date(string name) => DateTime.TryParse(Attribute(name), out var date) ? date : null;

        if (Attribute("Owner")?.Replace("/User.UserName=", string.Empty) is { } owner)
        {
            contentItem.Author = owner;
        }

        contentItem.CreatedUtc = Date("CreatedUtc") ?? contentItem.CreatedUtc;
        contentItem.PublishedUtc = Date("PublishedUtc") ?? contentItem.PublishedUtc;
        contentItem.ModifiedUtc = Date("ModifiedUtc") ?? contentItem.ModifiedUtc;
    }
}
