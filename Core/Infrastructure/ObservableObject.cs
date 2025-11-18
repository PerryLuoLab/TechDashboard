using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TechDashboard.Core.Infrastructure
{
    /// <summary>
    /// Base class for observable objects that implement INotifyPropertyChanged interface.
    /// Provides SetProperty utility method for property change notifications.
    /// </summary>
    [Serializable]
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event for the specified property.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        /// <param name="action">Optional action to execute after raising the event.</param>
        protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null, Action? action = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName ?? string.Empty));
            action?.Invoke();
        }

        /// <summary>
        /// Sets the property value and raises PropertyChanged event if the value has changed.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="field">Reference to the backing field.</param>
        /// <param name="value">The new value to set.</param>
        /// <param name="propertyName">The name of the property (automatically captured).</param>
        /// <param name="action">Optional action to execute after raising the event.</param>
        protected virtual void SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null, Action? action = null)
        {
            if (!Equals(field, value))
            {
                field = value;
                RaisePropertyChanged(propertyName, action);
            }
        }
    }
}
