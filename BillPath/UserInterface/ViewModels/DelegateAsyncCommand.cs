using System;
using System.Threading;
using System.Threading.Tasks;

namespace BillPath.UserInterface.ViewModels
{
    public sealed class DelegateAsyncCommand
        : AsyncCommand
    {
        public delegate Task Callback(object parameter, CancellationToken cancellationToken);

        private readonly Callback _callback;

        public DelegateAsyncCommand(Callback callback)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            _callback = callback;
        }

        new public bool CanExecute
        {
            get
            {
                return base.CanExecute;
            }
            set
            {
                base.CanExecute = value;
            }
        }

        protected override Task OnExecuteAsync(object parameter)
        {
            return _callback(parameter, CancellationToken);
        }
    }
    public sealed class DelegateAsyncCommand<TParameter>
        : AsyncCommand<TParameter>
    {
        public delegate Task Callback(TParameter parameter, CancellationToken cancellationToken);

        private readonly Callback _callback;

        public DelegateAsyncCommand(Callback callback)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            _callback = callback;
        }

        protected override Task OnExecuteAsync(TParameter parameter)
        {
            return _callback(parameter, CancellationToken);
        }
    }
}