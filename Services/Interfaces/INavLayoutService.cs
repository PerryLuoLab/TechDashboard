namespace TechDashboard.Services.Interfaces
{
    /// <summary>
    /// Service to compute navigation layout metrics.
    /// </summary>
    public interface INavLayoutService
    {
        /// <summary>
        /// Calculates optimal expanded navigation width based on localized labels, DPI and theme resources.
        /// </summary>
        /// <param name="visualForDpi">A visual element used to get DPI info (e.g., the Window).</param>
        /// <returns>Optimal expanded width in device independent units.</returns>
        double CalculateOptimalNavWidth(System.Windows.Media.Visual visualForDpi);
    }
}
