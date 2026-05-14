#nullable enable

using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.Views;
using System.Collections.Generic;

namespace Lombiq.HelpfulExtensions.Extensions.SimpleIcons;

public class SimpleIconViewModel : ShapeViewModel
{
    public const string ShapeType = "SimpleIcon";

    public string? Source { get; set; }
    public string? IconClasses { get; set; }
    public string? LabelClasses { get; set; }
    public int Size { get; set; } = 24;
    public string? Title { get; set; }
    public bool ShowLabel { get; set; }

    public SimpleIconViewModel() => Metadata.Type = ShapeType;

    public SimpleIconViewModel(SimpleIconTagHelper helper)
        : this()
    {
        Source = helper.Source;
        IconClasses = helper.IconClasses;
        LabelClasses = helper.LabelClasses;
        Size = helper.Size;
        Title = helper.Title;
        ShowLabel = helper.ShowLabel;
    }

    public static SimpleIconViewModel FromShape(IShape shape)
    {
        if (shape.Properties.GetMaybe("ViewModel") is SimpleIconViewModel shapeViewModel)
        {
            return shapeViewModel;
        }

        return new SimpleIconViewModel
        {
            Source = shape.Properties.GetMaybe(nameof(Source))?.ToString(),
            IconClasses = shape.Properties.GetMaybe(nameof(IconClasses))?.ToString(),
            LabelClasses = shape.Properties.GetMaybe(nameof(LabelClasses))?.ToString(),
            Size = shape.Properties.GetMaybe(nameof(Size)) is int sizeInt ? sizeInt : 24,
            Title = shape.Properties.GetMaybe(nameof(Title))?.ToString(),
            ShowLabel = shape.Properties.GetMaybe(nameof(ShowLabel)) is true or "true",
        };
    }
}
