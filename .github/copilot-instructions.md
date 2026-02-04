# GitHub Copilot Instructions - CodingGuidelines Repository

## Repository Overview

This repository contains **IntelliTect's coding conventions and guidelines** along with a **Roslyn analyzer** (IntelliTect.Analyzers) that enforces these conventions. The repository also hosts coding guidelines documentation as a Jekyll-based website at https://essentialcsharp.com/guidelines.

**Repository Stats:**
- Size: ~207 MB
- Language: C# (.NET/Roslyn analyzers)
- Framework: .NET 10 (with multi-targeting for .NET 8, 9, 10)
- Projects: 4 main projects in the IntelliTect.Analyzer solution
- Source Files: ~56 C# files in analyzer, ~11 markdown docs
- Test Framework: MSTest

## Build & Validation Commands

### Prerequisites
- .NET SDK 10.0.100 or later (specified in `global.json` with `rollForward: minor` and `allowPrerelease: true`)
- PowerShell (workflows default to pwsh shell)

### Standard Build Sequence

**ALWAYS follow this exact sequence:**

1. **Restore dependencies** (REQUIRED first step):
   ```bash
   dotnet restore
   ```
   - Takes ~2-5 seconds on clean environment
   - Uses NuGet.config with two package sources: nuget.org and Azure DevOps daily feed
   - Restores to solution root directory

2. **Build** (use same configuration as CI):
   ```bash
   dotnet build -p:ContinuousIntegrationBuild=True --no-restore --configuration Release
   ```
   - Takes ~7-14 seconds
   - **IMPORTANT:** Always use `-p:ContinuousIntegrationBuild=True` to match CI behavior
   - Uses `--no-restore` flag (restore must be done separately first)
   - Builds 4 projects: IntelliTect.Analyzer, IntelliTect.Analyzer.CodeFixes, IntelliTect.Analyzer.Tests, IntelliTect.Analyzer.Integration.Tests
   - Multi-targets: netstandard2.0 (analyzer), net8.0, net9.0, net10.0 (tests)
   - Output: bin/Release/{framework}/ directories

3. **Test**:
   ```bash
   dotnet test --no-build --configuration Release --verbosity normal --logger trx --results-directory ./TestResults
   ```
   - Takes ~8-20 seconds
   - Runs 78 tests across all target frameworks
   - **IMPORTANT:** Must use `--no-build` flag and same configuration as build
   - Creates TRX files in ./TestResults for CI reporting

### Clean Build
```bash
dotnet clean
dotnet restore
dotnet build -p:ContinuousIntegrationBuild=True --configuration Release
```

## Known Issues & Workarounds

### Azure DevOps Daily Feed Reliability Issue
**Problem:** The XMLtoMD tools project depends on `System.CommandLine.DragonFruit` from the Azure DevOps daily feed (`https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-libraries/nuget/v3/index.json`). This feed can be temporarily unavailable with errors like:
```
Resource temporarily unavailable (pe3vsblobprodcus354.vsblob.vsassets.io:443)
```

**Impact:** 
- Cannot run XMLtoMD conversion tool locally without cached packages
- Workflow `ConvertGuidelinesXmlToMarkdown.yml` may fail on this step
- Does NOT affect main analyzer build/test (IntelliTect.Analyzer solution)

**Workaround:**
- The main analyzer solution builds independently and does NOT require the daily feed
- XMLtoMD conversion is handled by CI workflows that have cached packages
- If you need to test XML conversion locally, wait and retry when feed is available
- **DO NOT** modify the analyzer solution - it works reliably

## Project Structure

### Root Files
```
.editorconfig                    # Coding style configuration (IntelliTect conventions)
.gitignore                       # Git ignore rules
Directory.Build.props            # MSBuild properties: TreatWarningsAsErrors=true, LangVersion=14.0
Directory.Packages.props         # Central package version management
NuGet.config                     # Package sources configuration
global.json                      # .NET SDK version pinning (10.0.100)
IntelliTect.Analyzer.sln         # Main solution file
```

### Main Projects (IntelliTect.Analyzer/)
```
IntelliTect.Analyzer/                       # Roslyn analyzer library (netstandard2.0)
  ├── IntelliTect.Analyzer.csproj           # Packable analyzer, NuGet: IntelliTect.Analyzers
  ├── Resources.resx                        # Diagnostic messages
  └── tools/install.ps1, uninstall.ps1      # NuGet package scripts

IntelliTect.Analyzer.CodeFixes/             # Code fix providers (netstandard2.0)
  └── IntelliTect.Analyzer.CodeFixes.csproj

IntelliTect.Analyzer.Test/                  # Unit tests (multi-target: net8.0, net9.0, net10.0)
  └── IntelliTect.Analyzer.Tests.csproj     # 78 tests using MSTest

IntelliTect.Analyzer.Integration.Tests/     # Integration tests (multi-target)
  └── IntelliTect.Analyzer.Integration.Tests.csproj
```

### Documentation (docs/)
```
docs/
  ├── Guidelines(8th Edition).xml           # Source XML for coding guidelines
  ├── coding/csharp.md                      # Generated markdown from XML
  ├── analyzers/                            # Analyzer documentation (00XX.Naming.md, etc.)
  ├── _config.yml, Gemfile, Gemfile.lock    # Jekyll site configuration
  └── [apis, databases, security, etc.]     # Other guideline categories
```

### Tools (Tools/)
```
Tools/XMLtoMD/GuidelineXmlToMD/             # Converts XML guidelines to markdown
  ├── GuidelineXmlToMD.csproj               # Uses System.CommandLine.DragonFruit
  └── Program.cs                            # Command-line tool
```

## GitHub Workflows & CI

### Primary CI Workflow: `.github/workflows/dotnetBuild.yml`
**Triggers:** Push/PR to main, manual dispatch  
**Steps:**
1. Checkout code
2. Setup .NET from global.json
3. `dotnet restore`
4. `dotnet build -p:ContinuousIntegrationBuild=True --no-restore --configuration Release`
5. `dotnet test --no-build --configuration Release --verbosity normal --logger trx --results-directory ./TestResults`
6. Create failed tests playlist artifact (on failure)
7. Auto-merge Dependabot PRs

**Shell:** PowerShell (`pwsh`)

### Other Workflows
- `Deploy.yml`: Publishes NuGet package on release tags (v*)
- `ConvertGuidelinesXmlToMarkdown.yml`: Auto-converts XML to MD when XML changes (uses .NET 5.0.x/6.0.x)
- `manuallyRunXmlToMD.yml`: Manual XML to MD conversion
- `copy.yml`: Syncs docs/ to CodingGuidelinesSite repo

## Code Style & Linting

### Configuration Files
- `.editorconfig`: Enforces IntelliTect coding conventions
  - Private fields: `_PascalCase` (underscore prefix, warning level)
  - Properties: `PascalCase` (warning level)
  - `var` usage: Restricted (warnings for built-in types and unclear cases)
  - Braces: Required for multi-line expressions (Roslynator rules RCS1001, RCS1003, RCS1007)
  - Parentheses: Required for operator precedence clarity (RCS1123)

### Build Configuration
- `TreatWarningsAsErrors=true` in Directory.Build.props
- **All warnings are treated as errors** - zero tolerance policy
- LangVersion: 14.0 (C# 14)

## Making Code Changes

### Before Making Changes
1. Understand that warnings = errors in this repo
2. Review `.editorconfig` for naming and style conventions
3. Run existing tests to establish baseline: `dotnet test --configuration Release`

### After Making Changes
1. Build: `dotnet build -p:ContinuousIntegrationBuild=True --configuration Release`
2. Test: `dotnet test --no-build --configuration Release`
3. Verify zero warnings/errors (build treats warnings as errors)

### Common Validation Errors
- **Naming violations:** Private fields must start with underscore and be PascalCase (`_FieldName`)
- **var usage:** Don't use `var` for built-in types (int, string, etc.)
- **Missing braces:** Multi-line if/else/for must have braces
- **Parentheses:** Add parentheses for complex arithmetic/binary operators

## Additional Notes

- **NuGet package:** Published as `IntelliTect.Analyzers` (note the 's')
- **Documentation site:** https://essentialcsharp.com/guidelines (not the old GitHub Pages URL)
- **Jekyll serving:** Not required for analyzer development (only for docs website changes)
- **Multi-targeting:** Tests run on net8.0, net9.0, and net10.0 - all must pass
- **Trust these instructions:** Only search/explore if information here is incomplete or incorrect

## Quick Reference Commands

```bash
# Full clean build and test (most common workflow)
dotnet clean
dotnet restore
dotnet build -p:ContinuousIntegrationBuild=True --configuration Release
dotnet test --no-build --configuration Release

# Check SDK version
dotnet --version  # Should be 10.0.x

# View solution structure
dotnet sln IntelliTect.Analyzer.sln list
```
