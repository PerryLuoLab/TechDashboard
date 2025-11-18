using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace TechDashboard.Core.Infrastructure
{
    /// <summary>
    /// Custom animation class for animating GridLength properties.
    /// Supports easing functions for smooth transitions between grid column/row sizes.
    /// </summary>
    public class GridLengthAnimation : AnimationTimeline
    {
        /// <summary>
        /// Dependency property for the starting value of the animation.
        /// </summary>
        public static readonly DependencyProperty FromProperty =
            DependencyProperty.Register(nameof(From), typeof(GridLength), 
                typeof(GridLengthAnimation));

        /// <summary>
        /// Dependency property for the ending value of the animation.
        /// </summary>
        public static readonly DependencyProperty ToProperty =
            DependencyProperty.Register(nameof(To), typeof(GridLength), 
                typeof(GridLengthAnimation));

        /// <summary>
        /// Gets or sets the starting value of the animation.
        /// </summary>
        public GridLength From
        {
            get => (GridLength)GetValue(FromProperty);
            set => SetValue(FromProperty, value);
        }

        /// <summary>
        /// Gets or sets the ending value of the animation.
        /// </summary>
        public GridLength To
        {
            get => (GridLength)GetValue(ToProperty);
            set => SetValue(ToProperty, value);
        }

        /// <summary>
        /// Gets or sets the easing function for the animation to create smooth, non-linear transitions.
        /// </summary>
        public IEasingFunction? EasingFunction { get; set; }

        /// <summary>
        /// Gets the type of property that can be animated by this animation class.
        /// </summary>
        public override Type TargetPropertyType => typeof(GridLength);

        /// <summary>
        /// Creates a new instance of the GridLengthAnimation class.
        /// </summary>
        /// <returns>A new instance of the animation.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new GridLengthAnimation();
        }

        /// <summary>
        /// Calculates the current value of the animation based on the progress.
        /// </summary>
        /// <param name="defaultOriginValue">The default origin value.</param>
        /// <param name="defaultDestinationValue">The default destination value.</param>
        /// <param name="animationClock">The animation clock providing timing information.</param>
        /// <returns>The current animated GridLength value.</returns>
        public override object GetCurrentValue(object defaultOriginValue, 
            object defaultDestinationValue, AnimationClock animationClock)
        {
            if (animationClock.CurrentProgress == null)
                return defaultOriginValue;

            var from = From.Value;
            var to = To.Value;

            var progress = animationClock.CurrentProgress.Value;
            
            // Apply easing function if available
            if (EasingFunction != null)
            {
                progress = EasingFunction.Ease(progress);
            }

            var currentValue = from + ((to - from) * progress);

            return new GridLength(currentValue, GridUnitType.Pixel);
        }
    }
}
