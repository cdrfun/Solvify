# 01-upgrade-solution: Retarget the CLI and test projects to .NET 10

Update the application and test projects together so the solution stays aligned on the new target framework. The assessment shows a small, homogeneous solution: 2 SDK-style projects on net8.0, 10 compatible packages, 0 incompatible packages, and 0 API compatibility issues. That makes this a direct framework bump with no package replacement work expected.

Focus on the two project files identified in the assessment and validate any environment-specific settings that could affect the upgrade, such as toolchain or project-level framework assumptions. The test project references the CLI project, so both TFMs must stay aligned and the build must be checked end to end after the retargeting change.

## Scope Inventory
- **Projects affected**: `Solvify.Cli/Solvify.Cli.csproj`, `Solvify.Cli.Test/Solvify.Cli.Test.csproj`
- **Distinct concerns**: target framework retargeting, solution restore/build validation, test compatibility
- **Assessment signals**:
  - Both projects are already SDK-style and currently target `net8.0`
  - No incompatible packages were found for the move to `net10.0`
  - No binary, source, or behavioral API issues were reported in the assessment
  - The test project depends on the CLI project, so the CLI must remain buildable for tests to pass
- **Key files to inspect**: both project files plus any solution-level build settings that might affect TFM resolution

**Done when**: both project files target net10.0, restore succeeds, and the solution builds without errors or warnings in the modified projects.
