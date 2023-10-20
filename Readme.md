# Lombiq Helpful Extensions for Orchard Core

[![Lombiq.HelpfulExtensions NuGet](https://img.shields.io/nuget/v/Lombiq.HelpfulExtensions?label=Lombiq.HelpfulExtensions)](https://www.nuget.org/packages/Lombiq.HelpfulExtensions/) [![Lombiq.HelpfulExtensions UI Test Extensions NuGet](https://img.shields.io/nuget/v/Lombiq.HelpfulExtensions.Tests.UI?label=Lombiq.HelpfulExtensions.Tests.UI)](https://www.nuget.org/packages/Lombiq.HelpfulExtensions.Tests.UI/)

## About

Orchard Core module containing some handy extensions (e.g. useful content types and widgets).

We at [Lombiq](https://lombiq.com/) also used this module for the following projects:

- The new [City of Santa Monica website](https://santamonica.gov/) when migrating it from Orchard 1 to Orchard Core ([see case study](https://lombiq.com/blog/helping-the-city-of-santa-monica-with-orchard-core-consulting)).
- The new [Smithsonian Folkways Recordings website](https://folkways.si.edu/) when migrating it from Orchard 1 to Orchard Core ([see case study](https://lombiq.com/blog/smithsonian-folkways-recordings-now-upgraded-to-orchard-core)).
- The new [Xero4PowerBI website](https://xero4powerbi.com/) ([see case study](https://dotnest.com/blog/xero4powerbi-website-case-study-migration-to-orchard-core)).<!-- #spell-check-ignore-line -->
- The new [Lombiq website](https://lombiq.com/) when migrating it from Orchard 1 to Orchard Core ([see case study](https://lombiq.com/blog/how-we-renewed-and-migrated-lombiq-com-from-orchard-1-to-orchard-core)).
- The new [Ik wil een taart website](https://ikwileentaart.nl/) ([see case study](https://dotnest.com/blog/revamping-ik-wil-een-taart-migrating-an-old-version-of-orchard-core-website-with-custom-theme-and-commerce-logic-to-dotnest)).<!-- #spell-check-ignore-line -->
- The new client portal for [WTW](https://www.wtwco.com/) ([see case study](https://lombiq.com/blog/lombiq-s-journey-with-wtw-s-client-portal)).
- The new [Show Orchard website](https://showorchard.com/) when migrating it from Orchard 1 DotNest to DotNest Core ([see case study](https://dotnest.com/blog/show-orchard-case-study-migrating-an-orchard-1-dotnest-site-to-orchard-core)).<!-- #spell-check-ignore-line -->

This module is also available on all sites of [DotNest, the Orchard SaaS](https://dotnest.com/).

Do you want to quickly try out this project and see it in action? Check it out in our [Open-Source Orchard Core Extensions](https://github.com/Lombiq/Open-Source-Orchard-Core-Extensions) full Orchard Core solution and also see our other useful Orchard Core-related open-source projects!

Note that this module has an Orchard 1 version in the [dev-orchard-1 branch](https://github.com/Lombiq/Helpful-Extensions/tree/dev-orchard-1).

## Extensions

The module consists of the following independent extensions (all in their own features):

### Code Generation Helpful Extensions

#### Content definition code generation

Generates migration code from content definitions. You can use this to create (or edit) a content type on the admin and then move its creation to a migration class. Generated migration code is displayed under the content types' editors, just enable the feature. Check out [this demo video](https://www.youtube.com/watch?v=KOlsLaIzgm8) to see this in action.

![Content definition code generation textbox on the admin, showing generated migration code for the Page content type.](Docs/Attachments/ContentTypeCodeGeneration.png)

### Orchard 1 Recipe Migration

Contains extendable services which convert an _export.xml_ file from Orchard 1 into an Orchard Core recipe JSON file with a `content` step. Content Type exports are not supported, because the service relies on the existence of the target content types to initialize the content items that go into the recipe.

The converter can be accessed from the Admin dashboard in the **Configuration > Import/Export > Orchard 1 Recipe Migration** menu item.

To extend the built-in functionality implement these services:

- `IOrchardContentConverter`: Used to set up a single new `ContentItem` using the data in the matching `<Content>` entry.
- `IOrchardExportConverter`: Used to update or filter the final list of content items. It has access to the entire export XML file.
- `IOrchardUserConverter`: Used to create a new `User` using the data in the matching `<Content>` entry.

The built-in converters handle the following O1 content parts:

- `CommonPart`
- `AutoroutePart`
- `BodyPart`
- `TitlePart`
- `IdentityPart`
- `ListPart`
- `GraphMetadata` (added by <https://github.com/Lombiq/Associativy-Core>)
- `UserPart`

Additionally, if a custom converter fills in the `OrchardIds` content part's `Parent` property on the generated content item, then it also adds it to the parent content item's `ListPart`.

#### User Migration

Migrating users from Orchard 1 is also possible with this feature: Import the same way as other Orchard 1 contents, and users will generate automatically, meaning you don't have to do anything else.

Each generated user will have the corresponding email, user-name and roles, and a new random password. **Keep in mind that in Orchard 1, user-names could contain any characters, but in Orchard Core, it is limited by default.** [Check out the default configuration](https://github.com/OrchardCMS/OrchardCore/blob/main/src/OrchardCore.Modules/OrchardCore.Setup/Startup.cs#L44) and adjust it in your application if needed.

If your use-case would be different than what's done in the default user converter, implement the `IOrchardUserConverter` service, and the default one won't be executed.

### Content Sets

Adds an attachable (named) content part that ties together a content item and a set of variants for it. This is similar to Content Localization part, but generic and not tied to the culture options. The `IContentSetEventHandler.GetSupportedOptionsAsync()` extension point is used to generate the valid options for a content item with an attached `ContentSetPart`.

The content items are indexed into the `ContentSetIndex`. The `IContentSetManager` has methods to retrieve the existing content items (or just the index rows) for a specific content set.

When the content part is attached, a new dropdown is added to the content item in the admin dashboard's content items list. This is similar in design to the dropdown added by the Content Localization part. The label is the named part's display text and you can use the dropdown (or the listing in the editor view) to select an option. When an option is selected the content item is cloned and that option's key assigned to it.

#### Generating content set options

You can generate your custom content set options two ways:

- Create a service which implements the `IContentSetEventHandler` interface.
- Create a workflow with the _Creating Content Set_ startup event. The workflow should return an output `MemberLinks` which should contain an array of `{ "Key": string, "DisplayText": string }` objects. Further details can be seen on the event's editor screen.

The latter can be used even if you don't have access to the code, e.g. on DotNest. With either approach you only have to provide the `Key` and `DisplayText` properties, anything else is automatically filled in by the module. In both cases you have access to the context such as the current content item's key, the related part's part definition, etc. You can use this information to only create options selectively.

#### Content Set Content Picker Field

You can add this content field to any content item that also has a Content Set part. The field's technical name should be the same as the attached part's technical name. Besides that, no further configuration is needed. If there are available variants for a content item with this field, it will display a comma separated list of links where the option names are the link text.

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

When applied to a content type definition, `StrictSecuritySetting` requires the user to have the exact Securable permission for that content type. For example if you apply it to Page, then just having the common ViewContent permission won't be enough and you must explicitly have the View_Page permission too. Don't worry, the normal implications such as ViewOwn being fulfilled by View still apply within the content type, they just no longer imply their common counterparts.

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

### Target blank

Gives all external links the `target="_blank"` attribute.

### Reset Password activity

Adds a workflow activity that generates a reset password token for the specified user. You can define the source of the User object using a JavaScript expression. It will set the token and the URL to the workflow `LastResult` property and optionally it can set them to the `Properties` dictionary to a key that you define as an activity parameter.

### Trumbowyg code-snippet

Adds prettified code-snippet inserting functionality to Trumbowyg editor by using a slightly modified version of [Trumbowyg highlight plugin](https://alex-d.github.io/Trumbowyg/documentation/plugins/#plugin-highlight). You need to add the highlight button to your Trumbowyg editor options to enable it.

```text
{
    btns: [
        ['highlight']
    ],
}
```

[Prism](https://prismjs.com/) is used to prettify the code. Currently the following formats are supported:

- clike
- cpp
- cs
- csharp
- css
- dotnet
- graphql
- html
- js
- json
- markup-templating
- mathml
- md
- plsql
- powershell
- scss
- sql
- ssml
- svg
- ts
- xml
- yaml
- yml

Then you need to link the Trumbowyg and Prism styles and scripts where you want it to be used. E.g. if you want to add it to BlogPost content type you can do it with the help of [Lombiq.HelpfulLibraries.OrchardCore](https://github.com/Lombiq/Helpful-Libraries/blob/dev/Lombiq.HelpfulLibraries.OrchardCore/Readme.md) in a IResourceFilterProvider:

```csharp
builder.WhenContentType("BlogPost").RegisterStylesheet(Lombiq.HelpfulExtensions.Constants.ResourceNames.Prism);
builder.WhenContentType("BlogPost").RegisterFootScript(Lombiq.HelpfulExtensions.Constants.ResourceNames.Prism);

builder.WhenContentTypeEditor("BlogPost").RegisterFootScript(Lombiq.HelpfulExtensions.Constants.ResourceNames.TrumbowygHighlight);
builder.WhenContentTypeEditor("BlogPost").RegisterStylesheet(Lombiq.HelpfulExtensions.Constants.ResourceNames.TrumbowygHighlight);
```

## Contributing and support

Bug reports, feature requests, comments, questions, code contributions and love letters are warmly welcome. You can send them to us via GitHub issues and pull requests. Please adhere to our [open-source guidelines](https://lombiq.com/open-source-guidelines) while doing so.

This project is developed by [Lombiq Technologies](https://lombiq.com/). Commercial-grade support is available through Lombiq.
