using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using TechDashboard.ViewModels;

namespace TechDashboard
{
    public partial class MainWindow : Window
    {
        // Navigation panel width constants
        private const double ExpandedNavWidth = 260;
        private const double CollapsedNavWidth = 60;
        private const double SnapThreshold = 100; // Midpoint for snapping decision

        // Drag state
        private bool _isDragging = false;
        private Point _dragStartPoint;
        private double _dragStartWidth;
        private bool _isAnimating = false;

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
                    // Subscribe to property changes
                    vm.PropertyChanged += Vm_PropertyChanged;

                    // Set initial width without animation
                    double initialWidth = vm.IsNavExpanded ? ExpandedNavWidth : CollapsedNavWidth;
                    NavColumn.Width = new GridLength(initialWidth);

                    // Update toggle icon
                    UpdateToggleIcon(vm.IsNavExpanded);
                }

                // Setup drag handlers
                NavPanel.MouseLeftButtonDown += NavPanel_MouseLeftButtonDown;
                NavPanel.MouseMove += NavPanel_MouseMove;
                NavPanel.MouseLeftButtonUp += NavPanel_MouseLeftButtonUp;
                NavPanel.MouseLeave += NavPanel_MouseLeave;

                // Window-level handlers for better drag experience
                this.MouseMove += MainWindow_MouseMove;
                this.MouseLeftButtonUp += MainWindow_MouseLeftButtonUp;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"OnLoaded error: {ex.Message}");
                MessageBox.Show($"Initialization error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region Drag Functionality

        private void NavPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (_isAnimating) return;

                var pos = e.GetPosition(NavPanel);
                var currentWidth = NavColumn.ActualWidth;

                // Define drag zone
                bool isInDragZone = false;

                if (currentWidth > CollapsedNavWidth + 10)
                {
                    // Expanded: drag from right edge (5px zone)
                    isInDragZone = pos.X >= NavPanel.ActualWidth - 5;
                }
                else
                {
                    // Collapsed: drag from entire panel except buttons
                    var hitElement = NavPanel.InputHitTest(pos);
                    isInDragZone = !(hitElement is Button || IsChildOf(hitElement as DependencyObject, typeof(Button)));
                }

                if (isInDragZone)
                {
                    _isDragging = true;
                    _dragStartPoint = e.GetPosition(this);
                    _dragStartWidth = currentWidth;
                    NavPanel.CaptureMouse();
                    Mouse.OverrideCursor = Cursors.SizeWE;
                    e.Handled = true;

                    System.Diagnostics.Debug.WriteLine($"Drag started from width: {_dragStartWidth}");
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

                    // Show resize cursor near edge
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

                // Clamp width between collapsed and expanded
                newWidth = Math.Max(CollapsedNavWidth, Math.Min(ExpandedNavWidth, newWidth));

                // Stop any running animations
                NavColumn.BeginAnimation(ColumnDefinition.WidthProperty, null);

                // Update width directly during drag
                NavColumn.Width = new GridLength(newWidth);

                // Update ViewModel state in real-time based on width
                if (DataContext is MainViewModel vm)
                {
                    // Determine if we should show as expanded or collapsed
                    bool shouldShowExpanded = newWidth > SnapThreshold;

                    // Only update if state actually changed
                    if (vm.IsNavExpanded != shouldShowExpanded)
                    {
                        vm.IsNavExpanded = shouldShowExpanded;
                    }
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

                // Show resize cursor at appropriate locations
                if (currentWidth > CollapsedNavWidth + 10)
                {
                    // Expanded state: resize cursor at right edge
                    if (pos.X >= NavPanel.ActualWidth - 5)
                    {
                        Mouse.OverrideCursor = Cursors.SizeWE;
                        return;
                    }
                }
                else
                {
                    // Collapsed state: resize cursor on entire panel (except buttons)
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

                // Get final width after drag
                var finalWidth = NavColumn.ActualWidth;

                System.Diagnostics.Debug.WriteLine($"Drag ended at width: {finalWidth}");

                if (DataContext is MainViewModel vm)
                {
                    // Snap to nearest state based on threshold
                    bool shouldExpand = finalWidth >= SnapThreshold;

                    // Always animate to the target state after drag
                    vm.IsNavExpanded = shouldExpand;

                    // Force animation to snap position
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
                    // Only animate if not currently dragging
                    if (!_isDragging)
                    {
                        System.Diagnostics.Debug.WriteLine($"VM PropertyChanged: IsNavExpanded = {vm.IsNavExpanded}");
                        AnimateNavWidth(vm.IsNavExpanded);
                        UpdateToggleIcon(vm.IsNavExpanded);
                    }
                    else
                    {
                        // During drag, just update the icon
                        UpdateToggleIcon(vm.IsNavExpanded);
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
                    // E76B = ChevronLeft (◀), E76C = ChevronRight (▶)
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
                double targetWidth = expanded ? ExpandedNavWidth : CollapsedNavWidth;
                double currentWidth = NavColumn.ActualWidth;

                System.Diagnostics.Debug.WriteLine($"AnimateNavWidth: {currentWidth} -> {targetWidth} (expanded={expanded})");

                // Skip if already at target (within 1px tolerance)
                if (Math.Abs(currentWidth - targetWidth) < 1)
                {
                    NavColumn.Width = new GridLength(targetWidth);
                    return;
                }

                // Stop any running animation
                NavColumn.BeginAnimation(ColumnDefinition.WidthProperty, null);

                // Ensure we have a pixel width to animate from
                if (NavColumn.Width.IsAuto || NavColumn.Width.IsStar)
                {
                    NavColumn.Width = new GridLength(currentWidth);
                }

                _isAnimating = true;

                // Create smooth animation
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
                        System.Diagnostics.Debug.WriteLine($"Animation completed at: {targetWidth}");
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
                // Fallback: set width directly
                try
                {
                    _isAnimating = false;
                    double targetWidth = expanded ? ExpandedNavWidth : CollapsedNavWidth;
                    NavColumn.Width = new GridLength(targetWidth);
                }
                catch { }
            }
        }

        #endregion

        #region Helper Methods

        // Check if element is child of specific type
        private bool IsChildOf(DependencyObject child, Type parentType)
        {
            if (child == null) return false;

            DependencyObject parent = System.Windows.Media.VisualTreeHelper.GetParent(child);

            while (parent != null)
            {
                if (parent.GetType() == parentType)
                    return true;

                parent = System.Windows.Media.VisualTreeHelper.GetParent(parent);
            }

            return false;
        }

        #endregion
    }
}