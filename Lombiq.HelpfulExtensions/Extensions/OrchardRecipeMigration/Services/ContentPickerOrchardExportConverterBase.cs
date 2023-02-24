using LombiqDotCom.Models;
using Newtonsoft.Json.Linq;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LombiqDotCom.Services;

public abstract class ContentPickerOrchardExportConverterBase : IOrchardExportConverter
{
    protected abstract string ContentPickerFieldName { get; }

    public Task UpdateContentItemsAsync(XDocument document, IList<ContentItem> contentItems)
    {
        var itemsById = contentItems
            .Where(item => !string.IsNullOrEmpty(item.As<OrchardIds>()?.ExportId))
            .ToDictionary(item => item.As<OrchardIds>().ExportId);

        foreach (var item in itemsById.Values.Where(item => !string.IsNullOrEmpty(item.As<OrchardIds>().Parent)))
        {
            var parentId = item.As<OrchardIds>().Parent;
            if (!itemsById.TryGetValue(parentId, out var parent) ||
                !parent.Has(parent.ContentType) ||
                GetContentTypePart(parent) is not JObject contentItemPart ||
                contentItemPart.GetMaybe(ContentPickerFieldName) is not JObject contentPickerJson)
            {
                continue;
            }

            var contentPicker = contentPickerJson.ToObject<ContentPickerField>();
            contentPicker.ContentItemIds = (contentPicker.ContentItemIds ?? Array.Empty<string>())
                .Concat(new[] { item.ContentItemId })
                .ToArray();
            contentItemPart[ContentPickerFieldName] = JToken.FromObject(contentPicker);
        }

        return Task.CompletedTask;
    }

    private static JToken GetContentTypePart(ContentItem item)
    {
        var json = item.Content as JObject;
        return json.GetMaybe(item.ContentType) ?? json.GetMaybe(item.ContentType + "Part");
    }
}
