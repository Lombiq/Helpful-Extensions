# Liquid

Adds various Liquid tags and filters. For more information about Liquid in Orchard Core, see [here](https://docs.orchardcore.net/en/main/reference/modules/Liquid/).

## Tags

- `{% ifnotempty condition %)...{% endifnotempty %}`: Evaluates `condition` and if it's not a falsey value, converts the result to string. The statements inside this block are evaluated if the aforementioned string result is not `null`, empty or whitespace.
- `{% assign_array 'name', 'value1', 'value2', 'etc' %}`: Assigns a new array type variable with the provided name and arbitrary initial values. You can also type `{% assign_array 'name' %}` to create an empty array that you can fill up using a loop.
- `{% ifauthorized permission: 'View', contentItem: 'content item ID' %)...{% endifauthorized %}`: Uses `IAuthorizationService` to perform authorization on the current user. It has the arguments listed below.
  - permission: The required permission's technical name. Case-insensitive.
  - contentItem: If specified, the permission is checked for the content item with the provided ID.
  - user: If specified, the user with this name is looked up instead of the current user.
  - email: If specified, the user with this e-mail is looked up instead of the current user.
  - invert: If the `true` value is specified, then the contents of the tag are evaluated only if the authorization fails.

## Filters

- `is_not_empty`: Returns `true` if the input is not null and not empty.
- `shapes_build_display: 'Summary'`: Behaves the same way as the built-in `shape_build_display` filter, except both the input and output are arrays for bulk operation (e.g. from queries).
- `shapes_render`: Behaves the same way as the built-in `shape_render` filter, except both the input and output are arrays for bulk operation (e.g. from queries).
- `shuffle`: If the input is an array, it returns a new array where the input's items are sorted in a random order.

### Array filter example

You can use `shapes_build_display` and `shapes_render` to easily display items from a query. For example, using the query in the "blog" recipe:

```liquid
{{ Queries['RecentBlogPosts'] | query | shapes_build_display: 'Summary' | shapes_render }}
```
