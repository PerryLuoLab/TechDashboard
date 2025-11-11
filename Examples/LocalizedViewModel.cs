using System;
using System.Windows.Input;
using TechDashboard.Infrastructure;
using TechDashboard.Helpers;

namespace TechDashboard.ViewModels
{
    /// <summary>
    /// Example ViewModel showing how to use WPFLocalizeExtension
    /// This can replace or complement the existing MainViewModel
    /// </summary>
    public class LocalizedViewModel : ObservableObject
    {
        private string _currentLanguage = "en-US";
        private string _userName = "User";

        public LocalizedViewModel()
        {
            ChangeLanguageCommand = new RelayCommand(param => ChangeLanguage(param?.ToString()));
        }

        #region Properties

        /// <summary>
        /// Current language code
        /// </summary>
        public string CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                SetProperty(ref _currentLanguage, value);
                // When language changes, notify all localized properties
                RaisePropertyChanged(nameof(CurrentLanguageDisplay));
                RaisePropertyChanged(nameof(WelcomeMessage));
                RaisePropertyChanged(nameof(DashboardTitle));
                RaisePropertyChanged(nameof(OverviewText));
            }
        }

        /// <summary>
        /// Display name of current language (localized)
        /// </summary>
        public string CurrentLanguageDisplay => LocalizationHelper.GetLanguageDisplayName(CurrentLanguage);

        /// <summary>
        /// User name for demonstration
        /// </summary>
        public string UserName
        {
            get => _userName;
            set
            {
                SetProperty(ref _userName, value);
                RaisePropertyChanged(nameof(WelcomeMessage));
            }
        }

        /// <summary>
        /// Example: Simple localized string
        /// </summary>
        public string DashboardTitle => LocalizationHelper.GetString("Nav_Dashboard");

        /// <summary>
        /// Example: Another localized string
        /// </summary>
        public string OverviewText => LocalizationHelper.GetString("Nav_Overview");

        /// <summary>
        /// Example: Formatted localized string
        /// Note: You would need to add "Welcome_Message" = "Welcome, {0}!" to your resources
        /// </summary>
        public string WelcomeMessage => LocalizationHelper.GetFormattedString("Welcome_Message", UserName);

        #endregion

        #region Commands

        public ICommand ChangeLanguageCommand { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Changes the application language
        /// </summary>
        private void ChangeLanguage(string? languageCode)
        {
            if (string.IsNullOrEmpty(languageCode))
                return;

            try
            {
                // Method 1: Using LocalizationHelper
                LocalizationHelper.ChangeLanguage(languageCode);

                // Method 2: Using App.ApplyLanguage (if you keep both systems)
                // App.ApplyLanguage(languageCode);

                // Update current language property
                CurrentLanguage = languageCode;

                System.Diagnostics.Debug.WriteLine($"Language changed to: {languageCode}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error changing language: {ex.Message}");
            }
        }

        #endregion
    }
}

/*
 * USAGE IN XAML:
 * 
 * 1. Set DataContext:
 * <Window.DataContext>
 *     <vm:LocalizedViewModel />
 * </Window.DataContext>
 * 
 * 2. Bind to localized properties:
 * <TextBlock Text="{Binding DashboardTitle}" />
 * <TextBlock Text="{Binding WelcomeMessage}" />
 * 
 * 3. Or use lex:Loc directly (recommended):
 * <TextBlock Text="{lex:Loc Nav_Dashboard}" />
 * 
 * 4. Language selector:
 * <ComboBox SelectedValue="{Binding CurrentLanguage}" 
 *           DisplayMemberPath="Name">
 *     <ComboBoxItem Name="en-US" Content="English" />
 *     <ComboBoxItem Name="zh-CN" Content="¼òÌåÖÐÎÄ" />
 *     ...
 * </ComboBox>
 * 
 * BENEFITS:
 * - Localized strings automatically update when culture changes
 * - No need to manually RaisePropertyChanged for each localized property
 * - WPFLocalizeExtension handles everything automatically
 * - Can still access localized strings in code-behind when needed
 */
