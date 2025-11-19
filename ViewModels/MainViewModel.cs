using System.Windows.Input;
using TechDashboard.Core.Infrastructure;  // ? 更新引用
using TechDashboard.Services.Interfaces;

namespace TechDashboard.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly ILocalizationService _localizationService;
        private readonly IThemeService _themeService;

        private bool _isNavExpanded = true;
        private string _currentTheme = "Light";
        private string _currentLanguage = "en-US";
        private string _currentPage = "Overview";
        private double _navWidth = 260;

        public MainViewModel(ILocalizationService localizationService, IThemeService themeService)
        {
            _localizationService = localizationService ?? throw new System.ArgumentNullException(nameof(localizationService));
            _themeService = themeService ?? throw new System.ArgumentNullException(nameof(themeService));

            // Initialize with current culture
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
            set => SetProperty(ref _isNavExpanded, value, action: () =>
            {
                // [Fix 4] Removed hardcoded width logic. 
                // The View (MainWindow.xaml.cs) drives the animation and updates NavWidth.
            });
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

        public bool IsOverviewPage => CurrentPage == "Overview";
        public bool IsAnalyticsPage => CurrentPage == "Analytics";
        public bool IsReportsPage => CurrentPage == "Reports";
        public bool IsSettingsPage => CurrentPage == "Settings";

        public string CurrentLanguageDisplay => _localizationService.GetLanguageDisplayName(CurrentLanguage);

        public string CurrentThemeDisplay => _themeService.GetThemeDisplayName(CurrentTheme);

        public string CurrentPageDisplay
        {
            get
            {
                var pageKey = CurrentPage switch
                {
                    "Overview" => "Status_Page_Overview",
                    "Analytics" => "Status_Page_Analytics",
                    "Reports" => "Status_Page_Reports",
                    "Settings" => "Status_Page_Settings",
                    _ => "Status_Page_Overview"
                };

                return _localizationService.GetString(pageKey);
            }
        }

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

        private void ToggleNavigation()
        {
            IsNavExpanded = !IsNavExpanded;
        }

        private void NavigateToPage(string? pageName)
        {
            if (!string.IsNullOrEmpty(pageName))
            {
                CurrentPage = pageName;
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

                // Force UI update
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
            var nextTheme = CurrentTheme switch
            {
                "Dark" => "Light",
                "Light" => "LightBlue",
                "LightBlue" => "Dark",
                _ => "Dark"
            };

            ChangeTheme(nextTheme);
        }

        private void QuickToggleLanguage()
        {
            var nextLanguage = CurrentLanguage switch
            {
                "en-US" => "zh-CN",
                "zh-CN" => "zh-TW",
                "zh-TW" => "ko-KR",
                "ko-KR" => "ja-JP",
                "ja-JP" => "en-US",
                _ => "en-US"
            };

            ChangeLanguage(nextLanguage);
        }

        #endregion
    }
}