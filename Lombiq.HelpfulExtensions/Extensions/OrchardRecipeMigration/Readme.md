# Orchard 1 Recipe Migration

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

## User Migration

Migrating users from Orchard 1 is also possible with this feature: Import the same way as other Orchard 1 contents, and users will be generated automatically, meaning you don't have to do anything else.

Each generated user will have the corresponding email, user-name, roles, and a new random password. **Keep in mind that in Orchard 1, user-names could contain any characters, but in Orchard Core, it is limited by default.** [Check out the default configuration](https://github.com/OrchardCMS/OrchardCore/blob/main/src/OrchardCore.Modules/OrchardCore.Setup/Startup.cs#L44) and adjust it in your application if needed.

If your use-case would be different than what's done in the default user converter, implement the `IOrchardUserConverter` service, and the default one won't be executed.
