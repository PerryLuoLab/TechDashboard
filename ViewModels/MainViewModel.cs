using System.Windows.Input;
using TechDashboard.Infrastructure;
using TechDashboard.Helpers;

namespace TechDashboard.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private bool _isNavExpanded = true;
        private string _currentTheme = "Dark";
        private string _currentLanguage = "en-US";
        private string _currentPage = "Overview";
        private double _navWidth = 260;

        public MainViewModel()
        {
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
                NavWidth = value ? 260 : 60;
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

        public string CurrentLanguageDisplay => LocalizationHelper.GetLanguageDisplayName(CurrentLanguage);

        public string CurrentThemeDisplay
        {
            get
            {
                var themeKey = CurrentTheme switch
                {
                    "Dark" => "Status_Theme_Dark",
                    "Light" => "Status_Theme_Light",
                    "BlueTech" => "Status_Theme_BlueTech",
                    _ => "Status_Theme_Dark"
                };

                return LocalizationHelper.GetString(themeKey);
            }
        }

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

                return LocalizationHelper.GetString(pageKey);
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
                App.ApplyTheme(themeName);
            }
        }

        private void ChangeLanguage(string? languageCode)
        {
            if (!string.IsNullOrEmpty(languageCode))
            {
                CurrentLanguage = languageCode;
                
                // Use WPFLocalizeExtension to change language
                LocalizationHelper.ChangeLanguage(languageCode);
                
                // Also call App.ApplyLanguage which now uses LocalizeDictionary internally
                App.ApplyLanguage(languageCode);
            }
        }

        private void QuickToggleTheme()
        {
            // Cycle through themes: Dark -> Light -> BlueTech -> Dark
            var nextTheme = CurrentTheme switch
            {
                "Dark" => "Light",
                "Light" => "BlueTech",
                "BlueTech" => "Dark",
                _ => "Dark"
            };

            ChangeTheme(nextTheme);
        }

        private void QuickToggleLanguage()
        {
            // Cycle through languages: en-US -> zh-CN -> zh-TW -> ko-KR -> ja-JP -> en-US
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