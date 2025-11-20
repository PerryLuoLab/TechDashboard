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
using Microsoft.Extensions.Logging;

namespace TechDashboard
{
    public partial class MainWindow : Window
    {
        // Services
        private readonly ILocalizationService _localizationService;
        private readonly ILogger<MainWindow> _logger;
        private readonly INavLayoutService _navLayoutService;

        // Animation state
        private bool _isAnimating = false;
        private double _expandedNavWidth = NavigationConstants.DefaultExpandedWidth;
        private bool _isCustomMaximized = false;
        private Rect _restoreBounds;

        public MainWindow(MainViewModel viewModel, ILocalizationService localizationService, ILogger<MainWindow> logger, INavLayoutService navLayoutService)
        {
            InitializeComponent();

            _localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _navLayoutService = navLayoutService ?? throw new ArgumentNullException(nameof(navLayoutService));
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
                    _expandedNavWidth = _navLayoutService.CalculateOptimalNavWidth(this);
                    NavColumn.MaxWidth = _expandedNavWidth;

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
                _logger.LogInformation("MainWindow loaded");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during MainWindow.OnLoaded");
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
            _logger.LogInformation("MainWindow unloaded");
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
                try { DragMove(); } catch (Exception ex) { _logger.LogDebug(ex, "DragMove failed"); }
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
                _logger.LogDebug("Window restored");
            }
            else
            {
                _restoreBounds = new Rect(Left, Top, Width, Height);

                var wa = SystemParameters.WorkArea;
                Left = wa.Left; Top = wa.Top; Width = wa.Width; Height = wa.Height;
                _isCustomMaximized = true;
                MaximizeIcon.Text = IconConstants.Common.ChromeRestore;
                _logger.LogDebug("Window maximized to work area");
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
                if (child is T result) return result;
                var childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null) return childOfChild;
            }
            return null;
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
                    _logger.LogDebug("Navigation toggled via preview double-click");
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
                    _logger.LogDebug("Navigation toggled via double-click");
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
                    _logger.LogInformation("Navigation expanded state changed: {Expanded}", vm.IsNavExpanded);
                }
                else if (e.PropertyName == nameof(MainViewModel.CurrentLanguage))
                {
                    _logger.LogInformation("Language changed to {Language}", (sender as MainViewModel)?.CurrentLanguage);
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        try
                        {
                            UpdateLayout();
                            InvalidateVisual();

                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                _expandedNavWidth = _navLayoutService.CalculateOptimalNavWidth(this);
                                NavColumn.MaxWidth = _expandedNavWidth;
                                if (sender is MainViewModel viewModel && viewModel.IsNavExpanded)
                                {
                                    NavColumn.Width = new GridLength(_expandedNavWidth);
                                    viewModel.NavWidth = _expandedNavWidth;
                                }
                            }), System.Windows.Threading.DispatcherPriority.Loaded);
                        }
                        catch (Exception innerEx) { _logger.LogError(innerEx, "Error updating layout after language change"); }
                    }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Vm_PropertyChanged handler failed");
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
                _logger.LogDebug("Navigation width animation complete. Expanded={Expanded} Width={Width}", expanded, targetWidth);
            };

            storyboard.Begin();
        }

        #endregion

        private void IconPickerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dlgObj = App.Services.GetService(typeof(TechDashboard.Views.IconPickerDialog));
                var dlg = dlgObj as TechDashboard.Views.IconPickerDialog;
                dlg?.ShowDialog();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to open IconPicker dialog");
            }
        }
    }
}
