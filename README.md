# Walk This Way

A [PowerToys Command Palette](https://learn.microsoft.com/windows/powertoys/command-palette/) extension for quickly switching between open windows. Walk This Way provides both a searchable window list and a dock band that displays all open applications for one-click switching.

## Features

- **Window Search**: Search across all open windows by title or process name with fuzzy matching
- **Dock Band**: Pin a band to the Command Palette dock showing all open applications
- **Switch Windows**: Instantly switch to any open window
- **Close Windows**: Close windows directly from the palette
- **End Task**: Terminate unresponsive processes

## Installation

### From GitHub Releases

1. Download the latest `.msix` package from [Releases](https://github.com/michaeljolley/WalkThisWay/releases)
2. Double-click the `.msix` file to install
3. Open Command Palette and search for "Walk This Way"

### From Source

See [Development](#development) below.

## Usage

### Command Palette

1. Open Command Palette (default: `Alt+Space`)
2. Type "Walk This Way" to open the window switcher
3. Search for windows by title or process name
4. Press Enter to switch to the selected window

### Dock Band

1. Open Command Palette settings
2. Enable the Dock
3. Add the "Open Windows" band
4. Click any application in the band to switch to it

## Development

### Prerequisites

- [Visual Studio 2022](https://visualstudio.microsoft.com/) or later
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Windows 10 SDK (10.0.26100.0)
- Developer Mode enabled on Windows

### Build

```shell
dotnet restore WalkThisWay/WalkThisWay.csproj -r win-x64
dotnet build WalkThisWay/WalkThisWay.csproj -r win-x64
```

### Test

```shell
dotnet test WalkThisWay.Tests/WalkThisWay.Tests.csproj -r win-x64
```

### Debug

1. Open `WalkThisWay.slnx` in Visual Studio
2. Set configuration to Debug
3. Start the project
4. Use "Reload Command Palette extensions" from within Command Palette

## Contributing

Contributions are welcome! Please open an issue or submit a pull request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
