# Simple Icons

Adds [Simple Icons](https://simpleicons.org/) icon library integration. All icons are directly available in the _~/Lombiq.HelpfulExtensions/vendors/simple-icons/icons/*.svg_ location.

## Shape and Tag Helper

You can display the icon in a more structured manner using the `SimpleIcon` shape or the `<simple-icon>` Razor tag helper. Use the tag helper from .cshtml files and the shape from Liquid.

```liquid
{% shape "SimpleIcon", Source: 'youtube', IconClasses: 'h-5 w-5 shrink-0', label-classes: 'font-semibold tracking-wide', Size: 24, Title: 'YouTube', ShowLabel: true %}
```

```cshtml
<simple-icon source="youtube"
             icon-classes="h-5 w-5 shrink-0"
             label-classes="font-semibold tracking-wide"
             title="YouTube"
             show-label="true"
             size="24"></simple-icon>
```
