# TechDashboard Architecture Overview

## 1. High-Level Architecture Layers

```
©°©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©´
©¦                    Presentation Layer                        ©¦
©¦  ©°©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©´  ©°©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©´  ©°©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©´      ©¦
©¦  ©¦ MainWindow   ©¦  ©¦ Theme XAML   ©¦  ©¦ Resource     ©¦      ©¦
©¦  ©¦ .xaml / .cs  ©¦  ©¦ Dictionaries ©¦  ©¦ Dictionaries ©¦      ©¦
©¦  ©¸©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¼  ©¸©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¼  ©¸©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¼      ©¦
©¸©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¼
                            ¨‹
©°©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©´
©¦                     ViewModel Layer                          ©¦
©¦  ©°©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©´   ©¦
©¦  ©¦ MainViewModel (State + Commands)                     ©¦   ©¦
©¦  ©¦ - Navigation state, theme, language, current page    ©¦   ©¦
©¦  ©¸©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¼   ©¦
©¸©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¼
                            ¨‹
©°©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©´
©¦                      Services Layer                          ©¦
©¦  ©°©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©´         ©°©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©´         ©¦
©¦  ©¦ LocalizationSvc  ©¦         ©¦  ThemeService    ©¦         ©¦
©¦  ©¦ (Culture mgmt)   ©¦         ©¦  (Theme loading) ©¦         ©¦
©¦  ©¸©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¼         ©¸©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¼         ©¦
©¸©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¼
                            ¨‹
©°©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©´
©¦                        Core Layer                            ©¦
©¦  ©°©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©´ ©°©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©´ ©°©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©´        ©¦
©¦  ©¦Infrastructure©¦ ©¦  Constants   ©¦ ©¦  Converters  ©¦        ©¦
©¦  ©¦(Base classes)©¦ ©¦ (Config vals)©¦ ©¦  (Bindings)  ©¦        ©¦
©¦  ©¸©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¼ ©¸©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¼ ©¸©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¼        ©¦
©¦                   ©°©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©´                          ©¦
©¦                   ©¦  Extensions  ©¦                          ©¦
©¦                   ©¦  (DI setup)  ©¦                          ©¦
©¦                   ©¸©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¼                          ©¦
©¸©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¼
```

## 2. Module Breakdown & Responsibilities

### 2.1 Core/Infrastructure (MVVM Foundation)
**Purpose**: Provide reusable base abstractions for MVVM pattern

| Class | Responsibility | Key Methods |
|-------|---------------|-------------|
| `ObservableObject` | Base for ViewModels with INotifyPropertyChanged | `SetProperty<T>()`, `RaisePropertyChanged()` |
| `RelayCommand` | ICommand implementation for UI bindings | `Execute()`, `CanExecute()` |
| `GridLengthAnimation` | Custom animation for column/row sizing | `GetCurrentValue()` with easing support |

**Dependencies**: None (pure WPF)
**Consumed by**: ViewModels, View code-behind

### 2.2 Core/Constants (Configuration Hub)
**Purpose**: Centralize all magic strings/numbers for maintainability

| Class | Configuration Domain | Example Constants |
|-------|---------------------|-------------------|
| `NavigationConstants` | Navigation panel behavior | `CollapsedWidth`, `AnimationDurationMs`, `ExpansionExtraBuffer` |
| `ThemeConstants` | Theme identifiers & paths | `ThemeNames.*`, `ThemeResourcePaths.*`, `ResourceKeys.*` |
| `LanguageConstants` | Language codes & display names | `CultureCodes.*`, `DisplayNames.*`, validation helpers |
| `IconConstants` | Segoe MDL2 icon glyphs | `Common.*`, `Navigation.*`, `Status.*` (200+ icons) |
| `PageConstants` | Logical page names | `Overview`, `Analytics`, `GetStatusKey()` |

**Dependencies**: None
**Consumed by**: All layers (ViewModels, Services, Views)

### 2.3 Core/Converters (Binding Bridge)
**Purpose**: Transform data between ViewModel and View layer

| Converter | Transformation | Use Case |
|-----------|---------------|----------|
| `ThemeConverter` | string ¡ú bool | ToggleButton IsChecked for theme selection |
| `LanguageConverter` | string ¡ú bool | ToggleButton IsChecked for language selection |
| `BoolToVisibilityConverter` | bool ? Visibility | Show/hide UI elements |
| `IconConverter` | string ¡ú formatted icon | (If needed) Icon data templates |

**Dependencies**: None
**Consumed by**: XAML bindings

### 2.4 Core/Extensions
**Purpose**: Configure dependency injection container

| Method | Registers | Lifetime |
|--------|-----------|----------|
| `AddApplicationServices()` | ILocalizationService, IThemeService | Singleton |
|  | MainViewModel | Transient |
|  | MainWindow | Singleton |
|  | LocalizationOptions | Scoped Configuration |

**Dependencies**: Microsoft.Extensions.DependencyInjection
**Consumed by**: App.xaml.cs OnStartup

### 2.5 Services Layer

#### LocalizationService
**Purpose**: Wraps WPFLocalizeExtension for culture management

| Method | Action |
|--------|--------|
| `ChangeLanguage(code)` | Switch application culture |
| `GetString(key)` | Retrieve localized text |
| `GetFormattedString(key, args)` | Get text with placeholders |
| `GetLanguageDisplayName(code)` | Human-readable language name |

**Dependencies**: WPFLocalizeExtension, LanguageConstants
**Consumed by**: MainViewModel, ThemeService

#### ThemeService
**Purpose**: Manage ResourceDictionary swapping for themes

| Method | Action |
|--------|--------|
| `ApplyTheme(name)` | Load theme XAML, replace merged dictionaries |
| `GetThemeDisplayName(name)` | Localized theme name |
| `GetAvailableThemes()` | List all theme identifiers |

**Dependencies**: ThemeConstants, ILocalizationService
**Consumed by**: MainViewModel, App.xaml.cs

### 2.6 ViewModels/MainViewModel
**Purpose**: Aggregate UI state and expose commands

**State Properties**:
- `IsNavExpanded`, `NavWidth` (navigation)
- `CurrentTheme`, `CurrentLanguage`, `CurrentPage` (user selections)
- Computed: `IsOverviewPage`, `CurrentThemeDisplay`, etc.

**Commands**:
- `ToggleNavCommand`: Expand/collapse navigation
- `NavigateCommand`: Switch page
- `ChangeThemeCommand`, `ChangeLanguageCommand`: Apply settings
- `QuickToggleThemeCommand`, `QuickToggleLanguageCommand`: Cycle presets

**Dependencies**: ILocalizationService, IThemeService, PageConstants
**Consumed by**: MainWindow DataContext binding

### 2.7 Presentation Layer

#### MainWindow.xaml.cs
**Purpose**: UI interaction glue (drag, animation, width calculation)

**Key Methods**:
- `CalculateOptimalNavWidth()`: Measures localized text with FormattedText
- `AnimateNavWidth()`: Triggers GridLengthAnimation
- Drag handlers: `NavPanel_MouseMove`, `UpdateDragWidth`

**Dependencies**: NavigationConstants, IconConstants, MainViewModel
**Consumes**: Services via ViewModel, direct UI element manipulation

#### Theme XAML Files
**Purpose**: Define theme-specific ResourceDictionary entries

**Structure**: Keyed by `ThemeConstants.ResourceKeys.*`
- Brushes: Window/Nav/Card backgrounds, text colors, status colors
- Styles: Button, MenuItem, ToggleButton templates
- Effects: Shadows, glows

**Dependencies**: None (pure XAML)
**Consumed by**: ThemeService (merged at runtime)

## 3. Data Flow Diagrams

### 3.1 Language Change Flow
```
User clicks language button
         ¡ý
MainWindow XAML (Command binding)
         ¡ý
MainViewModel.ChangeLanguageCommand.Execute(code)
         ¡ý
MainViewModel.ChangeLanguage(code)
         ¡ý
LocalizationService.ChangeLanguage(code)
         ¡ý
WPFLocalizeExtension updates all {lex:Loc} bindings
         ¡ý
MainViewModel raises PropertyChanged(CurrentLanguage)
         ¡ý
MainWindow.Vm_PropertyChanged detects language change
         ¡ý
MainWindow.CalculateOptimalNavWidth() re-measures text
         ¡ý
MainWindow.AnimateNavWidth() updates NavColumn.Width
         ¡ý
UI updates with new culture strings + navigation width
```

### 3.2 Theme Change Flow
```
User selects theme ToggleButton
         ¡ý
MainViewModel.ChangeThemeCommand.Execute(name)
         ¡ý
MainViewModel.ChangeTheme(name)
         ¡ý
ThemeService.ApplyTheme(name)
         ¡ý
ThemeService removes old theme dictionaries
         ¡ý
ThemeService.LoadResourceDictionary(ThemeConstants.GetThemeResourcePath(name))
         ¡ý
All {DynamicResource} bindings auto-refresh
         ¡ý
MainViewModel raises PropertyChanged(CurrentTheme)
         ¡ý
UI updates with new theme colors/styles
```

### 3.3 Navigation Width Calculation (v1.3)
```
Application Loads / Language Changes
         ¡ý
MainWindow.CalculateOptimalNavWidth()
         ¡ý
Retrieve localized strings:
  - Nav_Overview, Nav_Analytics, Nav_Reports, Nav_Settings, Nav_Collapse
         ¡ý
For each string:
  FormattedText(text, culture, Typeface("Segoe UI"), 14pt)
         ¡ý
Measure width: icon(16px) + spacing(12px) + text_width
         ¡ý
Find maximum content width
         ¡ý
Calculate: margin + padding + max_content + padding + margin + 6px buffer
         ¡ý
Clamp to [logicalMin, MaxExpandedWidth]
         ¡ý
Set _expandedNavWidth, NavColumn.MaxWidth
         ¡ý
If expanded: Animate to new width
```

## 4. Configuration & Constants Strategy

**Problem Solved**: Eliminate magic strings/numbers scattered across codebase

**Approach**: Five constants classes group related values
- Navigation behavior ¡ú `NavigationConstants`
- Theme metadata ¡ú `ThemeConstants`
- Language metadata ¡ú `LanguageConstants`
- UI icons ¡ú `IconConstants`
- Page identifiers ¡ú `PageConstants`

**Benefits**:
1. Single source of truth for configuration
2. Easy to extend (add new theme/language/page)
3. Compile-time safety (typos caught immediately)
4. IDE autocomplete support

## 5. Extension Points

### Adding a New Page
1. Define constant in `PageConstants` (e.g., `public const string Dashboard = "Dashboard"`)
2. Add localization keys in `Strings.resx` (e.g., `Nav_Dashboard`, `Status_Page_Dashboard`)
3. Add computed property in `MainViewModel` (e.g., `public bool IsDashboardPage => CurrentPage == PageConstants.Dashboard`)
4. Add Button in `MainWindow.xaml` with `Command="{Binding NavigateCommand}" CommandParameter="Dashboard"`
5. Add Grid with visibility binding `Visibility="{Binding IsDashboardPage, Converter={StaticResource BoolToVisibilityConverter}}"`

### Adding a New Language
1. Create `Strings.{code}.resx` file (e.g., `Strings.fr-FR.resx`)
2. Translate all keys
3. Update `LanguageConstants`:
   - Add `public const string French = "fr-FR"` in CultureCodes
   - Add `public const string French = "Fran?ais"` in DisplayNames
   - Update `GetDisplayName()` switch, `GetAllCultureCodes()` array
4. Update `ServiceCollectionExtensions` AvailableCultures array
5. Add ToggleButton in Settings page

### Adding a New Theme
1. Create `Themes/MyTheme.xaml` with all ResourceKeys from `ThemeConstants.ResourceKeys`
2. Update `ThemeConstants`:
   - Add `public const string MyTheme = "MyTheme"` in ThemeNames
   - Add `public const string MyTheme = "Themes/MyTheme.xaml"` in ThemeResourcePaths
   - Update `GetThemeResourcePath()` switch, `GetAllThemes()` array
3. Add localization key `Status_Theme_MyTheme` in all `Strings.*.resx`
4. Update `ThemeService.GetThemeDisplayName()` switch
5. Add ToggleButton in Settings page

### Adding New Icons
1. Find Segoe MDL2 code from [Microsoft docs](https://learn.microsoft.com/windows/apps/design/style/segoe-ui-symbol-font)
2. Add to appropriate `IconConstants` category (e.g., `public const string MyIcon = "\uE123";`)
3. Use in XAML: `<TextBlock Text="{x:Static icons:IconConstants+Common.MyIcon}" FontFamily="{x:Static icons:IconConstants.DefaultFontFamily}"/>`

## 6. Error Handling & Resilience

| Component | Error Scenario | Fallback Behavior |
|-----------|---------------|-------------------|
| LocalizationService | Missing translation key | Returns key string, logs debug message |
| LocalizationService | Invalid culture code | Falls back to DefaultCulture (zh-CN) |
| ThemeService | Theme file not found | Falls back to Light theme |
| ThemeService | Unknown theme name | Uses Light theme |
| MainWindow | Width calculation fails | Uses DefaultExpandedWidth (260px) |
| MainWindow | Navigation drag errors | Releases mouse capture, no crash |

**Logging**: Currently uses `System.Diagnostics.Debug.WriteLine()`
**Future**: Migrate to `ILogger` abstraction (v1.4 roadmap)

## 7. Testing Strategy

### Unit Test Targets
- `PageConstants.GetStatusKey()`: Validate all page mappings
- `LanguageConstants.GetDisplayName()`: Check all culture codes
- `ThemeConstants.GetThemeResourcePath()`: Verify path construction
- `LocalizationService.ChangeLanguage()`: Test culture switching, fallback
- `ThemeService.ApplyTheme()`: Test known/unknown themes, error recovery
- `MainViewModel` commands: Ensure state changes raise PropertyChanged

### Integration Test Targets
- Navigation width recalculation on language change
- Theme switching updates all DynamicResource bindings
- Navigation expand/collapse animation completion
- Drag-to-resize clamping behavior

### UI Test Scenarios
- Double-click navigation panel toggles expand/collapse
- Dragging right edge resizes within bounds
- Language buttons update all localized text
- Theme buttons change colors immediately

## 8. Performance Considerations

### Optimizations Applied
1. **Singleton Services**: LocalizationService, ThemeService instantiated once
2. **Transient ViewModels**: MainViewModel created per window (light object)
3. **Cached Measurements**: FormattedText reused per culture, not per render
4. **Eased Animations**: CubicEase reduces layout thrashing vs. linear
5. **OneWay Bindings**: Read-only properties use OneWay mode

### Bottleneck Prevention
- Avoid `Application.Current` lookups in tight loops
- Dispatcher.BeginInvoke() with appropriate priority for language refresh
- Theme dictionaries loaded once, cached by WPF ResourceDictionary system

## 9. Dependency Graph

```
App.xaml.cs
    ©¸©¤> ServiceCollectionExtensions
            ©¸©¤> LocalizationService (depends on LocalizationOptions)
            ©¸©¤> ThemeService (depends on ILocalizationService)
            ©¸©¤> MainViewModel (depends on both services)
            ©¸©¤> MainWindow (depends on MainViewModel, ILocalizationService)

MainViewModel
    ©¸©¤> ILocalizationService (for culture operations)
    ©¸©¤> IThemeService (for theme operations)
    ©¸©¤> PageConstants, ThemeConstants, LanguageConstants (for identifiers)

MainWindow.xaml.cs
    ©¸©¤> MainViewModel (via DataContext)
    ©¸©¤> ILocalizationService (for width calculation)
    ©¸©¤> NavigationConstants (for animation/sizing)
    ©¸©¤> IconConstants (for UI icons)

Services
    ©¸©¤> Constants (no circular dependencies)
    ©¸©¤> Options (configuration POCOs)
```

**Key Principle**: Services depend on Constants, never the reverse. Views depend on ViewModels and Services, but Services never reference Views.

## 10. Future Roadmap

### v1.4 Planned Improvements
1. **Logging Abstraction**: Replace `Debug.WriteLine` with `ILogger<T>`
2. **Settings Persistence**: Save theme/language/nav width to JSON file
3. **Data-Driven Navigation**: Replace hardcoded buttons with `ObservableCollection<PageDefinition>`
4. **Command CanExecute**: Conditional navigation based on app state

### v2.0 Vision
1. **Plugin System**: Load themes/pages/languages from external assemblies
2. **Hot Reload**: Update themes without restarting application
3. **Telemetry**: Usage analytics (theme popularity, language distribution)
4. **Accessibility**: Narrator support, keyboard navigation improvements

---
**Document Version**: 1.3  
**Last Updated**: 2024  
**Maintained by**: TechDashboard Team
