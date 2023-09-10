using Lombiq.HelpfulExtensions.Extensions.ContentSets.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentManagement.Metadata.Settings;
using System.Collections.Generic;
using System.Linq;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets.ViewModels;

public class ContentSetPartViewModel
{
    public string ContentSet { get; set; }
    public string Key { get; set; }

    [BindNever]
    public ContentTypePartDefinition Definition { get; set; }

    [BindNever]
    public ContentSetPart ContentSetPart { get; set; }

    [BindNever]
    public IEnumerable<ContentSetLinkViewModel> MemberLinks { get; set; } = Enumerable.Empty<ContentSetLinkViewModel>();

    [BindNever]
    public bool IsNew { get; set; }

    [BindNever]
    public string DisplayName =>
        Definition?
            .Settings?
            .Property(nameof(ContentTypePartSettings))?
            .Value
            .ToObject<ContentTypePartSettings>()?
            .DisplayName ?? Definition?.Name;
}

public record ContentSetLinkViewModel(bool IsDeleted, string DisplayText, string ContentItemId, string Key);
