using Orchard.ContentManagement.FieldStorage.InfosetStorage;
using Orchard.ContentManagement.Handlers;
using System.Xml;
using System.Xml.Linq;

namespace Orchard.ContentManagement.Drivers
{
    public static class ContentPartDriverExtensions
    {
        public static void CloneInfoset<TContent>(
            this ContentPartDriver<TContent> driver,
            TContent originalPart, TContent clonePart) where TContent : ContentPart, new()
        {
            if (!clonePart.Has<InfosetPart>()) return;

            var originalInfosetPart = originalPart.As<InfosetPart>();
            if (originalInfosetPart == null) return;

            var context = new ExportContentContext(
                originalPart.ContentItem,
                new XElement(XmlConvert.EncodeLocalName(originalPart.ContentItem.ContentType)));

            void exportInfoset(XElement element, bool versioned)
            {
                if (element == null) return;

                var elementName = GetInfosetXmlElementName(originalPart, versioned);
                foreach (var attribute in element.Attributes())
                {
                    context.Element(elementName).SetAttributeValue(attribute.Name, attribute.Value);
                }
            }

            exportInfoset(originalInfosetPart.VersionInfoset.Element.Element(originalPart.PartDefinition.Name), true);
            exportInfoset(originalInfosetPart.Infoset.Element.Element(originalPart.PartDefinition.Name), false);

            void importInfoset(XElement element, bool versioned)
            {
                if (element == null) return;

                foreach (var attribute in element.Attributes())
                {
                    clonePart.Store(attribute.Name.ToString(), attribute.Value, versioned);
                }
            }

            importInfoset(context.Data.Element(GetInfosetXmlElementName(clonePart, true)), true);
            importInfoset(context.Data.Element(GetInfosetXmlElementName(clonePart, false)), false);
        }


        private static string GetInfosetXmlElementName<TContent>(TContent part, bool versioned) where TContent : ContentPart, new() =>
            part.PartDefinition.Name + "-" + (versioned ? "VersionInfoset" : "Infoset");
    }
}