# Liquid

Adds various Liquid tags and filters. For more information about Liquid in Orchard Core, see [here](https://docs.orchardcore.net/en/main/reference/modules/Liquid/).

## Tags

- `{% ifnotempty condition %)`: Evaluates `condition` and if it's not a falsey value, converts the result to string. The statements inside this block are evaluated if the aforementioned string result is not `null`, empty or whitespace.  

## Filters

- `isnotempty`: Returns `true` if the input is not null and not empty.
