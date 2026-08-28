# Projects and dependencies analysis

This document provides a comprehensive overview of the projects and their dependencies in the context of upgrading to .NETCoreApp,Version=v10.0.

## Table of Contents

- [Executive Summary](#executive-Summary)
  - [Highlevel Metrics](#highlevel-metrics)
  - [Projects Compatibility](#projects-compatibility)
  - [Package Compatibility](#package-compatibility)
  - [API Compatibility](#api-compatibility)
  - [Binding Redirect Configuration](#binding-redirect-configuration)
- [Aggregate NuGet packages details](#aggregate-nuget-packages-details)
- [Top API Migration Challenges](#top-api-migration-challenges)
  - [Technologies and Features](#technologies-and-features)
  - [Most Frequent API Issues](#most-frequent-api-issues)
- [Projects Relationship Graph](#projects-relationship-graph)
- [Project Details](#project-details)

  - [Solvify.Cli.Test\Solvify.Cli.Test.csproj](#solvifyclitestsolvifyclitestcsproj)
  - [Solvify.Cli\Solvify.Cli.csproj](#solvifyclisolvifyclicsproj)


## Executive Summary

### Highlevel Metrics

| Metric | Count | Status |
| :--- | :---: | :--- |
| Total Projects | 2 | All require upgrade |
| Total NuGet Packages | 10 | All compatible |
| Total Code Files | 8 |  |
| Total Code Files with Incidents | 2 |  |
| Total Lines of Code | 655 |  |
| Total Number of Issues | 2 |  |
| Estimated LOC to modify | 0+ | at least 0,0% of codebase |

### Projects Compatibility

| Project | Target Framework | Difficulty | Package Issues | API Issues | Binding Issues | Est. LOC Impact | Description |
| :--- | :---: | :---: | :---: | :---: | :---: | :---: | :--- |
| [Solvify.Cli.Test\Solvify.Cli.Test.csproj](#solvifyclitestsolvifyclitestcsproj) | net8.0 | 🟢 Low | 0 | 0 | 0 |  | DotNetCoreApp, Sdk Style = True |
| [Solvify.Cli\Solvify.Cli.csproj](#solvifyclisolvifyclicsproj) | net8.0 | 🟢 Low | 0 | 0 | 0 |  | DotNetCoreApp, Sdk Style = True |

### Package Compatibility

| Status | Count | Percentage |
| :--- | :---: | :---: |
| ✅ Compatible | 10 | 100,0% |
| ⚠️ Incompatible | 0 | 0,0% |
| 🔄 Upgrade Recommended | 0 | 0,0% |
| ***Total NuGet Packages*** | ***10*** | ***100%*** |

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 532 |  |
| ***Total APIs Analyzed*** | ***532*** |  |

## Aggregate NuGet packages details

| Package | Current Version | Suggested Version | Projects | Description |
| :--- | :---: | :---: | :--- | :--- |
| coverlet.collector | 6.0.0 |  | [Solvify.Cli.Test.csproj](#solvifyclitestsolvifyclitestcsproj) | ✅Compatible |
| Microsoft.CodeCoverage | 17.8.0 |  | [Solvify.Cli.Test.csproj](#solvifyclitestsolvifyclitestcsproj) | ✅Compatible |
| Microsoft.NET.Test.Sdk | 17.8.0 |  | [Solvify.Cli.Test.csproj](#solvifyclitestsolvifyclitestcsproj) | ✅Compatible |
| Microsoft.TestPlatform.ObjectModel | 17.8.0 |  | [Solvify.Cli.Test.csproj](#solvifyclitestsolvifyclitestcsproj) | ✅Compatible |
| Microsoft.TestPlatform.TestHost | 17.8.0 |  | [Solvify.Cli.Test.csproj](#solvifyclitestsolvifyclitestcsproj) | ✅Compatible |
| MSTest.TestAdapter | 3.1.1 |  | [Solvify.Cli.Test.csproj](#solvifyclitestsolvifyclitestcsproj) | ✅Compatible |
| MSTest.TestFramework | 3.1.1 |  | [Solvify.Cli.Test.csproj](#solvifyclitestsolvifyclitestcsproj) | ✅Compatible |
| Newtonsoft.Json | 13.0.1 |  | [Solvify.Cli.Test.csproj](#solvifyclitestsolvifyclitestcsproj) | ✅Compatible |
| NuGet.Frameworks | 6.5.0 |  | [Solvify.Cli.Test.csproj](#solvifyclitestsolvifyclitestcsproj) | ✅Compatible |
| System.Reflection.Metadata | 1.6.0 |  | [Solvify.Cli.Test.csproj](#solvifyclitestsolvifyclitestcsproj) | ✅Compatible |

## Top API Migration Challenges

### Technologies and Features

| Technology | Issues | Percentage | Migration Path |
| :--- | :---: | :---: | :--- |

### Most Frequent API Issues

| API | Count | Percentage | Category |
| :--- | :---: | :---: | :--- |

## Projects Relationship Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart LR
    P1["<b>📦&nbsp;Solvify.Cli.csproj</b><br/><small>net8.0</small>"]
    P2["<b>📦&nbsp;Solvify.Cli.Test.csproj</b><br/><small>net8.0</small>"]
    P2 --> P1
    click P1 "#solvifyclisolvifyclicsproj"
    click P2 "#solvifyclitestsolvifyclitestcsproj"

```

## Project Details

<a id="solvifyclitestsolvifyclitestcsproj"></a>
### Solvify.Cli.Test\Solvify.Cli.Test.csproj

#### Project Info

- **Current Target Framework:** net8.0
- **Proposed Target Framework:** net10.0
- **SDK-style**: True
- **Project Kind:** DotNetCoreApp
- **Dependencies**: 1
- **Dependants**: 0
- **Number of Files**: 3
- **Number of Files with Incidents**: 1
- **Lines of Code**: 232
- **Estimated LOC to modify**: 0+ (at least 0,0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph current["Solvify.Cli.Test.csproj"]
        MAIN["<b>📦&nbsp;Solvify.Cli.Test.csproj</b><br/><small>net8.0</small>"]
        click MAIN "#solvifyclitestsolvifyclitestcsproj"
    end
    subgraph downstream["Dependencies (1"]
        P1["<b>📦&nbsp;Solvify.Cli.csproj</b><br/><small>net8.0</small>"]
        click P1 "#solvifyclisolvifyclicsproj"
    end
    MAIN --> P1

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 13 |  |
| ***Total APIs Analyzed*** | ***13*** |  |

<a id="solvifyclisolvifyclicsproj"></a>
### Solvify.Cli\Solvify.Cli.csproj

#### Project Info

- **Current Target Framework:** net8.0
- **Proposed Target Framework:** net10.0
- **SDK-style**: True
- **Project Kind:** DotNetCoreApp
- **Dependencies**: 0
- **Dependants**: 1
- **Number of Files**: 7
- **Number of Files with Incidents**: 1
- **Lines of Code**: 423
- **Estimated LOC to modify**: 0+ (at least 0,0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (1)"]
        P2["<b>📦&nbsp;Solvify.Cli.Test.csproj</b><br/><small>net8.0</small>"]
        click P2 "#solvifyclitestsolvifyclitestcsproj"
    end
    subgraph current["Solvify.Cli.csproj"]
        MAIN["<b>📦&nbsp;Solvify.Cli.csproj</b><br/><small>net8.0</small>"]
        click MAIN "#solvifyclisolvifyclicsproj"
    end
    P2 --> MAIN

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 519 |  |
| ***Total APIs Analyzed*** | ***519*** |  |

