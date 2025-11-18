# TechDashboard - Modern WPF Dashboard Application

**English** | [简体中文](README.zh-CN.md)

[![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![WPF](https://img.shields.io/badge/WPF-Windows-blue.svg)](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)
[![Version](https://img.shields.io/badge/version-1.1-blue.svg)](V1.1_UPDATE_NOTES.md)

A modern, feature-rich dashboard application built with .NET 8 WPF, showcasing advanced UI/UX patterns and MVVM architecture.

## 📸 Screenshots

*Main dashboard interface with navigation panel*

### Theme Showcase
<div align="center">
  <img src="Assets/theme-dark.png" alt="Dark Theme" width="45%">
  <img src="Assets/theme-light.png" alt="Light Theme" width="45%">
  <img src="Assets/theme-bluetech.png" alt="Blue Tech Theme" width="45%">
</div>

*Three premium themes: Dark (Gray-Black), Light, and Blue Tech*

### Language Support
![Language Support](Assets/language-support.png)

*Multi-language support with 5 languages: English, Simplified Chinese, Traditional Chinese, Korean, and Japanese*

### Navigation Panel
![Navigation Panel](Assets/navigation-panel.png)

*Smart navigation panel with auto-width calculation and drag-to-resize*

## ✨ Features

### 🎨 Modern UI/UX
- **Three Premium Themes**: Dark (Gray-Black), Light, and Blue Tech - all carefully optimized
- **Semantic Color System** ✨ v1.1: Customized status colors (Success, Error, Warning, Info) for each theme, ensuring optimal contrast and readability
- **Smooth Animations**: All UI transitions animated with easing functions
- **Responsive Layout**: Adaptive design for various screen sizes
- **Gradient Effects**: Beautiful gradients and shadows throughout

### 🌐 Internationalization (i18n)
- **Multi-Language Support**: English, Simplified Chinese, Traditional Chinese, Korean, Japanese
- **Dynamic Switching**: Change language on-the-fly without restart
- **Resource-Based**: Easy to add more languages

### 🔄 Smart Navigation
- **Collapsible Sidebar**: Smooth expand/collapse with 200ms animation
- **Auto-Width Calculation**: Navigation width automatically adjusts to longest text
- **Language-Aware Resize**: Automatically recalculates width when language changes
- **Drag-to-Resize**: Drag the navigation panel edge to custom resize
- **Double-Click Expand/Collapse**: Double-click empty area to toggle
- **Visual Feedback**: Hover effects and selected state indicators

### 🎯 Technical Highlights
- **Clean MVVM Architecture**: Proper separation of concerns
- **Observable Pattern**: Reactive property updates with `INotifyPropertyChanged`
- **Command Pattern**: Reusable `RelayCommand` implementation
- **Theme Management**: Dynamic theme switching with merged dictionaries
- **Type-Safe Resources**: Strongly-typed access to localized strings
- **Constants Management** ✨ v1.1: Centralized constant classes eliminate magic strings
- **Complete Documentation** ✨ v1.1: All public APIs have XML documentation comments

## 📋 Requirements

- **.NET 8 SDK** or later
- **Windows 10/11** (WPF is Windows-only)
- **Visual Studio 2022** (recommended) or any .NET-compatible IDE

## 🚀 Getting Started

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/PerryLuoLab/TechDashboard.git
   cd TechDashboard
   ```

2. **Build the project**
   ```bash
   dotnet restore
   dotnet build
   ```

3. **Run the application**
   ```bash
   dotnet run --project TechDashboard.csproj
   ```

### Using Visual Studio

1. Open `TechDashboard.sln` in Visual Studio 2022
2. Press `F5` to build and run
3. Or use `Ctrl+F5` to run without debugging

## 📁 Project Structure

```
TechDashboard/
├── App.xaml                    # Application entry point & resources
├── App.xaml.cs                 # IoC container configuration
├── MainWindow.xaml             # Main window UI definition
├── MainWindow.xaml.cs          # Window logic & navigation handling
├── ServiceCollectionExtensions.cs  # Dependency injection
│
├── Options/
│   └── LocalizationOptions.cs  # Localization configuration
│
├── Services/
│   ├── Interfaces/
│   │   ├── ILocalizationService.cs  # Localization service interface
│   │   └── IThemeService.cs         # Theme service interface
│   ├── LocalizationService.cs       # Localization service
│   └── ThemeService.cs              # Theme service
│
├── Converters/
│   ├── ThemeConverter.cs            # Theme converter
│   ├── LanguageConverter.cs         # Language converter
│   └── BoolToVisibilityConverter.cs # Visibility converter ✨ v1.1
│
├── Infrastructure/
│   ├── ObservableObject.cs     # ViewModel base class
│   ├── RelayCommand.cs         # Generic command implementation
│   └── GridLengthAnimation.cs  # Grid length animation
│
├── Helpers/
│   ├── NavigationConstants.cs  # Navigation constants
│   ├── ThemeConstants.cs       # Theme constants ✨ v1.1
│   └── LanguageConstants.cs    # Language constants ✨ v1.1
│
├── ViewModels/
│   └── MainViewModel.cs        # Main window ViewModel
│
├── Themes/
│   ├── DarkTheme.xaml          # Dark theme (optimized colors) ✨ v1.1
│   ├── LightTheme.xaml         # Light theme (optimized colors) ✨ v1.1
│   └── BlueTechTheme.xaml      # Blue tech theme (optimized colors) ✨ v1.1
│
└── Resources/
    ├── Strings.resx            # English resources
    ├── Strings.zh-CN.resx      # Simplified Chinese
    ├── Strings.zh-TW.resx      # Traditional Chinese
    ├── Strings.ko-KR.resx      # Korean
    └── Strings.ja-JP.resx      # Japanese
```

## 🎨 Theme Customization

### Semantic Color System ✨ v1.1

Each theme now includes complete semantic status color brushes for optimal readability:

| Theme | Success | Error | Warning | Info |
|-------|---------|-------|---------|------|
| **Dark** | #4CAF50 | #F44336 | #FF9800 | #2196F3 |
| **BlueTech** | #00E676 | #FF5252 | #FFAB40 | #40C4FF |
| **Light** | #1A7F37 | #CF222E | #BF8700 | #0969DA |

**Usage Example**:
```xml
<TextBlock Text="Success!" Foreground="{DynamicResource SuccessBrush}"/>
<TextBlock Text="Error" Foreground="{DynamicResource ErrorBrush}"/>
<TextBlock Text="Warning" Foreground="{DynamicResource WarningBrush}"/>
<TextBlock Text="Info" Foreground="{DynamicResource InfoBrush}"/>
```

### Key Theme Resources

| Resource Key | Description |
|-------------|-------------|
| `WindowBackgroundBrush` | Main window background |
| `NavBackgroundBrush` | Navigation panel background |
| `CardBackgroundBrush` | Dashboard card background |
| `TextBrush` | Primary text color |
| `TextSecondaryBrush` | Secondary text color |
| `AccentBrush` | Accent/highlight color |
| `BorderBrush` | Border colors |
| `SuccessBrush` ✨ | Success status color |
| `ErrorBrush` ✨ | Error status color |
| `WarningBrush` ✨ | Warning status color |
| `InfoBrush` ✨ | Info status color |

### Adding a New Theme

1. Create a new XAML file in `Themes/` folder
2. Define color resources (including status colors)
3. Register theme in `ThemeConstants.cs`
4. Add toggle button in `MainWindow.xaml` Settings page

## 🌍 Adding New Languages

### Step-by-Step Guide

1. **Create language resource file**
   - Copy `Strings.resx` from `Resources/` folder
   - Rename to match language code (e.g., `Strings.fr-FR.resx`)

2. **Translate string resources**
   - Open `.resx` file in Visual Studio
   - Translate all string values

3. **Register in code**
   - Update `LanguageConstants.cs` to add new language code
   - Add language selector button in `MainWindow.xaml`

## 🔧 Advanced Features

### Navigation Panel Behaviors

| Action | Behavior |
|--------|----------|
| Click toggle button | Smooth expand/collapse animation |
| Drag panel edge | Resize to custom width |
| Double-click empty area | Quick expand/collapse |
| Mouse hover edge | Show resize cursor |

### Using Constants Classes ✨ v1.1

```csharp
using TechDashboard.Helpers;

// Theme constants
string theme = ThemeConstants.ThemeNames.Dark;
string key = ThemeConstants.ResourceKeys.SuccessBrush;

// Language constants
string lang = LanguageConstants.CultureCodes.SimplifiedChinese;
string display = LanguageConstants.GetDisplayName(lang);
```

## 🐛 Troubleshooting

### XAML Designer Issues (Design-Time Only)
If you see WPFLocalizeExtension errors in Visual Studio XAML designer:

**Don't worry!** This is a known design-time limitation:
- ✅ Build succeeds: `dotnet build` works fine
- ✅ Runtime works: Application runs perfectly
- ✅ Localization works: All translations display correctly
- ❌ Designer only: Visual Studio designer can't load the extension

**Solutions**: Ignore the designer errors or use XAML code view

### Theme Not Applying
- Ensure theme file exists in `Themes/` folder
- Verify `DynamicResource` is used (not `StaticResource`)

### Language Not Changing
- Confirm language file exists in `Resources/` folder
- Check if language is registered in `LanguageConstants.cs`

## 📈 Performance Tips

1. **Resource Dictionaries**: Merged dictionaries loaded once and cached
2. **Animations**: Use `GridLengthAnimation` for hardware acceleration
3. **Bindings**: OneWay bindings for read-only properties
4. **Constants**: Use constant classes to avoid string allocations

## 📝 Changelog

### v1.1 (Latest) - 2024
- ✅ Added optimized status color brushes for all themes
- ✅ Created `ThemeConstants.cs` and `LanguageConstants.cs`
- ✅ Separated `BoolToVisibilityConverter` into independent file
- ✅ Added complete XML documentation comments
- ✅ Eliminated magic strings, improved type safety

Details: [V1.1_UPDATE_NOTES.md](V1.1_UPDATE_NOTES.md)

### v1.0 - 2024
- Initial release

## 🤝 Contributing

Contributions are welcome! Please fork the repository and submit a Pull Request.

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Microsoft WPF Team
- Material Design and GitHub Design System
- Community contributors

## 📞 Support

- **Issues**: [GitHub Issues](https://github.com/PerryLuoLab/TechDashboard/issues)
- **Email**: perryluox@yeah.net

---

**Made with ❤️ using .NET 8 and WPF**