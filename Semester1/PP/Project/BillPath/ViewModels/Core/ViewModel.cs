using System;
using System.ComponentModel;

namespace BillPath.ViewModels.Core
{
    public class ViewModel
        : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler eventHandler = PropertyChanged;
            if (eventHandler != null)
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public interface IViewModel<out TModel>
        : INotifyPropertyChanged
    {
        TModel Model
        {
            get;
        }
    }

    public class ViewModel<TModel>
        : ViewModel, IViewModel<TModel>, INotifyPropertyChanged
    {
        public ViewModel(TModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            Model = model;
        }

        public TModel Model
        {
            get;
            private set;
        }
    }
}