namespace TechDashboard.Core.Constants
{
    /// <summary>
    /// Navigation panel constants and configuration values.
    /// Defines dimensions, thresholds, and timing for navigation panel behavior.
    /// </summary>
    public static class NavigationConstants
    {
        /// <summary>
        /// Minimum width when navigation panel is collapsed (in pixels).
        /// Shows only icons without text labels.
        /// </summary>
        public const double CollapsedWidth = 60.0;

        /// <summary>
        /// Minimum width when navigation panel is expanded (in pixels).
        /// Ensures sufficient space for icons and text labels.
        /// </summary>
        public const double MinExpandedWidth = 200.0;

        /// <summary>
        /// Maximum width when navigation panel is expanded (in pixels).
        /// Prevents navigation panel from becoming too wide.
        /// </summary>
        public const double MaxExpandedWidth = 350.0;

        /// <summary>
        /// Default expanded width when calculation fails (in pixels).
        /// Fallback value for optimal width calculation.
        /// </summary>
        public const double DefaultExpandedWidth = 260.0;

        /// <summary>
        /// Width threshold for snapping decision between expanded and collapsed states (in pixels).
        /// Values below this snap to collapsed, above snap to expanded.
        /// </summary>
        public const double SnapThreshold = 100.0;

        /// <summary>
        /// Drag zone width at the right edge when expanded (in pixels).
        /// Defines the interactive area for resizing the navigation panel.
        /// </summary>
        public const double DragZoneWidth = 5.0;

        /// <summary>
        /// Additional width tolerance for collapsed state detection (in pixels).
        /// Provides margin for determining if panel is in collapsed state.
        /// </summary>
        public const double CollapsedTolerance = 10.0;

        /// <summary>
        /// Animation duration for navigation panel expansion/collapse (in milliseconds).
        /// Controls the speed of smooth transitions.
        /// </summary>
        public const int AnimationDurationMs = 200;

        /// <summary>
        /// Double-click time threshold (in milliseconds).
        /// Maximum time between clicks to register as double-click.
        /// </summary>
        public const int DoubleClickTimeThresholdMs = 400;

        /// <summary>
        /// Double-click distance threshold (in pixels).
        /// Maximum distance between click positions to register as double-click.
        /// </summary>
        public const double DoubleClickDistanceThreshold = 10.0;

        /// <summary>
        /// Padding and margin to add when calculating optimal navigation width (in pixels).
        /// Accounts for icons, spacing, and margins in width calculation.
        /// </summary>
        public const double WidthCalculationPadding = 32.0;

        /// <summary>
        /// Width tolerance for animation completion check (in pixels).
        /// Determines when animation has reached its target within acceptable precision.
        /// </summary>
        public const double WidthTolerance = 1.0;

        /// <summary>
        /// Extra buffer (in pixels) added to the computed max item width for expanded navigation.
        /// Ensures the longest item plus icon & spacing does not touch the edge. Requirement: +6px.
        /// </summary>
        public const double ExpansionExtraBuffer = 6.0;
    }
}
