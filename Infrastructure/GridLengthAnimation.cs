using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace TechDashboard.Infrastructure
{
    /// <summary>
    /// Animation class for GridLength properties
    /// </summary>
    public class GridLengthAnimation : AnimationTimeline
    {
        /// <summary>
        /// Gets or sets the starting value of the animation
        /// </summary>
        public GridLength From
        {
            get => (GridLength)GetValue(FromProperty);
            set => SetValue(FromProperty, value);
        }

        public static readonly DependencyProperty FromProperty =
            DependencyProperty.Register(nameof(From), typeof(GridLength), 
                typeof(GridLengthAnimation));

        /// <summary>
        /// Gets or sets the ending value of the animation
        /// </summary>
        public GridLength To
        {
            get => (GridLength)GetValue(ToProperty);
            set => SetValue(ToProperty, value);
        }

        public static readonly DependencyProperty ToProperty =
            DependencyProperty.Register(nameof(To), typeof(GridLength), 
                typeof(GridLengthAnimation));

        /// <summary>
        /// Gets or sets the easing function for the animation
        /// </summary>
        public IEasingFunction? EasingFunction { get; set; }

        public override Type TargetPropertyType => typeof(GridLength);

        protected override Freezable CreateInstanceCore()
        {
            return new GridLengthAnimation();
        }

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
