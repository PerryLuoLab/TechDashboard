using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using TechDashboard.Helpers;
using TechDashboard.ViewModels;

namespace TechDashboard.Services
{
    /// <summary>
    /// Service responsible for managing navigation panel behavior including drag, resize, and animations.
    /// </summary>
    public class NavigationPanelService
    {
        private readonly ColumnDefinition _navColumn;
        private readonly Border _navPanel;
        private readonly MainViewModel _viewModel;

        private double _calculatedExpandedWidth = NavigationConstants.DefaultExpandedWidth;
        private bool _isDragging;
        private Point _dragStartPoint;
        private double _dragStartWidth;
        private bool _isAnimating;
        private DateTime _lastClickTime = DateTime.MinValue;
        private Point _lastClickPosition = new Point(0, 0);

        /// <summary>
        /// Initializes a new instance of the NavigationPanelService.
        /// </summary>
        public NavigationPanelService(ColumnDefinition navColumn, Border navPanel, MainViewModel viewModel)
        {
            _navColumn = navColumn ?? throw new ArgumentNullException(nameof(navColumn));
            _navPanel = navPanel ?? throw new ArgumentNullException(nameof(navPanel));
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        }

        /// <summary>
        /// Initializes the navigation panel service.
        /// </summary>
        public void Initialize()
        {
            CalculateOptimalWidth();
            UpdateWidth(_viewModel.IsNavExpanded ? _calculatedExpandedWidth : NavigationConstants.CollapsedWidth);
            AttachEventHandlers();
        }

        /// <summary>
        /// Handles mouse left button down event.
        /// </summary>
        public void HandleMouseDown(MouseButtonEventArgs e)
        {
            if (_isAnimating) return;

            var pos = e.GetPosition(_navPanel);
            var currentWidth = _navColumn.ActualWidth;
            var currentTime = DateTime.Now;

            // Check for double-click when collapsed
            if (currentWidth <= NavigationConstants.CollapsedWidth + NavigationConstants.CollapsedTolerance)
            {
                if (IsClickOnEmptyArea(pos) && IsDoubleClick(currentTime, pos))
                {
                    _viewModel.IsNavExpanded = true;
                    e.Handled = true;
                    return;
                }

                _lastClickTime = currentTime;
                _lastClickPosition = pos;
            }

            // Check if we're in a drag zone
            if (IsInDragZone(pos, currentWidth))
            {
                StartDrag(e);
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles mouse move event.
        /// </summary>
        public void HandleMouseMove(MouseEventArgs e)
        {
            if (_isDragging)
            {
                UpdateDragWidth(e.GetPosition(_navPanel));
            }
            else if (!_isAnimating)
            {
                UpdateCursor(e.GetPosition(_navPanel));
            }
        }

        /// <summary>
        /// Handles mouse left button up event.
        /// </summary>
        public void HandleMouseUp()
        {
            if (_isDragging)
            {
                EndDrag();
            }
        }

        /// <summary>
        /// Handles mouse leave event.
        /// </summary>
        public void HandleMouseLeave()
        {
            if (!_isDragging && !_isAnimating)
            {
                Mouse.OverrideCursor = null;
            }
        }

        /// <summary>
        /// Handles property changed event from ViewModel.
        /// </summary>
        public void OnViewModelPropertyChanged(string propertyName)
        {
            if (propertyName == nameof(MainViewModel.IsNavExpanded))
            {
                if (!_isDragging)
                {
                    AnimateWidth(_viewModel.IsNavExpanded);
                }
            }
        }

        /// <summary>
        /// Calculates the optimal expanded width based on content.
        /// </summary>
        private void CalculateOptimalWidth()
        {
            try
            {
                var scrollViewer = FindVisualChild<ScrollViewer>(_navPanel);
                if (scrollViewer?.Content is StackPanel stackPanel)
                {
                    double maxWidth = 0;

                    foreach (var child in stackPanel.Children)
                    {
                        if (child is Button button)
                        {
                            button.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                            maxWidth = Math.Max(maxWidth, button.DesiredSize.Width);
                        }
                    }

                    maxWidth += NavigationConstants.WidthCalculationPadding;
                    _calculatedExpandedWidth = Math.Max(
                        NavigationConstants.MinExpandedWidth,
                        Math.Min(NavigationConstants.MaxExpandedWidth, maxWidth));
                }
                else
                {
                    _calculatedExpandedWidth = NavigationConstants.DefaultExpandedWidth;
                }
            }
            catch
            {
                _calculatedExpandedWidth = NavigationConstants.DefaultExpandedWidth;
            }
        }

        /// <summary>
        /// Checks if a click is on an empty area (not on a button).
        /// </summary>
        private bool IsClickOnEmptyArea(Point position)
        {
            var hitElement = _navPanel.InputHitTest(position);
            var hitDependencyObject = hitElement as DependencyObject;
            return !(hitElement is Button || IsChildOfButton(hitDependencyObject));
        }

        /// <summary>
        /// Checks if two clicks constitute a double-click.
        /// </summary>
        private bool IsDoubleClick(DateTime currentTime, Point currentPosition)
        {
            var timeSinceLastClick = (currentTime - _lastClickTime).TotalMilliseconds;
            var distance = Math.Sqrt(
                Math.Pow(currentPosition.X - _lastClickPosition.X, 2) +
                Math.Pow(currentPosition.Y - _lastClickPosition.Y, 2));

            return timeSinceLastClick < NavigationConstants.DoubleClickTimeThresholdMs &&
                   distance < NavigationConstants.DoubleClickDistanceThreshold;
        }

        /// <summary>
        /// Checks if a position is in a drag zone.
        /// </summary>
        private bool IsInDragZone(Point position, double currentWidth)
        {
            if (currentWidth > NavigationConstants.CollapsedWidth + NavigationConstants.CollapsedTolerance)
            {
                // Expanded: drag from right edge
                return position.X >= _navPanel.ActualWidth - NavigationConstants.DragZoneWidth;
            }
            else
            {
                // Collapsed: drag from entire panel except buttons
                return IsClickOnEmptyArea(position);
            }
        }

        /// <summary>
        /// Checks if an element is a child of a Button.
        /// </summary>
        private bool IsChildOfButton(DependencyObject? element)
        {
            if (element == null) return false;

            var parent = VisualTreeHelper.GetParent(element);
            while (parent != null)
            {
                if (parent is Button) return true;
                parent = VisualTreeHelper.GetParent(parent);
            }
            return false;
        }

        /// <summary>
        /// Starts the drag operation.
        /// </summary>
        private void StartDrag(MouseButtonEventArgs e)
        {
            _isDragging = true;
            _dragStartPoint = e.GetPosition(_navPanel);
            _dragStartWidth = _navColumn.ActualWidth;
            _navPanel.CaptureMouse();
            Mouse.OverrideCursor = Cursors.SizeWE;
        }

        /// <summary>
        /// Updates the width during drag.
        /// </summary>
        private void UpdateDragWidth(Point currentPoint)
        {
            var deltaX = currentPoint.X - _dragStartPoint.X;
            var newWidth = Math.Max(
                NavigationConstants.CollapsedWidth,
                Math.Min(_calculatedExpandedWidth, _dragStartWidth + deltaX));

            _navColumn.BeginAnimation(ColumnDefinition.WidthProperty, null);
            _navColumn.Width = new GridLength(newWidth);

            // Update ViewModel state in real-time
            var shouldShowExpanded = newWidth > NavigationConstants.SnapThreshold;
            if (_viewModel.IsNavExpanded != shouldShowExpanded)
            {
                _viewModel.IsNavExpanded = shouldShowExpanded;
            }
        }

        /// <summary>
        /// Ends the drag operation and snaps to appropriate state.
        /// </summary>
        private void EndDrag()
        {
            _isDragging = false;
            _navPanel.ReleaseMouseCapture();
            Mouse.OverrideCursor = null;

            var finalWidth = _navColumn.ActualWidth;
            var shouldExpand = finalWidth >= NavigationConstants.SnapThreshold;

            _viewModel.IsNavExpanded = shouldExpand;
            AnimateWidth(shouldExpand);
        }

        /// <summary>
        /// Updates the cursor based on position.
        /// </summary>
        private void UpdateCursor(Point position)
        {
            var currentWidth = _navColumn.ActualWidth;

            if (currentWidth > NavigationConstants.CollapsedWidth + NavigationConstants.CollapsedTolerance)
            {
                // Expanded: resize cursor at right edge
                if (position.X >= _navPanel.ActualWidth - NavigationConstants.DragZoneWidth)
                {
                    Mouse.OverrideCursor = Cursors.SizeWE;
                    return;
                }
            }
            else if (IsClickOnEmptyArea(position))
            {
                // Collapsed: resize cursor on empty area
                Mouse.OverrideCursor = Cursors.SizeWE;
                return;
            }

            Mouse.OverrideCursor = null;
        }

        /// <summary>
        /// Animates the navigation panel width to the target state.
        /// </summary>
        private void AnimateWidth(bool expanded)
        {
            var targetWidth = expanded ? _calculatedExpandedWidth : NavigationConstants.CollapsedWidth;
            var currentWidth = _navColumn.ActualWidth;

            if (Math.Abs(currentWidth - targetWidth) < NavigationConstants.WidthTolerance)
            {
                UpdateWidth(targetWidth);
                return;
            }

            _navColumn.BeginAnimation(ColumnDefinition.WidthProperty, null);

            if (_navColumn.Width.IsAuto || _navColumn.Width.IsStar)
            {
                _navColumn.Width = new GridLength(currentWidth);
            }

            _isAnimating = true;

            var animation = new DoubleAnimation
            {
                From = currentWidth,
                To = targetWidth,
                Duration = TimeSpan.FromMilliseconds(NavigationConstants.AnimationDurationMs),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
            };

            animation.Completed += (s, e) =>
            {
                _isAnimating = false;
                _navColumn.BeginAnimation(ColumnDefinition.WidthProperty, null);
                UpdateWidth(targetWidth);
            };

            _navColumn.BeginAnimation(ColumnDefinition.WidthProperty, animation);
        }

        /// <summary>
        /// Updates the width without animation.
        /// </summary>
        private void UpdateWidth(double width)
        {
            _navColumn.Width = new GridLength(width);
        }

        /// <summary>
        /// Attaches event handlers to the navigation panel.
        /// </summary>
        private void AttachEventHandlers()
        {
            _navPanel.MouseLeftButtonDown += (s, e) => HandleMouseDown(e);
            _navPanel.MouseMove += (s, e) => HandleMouseMove(e);
            _navPanel.MouseLeftButtonUp += (s, e) => HandleMouseUp();
            _navPanel.MouseLeave += (s, e) => HandleMouseLeave();
        }

        /// <summary>
        /// Finds a visual child of the specified type.
        /// </summary>
        private T? FindVisualChild<T>(DependencyObject? parent) where T : DependencyObject
        {
            if (parent == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T found) return found;

                var childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null) return childOfChild;
            }

            return null;
        }
    }
}

