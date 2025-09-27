# Liquid

Adds various Liquid tags and filters. For more information about Liquid in Orchard Core, see [here](https://docs.orchardcore.net/en/main/reference/modules/Liquid/).

## Tags

- `{% ifnotempty condition %)...{% endifnotempty %}`: Evaluates `condition` and if it's not a falsey value, converts the result to string. The statements inside this block are evaluated if the aforementioned string result is not `null`, empty or whitespace.
- `{% assign_array 'name', 'value1', 'value2', 'etc' %}`: Assigns a new array type variable with the provided name and arbitrary initial values. You can also type `{% assign_array 'name' %}` to create an empty array that you can fill up using a loop.

## Filters

- `is_not_empty`: Returns `true` if the input is not null and not empty.
- `shapes_build_display: 'Summary'`: Behaves the same way as the built-in `shape_build_display` filter, except both the input and output are arrays for bulk operation (e.g. from queries).
- `shapes_render`: Behaves the same way as the built-in `shape_render` filter, except both the input and output are arrays for bulk operation (e.g. from queries).

### Array filter example

You can use `shapes_build_display` and `shapes_render` to easily display items from a query. For example, using the query in the "blog" recipe:

```liquid
{{ Queries['RecentBlogPosts'] | query | shapes_build_display: 'Summary' | shapes_render }}
```
