# TechDashboard Architecture Overview

## 1. High-Level Layers

- Presentation (WPF Views): MainWindow.xaml and resource dictionaries (Themes)
- ViewModel Layer: State + Commands (MainViewModel)
- Services Layer: Cross-cutting concerns (LocalizationService, ThemeService)
- Core Layer: Reusable building blocks (Infrastructure, Constants, Converters, Extensions)
- Resources: Localization .resx dictionaries

## 2. Module Responsibilities

### Core/Infrastructure
Provides base abstractions for MVVM:
- ObservableObject: Implements INotifyPropertyChanged with helper SetProperty
- RelayCommand: ICommand implementation for binding UI actions
- GridLengthAnimation: Custom AnimationTimeline for smooth column width transitions

### Core/Constants
Centralized immutable configuration:
- NavigationConstants: Numbers controlling navigation panel behaviour
- ThemeConstants: Theme identifiers, resource paths, key names
- LanguageConstants: Language codes, display names, validation helpers
- IconConstants: Segoe MDL2 icon glyphs grouped semantically
- PageConstants: (New) Logical page names + mapping to localization keys

### Core/Converters
Bridge view bindings to model transformations:
- ThemeConverter, LanguageConverter: Map selected theme/language to bool for ToggleButton IsChecked
- BoolToVisibilityConverter: Show/hide elements based on boolean
- IconConverter: (If present) adapt icon usage in XAML

### Core/Extensions
- ServiceCollectionExtensions: Dependency injection registration helpers for services

### Services
Encapsulate mutable runtime behaviours:
- LocalizationService: Wraps WPFLocalizeExtension, culture switching, string retrieval, formatting
- ThemeService: ResourceDictionary swapping, theme validation via ThemeConstants

### ViewModels
- MainViewModel: Aggregates UI state (navigation, theme, language, current page); exposes commands. Pure logic (no UI types) except minimal dispatcher invocation for language refresh.

### Presentation (Views + Themes)
- MainWindow.xaml(.cs): UI layout + event glue (drag navigation width, animation). Calls CalculateOptimalNavWidth then animates width.
- Theme dictionaries: Provide ResourceDictionary assets keyed by ThemeConstants.ResourceKeys.

## 3. Data Flow

User interaction -> Command (RelayCommand) -> ViewModel state change -> PropertyChanged -> View updates via bindings.

Language change:
1. MainViewModel.ChangeLanguage -> LocalizationService.ChangeLanguage (culture switch)
2. View listens to PropertyChanged(CurrentLanguage) -> recalculates nav width -> updates column GridLength via animation

Theme change:
1. MainViewModel.ChangeTheme -> ThemeService.ApplyTheme -> replaces merged dictionaries
2. DynamicResource brushes update automatically; ViewModel raises CurrentThemeDisplay

Navigation expand/collapse:
1. ToggleNavCommand flips IsNavExpanded
2. View reacts to PropertyChanged(IsNavExpanded) -> AnimateNavWidth -> update NavWidth

## 4. Navigation Width Logic (Updated)
- Longest localized navigation label measured using FormattedText
- Width = margins + paddings + icon + spacing + longest text + 6px buffer (ExpansionExtraBuffer)
- Recalculated on load and when language changes.

## 5. Constants Strategy
All magic numbers / strings promoted to constants classes to ease maintenance and future extension (themes, pages, languages, navigation behaviour). ViewModel and Services rely only on constants rather than raw literals.

## 6. Extensibility Guidelines
- Add new page: Define constant in PageConstants, add localization key, update XAML bindings.
- Add new language: Create .resx file + add culture code/display name in LanguageConstants + register in options.
- Add new theme: New theme XAML + ThemeConstants additions + update selection UI.
- Add new icon: Extend IconConstants categories without touching UI code.

## 7. Error Handling & Fallbacks
- LocalizationService: Returns key if missing, logs debug message.
- ThemeService: Falls back to default theme if loading fails.
- Navigation width: Falls back to DefaultExpandedWidth when measurement errors occur.

## 8. Dependency Injection
- Services registered via ServiceCollectionExtensions (not shown here) for ILocalizationService and IThemeService.
- ViewModel constructed with service dependencies enabling testability (can inject mocks).

## 9. Testing Recommendations
- Unit test PageConstants.GetStatusKey mappings.
- Unit test LocalizationService with mock options to ensure culture switching works.
- Unit test ThemeService applying known/unknown themes.
- UI tests for nav width adaptation across languages.

## 10. Future Improvement Ideas
- Extract navigation items into a collection (e.g., ObservableCollection<PageDefinition>) instead of hard-coded buttons.
- Persist user preferences (theme, language, nav width) to local settings file.
- Implement ICommand CanExecute logic for navigation depending on feature availability.
- Add logging abstraction (ILogger) instead of Debug.WriteLine.

---
Generated architecture summary.
