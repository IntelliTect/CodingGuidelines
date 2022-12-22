; Shipped analyzer releases
; https://github.com/dotnet/roslyn-analyzers/blob/main/src/Microsoft.CodeAnalysis.Analyzers/ReleaseTrackingAnalyzers.Help.md

## Release 1.0

### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|--------------------
INTL0001 | Naming | Warning | NamingFieldPascalUnderscore
INTL0002 | Naming | Warning | NamingPropertyPascal
INTL0003 | Naming | Warning | NamingMethodPascal
INTL0101 | Formatting | Warning | AttributesOnSeparateLines
INTL0201 | Reliability | Warning | AsyncVoid
INTL0202 | Reliability | Warning | BanImplicitDateTimeToDateTimeOffsetConversion
INTL0301 | Performance | Info | FavorDirectoryEnumerationCalls
INTL0302 | Performance | Info | FavorDirectoryEnumerationCalls
INTL0303 | Flow | Info | UnusedLocalVariable, [Documentation](https://github.com/IntelliTect/CodingGuidelines)