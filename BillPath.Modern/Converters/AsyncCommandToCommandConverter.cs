using System;
using System.ComponentModel;
using System.Windows.Input;
using BillPath.UserInterface.ViewModels;

namespace BillPath.Modern.Converters
{
    public class AsyncCommandToCommandConverter
    {
        public class CommandAdapter
            : ICommand
        {
            private readonly AsyncCommand _asyncCommand;

            public CommandAdapter(AsyncCommand asyncCommand)
            {
                if (asyncCommand == null)
                    throw new ArgumentNullException(nameof(asyncCommand));

                _asyncCommand = asyncCommand;
                _asyncCommand.PropertyChanged += _AsyncCommandPropertyChanged;
            }
            private void _AsyncCommandPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (nameof(AsyncCommand.CanExecute).Equals(e?.PropertyName, StringComparison.OrdinalIgnoreCase))
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return _asyncCommand.CanExecute;
            }

            public async void Execute(object parameter)
            {
                await _asyncCommand.ExecuteAsync(parameter);
            }
        }
    }
}