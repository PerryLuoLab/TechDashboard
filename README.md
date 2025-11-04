# TechDashboard

A modern WPF dashboard application built with .NET 8, featuring a collapsible navigation panel, multiple themes, and smooth animations.

## Features

### 🎨 **Multiple Themes**
- **Dark Theme**: Classic dark interface with gray accents
- **Light Theme**: Clean light interface
- **Blue Tech Theme**: Transparent blue-tech style with vibrant gradients

### 📐 **Collapsible Navigation Panel**
- **Auto-sizing**: Navigation width automatically adapts to the longest navigation item (200-350px range)
- **Smooth Animation**: 250ms animated transition between expanded and collapsed states
- **VS Code Style**: 
  - Drag-to-resize from right edge (5px drag zone) when expanded
  - Drag from anywhere when collapsed
  - Double-click on empty space to expand when collapsed
- **Icon-only Mode**: When collapsed, shows only icons (24px) with tooltips
- **Manual Resize**: Continuous drag-to-resize functionality (0-350px range)

### 🎯 **Interactive Elements**
- **Selection Animation**: Navigation buttons slide right (4px) when selected
- **Hover Effects**: Smooth background color transitions
- **Progress Bars**: Vibrant gradient fills with theme-specific colors
- **Card Shadows**: Soft glow effects on dashboard cards

### 🏗️ **Architecture**
- **MVVM Pattern**: Clean separation of concerns
- **ObservableObject**: Base class for property change notifications
- **RelayCommand**: Command pattern implementation
- **Data Binding**: Full XAML data binding with converters

### 📊 **Dashboard Pages**
- **Overview**: System statistics and performance metrics
- **Analytics**: Data analysis page
- **Reports**: Report generation page
- **Settings**: Theme selection and appearance settings

## Requirements

- .NET 8 SDK
- Windows 10/11 (WPF requires Windows)

## Building and Running

### Using Command Line

1. Navigate to project root directory
2. Build the project:
   ```bash
   dotnet build
   ```
3. Run the application:
   ```bash
   dotnet run
   ```

### Using Visual Studio

1. Open `TechDashboard.sln` in Visual Studio
2. Press `F5` to build and run

## Project Structure

```
TechDashboard/
├── Commands/              # Command implementations
│   └── RelayCommand.cs
├── Converters/            # Value converters
│   └── ThemeConverter.cs
├── Helpers/               # Helper classes
│   └── ThemeManager.cs
├── Infrastructure/        # Base classes
│   └── ObservableObject.cs
├── Themes/               # Theme resource dictionaries
│   ├── DarkTheme.xaml
│   ├── LightTheme.xaml
│   └── BlueTechTheme.xaml
├── ViewModels/           # ViewModels
│   └── MainViewModel.cs
├── MainWindow.xaml       # Main window UI
├── MainWindow.xaml.cs    # Main window code-behind
├── App.xaml             # Application resources
└── App.xaml.cs          # Application startup
```

## Key Features Implementation

### Navigation Panel Auto-Sizing

The navigation panel automatically calculates its optimal width based on the longest navigation item:

```csharp
// Calculates width based on content measurement
private void CalculateOptimalNavWidth()
{
    // Measures all navigation buttons and finds the maximum width
    // Adds margins and padding to get total navigation width
    // Clamps between MinExpandedWidth (200px) and MaxExpandedWidth (350px)
}
```

### Double-Click to Expand

When the navigation is collapsed, double-clicking on empty space (not on buttons) expands it:

```csharp
private void NavPanel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
{
    // Checks if navigation is collapsed
    // Verifies click is on empty space (not a button)
    // Expands navigation if conditions are met
}
```

### Theme System

Themes are implemented as ResourceDictionaries and can be switched dynamically:

```csharp
// ApplyTheme method in App.xaml.cs
public static void ApplyTheme(string themeName)
{
    // Removes existing theme dictionaries
    // Loads new theme from Themes/{themeName}Theme.xaml
    // Falls back to DarkTheme if theme not found
}
```

### Blue Tech Theme Transparency

The Blue Tech theme features increased transparency:

- **Window Background**: 75% opacity (reduced from 85%)
- **Navigation Background**: 85% opacity
- **Card Background**: 80% opacity
- **Header Background**: 80% opacity

## Customization

### Adding New Navigation Items

1. Add a new `Button` in `MainWindow.xaml` within the `NavContent` StackPanel
2. Add corresponding page visibility logic
3. Update `MainViewModel.cs` with new page property

### Creating New Themes

1. Create a new XAML file in `Themes/` folder
2. Define color resources and styles
3. Add theme toggle button in Settings page
4. Update `App.ApplyTheme()` if needed

## Code Quality

- ✅ Full English comments throughout codebase
- ✅ MVVM pattern compliance
- ✅ Exception handling in all critical methods
- ✅ Clean separation of concerns
- ✅ 0 compilation warnings/errors

## License

This project is provided as-is for educational and demonstration purposes.
