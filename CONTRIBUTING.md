# Contributing to Walk This Way

You can help by reporting a bug, suggesting an improvement, or submitting a
focused change. Please follow our [Code of Conduct](CODE_OF_CONDUCT.md).

## Prerequisites

- Windows with [PowerToys](https://github.com/microsoft/PowerToys) and Command Palette enabled for manual testing.
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0), matching CI.
- [.NET 9 runtime](https://dotnet.microsoft.com/download/dotnet/9.0) to run the tests. Both projects currently target .NET 9.

Use an x64 environment for the test project. The extension also supports ARM64.

## Building and Testing

Run these commands from the repository root:

```powershell
dotnet build WalkThisWay\WalkThisWay.csproj -r win-x64
dotnet test WalkThisWay.Tests\WalkThisWay.Tests.csproj -r win-x64
```

For an ARM64 extension build, use `-r win-arm64` instead. CI builds and runs
the tests on Windows for pull requests targeting `main`.

## Reporting Bugs and Suggesting Features

Search [existing issues](https://github.com/baldbeardedbuilder/WalkThisWay/issues)
before opening a new one. Use the bug report or feature request form and
include enough detail for someone else to reproduce the problem or understand
what you need.

Remove private window titles, file paths, and other personal information from
screenshots. Report vulnerabilities privately using the
[Security Policy](SECURITY.md), not a public issue.

## Submitting Changes

1. Fork the repository and update your local `main` from this repository before creating a feature branch.
2. Make one focused change and follow the existing code style and `.editorconfig`.
3. Add or update tests for changed behavior and update any related documentation.
4. Build the extension, run the tests, and check affected behavior in Command Palette.
5. Open a pull request against `main` explaining the problem, the change, and how you checked it.

Use a short, descriptive commit message. Link related issues in your pull
request and make sure CI passes.

For changes to window handling, check search, dock updates, and virtual desktop
filtering. Use disposable windows when checking Close Window or End Task.
Ending a process can lose unsaved work.

## Project Structure

| Path | Purpose |
|------|---------|
| `WalkThisWay\Commands` | Switch, close, and end-task actions |
| `WalkThisWay\Components` | Window enumeration, event hooks, process data, and search results |
| `WalkThisWay\Helpers` | Windows API and virtual desktop integration |
| `WalkThisWay\Pages` | Window search and dock list items |
| `WalkThisWay\SettingsManager.cs` | Local preferences |
| `WalkThisWay.Tests` | MSTest tests using Moq where needed |

## Releases and License

See [Releasing](RELEASING.md) for the release process. By contributing, you
agree that your contributions are licensed under the [MIT License](LICENSE).
