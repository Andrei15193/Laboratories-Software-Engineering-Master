using System;
using System.Windows.Input;

namespace BillPath.ViewModels.Core
{
    internal abstract class Command<TViewModel>
        : ICommand
    {
        public Command(TViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException("viewModel");

            _viewModel = viewModel;
        }

        public abstract bool CanExecute(object parameter);
        public abstract void Execute(object parameter);

        public event EventHandler CanExecuteChanged;

        protected TViewModel ViewModel
        {
            get
            {
                return _viewModel;
            }
        }

        protected virtual void OnCanExecuteChanged()
        {
            EventHandler eventHandler = CanExecuteChanged;
            if (eventHandler != null)
                eventHandler(this, EventArgs.Empty);
        }

        private readonly TViewModel _viewModel;
    }


    internal abstract class Command<TViewModel, TParameter>
        : Command<TViewModel>
    {
        public Command(TViewModel viewModel)
            : base(viewModel)
        {
        }

        public abstract bool CanExecute(TParameter parameter);
        public abstract void Execute(TParameter parameter);

        public override bool CanExecute(object parameter)
        {
            return CanExecute((TParameter)parameter);
        }

        public override void Execute(object parameter)
        {
            Execute((TParameter)parameter);
        }
    }
}