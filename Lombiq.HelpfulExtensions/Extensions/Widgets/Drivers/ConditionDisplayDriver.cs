using Lombiq.HelpfulExtensions.Extensions.Widgets.ViewModels;
using Lombiq.HelpfulLibraries.OrchardCore.Contents;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Rules;

namespace Lombiq.HelpfulExtensions.Extensions.Widgets.Drivers;

public abstract class ConditionDisplayDriver<TCondition> : DisplayDriver<Condition, TCondition>
    where TCondition : Condition
{
    public override IDisplayResult Display(TCondition model) =>
        Combine(
            InitializeDisplayType(CommonContentDisplayTypes.Summary, model),
            InitializeDisplayType(CommonContentDisplayTypes.Thumbnail, model));

    protected abstract ConditionViewModel GetConditionViewModel(TCondition condition);

    private ShapeResult InitializeDisplayType(string displayType, TCondition model) =>
        Initialize<ConditionViewModel>(
                "Condition_Fields_" + displayType,
                target => GetConditionViewModel(model).CopyTo(target))
            .Location(displayType, CommonLocationNames.Content);
}
