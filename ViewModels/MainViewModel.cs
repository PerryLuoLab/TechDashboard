using TechDashboard.Commands;
using TechDashboard.Infrastructure;
using System.Windows.Input;

namespace TechDashboard.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private bool _isNavExpanded = true;
        private string _currentTheme = "Dark";

        public MainViewModel()
        {
            ToggleNavCommand = new RelayCommand(_ => IsNavExpanded = !IsNavExpanded);
            ToggleThemeCommand = new RelayCommand(_ =>
            {
                CurrentTheme = CurrentTheme == "Dark" ? "Light" : "Dark";
                App.ApplyTheme(CurrentTheme);
            });
        }

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

        public ICommand ToggleNavCommand { get; }
        public ICommand ToggleThemeCommand { get; }
    }
}
