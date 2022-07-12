namespace Lombiq.HelpfulExtensions.Tests.UI.Constants;

public static class XPathSelectors
{
    public const string AddWidgetButtonGroup = "//div[contains(@class, 'btn-widget-add-below-wrapper')]"
        + "/div/div[@class='btn-group']";
    public const string AddWidgetButton = $"{AddWidgetButtonGroup}/button[@title='Add Widget']";
    public const string WidgetList = $"{AddWidgetButtonGroup}/div[@class='dropdown-menu']";
    public const string WidgetEditorHeader = "//div[contains(@class, 'widget-template')]"
        + "/div[contains(@class, 'widget-editor')]"
        + "/div[contains(@class, 'widget-editor-header')]";
    public const string WidgetEditorHeaderText = $"{WidgetEditorHeader}/span[contains(@class, 'widget-editor-header-text')]";
    public const string FlowSettingsButtonGroup = $"{WidgetEditorHeader}/div[contains(@class, 'btn-widget-metadata')]"
        + "/div[@class='btn-group']/div[@class='btn-group']";
    public const string FlowSettingsButton = $"{FlowSettingsButtonGroup}/button[@title='Settings']";
    public const string FlowSettingsDropdown = $"{FlowSettingsButtonGroup}/div[contains(@class, 'dropdown-menu')]";
    public const string CustomClassesInput = "id('FlowPart-0_ContentItem_CustomClasses')";
}
