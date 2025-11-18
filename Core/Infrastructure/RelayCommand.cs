using System;
using System.Windows.Input;

namespace TechDashboard.Core.Infrastructure
{
    /// <summary>
    /// A reusable implementation of the ICommand interface for WPF applications.
    /// Allows executing an action with optional execution condition checking.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The action to execute when the command is invoked.</param>
        /// <param name="canExecute">Optional predicate to determine if the command can execute.</param>
        /// <exception cref="ArgumentNullException">Thrown if execute is null.</exception>
        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.</param>
        /// <returns>True if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

        /// <summary>
        /// Executes the command action.
        /// </summary>
        /// <param name="parameter">Data used by the command.</param>
        public void Execute(object? parameter) => _execute(parameter);

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// Raises the CanExecuteChanged event to notify WPF that the command state has changed.
        /// </summary>
        public void RaiseCanExecuteChanged() => CommandManager.InvalidateRequerySuggested();
    }
}
