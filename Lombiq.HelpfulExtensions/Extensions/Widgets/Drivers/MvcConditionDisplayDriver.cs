using Lombiq.HelpfulExtensions.Extensions.Widgets.Models;
using Lombiq.HelpfulExtensions.Extensions.Widgets.ViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.Widgets.Drivers;

public sealed class MvcConditionDisplayDriver : ConditionDisplayDriver<MvcCondition>
{
    private readonly IHtmlLocalizer<MvcConditionDisplayDriver> H;
    private readonly IStringLocalizer<MvcConditionDisplayDriver> T;

    public MvcConditionDisplayDriver(
        IHtmlLocalizer<MvcConditionDisplayDriver> htmlLocalizer,
        IStringLocalizer<MvcConditionDisplayDriver> stringLocalizer)
    {
        H = htmlLocalizer;
        T = stringLocalizer;
    }

    protected override IDisplayResult GetEditor(MvcCondition model) =>
        Initialize<MvcConditionViewModel>("MvcCondition_Fields_Edit", viewModel =>
        {
            viewModel.Area = model.Area;
            viewModel.Controller = model.Controller;
            viewModel.Action = model.Action;

            foreach (var (key, value) in model.OtherRouteValues)
            {
                viewModel.OtherRouteNames.Add(key);
                viewModel.OtherRouteValues.Add(value);
            }
        }).PlaceInContent();

    public override async Task<IDisplayResult> UpdateAsync(MvcCondition model, UpdateEditorContext context)
    {
        var viewModel = await context.CreateModelAsync<MvcConditionViewModel>(Prefix);

        if (viewModel.OtherRouteNames.Count != viewModel.OtherRouteValues.Count)
        {
            context.Updater.ModelState.AddModelError(
                nameof(viewModel.OtherRouteNames),
                T["The count of other route value names didn't match the count of other route values."]);
        }

        model.Area = viewModel.Area;
        model.Controller = viewModel.Controller;
        model.Action = viewModel.Action;

        model.OtherRouteValues.Clear();
        for (var i = 0; i < viewModel.OtherRouteNames.Count; i++)
        {
            model.OtherRouteValues[viewModel.OtherRouteNames[i]] = viewModel.OtherRouteValues[i];
        }

        return await EditAsync(model, context);
    }

    protected override ConditionViewModel GetConditionViewModel(MvcCondition condition)
    {
        IHtmlContentBuilder summaryHint = new HtmlContentBuilder();

        static IHtmlContentBuilder AppendIfNotEmpty(IHtmlContentBuilder summaryHint, string value, IHtmlContent label) =>
            string.IsNullOrEmpty(value) ? summaryHint : summaryHint.AppendHtml(label).AppendHtml(" ");

        summaryHint = AppendIfNotEmpty(summaryHint, condition.Area, H["Area: \"{0}\"", condition.Area]);
        summaryHint = AppendIfNotEmpty(summaryHint, condition.Controller, H["Controller: \"{0}\"", condition.Controller]);
        summaryHint = AppendIfNotEmpty(summaryHint, condition.Action, H["Action: \"{0}\"", condition.Action]);

        if (condition.OtherRouteValues.Any())
        {
            summaryHint = summaryHint.AppendHtml(
                H["Other route values: {0}", JsonSerializer.Serialize(condition.OtherRouteValues)]);
        }

        return new ConditionViewModel
        {
            Title = H["MVC condition"],
            Description = H["An MVC condition evaluates the currently route values such as controller and action name."],
            SummaryHint = summaryHint,
        };
    }
}
