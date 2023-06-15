using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Lombiq.HelpfulExtensions.Extensions.Widgets.Models;

public class ContentItemWidget : ContentPart
{
    public ContentPickerField ContentToDisplay { get; set; } = new();
    public TextField DisplayType { get; set; } = new();
    public TextField GroupId { get; set; } = new();
}
