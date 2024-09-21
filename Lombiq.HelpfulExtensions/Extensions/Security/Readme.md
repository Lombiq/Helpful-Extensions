# Security Extensions

## Strict Security

When applied to a content type definition, `StrictSecuritySetting` requires the user to have the exact Securable permission for that content type. For example if you apply it to Page, then just having the common ViewContent permission won't be enough and you must explicitly have the View_Page permission too. Don't worry, the normal implications such as ViewOwn being fulfilled by View still apply within the content type, they just no longer imply their common counterparts.

Make content type use strict security in migration:

```csharp
_contentDefinitionManager.AlterTypeDefinition("Page", type => type
    .Securable()
    .WithSettings(new StrictSecuritySettings { Enabled = true }));
```

You can also enable it by going to the content type editor on the admin side and checking the _Strict Securable_ checkbox.