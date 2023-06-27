using Lombiq.HelpfulExtensions.Extensions.ContentSets.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.ContentManagement.Metadata.Models;
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
}

public record ContentSetLinkViewModel(bool IsDeleted, string DisplayText, string ContentItemId, string Key);
