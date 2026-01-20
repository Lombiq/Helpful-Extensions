# Trumbowyg code snippet

Adds prettified code snippet inserting functionality to Trumbowyg editor by using a slightly modified version of [Trumbowyg highlight plugin](https://alex-d.github.io/Trumbowyg/documentation/plugins/#plugin-highlight). You need to add the highlight button to your Trumbowyg editor options to enable it.

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
using Lombiq.HelpfulLibraries.OrchardCore.ResourceManagement;
using static Lombiq.HelpfulExtensions.Extensions.Trumbowyg.Constants.PrismLanguageNames;
using static Lombiq.HelpfulExtensions.Extensions.Trumbowyg.Constants.ResourceNames;

public class ResourceFilters : IResourceFilterProvider
{
    public void AddResourceFilter(ResourceFilterBuilder builder)
    {
        const string MyContentType = "MyContentType";

        var resourceFilters = new[]
            {
                builder.WhenContentType(MyContentType),
                builder.WhenContentTypeEditor(MyContentType),
                builder.WhenContentTypeCreate(MyContentType),
                builder.WhenContentTypePreview(MyContentType),
            };

        foreach (var resourceFilter in resourceFilters)
        {
            resourceFilter.RegisterStylesheet(Prism, PrismLineHighlight, TrumbowygHighlight);
            resourceFilter.RegisterFootScript(Prism, PrismLineHighlight, TrumbowygHighlight, TrumbowygHighlightExtension);
            foreach (var language in AllLanguage)
            {
                resourceFilter.RegisterFootScript(language);
            }
        }
    }
}
```

This will add the required styles and scripts to the editor and display views of the specified content type. Make sure to replace `MyContentType` with the actual content type name where you want to use this feature. The following languages will be included beyond the base Prism languages:
- C#
- Docker
- Json
- Markdown
- Sql
- Liquid
- Regex
- Powershell

You can use the Coy Prism theme by registering the `Lombiq.HelpfulExtensions.Extensions.Trumbowyg.Constants.ResourceNames.PrismCoyTheme` as a stylesheet.

<!-- textlint-disable doubled-spaces -->
> [!TIP]
> If you want to add this to the `BlogPost` content type, then just enable the ["Lombiq Helpful Extensions - Trumbowyg code snippet - Blog Posts" feature](../TrumbowygBlogPosts/Readme.md), since it configures all of the above. Note though, that the `BlogPost` content type provided by Orchard Core's Blog recipe doesn't have a part or field that uses Trumbowyg, so you'll need to add one (e.g. an Html Field, or Html Body Part) first.
<!-- textlint-enable doubled-spaces -->
