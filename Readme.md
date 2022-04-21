# Lombiq Helpful Extensions for Orchard Core



[![Lombiq.HelpfulExtensions NuGet](https://img.shields.io/nuget/v/Lombiq.HelpfulExtensions?label=Lombiq.HelpfulExtensions)](https://www.nuget.org/packages/Lombiq.HelpfulExtensions/)


## About

Orchard Core module containing some handy extensions (e.g. useful content types and widgets). It's also available on all sites of [DotNest, the Orchard SaaS](https://dotnest.com/).

Do you want to quickly try out this project and see it in action? Check it out in our [Open-Source Orchard Core Extensions](https://github.com/Lombiq/Open-Source-Orchard-Core-Extensions) full Orchard Core solution and also see our other useful Orchard Core-related open-source projects!

Note that this module has an Orchard 1 version in the [dev-orchard-1 branch](https://github.com/Lombiq/Helpful-Extensions/tree/dev-orchard-1).


## Extensions

The module consists of the following independent extensions (all in their own features):

### Code Generation Helpful Extensions

#### Content definition code generation
Generates migration code from content definitions. You can use this to create (or edit) a content type on the admin and then move its creation to a migration class. Generated migration code is displayed under the content types' editors, just enable the feature. Check out [this demo video](https://www.youtube.com/watch?v=KOlsLaIzgm8) to see this in action.

![Content definition code generation textbox on the admin, showing generated migration code for the Page content type.](Docs/Attachments/ContentTypeCodeGeneration.png)

### Flows Helpful Extensions

Adds additional styling capabilities to the OrchardCore.Flows feature by making it possible to add classes to widgets in the Flow Part editor. Just add `AdditionalStylingPart` to the content type using `FlowPart`.

![Custom classes editor on a widget contained in Flow Part.](Docs/Attachments/FlowPartCustomClasses.png)

### Helpful Widgets

Adds multiple helpful widget content types. These are basic widgets that are added by built-in Orchard Core recipes though in case of using a custom setup recipe these can be added by this feature too.

Includes:

- ContainerWidget: Works as a container for further widgets. It has a FlowPart attached to it so it can contain additional widgets as well.
- HtmlWidget: Adds HTML editing and displaying capabilities using a WYSIWYG editor.
- LiquidWidget: Adds Liquid code editing and rendering capabilities.
- MenuWidget: Renders a Bootstrap navigation menu as a widget using the provided `MenuItem`s. 

### Helpful Content Types

Includes basic content types that are added by built-in Orchard Core recipes though in case of using a custom setup recipe these can be added by this feature too.

Includes:

- Page: Highly customizable page content type with FlowPart and AutoroutePart.

### Shape Tracing Helpful Extensions

Adds a dump of metadata to the output about every shape. This will help you understand how a shape is displayed and how you can override it. Just check out the HTML output. You can see a video demo of this feature in action [on YouTube](https://www.youtube.com/watch?v=WI4TEKVc9SA).

### Security Extensions


#### Strict Security

When applied to a content type definition, `StrictSecuritySetting` requires the user to have the exact Securable permission for that content type. For example if you apply it to Page, then just having the common ViewContent permission won't be enough and you must explicitly have the View_Page permission too. Don't worry, the normal implications such as ViewOwn beig fulfilled by View still apply within the content type, they just no longer imply their common counterparts.

Make content type use strict security in migration:
```csharp
_contentDefinitionManager.AlterTypeDefinition("Page", type => type
    .Securable()
    .WithSettings(new StrictSecuritySettings { Enabled = true }));
```

You can also enable it by going to the content type editor on the admin side and checking the _Strict Securable_ checkbox.


### Emails and Email Templates

#### Email Templates

Provides a shape-based email template rendering service. The email templates are represented by email template IDs that are also used to identify the corresponding shape using the following pattern: `EmailTemplate__{EmailTemplateID}`. E.g., for the `ContactUs` email template you need to create a shape with the `EmailTemplate__ContactUs` shape type.

In the email template shapes use the `Layout__EmailTemplate` as the `ViewLayout` to wrap it with a simple HTML layout.

To extend the layout you can override the `EmailTemplate_LayoutInjections` shape and inject content to the specific zones provided by the layout to activate it in every email template. E.g.,

```html
<zone name="Footer">
    Best,<br>
    My Awesome Team
</zone>
```
To add inline styles include:
```html
<zone name="Head">
    <style>
        /* CSS code... */
    </style>
</zone>
```

#### Deferred email sending

Use the `ShellScope.Current.SendEmailDeferred()` for sending emails. It'll send emails after the shell scope has ended without blocking the request.



## Contributing and support

Bug reports, feature requests, comments, questions, code contributions, and love letters are warmly welcome, please do so via GitHub issues and pull requests. Please adhere to our [open-source guidelines](https://lombiq.com/open-source-guidelines) while doing so.

This project is developed by [Lombiq Technologies](https://lombiq.com/). Commercial-grade support is available through Lombiq.
