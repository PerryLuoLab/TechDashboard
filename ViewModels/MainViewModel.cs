using System.Windows.Input;
using TechDashboard.Commands;
using TechDashboard.Infrastructure;

namespace TechDashboard.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private bool _isNavExpanded = true;
        private string _currentTheme = "Dark";
        private string _currentPage = "Overview";

        public MainViewModel()
        {
            ToggleNavCommand = new RelayCommand(_ => ToggleNavigation());
            NavigateCommand = new RelayCommand(param => NavigateToPage(param?.ToString()));
            ChangeThemeCommand = new RelayCommand(param => ChangeTheme(param?.ToString()));
        }

        #region Properties

        public bool IsNavExpanded
        {
            get => _isNavExpanded;
            set => SetProperty(ref _isNavExpanded, value);
        }

        public string CurrentTheme
        {
            get => _currentTheme;
            set => SetProperty(ref _currentTheme, value);
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

        #endregion

        #region Commands

        public ICommand ToggleNavCommand { get; }
        public ICommand NavigateCommand { get; }
        public ICommand ChangeThemeCommand { get; }

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

        #endregion
    }
}