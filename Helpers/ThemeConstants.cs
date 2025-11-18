namespace TechDashboard.Helpers
{
    /// <summary>
    /// Theme-related constants and configuration values.
    /// </summary>
    public static class ThemeConstants
    {
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
            /// Blue Tech theme
            /// </summary>
            public const string BlueTech = "BlueTech";
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
            /// Blue Tech theme resource dictionary path
            /// </summary>
            public const string BlueTech = "Themes/BlueTechTheme.xaml";
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
        /// Default theme to use when no preference is set
        /// </summary>
        public const string DefaultTheme = ThemeNames.Light;

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
                ThemeNames.BlueTech => ThemeResourcePaths.BlueTech,
                _ => ThemeResourcePaths.Light
            };
        }

        /// <summary>
        /// Gets all available theme names
        /// </summary>
        /// <returns>Array of theme names</returns>
        public static string[] GetAllThemes()
        {
            return new[]
            {
                ThemeNames.Dark,
                ThemeNames.Light,
                ThemeNames.BlueTech
            };
        }
    }
}
