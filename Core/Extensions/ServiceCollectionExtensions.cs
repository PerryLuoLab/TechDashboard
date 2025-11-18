using Microsoft.Extensions.DependencyInjection;
using TechDashboard.Options;
using TechDashboard.Services;
using TechDashboard.Services.Interfaces;
using TechDashboard.ViewModels;

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
            // Configure localization options
            services.Configure<LocalizationOptions>(options =>
            {
                options.AssemblyName = "TechDashboard";
                options.DictionaryName = "Strings";
                options.DefaultCulture = "zh-CN";
                options.AvailableCultures = new[] { "en-US", "zh-CN", "zh-TW", "ko-KR", "ja-JP" };
            });

            // Register services
            services.AddSingleton<ILocalizationService, LocalizationService>();
            services.AddSingleton<IThemeService, ThemeService>();

            // Register view models
            services.AddTransient<MainViewModel>();

            // Register views
            services.AddSingleton<MainWindow>();

            return services;
        }
    }
}
