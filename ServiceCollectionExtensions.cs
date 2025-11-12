using Microsoft.Extensions.DependencyInjection;
using TechDashboard.Options;
using TechDashboard.Services;
using TechDashboard.Services.Interfaces;
using TechDashboard.ViewModels;

namespace TechDashboard
{
    /// <summary>
    /// Service collection extensions for dependency injection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds all application services to the service collection
        /// </summary>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Configuration Options
            services.Configure<LocalizationOptions>(options =>
            {
                options.AssemblyName = "TechDashboard";
                options.DictionaryName = "Strings";
                options.DefaultCulture = "zh-CN";   //设置默认语言为简体中文   
                options.AvailableCultures = new[]
                {
                    "en-US", // English
                    "zh-CN", // Simplified Chinese
                    "zh-TW", // Traditional Chinese
                    "ko-KR", // Korean
                    "ja-JP"  // Japanese
                };
            });

            // Core Services (Singleton - 单例模式)
            services.AddSingleton<ILocalizationService, LocalizationService>();
            services.AddSingleton<IThemeService, ThemeService>();

            // ViewModels (Transient - 瞬态模式，每次请求创建新实例)
            services.AddTransient<MainViewModel>();

            return services;
        }
    }
}
