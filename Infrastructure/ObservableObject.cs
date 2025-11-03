using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TechDashboard.Infrastructure
{
    /// <summary>
    /// 可观察对象基类：实现 INotifyPropertyChanged，提供 SetProperty 工具方法。
    /// </summary>
    [Serializable]
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        [field: NonSerialized]
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null, Action? action = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName ?? string.Empty));
            action?.Invoke();
        }

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
