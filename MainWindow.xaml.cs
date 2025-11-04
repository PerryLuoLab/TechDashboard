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
        private const double ExpandedNavWidth = 260;
        private const double CollapsedNavWidth = 48;
        private const double MinNavWidth = 0;
        private bool _isDragging = false;
        private Point _dragStartPoint;
        private double _dragStartWidth;
        private bool _isUpdatingFromViewModel = false;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.PropertyChanged += Vm_PropertyChanged;
                UpdateToggleIcon(vm.IsNavExpanded);
                // 确保初始宽度正确
                NavColumn.Width = new GridLength(vm.IsNavExpanded ? ExpandedNavWidth : CollapsedNavWidth);
            }

            // 添加导航栏拖拽功能
            NavPanel.MouseLeftButtonDown += NavPanel_MouseLeftButtonDown;
            NavPanel.MouseMove += NavPanel_MouseMove;
            NavPanel.MouseLeftButtonUp += NavPanel_MouseLeftButtonUp;
            NavPanel.MouseLeave += NavPanel_MouseLeave;
            
            // 也为主窗口添加鼠标移动事件，确保拖拽在整个窗口内都能工作
            this.MouseMove += MainWindow_MouseMove;
            this.MouseLeftButtonUp += MainWindow_MouseLeftButtonUp;
        }

        private void NavPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var pos = e.GetPosition(NavPanel);
                var windowPos = e.GetPosition(this);
                var currentWidth = NavColumn.ActualWidth;
                
                // 检查是否在导航栏右边缘的拖拽区域（5px宽度）
                // 或者如果导航栏已经折叠（宽度 <= CollapsedNavWidth + 10），在整个导航栏区域内都可以拖拽
                bool canDrag = false;
                if (currentWidth > CollapsedNavWidth + 10)
                {
                    // 展开状态，只能在右边缘拖拽
                    canDrag = pos.X >= NavPanel.ActualWidth - 5 && pos.X <= NavPanel.ActualWidth;
                }
                else
                {
                    // 折叠状态，在整个导航栏区域内都可以拖拽（但不能点击按钮）
                    canDrag = windowPos.X <= currentWidth && 
                              !(NavPanel.InputHitTest(pos) is Button);
                }
                
                if (canDrag)
                {
                    _isDragging = true;
                    _dragStartPoint = windowPos;
                    _dragStartWidth = currentWidth;
                    NavPanel.CaptureMouse();
                    Mouse.OverrideCursor = Cursors.SizeWE;
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"NavPanel_MouseLeftButtonDown error: {ex.Message}");
            }
        }

        private void NavPanel_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (_isDragging)
                {
                    var currentPoint = e.GetPosition(this);
                    var deltaX = currentPoint.X - _dragStartPoint.X;
                    var newWidth = Math.Max(MinNavWidth, Math.Min(ExpandedNavWidth, _dragStartWidth + deltaX));
                    
                    // 直接修改列宽，不触发动画，避免冲突
                    _isUpdatingFromViewModel = true;
                    NavColumn.BeginAnimation(ColumnDefinition.WidthProperty, null); // 停止动画
                    NavColumn.Width = new GridLength(newWidth);
                    _isUpdatingFromViewModel = false;
                }
                else
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
                    var currentPoint = e.GetPosition(this);
                    var deltaX = currentPoint.X - _dragStartPoint.X;
                    var newWidth = Math.Max(MinNavWidth, Math.Min(ExpandedNavWidth, _dragStartWidth + deltaX));
                    
                    // 直接修改列宽，不触发动画，避免冲突
                    _isUpdatingFromViewModel = true;
                    NavColumn.BeginAnimation(ColumnDefinition.WidthProperty, null); // 停止动画
                    NavColumn.Width = new GridLength(newWidth);
                    _isUpdatingFromViewModel = false;
                }
                else
                {
                    var pos = e.GetPosition(this);
                    var currentWidth = NavColumn.ActualWidth;
                    // 如果鼠标在导航栏右边缘，更新光标
                    if (pos.X <= currentWidth + 5 && pos.X >= currentWidth - 5)
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

        private void UpdateCursor(Point pos)
        {
            var currentWidth = NavColumn.ActualWidth;
            // 显示调整大小光标：在右边缘5px范围内，或者导航栏完全折叠时在整个导航栏宽度范围内
            if ((pos.X >= NavPanel.ActualWidth - 5 && currentWidth > MinNavWidth) ||
                (currentWidth <= CollapsedNavWidth + 10 && pos.X >= currentWidth - 5 && pos.X <= currentWidth + 5))
            {
                Mouse.OverrideCursor = Cursors.SizeWE;
            }
            else
            {
                Mouse.OverrideCursor = null;
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
                if (_isDragging)
                {
                    _isDragging = false;
                    NavPanel.ReleaseMouseCapture();
                    Mouse.OverrideCursor = null;
                    
                    // 拖拽结束后，根据最终宽度更新ViewModel状态
                    var finalWidth = NavColumn.ActualWidth;
                    if (DataContext is MainViewModel vm)
                    {
                        if (finalWidth <= CollapsedNavWidth + 15)
                        {
                            vm.IsNavExpanded = false;
                            // 如果宽度接近0或小于CollapsedNavWidth，自动调整到CollapsedNavWidth
                            if (finalWidth < CollapsedNavWidth - 5)
                            {
                                AnimateNavWidth(false);
                            }
                        }
                        else if (finalWidth >= ExpandedNavWidth - 15)
                        {
                            vm.IsNavExpanded = true;
                        }
                        else
                        {
                            // 中间状态，根据更接近哪个值来决定
                            if (finalWidth < (CollapsedNavWidth + ExpandedNavWidth) / 2)
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
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"HandleDragEnd error: {ex.Message}");
            }
        }

        private void NavPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!_isDragging)
            {
                Mouse.OverrideCursor = null;
            }
        }

        private void Vm_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (_isUpdatingFromViewModel || _isDragging) return;

            try
            {
                if (e.PropertyName == nameof(MainViewModel.IsNavExpanded) && sender is MainViewModel vm)
                {
                    _isUpdatingFromViewModel = true;
                    AnimateNavWidth(vm.IsNavExpanded);
                    UpdateToggleIcon(vm.IsNavExpanded);
                    _isUpdatingFromViewModel = false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Vm_PropertyChanged error: {ex.Message}");
                _isUpdatingFromViewModel = false;
            }
        }

        private void UpdateToggleIcon(bool expanded)
        {
            try
            {
                if (ToggleIcon != null && !_isDragging)
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
                double targetWidth = expanded ? ExpandedNavWidth : CollapsedNavWidth;
                double currentWidth = NavColumn.ActualWidth;

                // 如果当前宽度已经是目标宽度，直接返回
                if (Math.Abs(currentWidth - targetWidth) < 1)
                {
                    // 确保宽度正确设置
                    NavColumn.Width = new GridLength(targetWidth);
                    return;
                }

                // 停止之前的动画
                NavColumn.BeginAnimation(ColumnDefinition.WidthProperty, null);

                // 确保当前宽度已设置（如果还没有设置）
                if (NavColumn.Width.IsAuto || NavColumn.Width.IsStar)
                {
                    NavColumn.Width = new GridLength(currentWidth);
                }

                // 创建新动画
                var animation = new DoubleAnimation
                {
                    From = currentWidth,
                    To = targetWidth,
                    Duration = TimeSpan.FromMilliseconds(250),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut },
                    FillBehavior = FillBehavior.HoldEnd
                };

                animation.Completed += (s, e) =>
                {
                    NavColumn.BeginAnimation(ColumnDefinition.WidthProperty, null);
                    NavColumn.Width = new GridLength(targetWidth);
                };

                NavColumn.BeginAnimation(ColumnDefinition.WidthProperty, animation);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"AnimateNavWidth error: {ex.Message}");
                // 如果动画失败，直接设置宽度
                try
                {
                    double targetWidth = expanded ? ExpandedNavWidth : CollapsedNavWidth;
                    NavColumn.Width = new GridLength(targetWidth);
                }
                catch { }
            }
        }

    }
}