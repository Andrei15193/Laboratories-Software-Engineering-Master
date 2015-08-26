using System;

namespace BillPath.UserInterface.ViewModels
{
    public sealed class DelegateCommand
        : Command
    {
        public delegate void Callback(object parameter);

        private readonly Callback _callback;

        public DelegateCommand(Callback callback)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            _callback = callback;
        }

        protected override void OnExecute(object parameter)
        {
            _callback(parameter);
        }
    }
    public sealed class DelegateCommand<TParameter>
        : Command<TParameter>
    {
        public delegate void Callback(TParameter parameter);

        private readonly Callback _callback;

        public DelegateCommand(Callback callback)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            _callback = callback;
        }

        protected override void OnExecute(TParameter parameter)
        {
            _callback(parameter);
        }
    }
}