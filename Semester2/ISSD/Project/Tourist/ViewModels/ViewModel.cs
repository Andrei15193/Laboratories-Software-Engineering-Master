using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace Tourist.ViewModels
{
    public class ViewModel
        : INotifyPropertyChanged
    {
        protected ViewModel()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            var eventHandler = PropertyChanged;
            if (eventHandler != null)
                eventHandler(this, eventArgs);
        }
    }
}