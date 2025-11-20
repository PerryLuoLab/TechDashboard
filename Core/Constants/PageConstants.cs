namespace TechDashboard.Core.Constants
{
    /// <summary>
    /// Centralized page name and related localization key constants.
    /// Eliminates hard-coded page strings scattered across ViewModels and Views.
    /// </summary>
    public static class PageConstants
    {
        /// <summary>Overview dashboard page.</summary>
        public const string Overview = "Overview";
        /// <summary>Analytics page.</summary>
        public const string Analytics = "Analytics";
        /// <summary>Reports page.</summary>
        public const string Reports = "Reports";
        /// <summary>Settings / configuration page.</summary>
        public const string Settings = "Settings";

        /// <summary>
        /// Gets the localization resource key for the status bar display of a page.
        /// </summary>
        /// <param name="pageName">Logical page name.</param>
        /// <returns>Localization key string.</returns>
        public static string GetStatusKey(string pageName) => pageName switch
        {
            Overview => "Status_Page_Overview",
            Analytics => "Status_Page_Analytics",
            Reports => "Status_Page_Reports",
            Settings => "Status_Page_Settings",
            _ => "Status_Page_Overview"
        };
    }
}
