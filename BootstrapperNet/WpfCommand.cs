using System;
using System.Windows.Input;

namespace BootstrapperNet
{
    /// <summary>
    /// <see cref="ICommand"/> implementation class
    /// </summary>
    public class WpfCommand : ICommand
    {
        private Func<object?, bool>? _canExecuteFunc { get; set; }
        private Action<object?>? _executeAction { get; set; }


        #region Constructors

        /// <summary>
        /// Constructor for <see cref="WpfCommand"/>
        /// </summary>
        /// <param name="canExecuteFunc">CanExecute method for the command</param>
        /// <param name="executeAction">Execute method for the command</param>
        public WpfCommand(Func<object?, bool> canExecuteFunc, Action<object?> executeAction)
        {
            _canExecuteFunc = canExecuteFunc;
            _executeAction = executeAction;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the CanExecute of the command
        /// </summary>
        public void UpdateCanExecute() =>
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        #endregion

        #region ICommand implementation

        /// <inheritdoc/>
        public event EventHandler? CanExecuteChanged;

        /// <inheritdoc/>
        public virtual bool CanExecute(object? parameter) =>
            _canExecuteFunc?.Invoke(parameter) ?? false;

        /// <inheritdoc/>
        public virtual void Execute(object? parameter) =>
            _executeAction?.Invoke(parameter);

        #endregion
    }
}
