using Lombiq.HelpfulLibraries.OrchardCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using OrchardCore.DisplayManagement;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.SimpleIcons;

[HtmlTargetElement("simple-icon")]
public class SimpleIconTagHelper : ShapeTagHelperBase<SimpleIconViewModel>
{
    [HtmlAttributeName("source")]
    public string Source { get; set; }

    [HtmlAttributeName("icon-classes")]
    public string IconClasses { get; set; }

    [HtmlAttributeName("label-classes")]
    public string LabelClasses { get; set; }

    [HtmlAttributeName("size")]
    public int Size { get; set; }

    [HtmlAttributeName("title")]
    public string Title { get; set; }

    [HtmlAttributeName("show-label")]
    public bool ShowLabel { get; set; }

    public SimpleIconTagHelper(IDisplayHelper displayHelper, IShapeFactory shapeFactory)
        : base(displayHelper, shapeFactory)
    {
    }

    protected override string ShapeType => SimpleIconViewModel.ShapeType;

    protected override ValueTask<SimpleIconViewModel> GetViewModelAsync(TagHelperContext context, TagHelperOutput output) =>
        ValueTask.FromResult(new SimpleIconViewModel(this));
}
