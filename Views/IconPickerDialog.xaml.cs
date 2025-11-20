using System;
using System.Windows;
using TechDashboard.ViewModels;

namespace TechDashboard.Views
{
    public partial class IconPickerDialog : Window
    {
        public IconPickerDialog(IconPickerViewModel vm)
        {
            InitializeComponent();
            DataContext = vm ?? throw new ArgumentNullException(nameof(vm));
        }

        private void Close_Click(object sender, RoutedEventArgs e) => Close();
    }
}
