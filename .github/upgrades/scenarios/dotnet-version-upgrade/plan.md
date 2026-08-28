# .NET Version Upgrade Plan

## Overview

**Target**: Upgrade the Solvify solution from .NET 8 to .NET 10.
**Scope**: 2 SDK-style projects — the CLI application and its test project — with a straightforward modern-to-modern framework bump and no compatibility blockers.

### Selected Strategy
**All-At-Once** — All projects upgraded simultaneously in a single operation.
**Rationale**: 2 projects, all on .NET 8, clear dependency structure.

## Tasks

### 01-upgrade-solution: Retarget the CLI and test projects to .NET 10

Update the application and test projects together so the solution stays aligned on the new target framework. The assessment showed both projects are already SDK-style and have no incompatible packages or API breaks, so this should be a direct framework bump with only minor build-time adjustments if any surface during restore or compilation.

Focus on the two project files identified in the assessment and validate any environment-specific settings that could affect the upgrade, such as toolchain or project-level framework assumptions. Keep the change atomic so the application and tests move together and remain compatible with each other.

**Done when**: both project files target net10.0, restore succeeds, and the solution builds without errors or warnings in the modified projects.

---

### 02-validate-solution: Build and test the upgraded solution

Run the full solution validation after the framework bump to confirm the CLI app and test project behave correctly on .NET 10. This task captures any regression introduced by the retargeting step and verifies that the solution is stable after the upgrade.

Use the build and test results to confirm the migration is complete and that no follow-up framework compatibility issues remain. If a warning or failure appears, resolve it in the upgraded projects before closing out the workflow.

**Done when**: the solution builds successfully and the test suite passes with no remaining compilation warnings or errors in the upgraded projects.