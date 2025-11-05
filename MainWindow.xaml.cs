using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using TechDashboard.ViewModels;

namespace TechDashboard
{
    public partial class MainWindow : Window
    {
        // Navigation panel width constants
        private const double CollapsedNavWidth = 60;
        private const double SnapThreshold = 100;
        private double _expandedNavWidth = 260; // 动态计算的展开宽度

        // Drag state
        private bool _isDragging = false;
        private Point _dragStartPoint;
        private double _dragStartWidth;
        private bool _isAnimating = false;

        // Double-click detection
        private DateTime _lastClickTime = DateTime.MinValue;
        private Point _lastClickPosition = new Point(0, 0);
        private const int DoubleClickTimeThresholdMs = 400;
        private const double DoubleClickDistanceThreshold = 10.0;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataContext is MainViewModel vm)
                {
                    // 计算最佳导航栏宽度
                    CalculateOptimalNavWidth();

                    // Subscribe to property changes
                    vm.PropertyChanged += Vm_PropertyChanged;

                    // Set initial width without animation
                    double initialWidth = vm.IsNavExpanded ? _expandedNavWidth : CollapsedNavWidth;
                    NavColumn.Width = new GridLength(initialWidth);
                    vm.NavWidth = initialWidth;

                    // Update toggle icon
                    UpdateToggleIcon(vm.IsNavExpanded);
                }

                // Setup drag handlers
                NavPanel.PreviewMouseLeftButtonDown += NavPanel_PreviewMouseLeftButtonDown;
                NavPanel.MouseLeftButtonDown += NavPanel_MouseLeftButtonDown;
                NavPanel.MouseMove += NavPanel_MouseMove;
                NavPanel.MouseLeftButtonUp += NavPanel_MouseLeftButtonUp;
                NavPanel.MouseLeave += NavPanel_MouseLeave;

                // Double-click detection is handled in NavPanel_MouseLeftButtonDown
                

                // Window-level handlers
                this.MouseMove += MainWindow_MouseMove;
                this.MouseLeftButtonUp += MainWindow_MouseLeftButtonUp;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"OnLoaded error: {ex.Message}");
            }
        }

        #region Navigation Width Calculation

        private void CalculateOptimalNavWidth()
        {
            try
            {
                double maxWidth = 0;
                const double iconWidth = 40; // Icon + padding
                const double margin = 50; // Left/right margins and spacing

                // 获取所有导航按钮的文本
                var navTexts = new[]
                {
                    FindResource("Nav_Overview")?.ToString() ?? "Overview",
                    FindResource("Nav_Analytics")?.ToString() ?? "Analytics",
                    FindResource("Nav_Reports")?.ToString() ?? "Reports",
                    FindResource("Nav_Settings")?.ToString() ?? "Settings",
                    FindResource("Nav_Collapse")?.ToString() ?? "Collapse"
                };

                // 测量每个文本的宽度
                var typeface = new Typeface(new FontFamily("Segoe UI"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);

                foreach (var text in navTexts)
                {
                    var formattedText = new FormattedText(
                        text,
                        System.Globalization.CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight,
                        typeface,
                        14, // Font size
                        Brushes.White,
                        VisualTreeHelper.GetDpi(this).PixelsPerDip);

                    maxWidth = Math.Max(maxWidth, formattedText.Width);
                }

                // 计算总宽度: Icon + 间距 + 最长文本 + 额外边距
                _expandedNavWidth = Math.Max(260, iconWidth + margin + maxWidth);
                _expandedNavWidth = Math.Min(_expandedNavWidth, 350); // 设置最大宽度限制

                NavColumn.MaxWidth = _expandedNavWidth;

                System.Diagnostics.Debug.WriteLine($"Calculated optimal nav width: {_expandedNavWidth}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CalculateOptimalNavWidth error: {ex.Message}");
                _expandedNavWidth = 260; // Fallback
            }
        }

        #endregion

        #region Double-Click Expand

        // Double-click detection is now handled directly in NavPanel_MouseLeftButtonDown using e.ClickCount
        // This method is kept for backward compatibility but not used
        private bool IsDoubleClick(DateTime currentTime, Point currentPosition)
        {
            // If never clicked before, this is not a double-click
            if (_lastClickTime == DateTime.MinValue)
            {
                return false;
            }

            var timeSinceLastClick = (currentTime - _lastClickTime).TotalMilliseconds;
            var distance = Math.Sqrt(
                Math.Pow(currentPosition.X - _lastClickPosition.X, 2) +
                Math.Pow(currentPosition.Y - _lastClickPosition.Y, 2));

            bool isDoubleClick = timeSinceLastClick < DoubleClickTimeThresholdMs &&
                                distance < DoubleClickDistanceThreshold;

            System.Diagnostics.Debug.WriteLine($"Double-click check: time={timeSinceLastClick}ms, distance={distance:F2}px, result={isDoubleClick}");

            return isDoubleClick;
        }

        #endregion

        #region Drag Functionality

        private void NavPanel_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (_isAnimating) return;

                var pos = e.GetPosition(NavPanel);
                var currentWidth = NavColumn.ActualWidth;

                if (DataContext is MainViewModel vm)
                {
                    var hitElement = NavPanel.InputHitTest(pos);
                    bool isOnEmptyArea = !(hitElement is Button || IsChildOf(hitElement as DependencyObject, typeof(Button)));

                    if (isOnEmptyArea)
                    {
                        System.Diagnostics.Debug.WriteLine($"Click detected: ClickCount={e.ClickCount}, Position=({pos.X:F2}, {pos.Y:F2}), EmptyArea={isOnEmptyArea}, IsExpanded={vm.IsNavExpanded}");

                        // Use ClickCount to detect double-click (WPF built-in feature)
                        if (e.ClickCount == 2)
                        {
                            // Toggle navigation panel state on double-click
                            if (currentWidth <= CollapsedNavWidth + 10 && !vm.IsNavExpanded)
                            {
                                // Collapsed: expand on double-click
                                System.Diagnostics.Debug.WriteLine("*** Double-click detected! Expanding navigation panel. ***");
                                vm.IsNavExpanded = true;
                            }
                            else if (currentWidth > CollapsedNavWidth + 10 && vm.IsNavExpanded)
                            {
                                // Expanded: collapse on double-click (but not when clicking on drag zone)
                                if (pos.X < NavPanel.ActualWidth - 5)
                                {
                                    System.Diagnostics.Debug.WriteLine("*** Double-click detected! Collapsing navigation panel. ***");
                                    vm.IsNavExpanded = false;
                                }
                                else
                                {
                                    // Clicking on drag zone - don't collapse, allow dragging
                                    System.Diagnostics.Debug.WriteLine("Double-click on drag zone - ignoring, allow dragging");
                                    return;
                                }
                            }
                            e.Handled = true;
                            return;
                        }
                        else if (e.ClickCount == 1)
                        {
                            // Single click on empty area
                            if (currentWidth <= CollapsedNavWidth + 10)
                            {
                                // Collapsed: don't start dragging, wait for possible double-click
                                System.Diagnostics.Debug.WriteLine("Single click on empty area (collapsed) - waiting for possible double-click");
                                e.Handled = true;
                                return;
                            }
                            else if (currentWidth > CollapsedNavWidth + 10 && pos.X < NavPanel.ActualWidth - 5)
                            {
                                // Expanded: don't start dragging unless on drag zone
                                System.Diagnostics.Debug.WriteLine("Single click on empty area (expanded) - waiting for possible double-click");
                                e.Handled = true;
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"PreviewMouseLeftButtonDown error: {ex.Message}");
            }
        }

        private void NavPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (_isAnimating) return;

                var pos = e.GetPosition(NavPanel);
                var currentWidth = NavColumn.ActualWidth;

                // Don't handle double-click here, it's handled in PreviewMouseLeftButtonDown
                // Just prevent dragging when collapsed
                if (currentWidth <= CollapsedNavWidth + 10)
                {
                    var hitElement = NavPanel.InputHitTest(pos);
                    bool isOnEmptyArea = !(hitElement is Button || IsChildOf(hitElement as DependencyObject, typeof(Button)));

                    if (isOnEmptyArea)
                    {
                        // Don't start dragging in collapsed state
                        e.Handled = true;
                        return;
                    }
                }

                // Only allow dragging when expanded
                bool isInDragZone = false;

                if (currentWidth > CollapsedNavWidth + 10)
                {
                    // Expanded state: drag zone is at the right edge
                    isInDragZone = pos.X >= NavPanel.ActualWidth - 5;
                }

                if (isInDragZone)
                {
                    _isDragging = true;
                    _dragStartPoint = e.GetPosition(this);
                    _dragStartWidth = currentWidth;
                    NavPanel.CaptureMouse();
                    Mouse.OverrideCursor = Cursors.SizeWE;
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MouseDown error: {ex.Message}");
            }
        }

        private void NavPanel_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (_isDragging)
                {
                    UpdateDragWidth(e.GetPosition(this));
                }
                else if (!_isAnimating)
                {
                    UpdateCursor(e.GetPosition(NavPanel));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"NavPanel_MouseMove error: {ex.Message}");
            }
        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (_isDragging)
                {
                    UpdateDragWidth(e.GetPosition(this));
                }
                else if (!_isAnimating)
                {
                    var pos = e.GetPosition(this);
                    var currentWidth = NavColumn.ActualWidth;

                    if (Math.Abs(pos.X - currentWidth) < 5)
                    {
                        Mouse.OverrideCursor = Cursors.SizeWE;
                    }
                    else if (!NavPanel.IsMouseOver)
                    {
                        Mouse.OverrideCursor = null;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MainWindow_MouseMove error: {ex.Message}");
            }
        }

        private void UpdateDragWidth(Point currentPoint)
        {
            try
            {
                var deltaX = currentPoint.X - _dragStartPoint.X;
                var newWidth = _dragStartWidth + deltaX;

                newWidth = Math.Max(CollapsedNavWidth, Math.Min(_expandedNavWidth, newWidth));

                NavColumn.BeginAnimation(ColumnDefinition.WidthProperty, null);
                NavColumn.Width = new GridLength(newWidth);

                if (DataContext is MainViewModel vm)
                {
                    bool shouldShowExpanded = newWidth > SnapThreshold;
                    if (vm.IsNavExpanded != shouldShowExpanded)
                    {
                        vm.IsNavExpanded = shouldShowExpanded;
                    }
                    vm.NavWidth = newWidth;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UpdateDragWidth error: {ex.Message}");
            }
        }

        private void UpdateCursor(Point pos)
        {
            try
            {
                var currentWidth = NavColumn.ActualWidth;

                if (currentWidth > CollapsedNavWidth + 10)
                {
                    if (pos.X >= NavPanel.ActualWidth - 5)
                    {
                        Mouse.OverrideCursor = Cursors.SizeWE;
                        return;
                    }
                }
                else
                {
                    var hitElement = NavPanel.InputHitTest(pos);
                    if (!(hitElement is Button || IsChildOf(hitElement as DependencyObject, typeof(Button))))
                    {
                        Mouse.OverrideCursor = Cursors.SizeWE;
                        return;
                    }
                }

                Mouse.OverrideCursor = null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UpdateCursor error: {ex.Message}");
            }
        }

        private void NavPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            HandleDragEnd();
        }

        private void MainWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDragging)
            {
                HandleDragEnd();
            }
        }

        private void HandleDragEnd()
        {
            try
            {
                if (!_isDragging) return;

                _isDragging = false;
                NavPanel.ReleaseMouseCapture();
                Mouse.OverrideCursor = null;

                var finalWidth = NavColumn.ActualWidth;

                if (DataContext is MainViewModel vm)
                {
                    bool shouldExpand = finalWidth >= SnapThreshold;
                    vm.IsNavExpanded = shouldExpand;
                    AnimateNavWidth(shouldExpand);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"HandleDragEnd error: {ex.Message}");
            }
        }

        private void NavPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!_isDragging && !_isAnimating)
            {
                Mouse.OverrideCursor = null;
            }
        }

        #endregion

        #region ViewModel Integration

        private void Vm_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == nameof(MainViewModel.IsNavExpanded) && sender is MainViewModel vm)
                {
                    if (!_isDragging)
                    {
                        AnimateNavWidth(vm.IsNavExpanded);
                        UpdateToggleIcon(vm.IsNavExpanded);
                    }
                    else
                    {
                        UpdateToggleIcon(vm.IsNavExpanded);
                    }
                }
                else if (e.PropertyName == nameof(MainViewModel.CurrentLanguage))
                {
                    // 语言切换后重新计算导航栏宽度
                    CalculateOptimalNavWidth();
                    if (sender is MainViewModel viewModel && viewModel.IsNavExpanded)
                    {
                        AnimateNavWidth(true);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Vm_PropertyChanged error: {ex.Message}");
            }
        }

        private void UpdateToggleIcon(bool expanded)
        {
            try
            {
                if (ToggleIcon != null)
                {
                    ToggleIcon.Text = expanded ? "\uE76B" : "\uE76C";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UpdateToggleIcon error: {ex.Message}");
            }
        }

        private void AnimateNavWidth(bool expanded)
        {
            try
            {
                double targetWidth = expanded ? _expandedNavWidth : CollapsedNavWidth;
                double currentWidth = NavColumn.ActualWidth;

                if (Math.Abs(currentWidth - targetWidth) < 1)
                {
                    NavColumn.Width = new GridLength(targetWidth);
                    if (DataContext is MainViewModel vm)
                    {
                        vm.NavWidth = targetWidth;
                    }
                    return;
                }

                NavColumn.BeginAnimation(ColumnDefinition.WidthProperty, null);

                if (NavColumn.Width.IsAuto || NavColumn.Width.IsStar)
                {
                    NavColumn.Width = new GridLength(currentWidth);
                }

                _isAnimating = true;

                var animation = new DoubleAnimation
                {
                    From = currentWidth,
                    To = targetWidth,
                    Duration = TimeSpan.FromMilliseconds(200),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
                };

                animation.Completed += (s, e) =>
                {
                    try
                    {
                        _isAnimating = false;
                        NavColumn.BeginAnimation(ColumnDefinition.WidthProperty, null);
                        NavColumn.Width = new GridLength(targetWidth);

                        if (DataContext is MainViewModel vm)
                        {
                            vm.NavWidth = targetWidth;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Animation completed error: {ex.Message}");
                    }
                };

                NavColumn.BeginAnimation(ColumnDefinition.WidthProperty, animation);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"AnimateNavWidth error: {ex.Message}");
                try
                {
                    _isAnimating = false;
                    double targetWidth = expanded ? _expandedNavWidth : CollapsedNavWidth;
                    NavColumn.Width = new GridLength(targetWidth);
                }
                catch { }
            }
        }

        #endregion

        #region Helper Methods

        private bool IsChildOf(DependencyObject? child, Type parentType)
        {
            if (child == null) return false;

            DependencyObject? parent = VisualTreeHelper.GetParent(child);

            while (parent != null)
            {
                if (parent.GetType() == parentType)
                    return true;

                parent = VisualTreeHelper.GetParent(parent);
            }

            return false;
        }

        #endregion
    }
}