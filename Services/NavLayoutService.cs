using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using TechDashboard.Core.Constants;
using TechDashboard.Services.Interfaces;

namespace TechDashboard.Services
{
    public class NavLayoutService : INavLayoutService
    {
        private readonly ILocalizationService _localizationService;
        // Cache Typeface to avoid allocation on every calculation
        private static readonly Typeface _cachedTypeface = new Typeface(new FontFamily("Segoe UI"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);

        public NavLayoutService(ILocalizationService localizationService)
        {
            _localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
        }

        public double CalculateOptimalNavWidth(Visual visualForDpi)
        {
            double maxContentWidth = 0;
            const double iconWidth = 16;
            const double iconTextSpacing = 12;
            const double horizontalButtonPadding = 12;
            const double containerSideMargin = 8;

            var dpi = VisualTreeHelper.GetDpi(visualForDpi).PixelsPerDip;
            var textBrush = Application.Current.TryFindResource(ThemeConstants.ResourceKeys.TextBrush) as SolidColorBrush ?? Brushes.White;
            var culture = CultureInfo.CurrentUICulture;

            var navTexts = new[]
            {
                _localizationService.GetString("Nav_Overview"),
                _localizationService.GetString("Nav_Analytics"),
                _localizationService.GetString("Nav_Reports"),
                _localizationService.GetString("Nav_Settings"),
                _localizationService.GetString("Nav_Collapse"),
                _localizationService.GetString("Nav_Expand")
            };

            foreach (var text in navTexts.Where(t => !string.IsNullOrWhiteSpace(t)))
            {
                double fontSize = 14;
                // Use cached typeface
                var ft = new FormattedText(text, culture, FlowDirection.LeftToRight, _cachedTypeface, fontSize, textBrush, dpi);
                double buttonTextWidth = ft.Width;
                double contentWidth = iconWidth + iconTextSpacing + buttonTextWidth;
                maxContentWidth = Math.Max(maxContentWidth, contentWidth);
            }

            double desired = containerSideMargin + horizontalButtonPadding + maxContentWidth + horizontalButtonPadding + containerSideMargin + NavigationConstants.ExpansionExtraBuffer;
            double logicalMin = Math.Max(NavigationConstants.CollapsedWidth + 20, 140);
            return Math.Max(logicalMin, Math.Min(desired, NavigationConstants.MaxExpandedWidth));
        }
    }
}