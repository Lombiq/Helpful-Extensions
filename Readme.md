# Helpful Extensions Orchard module Readme



## Project Description

Orchard module containing some handy extensions (e.g. filters for Projector). Note that this module has an Orchard Core version in the [dev branch](https://github.com/Lombiq/Helpful-Extensions/tree/dev).

## Extensions

The module consists of the following independent extensions (all in their own features):

### Code Generation Extensions

#### Content type code generation

Generates migration code from content definitions. You can use this to create (or edit) a content type on the admin and then move its creation to a migration class. Generated migration code is displayed under the content types' editors.

### Commands Extensions

#### Permission commands

There are two commands:

- lock frontend: locks access to the frontend of the site, i.e. only authenticated users will be able to access it.
- unlock frontend: anonymous users will be able to access the site's frontend too.

### Contents Extensions

#### Hint Field

The Hint Field is a content field. It displays a text in the editor of the content items that can be configured in the content type's editor. This way you can add hint texts to your content items' editors.

### Projector Extensions

#### ContainedByFilter Projector filter

Use this Projector filter to filter for content items that are contained by a specific content item. E.g. if there is a top page with ContainerPart that contains various other pages, then you can use this filter to fetch items that are contained by the top page.
On the UI you only have to specify the id of the container.

#### IdsInFilter Projector filter

You can give this filter a comma-separated list of content item ids. The filter will then fetch the contents with those ids. This is useful if you want to handle a specific set of content items together in a query, probably with other, differently filtered content items.

#### EnumFilterEditor Projector filter editor

When you have a Projector binding set up for an enum property on a record this will provide an editor for it when you edit the query. You are able to use this to add an equals or not equals filter on an enum property.

#### ForeignKeyFilterEditor Projector filter editor

When you have a Projector binding set up for a property on a record that represents a foreign key you can use this to filter on its value.

### Projectior Extensions - Search

#### SearchFilter Projector filter

This filter lets you specify a search query (can come from tokens) that will be used to match items against and then filter items with.

### Tokens Extensions

#### Current content token

Adds the Content.Current token that will return the content item for the given page (e.g. the currently opened blog post). You can chain Content tokens to it.

#### WrapNotEmpty token

Wraps the text if it's not empty from left and optionally from right with the specified texts.

You can use these extensions as described on the above pages.

The module is also available for [DotNest](http://dotnest.com/) sites.

The module's source is available in two public source repositories, automatically mirrored in both directions with [Git-hg Mirror](https://githgmirror.com):

- [https://bitbucket.org/Lombiq/helpful-extensions](https://bitbucket.org/Lombiq/helpful-extensions) (Mercurial repository)
- [https://github.com/Lombiq/Helpful-Extensions](https://github.com/Lombiq/Helpful-Extensions) (Git repository)

Bug reports, feature requests and comments are warmly welcome, **please do so via GitHub**.
Feel free to send pull requests too, no matter which source repository you choose for this purpose.

This project is developed by [Lombiq Technologies Ltd](http://lombiq.com/). Commercial-grade support is available through Lombiq.