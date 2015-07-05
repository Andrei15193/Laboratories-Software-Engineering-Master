using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
namespace Tourist.ViewModels
{
    public abstract class AsyncCommand<TParameter>
        : ICommand, INotifyPropertyChanged
    {
        private bool _executing = false;
        private bool _canExecute = true;
        private readonly Commands.CancelCommand _cancelCommand = new Commands.CancelCommand();

        public bool Executing
        {
            get
            {
                return _executing;
            }
            protected set
            {
                _executing = value;
                OnPropertyChanged();
                OnPropertyChanged("NotExecuting");
                OnCanExecuteChanged(EventArgs.Empty);
            }
        }
        public bool NotExecuting
        {
            get
            {
                return !Executing;
            }
        }

        bool ICommand.CanExecute(object parameter)
        {
            return (!Executing && CanExecute);
        }
        protected bool CanExecute
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
        public event EventHandler CanExecuteChanged;

        protected virtual void OnCanExecuteChanged(EventArgs eventArgs)
        {
            var eventHandler = CanExecuteChanged;
            if (eventHandler != null)
                eventHandler(this, eventArgs);
        }

        protected abstract Task ExecuteAsync(TParameter parameter, CancellationToken cancellationToken);
        public async Task ExecuteAsync(TParameter parameter)
        {
            var executeTask = default(Task);
            var cancellationTokenSource = default(CancellationTokenSource);

            try
            {
                Executing = true;

                cancellationTokenSource = new CancellationTokenSource();
                _cancelCommand.CancellationTokenSource = cancellationTokenSource;

                executeTask = ExecuteAsync(parameter, cancellationTokenSource.Token);
                await executeTask;

                OnFinished(EventArgs.Empty);
            }
            catch (OperationCanceledException)
            {
                OnCancelled(EventArgs.Empty);
            }
            catch
            {
                if (executeTask == null)
                    throw;

                OnFaulted(new TaskFaultedEventArgs(executeTask.Exception));
            }
            finally
            {
                _cancelCommand.CancellationTokenSource = null;
                cancellationTokenSource.Dispose();
                Executing = false;
            }
        }
        async void ICommand.Execute(object parameter)
        {
            await ExecuteAsync((TParameter)parameter);
        }
        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand;
            }
        }

        public event EventHandler Finished;
        public event EventHandler Cancelled;
        public event EventHandler<TaskFaultedEventArgs> Faulted;

        protected virtual void OnFinished(EventArgs eventArgs)
        {
            var eventHandler = Finished;
            if (eventHandler != null)
                eventHandler(this, eventArgs);
        }
        protected virtual void OnCancelled(EventArgs eventArgs)
        {
            var eventHandler = Cancelled;
            if (eventHandler != null)
                eventHandler(this, eventArgs);
        }
        protected virtual void OnFaulted(TaskFaultedEventArgs taskFaultedEventArgs)
        {
            var eventHandler = Faulted;
            if (eventHandler != null)
                eventHandler(this, taskFaultedEventArgs);
        }

        private static class Commands
        {
            internal class CancelCommand
                : ICommand
            {
                private CancellationTokenSource _cancellationTokenSource = null;

                internal CancellationTokenSource CancellationTokenSource
                {
                    get
                    {
                        return _cancellationTokenSource;
                    }
                    set
                    {
                        _cancellationTokenSource = value;
                        OnCanExecuteChanged(EventArgs.Empty);
                    }
                }

                public bool CanExecute(object parameter)
                {
                    return (_cancellationTokenSource != null);
                }
                public void Execute(object parameter)
                {
                    if (parameter == null)
                        _cancellationTokenSource.Cancel();
                    else if (parameter is int)
                        _cancellationTokenSource.CancelAfter((int)parameter);
                    else if (parameter is TimeSpan)
                        _cancellationTokenSource.CancelAfter((TimeSpan)parameter);
                    else
                        _cancellationTokenSource.CancelAfter(Convert.ToInt32(parameter));
                }

                public event EventHandler CanExecuteChanged;

                protected virtual void OnCanExecuteChanged(EventArgs eventArgs)
                {
                    var eventHandler = CanExecuteChanged;
                    if (eventHandler != null)
                        eventHandler(this, eventArgs);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var eventHandler = PropertyChanged;
            if (eventHandler != null)
                eventHandler(this, propertyChangedEventArgs);
        }
    }

    public abstract class AsyncCommand
        : AsyncCommand<object>
    {
    }
}