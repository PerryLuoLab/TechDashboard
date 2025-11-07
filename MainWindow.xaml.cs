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
                
                // Setup menu submenu handlers
                SetupMenuSubmenuHandlers();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"OnLoaded error: {ex.Message}");
            }
        }
        
        private void SetupMenuSubmenuHandlers()
        {
            // 为所有MenuItem添加SubmenuOpened事件处理
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
                    // 延迟执行，确保Popup已经创建
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        try
                        {
                            // 查找Popup
                            var popup = FindVisualChild<System.Windows.Controls.Primitives.Popup>(menuItem);
                            if (popup != null && popup.Child != null)
                            {
                                // Popup的Child通常是Border或Panel，设置其背景
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
                                        // 尝试查找内部的Border
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

        #region Navigation Width Calculation

        private void CalculateOptimalNavWidth()
        {
            try
            {
                double maxContentWidth = 0;
                const double iconWidth = 16; // 导航按钮图标宽度（FontSize 16）
                const double iconTextSpacing = 12; // 图标和文本之间的间距（Margin="12,0,0,0"）
                const double buttonPadding = 12; // 按钮左右padding（来自NavButtonStyle）
                const double stackPanelMargin = 8; // StackPanel的Margin（左右各8）
                
                // DASHBOARD Logo部分的尺寸
                const double dashboardIconWidth = 40; // DASHBOARD图标框宽度（IconBoxStyle）
                const double dashboardIconTextSpacing = 12; // DASHBOARD图标和文本间距（Margin="12,0,0,0"）

                var typeface = new Typeface(new FontFamily("Segoe UI"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
                var typefaceBold = new Typeface(new FontFamily("Segoe UI"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);
                var dpi = VisualTreeHelper.GetDpi(this).PixelsPerDip;

                // 强制刷新资源，确保获取到最新的语言资源
                // 使用TryFindResource确保能获取到资源，如果失败则使用默认值
                string GetResourceString(string key, string defaultValue)
                {
                    // 尝试从Application资源中获取
                    var resource = Application.Current?.TryFindResource(key);
                    if (resource != null)
                    {
                        var str = resource.ToString();
                        if (!string.IsNullOrEmpty(str))
                        {
                            System.Diagnostics.Debug.WriteLine($"Found resource {key}: {str}");
                            return str;
                        }
                    }
                    
                    // 尝试从当前窗口资源中获取
                    resource = this.TryFindResource(key);
                    if (resource != null)
                    {
                        var str = resource.ToString();
                        if (!string.IsNullOrEmpty(str))
                        {
                            System.Diagnostics.Debug.WriteLine($"Found resource in window {key}: {str}");
                            return str;
                        }
                    }
                    
                    System.Diagnostics.Debug.WriteLine($"Resource {key} not found, using default: {defaultValue}");
                    return defaultValue ?? string.Empty;
                }

                // 1. 计算DASHBOARD Logo部分的宽度
                var dashboardText = GetResourceString("Nav_Dashboard", "DASHBOARD");
                var dashboardTextWidth = new FormattedText(
                    dashboardText,
                    System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    typefaceBold,
                    16, // FontSize 16, Bold
                    Brushes.White,
                    dpi).Width;
                
                var dashboardWidth = dashboardIconWidth + dashboardIconTextSpacing + dashboardTextWidth;
                maxContentWidth = Math.Max(maxContentWidth, dashboardWidth);
                System.Diagnostics.Debug.WriteLine($"Dashboard text: '{dashboardText}', width: {dashboardTextWidth}, total: {dashboardWidth}");

                // 2. 计算所有导航按钮的宽度
                var navTexts = new[]
                {
                    GetResourceString("Nav_Overview", "Overview"),
                    GetResourceString("Nav_Analytics", "Analytics"),
                    GetResourceString("Nav_Reports", "Reports"),
                    GetResourceString("Nav_Settings", "Settings"),
                    GetResourceString("Nav_Collapse", "Collapse")
                };

                // 根据当前语言可能需要包含展开文本（某些语言较长）
                var expandText = GetResourceString("Nav_Expand", "Expand Navigation");
                if (!string.IsNullOrEmpty(expandText) && expandText.Length > 0)
                {
                    var allTexts = navTexts.ToList();
                    allTexts.Add(expandText);
                    navTexts = allTexts.ToArray();
                }

                // 测量每个导航按钮文本的宽度
                foreach (var text in navTexts)
                {
                    var textWidth = new FormattedText(
                        text,
                        System.Globalization.CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight,
                        typeface,
                        14, // Font size
                        Brushes.White,
                        dpi).Width;

                    // 导航按钮宽度 = 图标宽度 + 图标文本间距 + 文本宽度
                    var buttonContentWidth = iconWidth + iconTextSpacing + textWidth;
                    maxContentWidth = Math.Max(maxContentWidth, buttonContentWidth);
                    System.Diagnostics.Debug.WriteLine($"Nav text: '{text}', width: {textWidth}, button content: {buttonContentWidth}");
                }

                // 3. 计算总宽度
                // DASHBOARD部分：StackPanel左边距 + DASHBOARD内容宽度 + StackPanel右边距（不需要按钮padding）
                // 导航按钮部分：StackPanel左边距 + 按钮左边距 + 按钮内容宽度 + 按钮右边距 + StackPanel右边距
                // 为了统一处理，我们取两者中较大的值
                // 但考虑到DASHBOARD也可能需要一些右边距，我们统一使用按钮padding作为安全边距
                _expandedNavWidth = stackPanelMargin + // StackPanel左边距
                                   buttonPadding + // 左边距（确保按钮padding足够，也作为DASHBOARD的右边距）
                                   maxContentWidth + // 最长内容宽度（已包含DASHBOARD或按钮的完整宽度）
                                   buttonPadding + // 右边距（确保按钮padding足够）
                                   stackPanelMargin; // StackPanel右边距

                // 设置最小宽度（确保折叠状态可用）
                _expandedNavWidth = Math.Max(CollapsedNavWidth, _expandedNavWidth);
                // 设置最大宽度限制（防止过长）
                _expandedNavWidth = Math.Min(_expandedNavWidth, 350);

                NavColumn.MaxWidth = _expandedNavWidth;

                System.Diagnostics.Debug.WriteLine($"=== Calculated optimal nav width: {_expandedNavWidth} (max content width: {maxContentWidth}, dashboard width: {dashboardWidth}) ===");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CalculateOptimalNavWidth error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
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
                    // 使用Dispatcher延迟执行，确保资源字典已完全更新
                    // 使用ApplicationIdle优先级，确保资源字典更新完成后再计算
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        try
                        {
                            // 强制刷新布局和资源
                            this.UpdateLayout();
                            this.InvalidateVisual();
                            
                            // 等待一个渲染周期，确保资源字典完全加载
                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                try
                                {
                                    // 重新计算导航栏宽度
                                    CalculateOptimalNavWidth();
                                    
                                    if (sender is MainViewModel viewModel)
                                    {
                                        // 如果导航栏是展开状态，使用新的宽度
                                        if (viewModel.IsNavExpanded)
                                        {
                                            // 直接设置新宽度，无需动画（因为语言切换应该立即更新）
                                            NavColumn.Width = new GridLength(_expandedNavWidth);
                                            viewModel.NavWidth = _expandedNavWidth;
                                            System.Diagnostics.Debug.WriteLine($"Language changed to {viewModel.CurrentLanguage}, updated nav width to: {_expandedNavWidth}");
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