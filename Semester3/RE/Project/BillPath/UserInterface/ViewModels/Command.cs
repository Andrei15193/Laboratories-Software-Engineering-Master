using System;

namespace BillPath.UserInterface.ViewModels
{
    public abstract class Command
        : ViewModel
    {
        private bool _canExecute = true;

        public bool CanExecute
        {
            get
            {
                return _canExecute;
            }
            protected set
            {
                if (CanExecute != value)
                {
                    _canExecute = value;
                    OnCanExecuteChanged(EventArgs.Empty);
                }
            }
        }
        protected virtual void OnCanExecuteChanged(EventArgs eventArgs)
        {
            OnPropertyChanged(nameof(CanExecute));
        }

        public void Execute(object parameter)
        {
            if (!CanExecute)
                throw new InvalidOperationException();

            OnExecute(parameter);
        }

        protected abstract void OnExecute(object parameter);
    }
    public abstract class Command<TParameter>
        : Command
    {
        protected sealed override void OnExecute(object parameter)
        {
            if (parameter is TParameter)
                OnExecute((TParameter)parameter);
            else
                OnExecute(Convert.ChangeType(parameter, typeof(TParameter)));
        }
        protected abstract void OnExecute(TParameter parameter);
    }
}