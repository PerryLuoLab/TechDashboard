using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using TechDashboard.ViewModels;


namespace TechDashboard
{
    public partial class MainWindow : Window
    {
        private const double ExpandedNavWidth = 260;
        private const double CollapsedNavWidth = 70;
        private Button? _selectedButton;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            _selectedButton = BtnOverview;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.PropertyChanged += Vm_PropertyChanged;
                UpdateNavWidth(vm.IsNavExpanded, false);
            }
        }

        private void Vm_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainViewModel.IsNavExpanded) && sender is MainViewModel vm)
            {
                UpdateNavWidth(vm.IsNavExpanded, true);
                UpdateToggleIcon(vm.IsNavExpanded);
            }
        }

        private void UpdateNavWidth(bool expanded, bool animate)
        {
            double targetWidth = expanded ? ExpandedNavWidth : CollapsedNavWidth;

            if (!animate)
            {
                NavColumn.Width = new GridLength(targetWidth);
                return;
            }

            double currentWidth = NavColumn.ActualWidth;
            var widthAnimation = new DoubleAnimation
            {
                From = currentWidth,
                To = targetWidth,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
            };

            widthAnimation.Completed += (_, _) =>
            {
                NavColumn.Width = new GridLength(targetWidth);
            };

            NavColumn.BeginAnimation(ColumnDefinition.WidthProperty, widthAnimation);
        }

        private void UpdateToggleIcon(bool expanded)
        {
            ToggleIcon.Text = expanded ? "\uE76B" : "\uE76C";
        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button clickedButton) return;

            if (_selectedButton != null && _selectedButton != clickedButton)
            {
                _selectedButton.Style = FindResource("NavButtonStyle") as Style;
            }

            clickedButton.Style = FindResource("NavButtonSelectedStyle") as Style;
            _selectedButton = clickedButton;

            var pageName = clickedButton.Tag?.ToString() ?? "Unknown";
            System.Diagnostics.Debug.WriteLine($"µ¼º½µ½: {pageName}");
        }
    }
}