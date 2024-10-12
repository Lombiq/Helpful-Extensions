# Trumbowyg code-snippet

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
builder.WhenContentTypeCreate("BlogPost").RegisterFootScript(Lombiq.HelpfulExtensions.Constants.ResourceNames.TrumbowygHighlight);
builder.WhenContentTypeCreate("BlogPost").RegisterStylesheet(Lombiq.HelpfulExtensions.Constants.ResourceNames.TrumbowygHighlight);
```
