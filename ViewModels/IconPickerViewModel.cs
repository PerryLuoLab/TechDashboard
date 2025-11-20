using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TechDashboard.Core.Infrastructure;
using TechDashboard.Tools;

namespace TechDashboard.ViewModels
{
    public class IconPickerViewModel : ObservableObject
    {
        private readonly Dictionary<string, string> _allIconsCache;

        public ObservableCollection<string> Categories { get; } = new();
        public ObservableCollection<KeyValuePair<string, string>> FilteredIcons { get; } = new();

        private string? _selectedCategory;
        public string? SelectedCategory
        {
            get => _selectedCategory;
            set { SetProperty(ref _selectedCategory, value, action: Refresh); }
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set { SetProperty(ref _searchText, value, action: Refresh); }
        }

        private KeyValuePair<string, string>? _selectedIcon;
        public KeyValuePair<string, string>? SelectedIcon
        {
            get => _selectedIcon;
            set
            {
                if (!Equals(_selectedIcon, value))
                {
                    SetProperty(ref _selectedIcon, value);
                    if (value.HasValue)
                    {
                        var parts = value.Value.Key.Split('.');
                        if (parts.Length == 2)
                        {
                            SelectedXaml = $"<TextBlock Text=\"{{x:Static icons:IconConstants+{parts[0]}.{parts[1]}}}\" FontFamily=\"{{x:Static icons:IconConstants.DefaultFontFamily}}\"/>";
                            SelectedCSharp = $"IconConstants.{parts[0]}.{parts[1]}";
                        }
                    }
                }
            }
        }

        private string _selectedXaml = string.Empty;
        public string SelectedXaml { get => _selectedXaml; set => SetProperty(ref _selectedXaml, value); }

        private string _selectedCSharp = string.Empty;
        public string SelectedCSharp { get => _selectedCSharp; set => SetProperty(ref _selectedCSharp, value); }

        public ICommand SelectIconCommand { get; }
        public ICommand CopyXamlCommand { get; }
        public ICommand CopyCSharpCommand { get; }

        public IconPickerViewModel()
        {
            _allIconsCache = IconHelper.GetAllIcons();
            Categories.Add("All");
            foreach (var c in IconHelper.GetCategories()) Categories.Add(c);
            SelectedCategory = Categories.FirstOrDefault();

            SelectIconCommand = new RelayCommand(param => { if (param is KeyValuePair<string, string> kv) SelectedIcon = kv; });
            CopyXamlCommand = new RelayCommand(_ => { if (!string.IsNullOrWhiteSpace(SelectedXaml)) Clipboard.SetText(SelectedXaml); });
            CopyCSharpCommand = new RelayCommand(_ => { if (!string.IsNullOrWhiteSpace(SelectedCSharp)) Clipboard.SetText(SelectedCSharp); });
            Refresh();
        }

        private void Refresh()
        {
            IEnumerable<KeyValuePair<string, string>> iconsToShow = _allIconsCache;

            if (!string.IsNullOrWhiteSpace(SelectedCategory) && SelectedCategory != "All")
            {
                iconsToShow = iconsToShow.Where(kv => kv.Key.StartsWith(SelectedCategory + ".", StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                iconsToShow = iconsToShow.Where(kv => kv.Key.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }

            FilteredIcons.Clear();
            foreach (var kv in iconsToShow)
            {
                FilteredIcons.Add(kv);
            }
        }
    }
}
