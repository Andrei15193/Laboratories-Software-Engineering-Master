using System;
using System.Windows.Input;
using BillPath.UserInterface.ViewModels;
using Windows.UI.Xaml.Data;

namespace BillPath.Modern.Converters
{
    public class CommandConverter
        : IValueConverter
    {
        private class CommandAdapter
            : ICommand
        {
            private readonly AsyncCommand _asyncComand;

            public CommandAdapter(AsyncCommand asyncComand)
            {
                if (asyncComand == null)
                    throw new ArgumentNullException(nameof(asyncComand));

                _asyncComand = asyncComand;
                _asyncComand.PropertyChanged += (sender, e) => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }

            public bool CanExecute(object parameter)
            {
                return _asyncComand.CanExecute;
            }
            public event EventHandler CanExecuteChanged;

            public async void Execute(object parameter)
            {
                await _asyncComand.ExecuteAsync(parameter);
            }
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return new CommandAdapter((AsyncCommand)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}