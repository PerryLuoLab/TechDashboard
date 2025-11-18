using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using TechDashboard.ViewModels;
using TechDashboard.Services.Interfaces;
using TechDashboard.Infrastructure;

namespace TechDashboard
{
    public partial class MainWindow : Window
    {
        // Navigation panel width constants
        private const double CollapsedNavWidth = 60;
        private const double SnapThreshold = 100;
        private double _expandedNavWidth = 260;
        private const double DragZoneWidth = 5;

        // Drag state
        private bool _isDragging = false;
        private Point _dragStartPoint;
        private double _dragStartWidth;
        private bool _isAnimating = false;

        // Services
        private readonly ILocalizationService _localizationService;

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

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.PropertyChanged -= Vm_PropertyChanged;
            }

            NavPanel.PreviewMouseLeftButtonDown -= NavPanel_PreviewMouseLeftButtonDown;
            NavPanel.MouseLeftButtonDown -= NavPanel_MouseLeftButtonDown;
            NavPanel.MouseMove -= NavPanel_MouseMove;
            NavPanel.MouseLeftButtonUp -= NavPanel_MouseLeftButtonUp;
            NavPanel.MouseLeave -= NavPanel_MouseLeave;

            this.MouseMove -= MainWindow_MouseMove;
            this.MouseLeftButtonUp -= MainWindow_MouseLeftButtonUp;
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
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                MaximizeIcon.Text = "\uE922";
            }
            else
            {
                WindowState = WindowState.Maximized;
                MaximizeIcon.Text = "\uE923";
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
                const double buttonPadding = 12;
                const double stackPanelMargin = 8;

                var typeface = new Typeface(new FontFamily("Segoe UI"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
                var dpi = VisualTreeHelper.GetDpi(this).PixelsPerDip;
                var textBrush = Application.Current.TryFindResource("TextBrush") as SolidColorBrush ?? Brushes.White;
                var culture = System.Globalization.CultureInfo.CurrentUICulture;

                var navTexts = new[]
                {
                    _localizationService.GetString("Nav_Overview"),
                    _localizationService.GetString("Nav_Analytics"),
                    _localizationService.GetString("Nav_Reports"),
                    _localizationService.GetString("Nav_Settings"),
                    _localizationService.GetString("Nav_Collapse"),
                    _localizationService.GetString("Nav_Expand")
                };

                foreach (var text in navTexts.Where(t => !string.IsNullOrEmpty(t)))
                {
                    var textWidth = new FormattedText(text, culture, FlowDirection.LeftToRight, typeface, 14, textBrush, dpi).Width;
                    var buttonContentWidth = iconWidth + iconTextSpacing + textWidth;
                    maxContentWidth = Math.Max(maxContentWidth, buttonContentWidth);
                }

                _expandedNavWidth = stackPanelMargin + buttonPadding + maxContentWidth + buttonPadding + stackPanelMargin;
                _expandedNavWidth = Math.Max(CollapsedNavWidth, Math.Min(_expandedNavWidth, 350));

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
            var pos = e.GetPosition(NavPanel);
            
            // Only in drag zone at right edge (5px), prevent event bubbling
            if (pos.X >= NavPanel.ActualWidth - DragZoneWidth)
            {
                e.Handled = true;
                return;
            }

            // Handle double-click early (Preview) so inner controls like ScrollViewer cannot swallow it
            if (e.ClickCount == 2)
            {
                var originalSource = e.OriginalSource as DependencyObject;
                var isButton = IsChildOfButton(originalSource);

                System.Diagnostics.Debug.WriteLine($"[NavPanel_PreviewMouseLeftButtonDown] Double-click detected. IsChildOfButton={isButton}");

                if (!isButton)
                {
                    if (DataContext is MainViewModel vm)
                    {
                        System.Diagnostics.Debug.WriteLine("[NavPanel_PreviewMouseLeftButtonDown] Toggling navigation via command");
                        vm.ToggleNavCommand.Execute(null);
                        e.Handled = true;
                        return;
                    }
                }
            }
            // Don't prevent other mouse events to allow single-click etc.
        }

        private void NavPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"[NavPanel_MouseLeftButtonDown] Triggered - ClickCount: {e.ClickCount}, Source: {e.Source?.GetType().Name}, OriginalSource: {e.OriginalSource?.GetType().Name}");

            var pos = e.GetPosition(NavPanel);

            // Drag functionality: right edge 5px zone
            if (pos.X >= NavPanel.ActualWidth - DragZoneWidth)
            {
                System.Diagnostics.Debug.WriteLine("[NavPanel_MouseLeftButtonDown] In drag zone, starting drag");
                _isDragging = true;
                _dragStartPoint = e.GetPosition(this);
                _dragStartWidth = NavColumn.ActualWidth;
                NavPanel.CaptureMouse();
                Mouse.OverrideCursor = Cursors.SizeWE;
                e.Handled = true;
                return; // Exit early, don't process double-click
            }

            // Double-click functionality: blank area (not buttons)
            if (e.ClickCount == 2)
            {
                System.Diagnostics.Debug.WriteLine("[NavPanel_MouseLeftButtonDown] Double-click detected");
                
                // Check if not clicking on button or child element
                var originalSource = e.OriginalSource as DependencyObject;
                var isButton = IsChildOfButton(originalSource);
                
                System.Diagnostics.Debug.WriteLine($"[NavPanel_MouseLeftButtonDown] IsChildOfButton: {isButton}");
                
                if (!isButton)
                {
                    if (DataContext is MainViewModel vm)
                    {
                        System.Diagnostics.Debug.WriteLine("?? Double-click detected on NavPanel blank area, toggling navigation");
                        System.Diagnostics.Debug.WriteLine($"Current IsNavExpanded: {vm.IsNavExpanded}");
                        
                        // Use the ToggleNavCommand instead of direct property assignment
                        vm.ToggleNavCommand.Execute(null);
                        
                        System.Diagnostics.Debug.WriteLine($"After toggle command - IsNavExpanded: {vm.IsNavExpanded}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("?? WARNING: DataContext is not MainViewModel!");
                    }
                    e.Handled = true;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("[NavPanel_MouseLeftButtonDown] Click on button, ignoring");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"[NavPanel_MouseLeftButtonDown] Single click (ClickCount={e.ClickCount})");
            }
        }

        // Helper method to check if element is a button or child of button
        private bool IsChildOfButton(DependencyObject? element)
        {
            while (element != null)
            {
                if (element is Button)
                {
                    return true;
                }
                element = VisualTreeHelper.GetParent(element);
            }
            return false;
        }

        private void NavPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isAnimating) return;

            var pos = e.GetPosition(NavPanel);
            UpdateCursor(pos, e.OriginalSource as DependencyObject);

            if (_isDragging)
            {
                var currentPoint = e.GetPosition(this);
                UpdateDragWidth(currentPoint);
            }
        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                var currentPoint = e.GetPosition(this);
                UpdateDragWidth(currentPoint);
            }
        }

        private void UpdateDragWidth(Point currentPoint)
        {
            var deltaX = currentPoint.X - _dragStartPoint.X;
            var newWidth = _dragStartWidth + deltaX;

            newWidth = Math.Max(CollapsedNavWidth, Math.Min(newWidth, _expandedNavWidth));

            NavColumn.Width = new GridLength(newWidth);

            if (DataContext is MainViewModel vm)
            {
                vm.NavWidth = newWidth;
            }
        }

        private void UpdateCursor(Point pos, DependencyObject? source)
        {
            if (_isDragging || _isAnimating)
                return;

            // Check if hovering over drag zone at the right edge
            if (pos.X >= NavPanel.ActualWidth - DragZoneWidth)
            {
                Mouse.OverrideCursor = Cursors.SizeWE;
                return;
            }

            // Show double-arrow cursor on blank area (not buttons) for both expanded and collapsed states
            // This indicates that double-clicking will toggle the navigation panel
            if (DataContext is MainViewModel vm)
            {
                // Show cursor hint on blank area to indicate double-click toggle functionality
                if (!IsChildOfButton(source))
                {
                    Mouse.OverrideCursor = Cursors.SizeWE;
                    return;
                }
            }

            // Default: no special cursor
            Mouse.OverrideCursor = null;
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
            if (!_isDragging) return;

            _isDragging = false;
            NavPanel.ReleaseMouseCapture();
            Mouse.OverrideCursor = null;

            if (DataContext is MainViewModel vm)
            {
                var currentWidth = NavColumn.ActualWidth;

                if (currentWidth < SnapThreshold)
                {
                    vm.IsNavExpanded = false;
                    AnimateNavWidth(false);
                }
                else
                {
                    vm.IsNavExpanded = true;
                    AnimateNavWidth(true);
                }
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
                System.Diagnostics.Debug.WriteLine($"Vm_PropertyChanged error: {ex.Message}");
            }
        }

        private void UpdateToggleIcon(bool expanded)
        {
            if (ToggleIcon != null)
            {
                ToggleIcon.Text = expanded ? "\uE76B" : "\uE76C";
            }
        }

        private void AnimateNavWidth(bool expanded)
        {
            _isAnimating = true;

            var targetWidth = expanded ? _expandedNavWidth : CollapsedNavWidth;
            var currentWidth = NavColumn.ActualWidth;
            var duration = TimeSpan.FromMilliseconds(200);

            // 使用 Storyboard 和自定义动画
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
                
                if (DataContext is MainViewModel vm)
                {
                    vm.NavWidth = targetWidth;
                }
                
                // 确保最终宽度设置正确
                NavColumn.Width = new GridLength(targetWidth);
                
                System.Diagnostics.Debug.WriteLine($"? Animation completed: {(expanded ? "Expanded" : "Collapsed")} to {targetWidth}px");
            };

            storyboard.Begin();
        }

        private bool IsChildOf(DependencyObject? child, Type parentType)
        {
            while (child != null)
            {
                if (child.GetType() == parentType || child.GetType().IsSubclassOf(parentType))
                {
                    return true;
                }
                child = VisualTreeHelper.GetParent(child);
            }
            return false;
        }

        #endregion
    }
}