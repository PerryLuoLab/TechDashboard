namespace TechDashboard.Helpers
{
    public static class ThemeManager
    {
        public static void Apply(string themeName)
        {
            App.ApplyTheme(themeName);
        }
    }
}
