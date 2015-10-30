using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BillPath.UserInterface.ViewModels
{
    public class BindingSource
        : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs == null)
                throw new ArgumentNullException(nameof(propertyChangedEventArgs));

            PropertyChanged?.Invoke(this, propertyChangedEventArgs);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }
}