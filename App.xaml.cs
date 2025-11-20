using System;
using System.Globalization;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using TechDashboard.Core.Extensions;
using TechDashboard.Services.Interfaces;
using TechDashboard.Core.Constants; // added for ThemeConstants

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
            var themeService = _serviceProvider.GetRequiredService<IThemeService>();

            // Apply default theme from single source of truth (ThemeConstants.DefaultTheme)
            themeService.ApplyTheme(ThemeConstants.DefaultTheme);

            System.Diagnostics.Debug.WriteLine("Application services initialized");
            System.Diagnostics.Debug.WriteLine($"Localization: {localizationService.CurrentCulture.Name}");
            System.Diagnostics.Debug.WriteLine($"Theme: {themeService.CurrentTheme}");

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
    }
}