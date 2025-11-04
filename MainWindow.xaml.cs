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
        private const double CollapsedNavWidth = 48;

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
            }
        }

        private void Vm_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainViewModel.IsNavExpanded) && sender is MainViewModel vm)
            {
                AnimateNavWidth(vm.IsNavExpanded);
            }
        }

        private void AnimateNavWidth(bool expanded)
        {
            double targetWidth = expanded ? ExpandedNavWidth : CollapsedNavWidth;

            // Update collapse icon
            ToggleIcon.Text = expanded ? "\uE76B" : "\uE76C";

            // Animate width
            var animation = new DoubleAnimation
            {
                To = targetWidth,
                Duration = TimeSpan.FromMilliseconds(250),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
            };

            NavColumn.BeginAnimation(ColumnDefinition.WidthProperty, animation);
        }
    }
}