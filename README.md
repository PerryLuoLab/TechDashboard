# TechDashboard - Modern WPF Dashboard Application

**English** | [简体中文](README.zh-CN.md)

[![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![WPF](https://img.shields.io/badge/WPF-Windows-blue.svg)](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)

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
- **Three Premium Themes**: Dark (Gray-Black), Light, and Blue Tech
- **Smooth Animations**: All UI transitions are animated with easing functions
- **Responsive Layout**: Adaptive design that works on various screen sizes
- **Gradient Effects**: Beautiful gradients and shadow effects throughout

### 🌐 Internationalization (i18n)
- **Multi-Language Support**: English, Simplified Chinese (简体中文), Traditional Chinese (繁體中文), Korean (한국어), Japanese (日本語)
- **Dynamic Switching**: Change language on-the-fly without restart
- **Resource-Based**: Easy to add more languages by creating new resource dictionaries

### 🔄 Smart Navigation
- **Collapsible Sidebar**: Smooth expand/collapse with 200ms animation
- **Auto-Width Calculation**: Navigation width automatically adjusts to longest text (including DASHBOARD logo and all navigation items)
- **Language-Aware Resize**: Automatically recalculates and updates width when language changes
- **Drag-to-Resize**: Drag the navigation panel edge to custom resize
- **Double-Click Expand/Collapse**: Double-click empty area to expand (when collapsed) or collapse (when expanded)
- **Visual Feedback**: Hover effects and selected state indicators

### 🎯 Technical Highlights
- **Clean MVVM Architecture**: Proper separation of concerns
- **Observable Pattern**: Reactive property updates with `INotifyPropertyChanged`
- **Command Pattern**: Reusable `RelayCommand` implementation
- **Theme Management**: Dynamic theme switching with merged dictionaries
- **Type-Safe Resources**: Strongly-typed access to localized strings

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
├── App.xaml.cs                 # Theme & language management logic
├── MainWindow.xaml             # Main window UI definition
├── MainWindow.xaml.cs          # Window logic & drag handling
│
├── Commands/
│   └── RelayCommand.cs         # Generic command implementation
│
├── Converters/
│   └── ThemeConverter.cs       # Theme/Language toggle converters
│
├── Infrastructure/
│   └── ObservableObject.cs    # Base class for ViewModels
│
├── ViewModels/
│   └── MainViewModel.cs        # Main window ViewModel
│
├── Themes/
│   ├── DarkTheme.xaml          # Dark theme (Gray-Black tones)
│   ├── LightTheme.xaml         # Light theme
│   └── BlueTechTheme.xaml      # Blue tech theme
│
└── Languages/
    ├── en-US.xaml              # English resources
    ├── zh-CN.xaml              # Simplified Chinese resources
    ├── zh-TW.xaml              # Traditional Chinese resources
    ├── ko-KR.xaml              # Korean resources
    └── ja-JP.xaml              # Japanese resources
```

## 🎨 Theme Customization

### Adding a New Theme

1. Create a new XAML file in `Themes/` folder (e.g., `GreenTheme.xaml`)
2. Define color resources matching the pattern in existing themes:

```xml
<ResourceDictionary>
    <!-- Define your colors -->
    <Color x:Key="WindowBgColor">#YourColor</Color>
    <Color x:Key="NavBgColor">#YourColor</Color>
    <!-- ... more colors ... -->
    
    <!-- Create brushes -->
    <SolidColorBrush x:Key="NavBackgroundBrush" Color="{StaticResource NavBgColor}"/>
    <!-- ... more brushes ... -->
</ResourceDictionary>
```

3. Add toggle button in `MainWindow.xaml` Settings page
4. Theme automatically applies when selected

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

## 🌍 Adding New Languages

### Step-by-Step Guide

1. **Create language resource file**
   - Copy an existing language file from `Languages/` folder
   - Rename to match language code (e.g., `ja-JP.xaml` for Japanese)

2. **Translate all string resources**
   ```xml
   <ResourceDictionary xmlns:system="clr-namespace:System;assembly=mscorlib">
       <system:String x:Key="Menu_File">ファイル</system:String>
       <system:String x:Key="Menu_Edit">編集</system:String>
       <!-- ... more translations ... -->
   </ResourceDictionary>
   ```

3. **Add language selector button** in `MainWindow.xaml`:
   ```xml
   <ToggleButton Content="日本語" 
                 Style="{StaticResource LanguageToggleButton}"
                 IsChecked="{Binding CurrentLanguage, 
                            Converter={StaticResource LanguageConverter}, 
                            ConverterParameter=ja-JP}"
                 Command="{Binding ChangeLanguageCommand}" 
                 CommandParameter="ja-JP">
       <ToggleButton.Tag>
           <SolidColorBrush Color="#BC002D"/>
       </ToggleButton.Tag>
   </ToggleButton>
   ```

4. **Update ViewModel** display name mapping in `MainViewModel.cs`:
   ```csharp
   public string CurrentLanguageDisplay
   {
       get
       {
           return CurrentLanguage switch
           {
               "en-US" => "English",
               "zh-CN" => "简体中文",
               "ko-KR" => "한국어",
               "ja-JP" => "日本語",  // Add this line
               _ => "English"
           };
       }
   }
   ```

## 🔧 Advanced Features

### Navigation Panel Behaviors

| Action | Behavior |
|--------|----------|
| Click toggle button | Smooth expand/collapse animation |
| Drag panel edge | Resize to custom width |
| Double-click (collapsed) | Quick expand |
| Mouse hover edge | Show resize cursor |

### Width Calculation

The navigation width automatically adjusts based on:
- Longest navigation item text length
- Current font size and family
- Icon width and padding
- Configured margins

Formula: `Width = IconWidth + Margin + MaxTextWidth`

### Theme Switching Logic

```
User clicks theme button
    ↓
ViewModel.ChangeTheme(themeName)
    ↓
App.ApplyTheme(themeName)
    ↓
Remove old theme ResourceDictionary
    ↓
Load new theme ResourceDictionary
    ↓
UI automatically updates via DynamicResource bindings
```

## 🎯 Key Implementation Details

### 1. Smart Width Calculation
```csharp
private void CalculateOptimalNavWidth()
{
    // Measures actual text width using FormattedText
    // Adds icon, padding, and margins
    // Clamps between min (60) and max (350) values
}
```

### 2. Smooth Drag Handling
```csharp
// Drag threshold: 100px midpoint
// Snap to collapsed (60px) or expanded (calculated)
// Animation duration: 200ms with EaseInOut
```

### 3. Resource Management
- Themes use `DynamicResource` for hot-swapping
- Languages use `system:String` for localization
- All resources properly scoped and typed

## 🐛 Troubleshooting

### Theme Not Applying
- Ensure theme file exists in `Themes/` folder
- Check resource keys match between theme and usage
- Verify `DynamicResource` is used (not `StaticResource`)

### Language Not Changing
- Confirm language file exists in `Languages/` folder
- Check all required string keys are defined
- Restart app if changes don't appear immediately

### Navigation Panel Issues
- If drag doesn't work: Check for element blocking mouse input
- If width incorrect: Verify font size in calculation matches UI
- If animation jerky: Ensure no other animations running simultaneously

## 📈 Performance Tips

1. **Resource Dictionaries**: Merged dictionaries are loaded once and cached
2. **Animations**: Use `BeginAnimation` for hardware-accelerated transforms
3. **Bindings**: OneWay bindings for read-only properties reduce overhead
4. **Layout**: Avoid unnecessary layout passes by setting fixed heights where possible

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- WPF Team at Microsoft for the excellent framework
- Material Design Icons (Segoe MDL2 Assets)
- Community contributors for feedback and suggestions

## 📞 Support

- **Issues**: [GitHub Issues](https://github.com/PerryLuoLab/TechDashboard/issues)
- **Discussions**: [GitHub Discussions](https://github.com/PerryLuoLab/TechDashboard/discussions)
- **Email**: perryluox@yeah.net

---

**Made with ❤️ using .NET 8 and WPF**