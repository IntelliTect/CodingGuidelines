[![NuGet Status](http://img.shields.io/nuget/v/IntelliTect.Analyzers.svg?style=flat&label=IntelliTect.Analyzers)](https://www.nuget.org/packages/IntelliTect.Analyzers/)

[![CodingGuidelines Build](https://github.com/IntelliTect/CodingGuidelines/actions/workflows/dotnetBuild.yml/badge.svg)](https://github.com/IntelliTect/CodingGuidelines/actions/workflows/dotnetBuild.yml)

[Coding Snippets Repository](https://github.com/IntelliTect/IntelliTect.Snippets)

[Coding Snippets Extension](https://marketplace.visualstudio.com/items?itemName=IntelliTect.intellitectsnippets)

# Coding Guidelines / Design Guidelines
A repository to contain IntelliTect's tools for coding conventions. [https://intellitect.github.io/CodingGuidelines/](https://intellitect.github.io/CodingGuidelines/)

## Guidelines Site Maintenance
 There are two github actions that are used to update the CodingGuidelinesSite. One action ( *Update csharp Markdown* ) will run automatically when the XML file in the master branch is updated via a commit. The CodingGuidelines github page will then reflect the changes. After reviewing the "dev" site, there is another action ( *Update Docs Folder on CodingGuidelinesSite* ) that will move the new markdown file to production site [CodingGuidelinesSite]( https://intellitect.github.io/CodingGuidelinesSite/). 
There is also another action to manually run a xml to md conversion on any branch. There is a retired tool to extract guidelines from the manuscript word documents to an XML file [Manuscript Guidelines Extractor](https://github.com/IntelliTect/ManuscriptGuidelinesExtractor)
