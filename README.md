[![NuGet Status](http://img.shields.io/nuget/v/IntelliTect.Analyzers.svg?style=flat&label=IntelliTect.Analyzers)](https://www.nuget.org/packages/IntelliTect.Analyzers/)

[![Build Status](https://intellitect.visualstudio.com/CodingStandards/_apis/build/status/IntelliTect.CodingGuidelines?branchName=master)](https://intellitect.visualstudio.com/CodingStandards/_build/latest?definitionId=95&branchName=master)

[Coding Snippets Repository](https://github.com/IntelliTect/IntelliTect.Snippets)

[Coding Snippets Extension](https://marketplace.visualstudio.com/items?itemName=IntelliTect.intellitectsnippets)

# Coding Guidelines / Design Guidelines
A repository to contain IntelliTect's tools for coding conventions. [https://intellitect.github.io/CodingGuidelines/](https://intellitect.github.io/CodingGuidelines/)

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


#### INTL0003 - Methods PascalCase

Methods, including local functions, should be PascalCase

**Allowed**
```c#
class SomeClass
{
    public string GetEmpty() {

        var output = LocalFunction();

        string LocalFunction() {
            return string.Empty();
        }

        return output;
    }
}
```

**Disallowed**
```c#
class SomeClass
{
    public string getEmpty() {

        var output = localFunction();

        string localFunction() {
            return string.Empty();
        }

        return output;
    }
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


## 03XX block - Performance
#### INTL0301 - Favor using the method `EnumerateFiles` over the `GetFiles` method.

When using the `System.IO.Directory` class, it is suggested to use the `EnumerateFiles` static method
instead of the `GetFiles` method.  In the remarks section of the [documentation](https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.enumeratefiles), it is stated that using the `EnumerateFiles` method is more efficient because you can start enumerating the collection before all the results are returned:

> The EnumerateFiles and GetFiles methods differ as follows: When you use EnumerateFiles, you can start enumerating the 
> collection of names before the whole collection is returned; when you use GetFiles, you must wait for the whole array of 
> names to be returned before you can access the array. Therefore, when you are working with many files and directories, 
> EnumerateFiles can be more efficient.
> The returned collection is not cached; each call to the GetEnumerator on the collection will start a new enumeration.

#### INTL0302 - Favor using the method `EnumerateDirectories` over the `GetDirectories` method.

When using the `System.IO.Directory` class, it is suggested to use the `EnumerateDirectories` static method
instead of the `GetDirectories` method.  In the remarks section of the [documentation](https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.enumeratedirectories), it is stated that using the `EnumerateDirectories ` method is more efficient because you can start enumerating the collection before all the results are returned:

> The EnumerateDirectories and GetDirectories methods differ as follows: When you use EnumerateDirectories, you can start 
> enumerating the collection of names before the whole collection is returned; when you use GetDirectories, you must wait 
> for the whole array of names to be returned before you can access the array. Therefore, when you are working with many 
> files and directories, EnumerateDirectories can be more efficient.
> The returned collection is not cached; each call to the GetEnumerator on the collection will start a new enumeration.
