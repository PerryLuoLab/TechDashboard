# TechDashboard - Modern WPF Dashboard Application

**English** | [简体中文](README.zh-CN.md)

[![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![WPF](https://img.shields.io/badge/WPF-Windows-blue.svg)](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)
[![Version](https://img.shields.io/badge/version-1.3-blue.svg)](V1.3_UPDATE_NOTES.md)

A modern, feature-rich dashboard application built with .NET 8 WPF, showcasing advanced UI/UX patterns and MVVM architecture.

## 📸 Screenshots

*Main dashboard interface with navigation panel*

### Theme Showcase
<div align="center">
  <img src="Assets/theme-dark.png" alt="Dark Theme" width="45%">
  <img src="Assets/theme-light.png" alt="Light Theme" width="45%">
  <img src="Assets/theme-bluetech.png" alt="Blue Tech Theme" width="45%">
  <img src="Assets/theme-aurora.png" alt="Aurora Theme" width="45%">
</div>

*Four premium themes: Dark (Gray-Black), Light, Blue Tech, and Aurora (Cyberpunk/Synthwave)*

### Language Support
![Language Support](Assets/language-support.png)

*Multi-language support with 5 languages: English, Simplified Chinese, Traditional Chinese, Korean, and Japanese*

### Navigation Panel
![Navigation Panel](Assets/navigation-panel.png)

*Smart navigation panel with auto-width calculation and double-click to toggle*

## ✨ Features

### 🎨 Modern UI/UX
- **Four Premium Themes**: Dark (Gray-Black), Light, Blue Tech, and Aurora (Cyberpunk/Synthwave) - all carefully optimized
- **Aurora Theme Features** ✨ NEW:
  - Cyberpunk/Synthwave aesthetic with deep purple and cyan gradients
  - Colored glow shadows (purple/pink) instead of traditional black shadows
  - Micro-interaction animations: scale on hover (buttons), translate on hover (cards)
  - Breathing animation on icon boxes with pulsating glow effect
  - Modern glassmorphism with enhanced depth and layering
- **Semantic Color System** ✨ v1.1: Customized status colors (Success, Error, Warning, Info) for each theme
- **Unified Icon System** ✨ v1.3: Centralized icon management with grouped constants
- **Smooth Animations**: Eased transitions (navigation width, hover effects)
- **Adaptive Navigation Width** ✨ v1.3: Dynamically fits longest label + 6px buffer, auto-updates on language change

### 🌐 Internationalization (i18n)
- Multi-language support (English, Simplified Chinese, Traditional Chinese, Korean, Japanese)
- Runtime switching without restart
- Easy addition via .resx + LanguageConstants

### 🔄 Smart Navigation
- Collapsible sidebar (200ms animation)
- Double-click blank area to toggle
- Width recalculation on culture change

### 🧱 Architecture
- Clean MVVM (ObservableObject, RelayCommand)
- Core layer for reusables: Constants, Converters, Infrastructure
- Services for theme & localization abstraction
- No magic strings: PageConstants, ThemeConstants, LanguageConstants, IconConstants centralize identifiers

### 🎯 Technical Highlights
- Custom GridLengthAnimation for column transitions
- Strongly typed constants for themes, pages, languages, icons
- Measured text width using FormattedText for precision
- DI-friendly services (constructor injection)

## 📁 Project Structure
```
TechDashboard/
├── App.xaml / App.xaml.cs
├── MainWindow.xaml / MainWindow.xaml.cs
├── Core/
│   ├── Infrastructure/ (ObservableObject, RelayCommand, GridLengthAnimation)
│   ├── Constants/ (Navigation, Theme, Language, Icon, Page)
│   ├── Converters/ (Theme, Language, BoolToVisibility, Icon)
│   └── Extensions/ (ServiceCollectionExtensions)
├── Services/ (LocalizationService, ThemeService + Interfaces)
├── ViewModels/ (MainViewModel)
├── Themes/ (*.xaml theme dictionaries)
├── Resources/ (*.resx language files)
└── ARCHITECTURE.md
```

## 🧩 Module Responsibilities
See [ARCHITECTURE.md](ARCHITECTURE.md) for a full breakdown.

## 🚀 Getting Started
```bash
git clone https://github.com/PerryLuoLab/TechDashboard.git
cd TechDashboard
dotnet restore
dotnet run --project TechDashboard.csproj
```

## 🛠 Configuration
- Add theme: create XAML + update ThemeConstants
- Add language: create resx + update LanguageConstants
- Add page: extend PageConstants + add localization keys

## 🎨 Aurora Theme Details
The Aurora theme brings a modern cyberpunk/synthwave aesthetic with:
- **Color Palette**: Deep dark backgrounds (#0D0221) with high-saturation purple (#A855F7) and cyan (#06B6D4) accents
- **Glow Effects**: Colored drop shadows (purple/pink) create neon-like glowing effects
- **Micro-interactions**:
  - Buttons scale to 1.05x on hover with smooth easing (150ms)
  - Navigation items slide 3px to the right on hover
  - Dashboard cards lift up 5px on hover with storyboard animations
  - Icon boxes have breathing glow animation (2s cycle, sine easing)
- **Typography**: Enhanced text rendering with purple secondary text (#A78BFA)
- **Best for**: Dark mode enthusiasts, creative professionals, modern tech applications

## 📐 Navigation Width Logic (v1.3)
1. Measure all navigation labels with FormattedText
2. Longest label width + icon + spacing + paddings + margins + ExpansionExtraBuffer (=6px)
3. Apply as MaxWidth and animated target width when expanded

## 🧪 Testing Ideas
- Unit test PageConstants.GetStatusKey
- Theme load fallback scenario
- Localization switching retains navigation width recalculation

## 🐛 Troubleshooting
- Designer localization errors: safe to ignore (design-time limitation)
- Theme fails: ensure dictionary path matches ThemeConstants
- Language missing: verify culture code and resx naming

## 📝 Changelog
### v1.4 - 2024
- **Aurora Theme Added**: New cyberpunk/synthwave theme with micro-interactions
  - Colored glow shadows (purple/pink) for depth and modern feel
  - Scale animations on buttons (1.05x on hover)
  - Translate animations on cards (lift up 5px on hover)
  - Breathing glow animation on icon boxes
  - Deep purple to cyan gradient backgrounds
- Aurora theme localized in all 5 languages

### v1.3 - 2024
- Precise dynamic navigation width (+6px buffer)
- Removed magic page strings (PageConstants)
- ThemeService now uses ThemeConstants
- Architecture documentation added
- README updated to reflect improvements

### v1.2
- Core layer reorganization, constants consolidation

### v1.1
- Semantic color brushes, language/theme constants

### v1.0
- Initial release

## 🤝 Contributing
Fork + PR. Keep new identifiers in constants classes.

## 📝 License
MIT License. See LICENSE.

## 🙏 Acknowledgments
Microsoft WPF, community resources.

**Made with ❤️ using .NET 8 and WPF****Made with ❤️ using .NET 8 and WPF**