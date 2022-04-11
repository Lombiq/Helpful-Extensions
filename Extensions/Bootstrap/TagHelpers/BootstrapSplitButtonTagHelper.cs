using Microsoft.AspNetCore.Razor.TagHelpers;
using OrchardCore.DisplayManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.Bootstrap.TagHelpers;

[HtmlTargetElement("bootstrap-split-button", Attributes = "options")]
public class BootstrapSplitButtonTagHelper : TagHelper
{
    private readonly IDisplayHelper _displayHelper;
    private readonly IShapeFactory _shapeFactory;

    [HtmlAttributeName("type")]
    public string Type { get; set; }

    [HtmlAttributeName("text")]
    public string Text { get; set; }

    [HtmlAttributeName("class")]
    public string WrapperClasses { get; set; }

    [HtmlAttributeName("button-classes")]
    public string ButtonClasses { get; set; }

    [HtmlAttributeName("toggle-classes")]
    public string ToggleClasses { get; set; }

    [HtmlAttributeName("dropdown-classes")]
    public string DropdownClasses { get; set; }

    [HtmlAttributeName("options")]
    public IEnumerable<(string Url, string Text)> Options { get; set; }

    public BootstrapSplitButtonTagHelper(IDisplayHelper displayHelper, IShapeFactory factory)
    {
        _displayHelper = displayHelper;
        _shapeFactory = factory;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        IShape shape = await _shapeFactory.New.BootstrapSplitButton(
            Type: Type,
            Text: Text,
            WrapperClasses: WrapperClasses,
            ButtonClasses: ButtonClasses,
            ToggleClasses: ToggleClasses,
            DropdownClasses: DropdownClasses,
            Options: Options);
        var content = await _displayHelper.ShapeExecuteAsync(shape);

        output.TagName = null;
        output.TagMode = TagMode.StartTagAndEndTag;
        output.PostContent.SetHtmlContent(content);
    }
}
