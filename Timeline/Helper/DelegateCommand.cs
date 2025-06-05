using System;
using System.Windows.Input;

namespace Timeline.Helper
{
    class DelegateCommand<T> : ICommand
    {
        #region Member
        public event EventHandler CanExecuteChanged;

        readonly Func<T, bool> _canExecute;
        readonly Action<T> _executeAction;
        bool _canExecuteCache;

        #endregion

        public DelegateCommand(Action<T> executeAction, Func<T, bool> canExecute)
        {
            _executeAction = executeAction;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            bool temp = _canExecute((T)parameter);

            if (_canExecuteCache != temp)
            {
                _canExecuteCache = temp;

                CanExecuteChanged?.Invoke(this, new EventArgs());
            }

            return _canExecuteCache;
        }

        public void Execute(object parameter)
        {
            _executeAction?.Invoke((T)parameter);
        }
    }
}
