namespace TechDashboard.Core.Constants
{
    /// <summary>
    /// Theme-related constants and configuration values.
    /// </summary>
    public static class ThemeConstants
    {

        /// <summary>
        /// Default theme to use when no preference is set
        /// </summary>
        public const string DefaultTheme = ThemeNames.BlueTech;


        /// <summary>
        /// Available theme names
        /// </summary>
        public static class ThemeNames
        {
            /// <summary>
            /// Dark theme (Gray-Black tones)
            /// </summary>
            public const string Dark = "Dark";

            /// <summary>
            /// Light theme
            /// </summary>
            public const string Light = "Light";

            /// <summary>
            /// Light Blue theme (浅蓝色主题)
            /// </summary>
            public const string LightBlue = "LightBlue";

            /// <summary>
            /// BlueTech theme - Glassmorphism with Neon Glow (蓝科技主题 - 玻璃形态+霓虹辉光)
            /// </summary>
            public const string BlueTech = "BlueTech";

            /// <summary>
            /// Aurora theme - Cyberpunk/Synthwave with Glow Effects (极光主题 - 赛博朋克/蒸汽波+辉光效果)
            /// </summary>
            public const string Aurora = "Aurora";
        }

        /// <summary>
        /// Theme resource dictionary file paths
        /// </summary>
        public static class ThemeResourcePaths
        {
            /// <summary>
            /// Dark theme resource dictionary path
            /// </summary>
            public const string Dark = "Themes/DarkTheme.xaml";

            /// <summary>
            /// Light theme resource dictionary path
            /// </summary>
            public const string Light = "Themes/LightTheme.xaml";

            /// <summary>
            /// Light Blue theme resource dictionary path (浅蓝色主题)
            /// </summary>
            public const string LightBlue = "Themes/LightBlueTheme.xaml";

            /// <summary>
            /// BlueTech theme resource dictionary path (蓝科技主题 - 玻璃形态)
            /// </summary>
            public const string BlueTech = "Themes/BlueTechTheme.xaml";

            /// <summary>
            /// Aurora theme resource dictionary path (极光主题 - 赛博朋克)
            /// </summary>
            public const string Aurora = "Themes/AuroraTheme.xaml";
        }

        /// <summary>
        /// Dynamic resource key names used across themes
        /// </summary>
        public static class ResourceKeys
        {
            // Background Brushes
            public const string WindowBackgroundBrush = "WindowBackgroundBrush";
            public const string NavBackgroundBrush = "NavBackgroundBrush";
            public const string CardBackgroundBrush = "CardBackgroundBrush";
            public const string HeaderBackgroundBrush = "HeaderBackgroundBrush";

            // Text Brushes
            public const string TextBrush = "TextBrush";
            public const string TextSecondaryBrush = "TextSecondaryBrush";

            // Accent and State Brushes
            public const string AccentBrush = "AccentBrush";
            public const string HoverBrush = "HoverBrush";
            public const string SelectedBrush = "SelectedBrush";
            public const string BorderBrush = "BorderBrush";

            // Status Brushes
            public const string SuccessBrush = "SuccessBrush";
            public const string ErrorBrush = "ErrorBrush";
            public const string WarningBrush = "WarningBrush";
            public const string InfoBrush = "InfoBrush";

            // Gradient Brushes
            public const string AccentGradientBrush = "AccentGradientBrush";
            public const string ProgressGradientBrush = "ProgressGradientBrush";
            public const string ProgressBarBackgroundBrush = "ProgressBarBackgroundBrush";

            // Effects
            public const string CardShadow = "CardShadow";
            public const string AccentGlow = "AccentGlow";

            // Styles
            public const string AccentButton = "AccentButton";
            public const string NavButtonStyle = "NavButtonStyle";
            public const string ToolbarButtonStyle = "ToolbarButtonStyle";
            public const string WindowControlButtonStyle = "WindowControlButtonStyle";
            public const string CloseButtonStyle = "CloseButtonStyle";
            public const string ThemeToggleButtonStyle = "ThemeToggleButtonStyle";
            public const string DashboardCard = "DashboardCard";
            public const string ThemeToggleButton = "ThemeToggleButton";
            public const string LanguageToggleButton = "LanguageToggleButton";
            public const string IconBoxStyle = "IconBoxStyle";
        }



        /// <summary>
        /// Ordered theme sequence for UI toggling (single source of truth)
        /// </summary>
        public static readonly string[] OrderedThemes =
        {
            ThemeNames.Dark,
            ThemeNames.Light,
            ThemeNames.LightBlue,
            ThemeNames.BlueTech,
            ThemeNames.Aurora
        };

        /// <summary>
        /// HashSet for fast theme validation lookups.
        /// </summary>
        public static readonly System.Collections.Generic.HashSet<string> AllThemesSet = new System.Collections.Generic.HashSet<string>(OrderedThemes);

        /// <summary>
        /// Returns true if themeName is a defined theme.
        /// </summary>
        public static bool IsValidTheme(string? themeName) => themeName != null && AllThemesSet.Contains(themeName);

        /// <summary>
        /// Localization resource keys for theme display names
        /// </summary>
        public static readonly System.Collections.Generic.Dictionary<string,string> ThemeDisplayLocalizationKeys =
            new System.Collections.Generic.Dictionary<string,string>
            {
                { ThemeNames.Dark, "Status_Theme_Dark" },
                { ThemeNames.Light, "Status_Theme_Light" },
                { ThemeNames.LightBlue, "Status_Theme_LightBlue" },
                { ThemeNames.BlueTech, "Status_Theme_BlueTech" },
                { ThemeNames.Aurora, "Status_Theme_Aurora" }
            };

        /// <summary>
        /// Gets the resource path for a given theme name
        /// </summary>
        /// <param name="themeName">The theme name</param>
        /// <returns>The resource dictionary path for the theme</returns>
        public static string GetThemeResourcePath(string themeName)
        {
            return themeName switch
            {
                ThemeNames.Dark => ThemeResourcePaths.Dark,
                ThemeNames.Light => ThemeResourcePaths.Light,
                ThemeNames.LightBlue => ThemeResourcePaths.LightBlue,
                ThemeNames.BlueTech => ThemeResourcePaths.BlueTech,
                ThemeNames.Aurora => ThemeResourcePaths.Aurora,
                _ => ThemeResourcePaths.Light
            };
        }

        /// <summary>
        /// Gets all available theme names
        /// </summary>
        /// <returns>Array of theme names</returns>
        public static string[] GetAllThemes() => OrderedThemes;
    }
}
