using Lombiq.HelpfulLibraries.OrchardCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using OrchardCore.DisplayManagement;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.GoogleTag;

[HtmlTargetElement("google-tag")]
public class GoogleTagTagHelper : ShapeTagHelperBase<GoogleTagViewModel>
{
    [HtmlAttributeName("property-id")]
    public string PropertyId { get; set; }

    [HtmlAttributeName("cookie-domain")]
    public string CookieDomain { get; set; }

    public GoogleTagTagHelper(IDisplayHelper displayHelper, IShapeFactory shapeFactory)
        : base(displayHelper, shapeFactory)
    {
    }

    protected override string ShapeType => GoogleTagViewModel.ShapeType;

    protected override ValueTask<GoogleTagViewModel> GetViewModelAsync(TagHelperContext context, TagHelperOutput output) =>
        ValueTask.FromResult(new GoogleTagViewModel(PropertyId, CookieDomain));
}
