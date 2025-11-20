using Microsoft.Extensions.DependencyInjection;
using TechDashboard.Options;
using TechDashboard.Services;
using TechDashboard.Services.Interfaces;
using TechDashboard.ViewModels;
using TechDashboard.Core.Constants;
using Serilog;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace TechDashboard.Core.Extensions
{
    /// <summary>
    /// Extension methods for configuring application services in the dependency injection container.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds all application services to the specified <see cref="IServiceCollection"/>.
        /// This includes localization, theme management, and view models.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        /// <returns>The service collection for method chaining.</returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            // Configure Serilog from configuration
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddSerilog(Log.Logger, dispose: true);
            });

            // Configure localization options
            services.Configure<LocalizationOptions>(options =>
            {
                options.AssemblyName = "TechDashboard";
                options.DictionaryName = "Strings";
                options.DefaultCulture = configuration["Localization:DefaultCulture"] ?? LanguageConstants.DefaultLanguage;
                options.AvailableCultures = LanguageConstants.GetAllCultureCodes();
            });

            // Register services
            services.AddSingleton<ILocalizationService, LocalizationService>();
            services.AddSingleton<IThemeService, ThemeService>();
            services.AddSingleton<INavLayoutService, NavLayoutService>();

            // View models as singletons to share the same instance across window and app
            services.AddSingleton<MainViewModel>();

            services.AddSingleton<MainWindow>();
            services.AddSingleton<Core.Services.UserPreferencesService>();
            services.AddTransient<ViewModels.IconPickerViewModel>();
            services.AddTransient<Views.IconPickerDialog>();
            services.AddSingleton<TechDashboard.Services.Interfaces.INavLayoutService, TechDashboard.Services.NavLayoutService>();

            return services;
        }
    }
}
