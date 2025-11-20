using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TechDashboard.Core.Constants;
using Microsoft.Extensions.Logging;

namespace TechDashboard.Core.Services
{
    /// <summary>
    /// Simple user preferences persistence (theme, language, nav expanded) with concurrency protection.
    /// Stored in %AppData%/TechDashboard/userprefs.json
    /// </summary>
    public class UserPreferencesService
    {
        private const string PreferencesFileName = "userprefs.json";
        private const string GlobalMutexName = "Global/TechDashboard_UserPrefs_Mutex";
        private readonly string _prefsPath;
        private readonly object _fileLock = new object();
        private readonly ILogger<UserPreferencesService> _logger;

        public UserPreferencesService(ILogger<UserPreferencesService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var dir = Path.Combine(appData, "TechDashboard");
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            _prefsPath = Path.Combine(dir, PreferencesFileName);
        }

        public UserPreferences Load()
        {
            try
            {
                using var mutex = new Mutex(false, GlobalMutexName);
                mutex.WaitOne(TimeSpan.FromSeconds(2));
                try
                {
                    lock (_fileLock)
                    {
                        if (File.Exists(_prefsPath))
                        {
                            using var fs = new FileStream(_prefsPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                            var prefs = JsonSerializer.Deserialize<UserPreferences>(fs);
                            if (prefs != null)
                            {
                                if (!ThemeConstants.IsValidTheme(prefs.Theme)) prefs.Theme = ThemeConstants.DefaultTheme;
                                if (string.IsNullOrWhiteSpace(prefs.Language)) prefs.Language = LanguageConstants.DefaultLanguage;
                                return prefs;
                            }
                        }
                    }
                }
                finally { try { mutex.ReleaseMutex(); } catch { } }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to load user preferences, using defaults");
            }
            return new UserPreferences { Theme = ThemeConstants.DefaultTheme, Language = LanguageConstants.DefaultLanguage, IsNavExpanded = true };
        }

        public async Task SaveAsync(UserPreferences prefs)
        {
            try
            {
                using var mutex = new Mutex(false, GlobalMutexName);
                mutex.WaitOne(TimeSpan.FromSeconds(2));
                try
                {
                    lock (_fileLock)
                    {
                        var tempPath = _prefsPath + ".tmp";
                        var json = JsonSerializer.Serialize(prefs, new JsonSerializerOptions { WriteIndented = true });
                        File.WriteAllText(tempPath, json);
                        if (File.Exists(_prefsPath)) File.Replace(tempPath, _prefsPath, null);
                        else File.Move(tempPath, _prefsPath);
                    }
                }
                finally { try { mutex.ReleaseMutex(); } catch { } }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save user preferences");
            }
        }
    }

    public class UserPreferences
    {
        public string Theme { get; set; } = ThemeConstants.DefaultTheme;
        public string Language { get; set; } = LanguageConstants.DefaultLanguage;
        public bool IsNavExpanded { get; set; } = true;
    }
}
