# Helpful Widgets

Adds multiple helpful widget content types. These are basic widgets that are added by built-in Orchard Core recipes though in case of using a custom setup recipe these can be added by this feature too.

## ContainerWidget

Works as a container for further widgets. It has a FlowPart attached to it so it can contain additional widgets as well.

## HtmlWidget

Adds HTML editing and displaying capabilities using a WYSIWYG editor.

# LiquidWidget

Adds Liquid code editing and rendering capabilities.

# MenuWidget ("Menu Navigation Provider Widget")

Renders a Bootstrap navigation menu as a widget using the provided `MenuItem`s.

> ℹ️ Normally the menu is populated from `INavigationProvider` implementations that create the menu items programmatically. If you use [Lombiq Base Theme for Orchard Core](https://github.com/Lombiq/Orchard-Base-Theme), it has [`MainMenuNavigationProvider`](https://github.com/Lombiq/Orchard-Base-Theme/blob/dev/Lombiq.BaseTheme/Services/MainMenuNavigationProvider.cs). This automatically translates an existing content item with a `MenuItemsListPart` and the `main-menu` alias into compatible navigation, so you can edit menu items from the content editor.
