# Content Sets

Adds an attachable (named) content part that ties together a content item and a set of variants for it. This is similar to Content Localization part, but generic and not tied to the culture options. The `IContentSetEventHandler.GetSupportedOptionsAsync()` extension point is used to generate the valid options for a content item with an attached `ContentSetPart`.

The content items are indexed into the `ContentSetIndex`. The `IContentSetManager` has methods to retrieve the existing content items (or just the index rows) for a specific content set.

When the content part is attached, a new dropdown is added to the content item in the admin dashboard's content items list. This is similar in design to the dropdown added by the Content Localization part. The label is the named part's display text and you can use the dropdown (or the listing in the editor view) to select an option. When an option is selected the content item is cloned and that option's key assigned to it.

## Generating content set options

You can generate your custom content set options two ways:

- Create a service which implements the `IContentSetEventHandler` interface.
- Create a workflow with the _Creating Content Set_ startup event. The workflow should return an output `MemberLinks` which should contain an array of `{ "Key": string, "DisplayText": string }` objects. Further details can be seen on the event's editor screen.

The latter can be used even if you don't have access to the code, e.g. on DotNest. With either approach you only have to provide the `Key` and `DisplayText` properties, anything else is automatically filled in by the module. In both cases you have access to the context such as the current content item's key, the related part's part definition, etc. You can use this information to only create options selectively.

## Content Set Content Picker Field

You can add this content field to any content item that also has a Content Set part. The field's technical name should be the same as the attached part's technical name. Besides that, no further configuration is needed. If there are available variants for a content item with this field, it will display a comma separated list of links where the option names are the link text.