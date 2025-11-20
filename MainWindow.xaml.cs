using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using TechDashboard.Core.Constants;
using TechDashboard.Core.Infrastructure;
using TechDashboard.Services.Interfaces;
using TechDashboard.ViewModels;

namespace TechDashboard
{
    public partial class MainWindow : Window
    {
        // Services
        private readonly ILocalizationService _localizationService;

        // Animation state
        private bool _isAnimating = false;
        private double _expandedNavWidth = NavigationConstants.DefaultExpandedWidth;
        private bool _isCustomMaximized = false;
        private Rect _restoreBounds;

        public MainWindow(MainViewModel viewModel, ILocalizationService localizationService)
        {
            InitializeComponent();

            _localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
            DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataContext is MainViewModel vm)
                {
                    CalculateOptimalNavWidth();

                    vm.PropertyChanged += Vm_PropertyChanged;

                    double initialWidth = vm.IsNavExpanded ? _expandedNavWidth : NavigationConstants.CollapsedWidth;
                    NavColumn.Width = new GridLength(initialWidth);
                    vm.NavWidth = initialWidth;

                    UpdateToggleIcon(vm.IsNavExpanded);
                }

                // Keep double-click toggle support (no drag resize)
                NavPanel.PreviewMouseLeftButtonDown += NavPanel_PreviewMouseLeftButtonDown;
                NavPanel.MouseLeftButtonDown += NavPanel_MouseLeftButtonDown;

                SetupMenuSubmenuHandlers();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"OnLoaded error: {ex.Message}");
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.PropertyChanged -= Vm_PropertyChanged;
            }

            NavPanel.PreviewMouseLeftButtonDown -= NavPanel_PreviewMouseLeftButtonDown;
            NavPanel.MouseLeftButtonDown -= NavPanel_MouseLeftButtonDown;
        }

        #region Window Controls

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                MaximizeRestore();
            }
            else
            {
                try { DragMove(); } catch { }
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void MaximizeButton_Click(object sender, RoutedEventArgs e) => MaximizeRestore();

        private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();

        private void MaximizeRestore()
        {
            if (_isCustomMaximized)
            {
                Left = _restoreBounds.Left;
                Top = _restoreBounds.Top;
                Width = _restoreBounds.Width;
                Height = _restoreBounds.Height;
                _isCustomMaximized = false;
                MaximizeIcon.Text = IconConstants.Common.ChromeMaximize;
            }
            else
            {
                _restoreBounds = new Rect(Left, Top, Width, Height);

                var wa = SystemParameters.WorkArea;
                Left = wa.Left;
                Top = wa.Top;
                Width = wa.Width;
                Height = wa.Height;
                _isCustomMaximized = true;
                MaximizeIcon.Text = IconConstants.Common.ChromeRestore;
            }
        }

        #endregion

        #region Menu Handlers

        private void SetupMenuSubmenuHandlers()
        {
            if (MainMenu != null)
            {
                foreach (MenuItem item in MainMenu.Items)
                {
                    item.SubmenuOpened += MenuItem_SubmenuOpened;
                }
            }
        }

        private void MenuItem_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem && menuItem.HasItems)
            {
                foreach (var item in menuItem.Items)
                {
                    if (item is MenuItem subItem)
                    {
                        var popup = FindVisualChild<System.Windows.Controls.Primitives.Popup>(subItem);
                        if (popup != null)
                        {
                            popup.AllowsTransparency = true;
                        }
                    }
                }
            }
        }

        private T? FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T result)
                {
                    return result;
                }
                var childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null)
                {
                    return childOfChild;
                }
            }
            return null;
        }

        #endregion

        #region Navigation Width Calculation

        private void CalculateOptimalNavWidth()
        {
            try
            {
                double maxContentWidth = 0;
                const double iconWidth = 16;
                const double iconTextSpacing = 12;
                const double horizontalButtonPadding = 12;
                const double containerSideMargin = 8;

                var typeface = new Typeface(new FontFamily("Segoe UI"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
                var dpi = VisualTreeHelper.GetDpi(this).PixelsPerDip;
                var textBrush = Application.Current.TryFindResource("TextBrush") as SolidColorBrush ?? Brushes.White;
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
                    var ft = new FormattedText(text, culture, FlowDirection.LeftToRight, typeface, fontSize, textBrush, dpi);
                    double buttonTextWidth = ft.Width;
                    double contentWidth = iconWidth + iconTextSpacing + buttonTextWidth;
                    maxContentWidth = Math.Max(maxContentWidth, contentWidth);
                }

                double desired = containerSideMargin + horizontalButtonPadding + maxContentWidth + horizontalButtonPadding + containerSideMargin + NavigationConstants.ExpansionExtraBuffer;
                double logicalMin = Math.Max(NavigationConstants.CollapsedWidth + 20, 140);
                _expandedNavWidth = Math.Max(logicalMin, Math.Min(desired, NavigationConstants.MaxExpandedWidth));
                NavColumn.MaxWidth = _expandedNavWidth;

                Debug.WriteLine($"[CalculateOptimalNavWidth] Longest content width={maxContentWidth:F1}, desired={desired:F1}, final={_expandedNavWidth:F1}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"CalculateOptimalNavWidth error: {ex.Message}");
                _expandedNavWidth = NavigationConstants.DefaultExpandedWidth;
                NavColumn.MaxWidth = _expandedNavWidth;
            }
        }

        #endregion

        #region NavPanel Toggle (Double-Click Only)

        private void NavPanel_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var originalSource = e.OriginalSource as DependencyObject;
                if (!IsChildOfButton(originalSource) && DataContext is MainViewModel vm)
                {
                    vm.ToggleNavCommand.Execute(null);
                    e.Handled = true;
                }
            }
        }

        private void NavPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var originalSource = e.OriginalSource as DependencyObject;
                if (!IsChildOfButton(originalSource) && DataContext is MainViewModel vm)
                {
                    vm.ToggleNavCommand.Execute(null);
                    e.Handled = true;
                }
            }
        }

        private bool IsChildOfButton(DependencyObject? element)
        {
            while (element != null)
            {
                if (element is Button) return true;
                element = VisualTreeHelper.GetParent(element);
            }
            return false;
        }

        #endregion

        #region ViewModel Integration

        private void Vm_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == nameof(MainViewModel.IsNavExpanded) && sender is MainViewModel vm)
                {
                    AnimateNavWidth(vm.IsNavExpanded);
                    UpdateToggleIcon(vm.IsNavExpanded);
                }
                else if (e.PropertyName == nameof(MainViewModel.CurrentLanguage))
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        try
                        {
                            UpdateLayout();
                            InvalidateVisual();

                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                CalculateOptimalNavWidth();
                                if (sender is MainViewModel viewModel && viewModel.IsNavExpanded)
                                {
                                    NavColumn.Width = new GridLength(_expandedNavWidth);
                                    viewModel.NavWidth = _expandedNavWidth;
                                }
                            }), System.Windows.Threading.DispatcherPriority.Loaded);
                        }
                        catch (Exception) { }
                    }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Vm_PropertyChanged error: {ex.Message}");
            }
        }

        private void UpdateToggleIcon(bool expanded)
        {
            if (ToggleIcon != null)
            {
                ToggleIcon.Text = expanded ? IconConstants.Navigation.ChevronLeft : IconConstants.Navigation.ChevronRight;
            }
        }

        private void AnimateNavWidth(bool expanded)
        {
            _isAnimating = true;

            var targetWidth = expanded ? _expandedNavWidth : NavigationConstants.CollapsedWidth;
            var currentWidth = NavColumn.ActualWidth;
            var duration = TimeSpan.FromMilliseconds(NavigationConstants.AnimationDurationMs);

            var storyboard = new Storyboard();
            var animation = new GridLengthAnimation
            {
                From = new GridLength(currentWidth),
                To = new GridLength(targetWidth),
                Duration = new Duration(duration),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
            };

            Storyboard.SetTarget(animation, NavColumn);
            Storyboard.SetTargetProperty(animation, new PropertyPath(ColumnDefinition.WidthProperty));
            storyboard.Children.Add(animation);

            storyboard.Completed += (s, e) =>
            {
                _isAnimating = false;
                if (DataContext is MainViewModel vm) vm.NavWidth = targetWidth;
                NavColumn.Width = new GridLength(targetWidth);
            };

            storyboard.Begin();
        }

        #endregion
    }
}
