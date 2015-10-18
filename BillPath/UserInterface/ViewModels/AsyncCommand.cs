using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace BillPath.UserInterface.ViewModels
{
    public abstract class AsyncCommand
        : ViewModel
    {
        public class CancelAsyncCommand
            : Command
        {
            private readonly AsyncCommand _asyncCommand;

            internal CancelAsyncCommand(AsyncCommand asyncCommand)
            {
                if (asyncCommand == null)
                    throw new ArgumentNullException(nameof(asyncCommand));

                CanExecute = false;
                _asyncCommand = asyncCommand;
                _asyncCommand.PropertyChanged += _AsyncCommandPropertyChanged;
            }
            private void _AsyncCommandPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (nameof(Executing).Equals(e?.PropertyName, StringComparison.OrdinalIgnoreCase))
                    CanExecute = _asyncCommand.Executing;
            }

            protected override void OnExecute(object parameter)
            {
                _asyncCommand._cancellationTokenSource.Cancel();
                CanExecute = false;
            }
        }

        private bool _canExecute;
        private bool _executing;
        private CancellationTokenSource _cancellationTokenSource;

        public AsyncCommand()
        {
            _canExecute = true;
            _executing = false;
            _cancellationTokenSource = null;
            CancelCommand = new CancelAsyncCommand(this);
        }

        public bool CanExecute
        {
            get
            {
                return (_canExecute && !_executing);
            }
            protected set
            {
                if (CanExecute == value)
                    _canExecute = value;
                else
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

        public bool Executing
        {
            get
            {
                return _executing;
            }
            private set
            {
                _executing = value;
                if (value)
                {
                    OnCanExecuteChanged(EventArgs.Empty);
                    OnExecutingChanged(EventArgs.Empty);
                }
                else
                {
                    OnExecutingChanged(EventArgs.Empty);
                    OnCanExecuteChanged(EventArgs.Empty);
                }
            }
        }
        protected virtual void OnExecutingChanged(EventArgs eventArgs)
        {
            OnPropertyChanged(nameof(Executing));
        }

        public bool Canceling
        {
            get
            {
                return (_cancellationTokenSource != null && _cancellationTokenSource.IsCancellationRequested);
            }
        }
        protected CancellationToken CancellationToken
        {
            get
            {
                return _cancellationTokenSource.Token;
            }
        }

        public Command CancelCommand
        {
            get;
        }

        public async Task ExecuteAsync(object parameter)
        {
            if (!CanExecute)
                throw new InvalidOperationException();

            try
            {
                Executing = true;
                using (_cancellationTokenSource = new CancellationTokenSource())
                    await OnExecuteAsync(parameter);
            }
            finally
            {
                Executing = false;
                _cancellationTokenSource = null;
            }
        }

        protected abstract Task OnExecuteAsync(object parameter);
    }
    public abstract class AsyncCommand<TParameter>
        : AsyncCommand
    {
        protected sealed override Task OnExecuteAsync(object parameter)
        {
            if (parameter is TParameter)
                return OnExecuteAsync((TParameter)parameter);
            else
                return OnExecuteAsync(Convert.ChangeType(parameter, typeof(TParameter)));
        }
        protected abstract Task OnExecuteAsync(TParameter parameter);
    }
}