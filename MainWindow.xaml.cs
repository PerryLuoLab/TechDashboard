using TechDashboard.ViewModels;
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
                UpdateNavWidth(vm.IsNavExpanded, false);
            }
        }

        private void Vm_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainViewModel.IsNavExpanded) && sender is MainViewModel vm)
            {
                UpdateNavWidth(vm.IsNavExpanded, true);
            }
        }

        private void UpdateNavWidth(bool expanded, bool animate)
        {
            double to = expanded ? ExpandedNavWidth : CollapsedNavWidth;
            if (!animate)
            {
                NavColumn.Width = new GridLength(to);
                return;
            }

            double from = NavColumn.ActualWidth;
            var anim = new DoubleAnimation(from, to, TimeSpan.FromMilliseconds(280))
            {
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            anim.Completed += (_, _) => NavColumn.Width = new GridLength(to);
            NavColumn.BeginAnimation(ColumnDefinition.WidthProperty, anim);
        }
    }
}