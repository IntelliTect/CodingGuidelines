[![NuGet Status](http://img.shields.io/nuget/v/IntelliTect.Analyzers.svg?style=flat&label=IntelliTect.Analyzers)](https://www.nuget.org/packages/IntelliTect.Analyzers/)

[![Build Status](https://intellitect.visualstudio.com/CodingStandards/_apis/build/status/IntelliTect.CodingStandards?branchName=master)](https://intellitect.visualstudio.com/CodingStandards/_build/latest?definitionId=76&branchName=master)

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

## 01XX block - Formatting
#### INTL0101 - Attributes on separate lines

All attributes should be on separate lines and be wrapped in their own braces.

**Allowed**
```c#
[FooClass]
[BarClass]
class SomeClass
{
    [Foo]
    [Bar]
    public string MyProperty { get; set; }
}
```

**Disallowed**
```c#
[FooClass, BarClass]
class SomeClass
{
    [Foo][Bar]
    public string MyProperty { get; set; }
}
```
