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
        private double _expandedNavWidth = 260;

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
                    CalculateOptimalNavWidth();

                    vm.PropertyChanged += Vm_PropertyChanged;

                    double initialWidth = vm.IsNavExpanded ? _expandedNavWidth : CollapsedNavWidth;
                    NavColumn.Width = new GridLength(initialWidth);
                    vm.NavWidth = initialWidth;

                    UpdateToggleIcon(vm.IsNavExpanded);
                }

                NavPanel.PreviewMouseLeftButtonDown += NavPanel_PreviewMouseLeftButtonDown;
                NavPanel.MouseLeftButtonDown += NavPanel_MouseLeftButtonDown;
                NavPanel.MouseMove += NavPanel_MouseMove;
                NavPanel.MouseLeftButtonUp += NavPanel_MouseLeftButtonUp;
                NavPanel.MouseLeave += NavPanel_MouseLeave;

                this.MouseMove += MainWindow_MouseMove;
                this.MouseLeftButtonUp += MainWindow_MouseLeftButtonUp;

                SetupMenuSubmenuHandlers();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"OnLoaded error: {ex.Message}");
            }
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
                try
                {
                    DragMove();
                }
                catch { }
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            MaximizeRestore();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MaximizeRestore()
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                MaximizeIcon.Text = "\uE922"; // Maximize icon
            }
            else
            {
                WindowState = WindowState.Maximized;
                MaximizeIcon.Text = "\uE923"; // Restore icon
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
            try
            {
                if (sender is MenuItem menuItem)
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        try
                        {
                            var popup = FindVisualChild<System.Windows.Controls.Primitives.Popup>(menuItem);
                            if (popup != null && popup.Child != null)
                            {
                                var cardBackground = Application.Current.TryFindResource("CardBackgroundBrush") as SolidColorBrush;
                                if (cardBackground != null)
                                {
                                    if (popup.Child is Border border)
                                    {
                                        border.Background = cardBackground;
                                    }
                                    else if (popup.Child is Panel panel)
                                    {
                                        panel.Background = cardBackground;
                                    }
                                    else
                                    {
                                        var innerBorder = FindVisualChild<Border>(popup.Child);
                                        if (innerBorder != null)
                                        {
                                            innerBorder.Background = cardBackground;
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex2)
                        {
                            System.Diagnostics.Debug.WriteLine($"MenuItem_SubmenuOpened inner error: {ex2.Message}");
                        }
                    }), System.Windows.Threading.DispatcherPriority.Loaded);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MenuItem_SubmenuOpened error: {ex.Message}");
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
                const double buttonPadding = 12;
                const double stackPanelMargin = 8;

                const double dashboardIconWidth = 40;
                const double dashboardIconTextSpacing = 12;

                var typeface = new Typeface(new FontFamily("Segoe UI"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
                var typefaceBold = new Typeface(new FontFamily("Segoe UI"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);
                var dpi = VisualTreeHelper.GetDpi(this).PixelsPerDip;

                // Get the text brush from resources
                var textBrush = Application.Current.TryFindResource("TextBrush") as SolidColorBrush ?? Brushes.White;

                string GetResourceString(string key, string defaultValue)
                {
                    var resource = Application.Current?.TryFindResource(key);
                    if (resource != null)
                    {
                        var str = resource.ToString();
                        if (!string.IsNullOrEmpty(str))
                        {
                            return str;
                        }
                    }

                    resource = this.TryFindResource(key);
                    if (resource != null)
                    {
                        var str = resource.ToString();
                        if (!string.IsNullOrEmpty(str))
                        {
                            return str;
                        }
                    }

                    return defaultValue ?? string.Empty;
                }

                var dashboardText = GetResourceString("Nav_Dashboard", "DASHBOARD");
                var dashboardTextWidth = new FormattedText(
                    dashboardText,
                    System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    typefaceBold,
                    16,
                    textBrush,
                    dpi).Width;

                var dashboardWidth = dashboardIconWidth + dashboardIconTextSpacing + dashboardTextWidth;
                maxContentWidth = Math.Max(maxContentWidth, dashboardWidth);

                var navTexts = new[]
                {
                    GetResourceString("Nav_Overview", "Overview"),
                    GetResourceString("Nav_Analytics", "Analytics"),
                    GetResourceString("Nav_Reports", "Reports"),
                    GetResourceString("Nav_Settings", "Settings"),
                    GetResourceString("Nav_Collapse", "Collapse")
                };

                var expandText = GetResourceString("Nav_Expand", "Expand Navigation");
                if (!string.IsNullOrEmpty(expandText) && expandText.Length > 0)
                {
                    var allTexts = navTexts.ToList();
                    allTexts.Add(expandText);
                    navTexts = allTexts.ToArray();
                }

                foreach (var text in navTexts)
                {
                    var textWidth = new FormattedText(
                        text,
                        System.Globalization.CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight,
                        typeface,
                        14,
                        textBrush,
                        dpi).Width;

                    var buttonContentWidth = iconWidth + iconTextSpacing + textWidth;
                    maxContentWidth = Math.Max(maxContentWidth, buttonContentWidth);
                }

                _expandedNavWidth = stackPanelMargin +
                                    buttonPadding +
                                    maxContentWidth +
                                    buttonPadding +
                                    stackPanelMargin;

                _expandedNavWidth = Math.Max(CollapsedNavWidth, _expandedNavWidth);
                _expandedNavWidth = Math.Min(_expandedNavWidth, 350);

                NavColumn.MaxWidth = _expandedNavWidth;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CalculateOptimalNavWidth error: {ex.Message}");
                _expandedNavWidth = 260;
            }
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
                        if (e.ClickCount == 2)
                        {
                            if (currentWidth <= CollapsedNavWidth + 10 && !vm.IsNavExpanded)
                            {
                                vm.IsNavExpanded = true;
                            }
                            else if (currentWidth > CollapsedNavWidth + 10 && vm.IsNavExpanded)
                            {
                                if (pos.X < NavPanel.ActualWidth - 5)
                                {
                                    vm.IsNavExpanded = false;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            e.Handled = true;
                            return;
                        }
                        else if (e.ClickCount == 1)
                        {
                            if (currentWidth <= CollapsedNavWidth + 10)
                            {
                                e.Handled = true;
                                return;
                            }
                            else if (currentWidth > CollapsedNavWidth + 10 && pos.X < NavPanel.ActualWidth - 5)
                            {
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

                if (currentWidth <= CollapsedNavWidth + 10)
                {
                    var hitElement = NavPanel.InputHitTest(pos);
                    bool isOnEmptyArea = !(hitElement is Button || IsChildOf(hitElement as DependencyObject, typeof(Button)));

                    if (isOnEmptyArea)
                    {
                        e.Handled = true;
                        return;
                    }
                }

                bool isInDragZone = false;

                if (currentWidth > CollapsedNavWidth + 10)
                {
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
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        try
                        {
                            this.UpdateLayout();
                            this.InvalidateVisual();

                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                try
                                {
                                    CalculateOptimalNavWidth();

                                    if (sender is MainViewModel viewModel)
                                    {
                                        if (viewModel.IsNavExpanded)
                                        {
                                            NavColumn.Width = new GridLength(_expandedNavWidth);
                                            viewModel.NavWidth = _expandedNavWidth;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine($"Error calculating nav width after language change: {ex.Message}");
                                }
                            }), System.Windows.Threading.DispatcherPriority.Loaded);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error updating layout after language change: {ex.Message}");
                        }
                    }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
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