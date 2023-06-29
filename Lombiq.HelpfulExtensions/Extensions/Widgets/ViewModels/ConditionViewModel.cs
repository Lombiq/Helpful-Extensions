using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Localization;

namespace Lombiq.HelpfulExtensions.Extensions.Widgets.ViewModels;

public class ConditionViewModel
{
    public LocalizedHtmlString Title { get; set; }
    public LocalizedHtmlString Description { get; set; }
    public IHtmlContent SummaryHint { get; set; }

    public void CopyTo(ConditionViewModel target)
    {
        target.Title = Title;
        target.Description = Description;
        target.SummaryHint = SummaryHint;
    }
}
