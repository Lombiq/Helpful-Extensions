using Lombiq.HelpfulExtensions.Extensions.Widgets.ViewModels;
using Lombiq.HelpfulLibraries.OrchardCore.Contents;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Rules;
using System.Collections.Generic;

namespace Lombiq.HelpfulExtensions.Extensions.Widgets.Drivers;

public abstract class ConditionDisplayDriver<TCondition> : DisplayDriver<Condition, TCondition>
    where TCondition : Condition
{
    public override IDisplayResult Display(TCondition model) =>
        Combine(
            InitializeDisplayType(CommonContentDisplayTypes.Summary, model),
            InitializeDisplayType(CommonContentDisplayTypes.Thumbnail, model));

    public override IDisplayResult Edit(TCondition model) =>
        Combine(
            InitializeDisplayType(CommonContentDisplayTypes.Detail, model, "Title"),
            GetEditor(model));

    protected abstract ConditionViewModel GetConditionViewModel(TCondition condition);
    protected abstract IDisplayResult GetEditor(TCondition model);

    private ShapeResult InitializeDisplayType(string displayType, TCondition model, string shapeTypeSuffix = null) =>
        Initialize<ConditionViewModel>(
                string.Join("_", new[] { "Condition", "Fields", displayType, shapeTypeSuffix }.WhereNot(string.IsNullOrEmpty)),
                target => GetConditionViewModel(model).CopyTo(target))
            .Location(displayType, CommonLocationNames.Content);
}
