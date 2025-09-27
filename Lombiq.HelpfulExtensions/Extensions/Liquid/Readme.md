# Liquid

Adds various Liquid tags and filters. For more information about Liquid in Orchard Core, see [here](https://docs.orchardcore.net/en/main/reference/modules/Liquid/).

## Tags

- `{% ifnotempty condition %)`: Evaluates `condition` and if it's not a falsey value, converts the result to string. The statements inside this block are evaluated if the aforementioned string result is not `null`, empty or whitespace.
- `{% assignarray 'name', 'value1', 'value2', 'etc' %}`: Assigns a new array type variable with the provided name and arbitrary initial values. You can also type `{% assignarray 'name' %}` to create an empty array that you can fill up using a loop.

## Filters

- `isnotempty`: Returns `true` if the input is not null and not empty.
