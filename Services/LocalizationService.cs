using System;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Options;
using TechDashboard.Options;
using TechDashboard.Services.Interfaces;
using WPFLocalizeExtension.Engine;
using TechDashboard.Core.Constants;

namespace TechDashboard.Services
{
    /// <summary>
    /// Implementation of localization service using WPFLocalizeExtension
    /// </summary>
    public class LocalizationService : ILocalizationService
    {
        private readonly LocalizationOptions _options;

        public LocalizationService(IOptions<LocalizationOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            
            // Initialize LocalizeDictionary with default culture
            LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
            LocalizeDictionary.Instance.Culture = new CultureInfo(_options.DefaultCulture);
        }

        /// <inheritdoc/>
        public CultureInfo CurrentCulture
        {
            get => LocalizeDictionary.Instance.Culture;
            set
            {
                if (value != null)
                {
                    LocalizeDictionary.Instance.Culture = value;
                }
            }
        }

        /// <inheritdoc/>
        public string GetString(string key)
        {
            try
            {
                var localizedObject = LocalizeDictionary.Instance.GetLocalizedObject(
                    _options.AssemblyName,
                    _options.DictionaryName,
                    key,
                    LocalizeDictionary.Instance.Culture
                );

                return localizedObject?.ToString() ?? key;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting localized string for key '{key}': {ex.Message}");
                return key;
            }
        }

        /// <inheritdoc/>
        public string GetFormattedString(string key, params object[] args)
        {
            try
            {
                var template = GetString(key);
                return string.Format(template, args);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error formatting localized string for key '{key}': {ex.Message}");
                return key;
            }
        }

        /// <inheritdoc/>
        public void ChangeLanguage(string cultureCode)
        {
            try
            {
                var culture = new CultureInfo(cultureCode);
                
                // Validate that the culture is supported
                if (!_options.AvailableCultures.Contains(cultureCode))
                {
                    System.Diagnostics.Debug.WriteLine($"Warning: Culture '{cultureCode}' is not in the list of available cultures.");
                }
                
                LocalizeDictionary.Instance.Culture = culture;
                
                System.Diagnostics.Debug.WriteLine($"Language changed to: {cultureCode}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to change language to {cultureCode}: {ex.Message}");
                
                // Fallback to default culture
                LocalizeDictionary.Instance.Culture = new CultureInfo(_options.DefaultCulture);
            }
        }

        /// <inheritdoc/>
        public CultureInfo[] GetAvailableCultures()
        {
            return _options.AvailableCultures
                .Select(code =>
                {
                    try
                    {
                        return new CultureInfo(code);
                    }
                    catch
                    {
                        System.Diagnostics.Debug.WriteLine($"Warning: Invalid culture code '{code}'");
                        return null;
                    }
                })
                .Where(c => c != null)
                .ToArray()!;
        }

        /// <inheritdoc/>
        public string GetLanguageDisplayName(string cultureCode)
        {
            // Delegate to centralized constants to avoid duplicated hard-coded values
            return LanguageConstants.GetDisplayName(cultureCode);
        }
    }
}
