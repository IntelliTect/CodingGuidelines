# Python Guidelines
...

## Sections

- [Python Guidelines](#python-guidelines)
  - [Sections](#sections)
- [Guidelines](#guidelines)
  - [Pythonic](#pythonic)
    - [Comprehensions](#comprehensions)

# Guidelines

## Pythonic

...

### Comprehensions
Favor list comprehensions over `.map`
```python
[
    index
    for index
    in range(5)
]
```

## Importing Modules
Favor using using module notation (?) over import functions directly. Importing functions directly generally reduces readability, as it is not immediately apparent that the function is being imported.

Avoid
```python
from top.utils import do_thing

do_thing()
```

Favor
```python
from top import utils

utils.do_thing()
```