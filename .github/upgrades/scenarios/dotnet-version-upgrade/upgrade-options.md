# Upgrade Options — Solvify

Assessment: 2 SDK-style projects on net8.0; no incompatible packages, no API breaks, straightforward modern-to-modern upgrade.

## Strategy

### Upgrade Strategy
Assessment shows a very small, homogeneous modern .NET solution with a simple framework bump path and no compatibility blockers, so an all-at-once upgrade keeps the change atomic and easy to validate.

| Value | Description |
|-------|-------------|
| **All-At-Once** (selected) | Upgrade all projects simultaneously in a single atomic operation. |
