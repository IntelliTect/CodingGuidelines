# CodingStandards
A repository to contain IntelliTect's tools for coding conventions.
IntelliTect conventions can be found [here](https://docs.google.com/document/d/1_LEucqeAg7wtKvuI4dWS79ntEgJ2GKb-amr0k6xLS3Q/edit#heading=h.lpr8ztld62uc).

## 00XX block - Naming
#### INTL0001 - Fields _PascalCase

Fields should be specified in _PascalCase. *Always* with a leading underscore, regardless
of visibility modifier.

**Allowed**
```c#
class SomeClass
{
    public string _MyField;
}
```
**Disallowed**
```c#
class SomeClass
{
    public string _myField;
    public string myField;
}
```

#### INTL0002 - Properties PascalCase

Fields should be PascalCase

**Allowed**
```c#
class SomeClass
{
    public string MyProperty { get; set; }
}
```

**Disallowed**
```c#
class SomeClass
{
    public string myProperty { get; set; }
    public string _MyProperty { get; set; }
}
```