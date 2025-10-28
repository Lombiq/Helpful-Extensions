# Helpful Widgets

Adds multiple helpful widget content types. These are basic widgets that are added by built-in Orchard Core recipes though in case of using a custom setup recipe these can be added by this feature too.

## ContainerWidget

Works as a container for further widgets. It has a `FlowPart` attached to it so it can contain additional widgets as well.

It has a `TitlePart`, which doesn't get displayed on the front end. You can use it to make the admin editor more organized. This title is also added as a class and data-attribute to the widget's wrapper element. For example, if the title is "About this project", then the wrapper will look like this:

```html
<div class="widget widget-container-widget widget-align-justify widget-container-widget-title-about-this-project" data-title="About this project">
```

## HtmlWidget

Adds HTML editing and displaying capabilities using a WYSIWYG editor.

## LiquidWidget

Adds Liquid code editing and rendering capabilities.

## MenuWidget ("Menu Navigation Provider Widget")

Renders a Bootstrap navigation menu as a widget using the provided `MenuItem`s.

> [!NOTE]
> The menu is populated from `INavigationProvider` implementations that create the menu items programmatically. If you use [Lombiq Base Theme for Orchard Core](https://github.com/Lombiq/Orchard-Base-Theme), it has [`MainMenuNavigationProvider`](https://github.com/Lombiq/Orchard-Base-Theme/blob/dev/Lombiq.BaseTheme.Core/Services/MainMenuNavigationProvider.cs). This automatically translates an existing content item with a `MenuItemsListPart` and the `main-menu` alias into compatible navigation, so you can edit menu items from the content editor.
