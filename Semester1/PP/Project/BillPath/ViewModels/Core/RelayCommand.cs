using System;
using System.Windows.Input;

namespace BillPath.ViewModels.Core
{
    public class RelayCommand
        : ICommand
    {
        public RelayCommand(Action<object> execute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = null;
        }
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute, Action<EventArgs> canExecuteChanged)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            if (canExecuteChanged == null)
                throw new ArgumentNullException("canExecuteChanged");
            if (canExecute == null)
                throw new ArgumentNullException("canExecute");

            _execute = execute;
            _canExecute = canExecute;
            canExecuteChanged += OnCanExecuteChanged;
        }

        public bool CanExecute(object parameter)
        {
            return (_canExecute == null || _canExecute(parameter));
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        protected virtual void OnCanExecuteChanged(EventArgs eventArgs)
        {
            EventHandler eventHandler = CanExecuteChanged;

            if (eventHandler != null)
                eventHandler(this, eventArgs);
        }

        private Action<object> _execute;
        private Func<object, bool> _canExecute;
    }

    public class RelayCommand<TParameter>
        : ICommand
    {
        public RelayCommand(Action<TParameter> execute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = null;
        }
        public RelayCommand(Action<TParameter> execute, Func<TParameter, bool> canExecute, Action<EventArgs> canExecuteChanged)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            if (canExecuteChanged == null)
                throw new ArgumentNullException("canExecuteChanged");
            if (canExecute == null)
                throw new ArgumentNullException("canExecute");

            _execute = execute;
            _canExecute = canExecute;
            canExecuteChanged += OnCanExecuteChanged;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(TParameter parameter)
        {
            return (_canExecute == null || _canExecute(parameter));
        }

        public void Execute(TParameter parameter)
        {
            _execute(parameter);
        }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute((TParameter)parameter);
        }

        void ICommand.Execute(object parameter)
        {
            Execute((TParameter)parameter);
        }

        protected virtual void OnCanExecuteChanged(EventArgs eventArgs)
        {
            EventHandler eventHandler = CanExecuteChanged;

            if (eventHandler != null)
                eventHandler(this, eventArgs);
        }

        private readonly Action<TParameter> _execute;
        private readonly Func<TParameter, bool> _canExecute;
    }
}