using System;
using System.Globalization;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using TechDashboard.Core.Extensions;
using TechDashboard.Services.Interfaces;
using TechDashboard.Core.Constants;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

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

            var logger = _serviceProvider.GetRequiredService<ILogger<App>>();
            AppDomain.CurrentDomain.UnhandledException += (s, exArgs) =>
            {
                logger.LogCritical(exArgs.ExceptionObject as Exception, "Unhandled domain exception");
            };
            DispatcherUnhandledException += (s, exArgs) =>
            {
                logger.LogError(exArgs.Exception, "Dispatcher unhandled exception");
                exArgs.Handled = true; // prevent crash, optionally show dialog
            };
            TaskScheduler.UnobservedTaskException += (s, exArgs) =>
            {
                logger.LogError(exArgs.Exception, "Unobserved task exception");
                exArgs.SetObserved();
            };

            var localizationService = _serviceProvider.GetRequiredService<ILocalizationService>();
            var themeService = _serviceProvider.GetRequiredService<IThemeService>();

            // Apply default theme from single source of truth (ThemeConstants.DefaultTheme)
            themeService.ApplyTheme(ThemeConstants.DefaultTheme);

            logger.LogInformation("Application services initialized. Culture={Culture} Theme={Theme}", localizationService.CurrentCulture.Name, themeService.CurrentTheme);

            // Create and show MainWindow using DI
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();

            var prefsService = _serviceProvider.GetRequiredService<TechDashboard.Core.Services.UserPreferencesService>();
            var prefs = prefsService.Load();
            themeService.ApplyTheme(prefs.Theme);
            localizationService.ChangeLanguage(prefs.Language);
            var vm = _serviceProvider.GetRequiredService<ViewModels.MainViewModel>();
            vm.IsNavExpanded = prefs.IsNavExpanded;
            logger.LogInformation("Loaded user preferences Theme={Theme} Language={Language} IsNavExpanded={IsNav}", prefs.Theme, prefs.Language, prefs.IsNavExpanded);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (_serviceProvider is not null)
            {
                var prefsService = _serviceProvider.GetService<TechDashboard.Core.Services.UserPreferencesService>();
                var localizationService = _serviceProvider.GetService<ILocalizationService>();
                var themeService = _serviceProvider.GetService<IThemeService>();
                var vm = _serviceProvider.GetService<ViewModels.MainViewModel>();

                if (prefsService != null && localizationService != null && themeService != null && vm != null)
                {
                    var prefs = new TechDashboard.Core.Services.UserPreferences
                    {
                        Theme = themeService.CurrentTheme,
                        Language = localizationService.CurrentCulture.Name,
                        IsNavExpanded = vm.IsNavExpanded
                    };
                    // Use synchronous Save to avoid deadlocks during shutdown
                    prefsService.Save(prefs);
                }
            }
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
            base.OnExit(e);
        }
    }
}