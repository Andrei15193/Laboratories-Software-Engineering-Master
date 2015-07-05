using System;
using System.Windows.Input;
namespace Tourist.ViewModels
{
    public abstract class Command<TParameter>
        : ICommand
    {
        private bool _canExecute;

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute((TParameter)parameter);
        }
        void ICommand.Execute(object parameter)
        {
            Execute((TParameter)parameter);
        }

        public virtual bool CanExecute(TParameter parameter)
        {
            return CanExecuteCommand;
        }
        public abstract void Execute(TParameter parameter);

        public event EventHandler CanExecuteChanged;
        protected void OnCanExecuteChanged(EventArgs eventArgs)
        {
            EventHandler eventHandler = CanExecuteChanged;
            if (eventHandler != null)
                eventHandler(this, eventArgs);
        }
        protected bool CanExecuteCommand
        {
            get
            {
                return _canExecute;
            }
            set
            {
                _canExecute = value;
                OnCanExecuteChanged(EventArgs.Empty);
            }
        }
    }
}