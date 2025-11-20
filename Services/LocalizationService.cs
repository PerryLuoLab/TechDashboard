using System;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Options;
using TechDashboard.Options;
using TechDashboard.Services.Interfaces;
using WPFLocalizeExtension.Engine;
using TechDashboard.Core.Constants;
using Microsoft.Extensions.Logging;

namespace TechDashboard.Services
{
    /// <summary>
    /// Implementation of localization service using WPFLocalizeExtension
    /// </summary>
    public class LocalizationService : ILocalizationService
    {
        private readonly LocalizationOptions _options;
        private readonly ILogger<LocalizationService> _logger;

        public LocalizationService(IOptions<LocalizationOptions> options, ILogger<LocalizationService> logger)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // Initialize LocalizeDictionary with default culture
            LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
            LocalizeDictionary.Instance.Culture = new CultureInfo(_options.DefaultCulture);
            _logger.LogInformation("Localization initialized with culture {Culture}", _options.DefaultCulture);
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
                    _logger.LogDebug("Current culture set to {Culture}", value.Name);
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
                _logger.LogError(ex, "Error retrieving localized string for key {Key}", key);
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
                _logger.LogError(ex, "Error formatting localized string for key {Key}", key);
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
                    _logger.LogWarning("Culture {Culture} not in available list", cultureCode);
                }
                LocalizeDictionary.Instance.Culture = culture;
                _logger.LogInformation("Language changed to {CultureCode}", cultureCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to change language to {CultureCode}, applying fallback {Fallback}", cultureCode, _options.DefaultCulture);
                
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
                    try { return new CultureInfo(code); }
                    catch
                    {
                        _logger.LogWarning("Invalid culture code {Code}", code);
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
