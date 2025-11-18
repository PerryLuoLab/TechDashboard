using Microsoft.Extensions.DependencyInjection;
using TechDashboard.Options;
using TechDashboard.Services;
using TechDashboard.Services.Interfaces;
using TechDashboard.ViewModels;

namespace TechDashboard
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.Configure<LocalizationOptions>(options =>
            {
                options.AssemblyName = "TechDashboard";
                options.DictionaryName = "Strings";
                options.DefaultCulture = "zh-CN";
                options.AvailableCultures = new[] { "en-US", "zh-CN", "zh-TW", "ko-KR", "ja-JP" };
            });

            services.AddSingleton<ILocalizationService, LocalizationService>();
            services.AddSingleton<IThemeService, ThemeService>();

            services.AddTransient<MainViewModel>();

            // [Fix 5] Register MainWindow for DI
            services.AddSingleton<MainWindow>();

            return services;
        }
    }
}