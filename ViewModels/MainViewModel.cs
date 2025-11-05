using System.Windows.Input;
using TechDashboard.Commands;
using TechDashboard.Infrastructure;

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
        }

        #region Properties

        public bool IsNavExpanded
        {
            get => _isNavExpanded;
            set => SetProperty(ref _isNavExpanded, value, action: () =>
            {
                // 当展开/折叠状态改变时，更新导航宽度
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
            set => SetProperty(ref _currentTheme, value);
        }

        public string CurrentLanguage
        {
            get => _currentLanguage;
            set => SetProperty(ref _currentLanguage, value);
        }

        public string CurrentPage
        {
            get => _currentPage;
            set => SetProperty(ref _currentPage, value);
        }

        public bool IsOverviewPage => CurrentPage == "Overview";
        public bool IsAnalyticsPage => CurrentPage == "Analytics";
        public bool IsReportsPage => CurrentPage == "Reports";
        public bool IsSettingsPage => CurrentPage == "Settings";

        // 获取显示用的语言名称
        public string CurrentLanguageDisplay
        {
            get
            {
                return CurrentLanguage switch
                {
                    "en-US" => "English",
                    "zh-CN" => "简体中文",
                    "ko-KR" => "한국어",
                    _ => "English"
                };
            }
        }

        #endregion

        #region Commands

        public ICommand ToggleNavCommand { get; }
        public ICommand NavigateCommand { get; }
        public ICommand ChangeThemeCommand { get; }
        public ICommand ChangeLanguageCommand { get; }

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
                RaisePropertyChanged(nameof(IsOverviewPage));
                RaisePropertyChanged(nameof(IsAnalyticsPage));
                RaisePropertyChanged(nameof(IsReportsPage));
                RaisePropertyChanged(nameof(IsSettingsPage));
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
                App.ApplyLanguage(languageCode);
                RaisePropertyChanged(nameof(CurrentLanguageDisplay));
            }
        }

        #endregion
    }
}