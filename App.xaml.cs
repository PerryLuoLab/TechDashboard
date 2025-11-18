using System;
using System.Globalization;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using TechDashboard.Services.Interfaces;

namespace TechDashboard
{
    public partial class App : Application
    {
        private IServiceProvider? _serviceProvider;

        /// <summary>
        /// Gets the service provider for dependency injection
        /// </summary>
        public static IServiceProvider Services { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Configure services
            var services = new ServiceCollection();
            services.AddApplicationServices();

            // Build service provider
            _serviceProvider = services.BuildServiceProvider();
            Services = _serviceProvider;

            // Initialize localization service (will set default culture)
            var localizationService = _serviceProvider.GetRequiredService<ILocalizationService>();

            System.Diagnostics.Debug.WriteLine("? Application services initialized");
            System.Diagnostics.Debug.WriteLine($"  Localization: {localizationService.CurrentCulture.Name}");

            // Create and show MainWindow using DI
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Dispose service provider
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }

            base.OnExit(e);
        }

        /// <summary>
        /// Applies a theme to the application
        /// </summary>
        /// <param name="themeName">Theme name</param>
        [Obsolete("Use IThemeService instead")]
        public static void ApplyTheme(string themeName)
        {
            var themeService = Services?.GetService<IThemeService>();
            themeService?.ApplyTheme(themeName);
        }

        /// <summary>
        /// Changes the application language
        /// </summary>
        /// <param name="languageCode">Language code</param>
        [Obsolete("Use ILocalizationService instead")]
        public static void ApplyLanguage(string languageCode)
        {
            if (Services == null) return;

            try
            {
                var localizationService = Services.GetRequiredService<ILocalizationService>();
                localizationService.ChangeLanguage(languageCode);

                var newCulture = new CultureInfo(languageCode);

                // Force UI update
                Current.Dispatcher.Invoke(() =>
                {
                    foreach (Window window in Current.Windows)
                    {
                        window.Language = System.Windows.Markup.XmlLanguage.GetLanguage(newCulture.IetfLanguageTag);
                    }
                });

                System.Diagnostics.Debug.WriteLine($"? Language changed to: {languageCode}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"? Error changing language: {ex.Message}");
            }
        }
    }
}