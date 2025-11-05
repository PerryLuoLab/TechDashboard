namespace TechDashboard.Helpers
{
    /// <summary>
    /// Navigation panel constants and configuration values.
    /// </summary>
    public static class NavigationConstants
    {
        /// <summary>
        /// Minimum width when navigation panel is collapsed (in pixels).
        /// </summary>
        public const double CollapsedWidth = 60.0;

        /// <summary>
        /// Minimum width when navigation panel is expanded (in pixels).
        /// </summary>
        public const double MinExpandedWidth = 200.0;

        /// <summary>
        /// Maximum width when navigation panel is expanded (in pixels).
        /// </summary>
        public const double MaxExpandedWidth = 350.0;

        /// <summary>
        /// Default expanded width when calculation fails (in pixels).
        /// </summary>
        public const double DefaultExpandedWidth = 260.0;

        /// <summary>
        /// Width threshold for snapping decision between expanded and collapsed states (in pixels).
        /// </summary>
        public const double SnapThreshold = 100.0;

        /// <summary>
        /// Drag zone width at the right edge when expanded (in pixels).
        /// </summary>
        public const double DragZoneWidth = 5.0;

        /// <summary>
        /// Additional width tolerance for collapsed state detection (in pixels).
        /// </summary>
        public const double CollapsedTolerance = 10.0;

        /// <summary>
        /// Animation duration for navigation panel expansion/collapse (in milliseconds).
        /// </summary>
        public const int AnimationDurationMs = 200;

        /// <summary>
        /// Double-click time threshold (in milliseconds).
        /// </summary>
        public const int DoubleClickTimeThresholdMs = 400;

        /// <summary>
        /// Double-click distance threshold (in pixels).
        /// </summary>
        public const double DoubleClickDistanceThreshold = 10.0;

        /// <summary>
        /// Padding and margin to add when calculating optimal navigation width (in pixels).
        /// </summary>
        public const double WidthCalculationPadding = 32.0;

        /// <summary>
        /// Width tolerance for animation completion check (in pixels).
        /// </summary>
        public const double WidthTolerance = 1.0;
    }
}

