using System.Windows.Input;
using TechDashboard.Core.Infrastructure;
using TechDashboard.Services.Interfaces;
using TechDashboard.Core.Constants;

namespace TechDashboard.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly ILocalizationService _localizationService;
        private readonly IThemeService _themeService;

        private bool _isNavExpanded = true;
        private string _currentTheme = ThemeConstants.DefaultTheme;
        private string _currentLanguage = LanguageConstants.DefaultLanguage;
        private string _currentPage = PageConstants.Overview;
        private double _navWidth = NavigationConstants.DefaultExpandedWidth;

        public MainViewModel(ILocalizationService localizationService, IThemeService themeService)
        {
            _localizationService = localizationService ?? throw new System.ArgumentNullException(nameof(localizationService));
            _themeService = themeService ?? throw new System.ArgumentNullException(nameof(themeService));

            // Initialize with current culture & theme from services
            _currentLanguage = _localizationService.CurrentCulture.Name;
            _currentTheme = _themeService.CurrentTheme;

            ToggleNavCommand = new RelayCommand(_ => ToggleNavigation());
            NavigateCommand = new RelayCommand(param => NavigateToPage(param?.ToString()));
            ChangeThemeCommand = new RelayCommand(param => ChangeTheme(param?.ToString()));
            ChangeLanguageCommand = new RelayCommand(param => ChangeLanguage(param?.ToString()));
            QuickToggleThemeCommand = new RelayCommand(_ => QuickToggleTheme());
            QuickToggleLanguageCommand = new RelayCommand(_ => QuickToggleLanguage());
        }

        #region Properties

        public bool IsNavExpanded
        {
            get => _isNavExpanded;
            set => SetProperty(ref _isNavExpanded, value);
        }

        public double NavWidth
        {
            get => _navWidth;
            set => SetProperty(ref _navWidth, value);
        }

        public string CurrentTheme
        {
            get => _currentTheme;
            set => SetProperty(ref _currentTheme, value, action: () =>
            {
                RaisePropertyChanged(nameof(CurrentThemeDisplay));
            });
        }

        public string CurrentLanguage
        {
            get => _currentLanguage;
            set => SetProperty(ref _currentLanguage, value, action: () =>
            {
                RaisePropertyChanged(nameof(CurrentLanguageDisplay));
                RaisePropertyChanged(nameof(CurrentThemeDisplay));
                RaisePropertyChanged(nameof(CurrentPageDisplay));
            });
        }

        public string CurrentPage
        {
            get => _currentPage;
            set => SetProperty(ref _currentPage, value, action: () =>
            {
                RaisePropertyChanged(nameof(IsOverviewPage));
                RaisePropertyChanged(nameof(IsAnalyticsPage));
                RaisePropertyChanged(nameof(IsReportsPage));
                RaisePropertyChanged(nameof(IsSettingsPage));
                RaisePropertyChanged(nameof(CurrentPageDisplay));
            });
        }

        public bool IsOverviewPage => CurrentPage == PageConstants.Overview;
        public bool IsAnalyticsPage => CurrentPage == PageConstants.Analytics;
        public bool IsReportsPage => CurrentPage == PageConstants.Reports;
        public bool IsSettingsPage => CurrentPage == PageConstants.Settings;

        public string CurrentLanguageDisplay => _localizationService.GetLanguageDisplayName(CurrentLanguage);

        public string CurrentThemeDisplay => _themeService.GetThemeDisplayName(CurrentTheme);

        public string CurrentPageDisplay => _localizationService.GetString(PageConstants.GetStatusKey(CurrentPage));

        #endregion

        #region Commands

        public ICommand ToggleNavCommand { get; }
        public ICommand NavigateCommand { get; }
        public ICommand ChangeThemeCommand { get; }
        public ICommand ChangeLanguageCommand { get; }
        public ICommand QuickToggleThemeCommand { get; }
        public ICommand QuickToggleLanguageCommand { get; }

        #endregion

        #region Methods

        private void ToggleNavigation() => IsNavExpanded = !IsNavExpanded;

        private void NavigateToPage(string? pageName)
        {
            if (!string.IsNullOrEmpty(pageName))
            {
                // Accept only known pages
                var known = new[] { PageConstants.Overview, PageConstants.Analytics, PageConstants.Reports, PageConstants.Settings };
                if (System.Array.IndexOf(known, pageName) >= 0)
                {
                    CurrentPage = pageName;
                }
            }
        }

        private void ChangeTheme(string? themeName)
        {
            if (!string.IsNullOrEmpty(themeName))
            {
                CurrentTheme = themeName;
                _themeService.ApplyTheme(themeName);
            }
        }

        private void ChangeLanguage(string? languageCode)
        {
            if (!string.IsNullOrEmpty(languageCode))
            {
                CurrentLanguage = languageCode;
                _localizationService.ChangeLanguage(languageCode);

                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    foreach (System.Windows.Window window in System.Windows.Application.Current.Windows)
                    {
                        window.Language = System.Windows.Markup.XmlLanguage.GetLanguage(languageCode);
                    }
                });
            }
        }

        private void QuickToggleTheme()
        {
            var sequence = new[] { ThemeConstants.ThemeNames.Dark, ThemeConstants.ThemeNames.Light, ThemeConstants.ThemeNames.LightBlue, ThemeConstants.ThemeNames.BlueTech };
            int idx = System.Array.IndexOf(sequence, CurrentTheme);
            var nextTheme = sequence[(idx + 1 + sequence.Length) % sequence.Length];
            ChangeTheme(nextTheme);
        }

        private void QuickToggleLanguage()
        {
            var langs = LanguageConstants.GetAllCultureCodes();
            int idx = System.Array.IndexOf(langs, CurrentLanguage);
            var nextLang = langs[(idx + 1 + langs.Length) % langs.Length];
            ChangeLanguage(nextLang);
        }

        #endregion
    }
}