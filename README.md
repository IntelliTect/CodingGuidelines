[![NuGet Status](http://img.shields.io/nuget/v/IntelliTect.Analyzers.svg?style=flat&label=IntelliTect.Analyzers)](https://www.nuget.org/packages/IntelliTect.Analyzers/)

[![CodingGuidelines Build](https://github.com/IntelliTect/CodingGuidelines/actions/workflows/dotnetBuild.yml/badge.svg)](https://github.com/IntelliTect/CodingGuidelines/actions/workflows/dotnetBuild.yml)

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
> 

#### INTL0303 - Do not implicitly convert between `DateTime` and `DateTimeOffset`.

Code relying on certain behaviors may function correctly when run in tests locally because all code will be running in 
the same timezone. This same code will fail or have different behavior when some of it is running in a hosted environment 
like Azure where the time zone is often set to UTC.

See the feature proposal to remove this from the [dotnet corelib](https://github.com/dotnet/runtime/issues/32954).

> Misleading usecase is that TimeZoneInfo.ConvertTime has 2 overloads for DateTime and DateTimeOffset. When result of the 
> first one is assigned to DateTimeOffset typed variable, DateTimeOffset record with local time zone offset is created. 
> This is unclear for common code reader there's something unintended behaviour may take a place ((hey, I've supplied date, 
> time and timezone to this function, and expect it to return date&time object for this timezone)), because types of either 
> DateTime or DateTimeOffset that comes to ConvertTime argument may be masked by complex computational expression.

#### INTL0303 - Do not implicitly convert between `DateTime` and `DateTimeOffset`.

Code relying on certain behaviors may function correctly when run in tests locally because all code will be running in 
the same timezone. This same code will fail or have different behavior when some of it is running in a hosted environment 
like Azure where the time zone is often set to UTC.

See the feature proposal to remove this from the [dotnet corelib](https://github.com/dotnet/runtime/issues/32954).

> Misleading usecase is that TimeZoneInfo.ConvertTime has 2 overloads for DateTime and DateTimeOffset. When result of the 
> first one is assigned to DateTimeOffset typed variable, DateTimeOffset record with local time zone offset is created. 
> This is unclear for common code reader there's something unintended behaviour may take a place ((hey, I've supplied date, 
> time and timezone to this function, and expect it to return date&time object for this timezone)), because types of either 
> DateTime or DateTimeOffset that comes to ConvertTime argument may be masked by complex computational expression.

##### Guidelines Site Maintenance
 There are two github actions that are used to update the CodingGuidelinesSite. One action ( *Update csharp Markdown* ) will run automatically when the XML file in the master branch is updated via a commit. The CodingGuidelines github page will then reflect the changes. After reviewing the "dev" site, there is another action ( *Update Docs Folder on CodingGuidelinesSite* ) that will move the new markdown file to production site [CodingGuidelinesSite]( https://intellitect.github.io/CodingGuidelinesSite/). 
There is also another action to manually run a xml to md conversion on any branch.
