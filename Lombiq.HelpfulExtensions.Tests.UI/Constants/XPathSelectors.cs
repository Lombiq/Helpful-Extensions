namespace Lombiq.HelpfulExtensions.Tests.UI.Constants;

public static class XPathSelectors
{
    public const string AddWidgetButtonGroup = "//div[contains(@class, 'btn-widget-add-below-wrapper')]"
        + "/div/div[@class='btn-group']";
    public const string AddWidgetButton = $"{AddWidgetButtonGroup}/button[@title='Add Widget']";
    public const string BlockquoteWidgetButton = $"{AddWidgetButtonGroup}/div[@class='dropdown-menu']"
        + "/a[text()='Blockquote']";
    public const string WidgetEditorHeader = "//div[contains(@class, 'widget-template')]"
        + "/div[contains(@class, 'widget-editor')]"
        + "/div[contains(@class, 'widget-editor-header')]";
    public const string FlowSettingsButtonGroup = $"{WidgetEditorHeader}/div[contains(@class, 'btn-widget-metadata')]"
        + "/div[@class='btn-group']/div[@class='btn-group']";
    public const string FlowSettingsButton = $"{FlowSettingsButtonGroup}/button[@title='Settings']";
    public const string CustomClassesInput = $"{FlowSettingsButtonGroup}/div[contains(@class, 'dropdown-menu')]"
        + "/div[@class='form-group']/input[@id='FlowPart-0_ContentItem_CustomClasses']";
}
