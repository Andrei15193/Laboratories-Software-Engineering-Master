using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
namespace Tourist.ViewModels
{
    public class DataViewModel<TDataModel>
        : ViewModel
    {
        private class DataPropertyViewModel<TViewModelProperty>
            : ViewModel, IDataPropertyViewModel<TViewModelProperty>
        {
            private readonly ObservableCollection<string> _errors;

            private readonly DataViewModel<TDataModel> _dataViewModel;
            private readonly string _propertyName;
            private readonly Func<TViewModelProperty> _propertyGetter;
            private readonly Action<TViewModelProperty> _propertySetter;
            private readonly string _setErrorMessage;

            internal DataPropertyViewModel(DataViewModel<TDataModel> dataViewModel, string propertyName, Func<TViewModelProperty> propertyGetter, Action<TViewModelProperty> propertySetter, string setErrorMessage = null)
            {
                if (dataViewModel == null)
                    throw new ArgumentNullException("dataViewModel");
                if (string.IsNullOrWhiteSpace(propertyName))
                    if (propertyName == null)
                        throw new ArgumentNullException("propertyName");
                    else
                        throw new ArgumentException("Cannot be empty or white space!", "propertyName");
                if (propertyGetter == null)
                    throw new ArgumentNullException("propertyGetter");
                if (propertySetter == null)
                    throw new ArgumentNullException("propertySetter");

                _errors = new ObservableCollection<string>();
                Errors = new ReadOnlyObservableCollection<string>(_errors);

                _dataViewModel = dataViewModel;
                _propertyName = propertyName;
                _propertyGetter = propertyGetter;
                _propertySetter = propertySetter;
                _setErrorMessage = setErrorMessage;

                var dataErrorInfo = _dataViewModel.DataModel as INotifyDataErrorInfo;
                if (dataErrorInfo != null)
                {
                    dataErrorInfo.ErrorsChanged += _DataModelErrorsChanged;
                    _SetDataModelErrors();
                }
            }
            private void _DataModelErrorsChanged(object sender, DataErrorsChangedEventArgs e)
            {
                if (e.PropertyName == null || _propertyName.Equals(e.PropertyName, StringComparison.OrdinalIgnoreCase))
                {
                    _errors.Clear();
                    _SetDataModelErrors();
                }
            }

            public TViewModelProperty Value
            {
                get
                {
                    return _propertyGetter();
                }
                set
                {
                    try
                    {
                        _errors.Clear();
                        _propertySetter(value);
                    }
                    catch (Exception exception)
                    {
                        if (string.IsNullOrWhiteSpace(_setErrorMessage))
                            _errors.Add(exception.Message);
                        else
                            _errors.Add(_setErrorMessage);
                    }
                    finally
                    {
                        OnPropertyChanged();
                    }
                }
            }
            private void _SetDataModelErrors()
            {
                _errors.Clear();
                var dataErrorInfo = _dataViewModel.DataModel as INotifyDataErrorInfo;
                if (dataErrorInfo != null)
                    foreach (var error in dataErrorInfo.GetErrors(_propertyName).OfType<string>())
                        if (!string.IsNullOrWhiteSpace(error))
                            _errors.Add(error);
            }

            public ReadOnlyObservableCollection<string> Errors
            {
                get;
                private set;
            }
        }
        private class CollectionPropertyViewModel<TItem>
            : ViewModel, ICollectionPropertyViewModel<TItem>
        {
            private readonly DataViewModel<TDataModel> _dataViewModel;
            private readonly ObservableCollection<string> _errors;
            private readonly string _propertyName;

            public CollectionPropertyViewModel(DataViewModel<TDataModel> dataViewModel, string propertyName, ICollection<TItem> collection)
            {
                if (dataViewModel == null)
                    throw new ArgumentNullException("dataViewModel");
                if (string.IsNullOrWhiteSpace(propertyName))
                    if (propertyName == null)
                        throw new ArgumentNullException("propertyName");
                    else
                        throw new ArgumentException("Cannot be empty or white space!", "propertyName");

                _dataViewModel = dataViewModel;
                _propertyName = propertyName;

                var observableCollection = new ObservableCollection<TItem>(collection);
                Value = new ReadOnlyObservableCollection<TItem>(observableCollection);
                observableCollection.CollectionChanged += delegate { OnPropertyChanged("Value"); };

                var aggregateCollection = new AggregateCollection(observableCollection, collection);
                AddItemCommand = new Commands.AddItemCommand(aggregateCollection);
                RemoveItemCommand = new Commands.RemoveItemCommand(aggregateCollection);
                ClearItemsCommand = new Commands.ClearItemsCommand(aggregateCollection);

                _errors = new ObservableCollection<string>();
                Errors = new ReadOnlyObservableCollection<string>(_errors);

                var dataErrorInfo = _dataViewModel.DataModel as INotifyDataErrorInfo;
                if (dataErrorInfo != null)
                {
                    dataErrorInfo.ErrorsChanged += _DataModelErrorsChanged;
                    _SetDataModelErrors();
                }
            }

            public ReadOnlyObservableCollection<TItem> Value
            {
                get;
                private set;
            }

            public ICommand AddItemCommand
            {
                get;
                private set;
            }
            public ICommand RemoveItemCommand
            {
                get;
                private set;
            }
            public ICommand ClearItemsCommand
            {
                get;
                private set;
            }

            public ReadOnlyObservableCollection<string> Errors
            {
                get;
                private set;
            }

            private void _DataModelErrorsChanged(object sender, DataErrorsChangedEventArgs e)
            {
                if (e.PropertyName == null || _propertyName.Equals(e.PropertyName, StringComparison.OrdinalIgnoreCase))
                {
                    _errors.Clear();
                    _SetDataModelErrors();
                }
            }
            private void _SetDataModelErrors()
            {
                _errors.Clear();
                var dataErrorInfo = _dataViewModel.DataModel as INotifyDataErrorInfo;
                if (dataErrorInfo != null)
                    foreach (var error in dataErrorInfo.GetErrors(_propertyName).OfType<string>())
                        if (!string.IsNullOrWhiteSpace(error))
                            _errors.Add(error);
            }

            private static class Commands
            {
                internal class AddItemCommand
                    : Command<TItem>
                {
                    private readonly AggregateCollection _aggregateCollection;

                    public AddItemCommand(AggregateCollection aggregateCollection)
                    {
                        if (aggregateCollection == null)
                            throw new ArgumentNullException("aggregateCollection");

                        _aggregateCollection = aggregateCollection;
                        CanExecuteCommand = true;
                    }

                    public override void Execute(TItem parameter)
                    {
                        _aggregateCollection.Add(parameter);
                    }
                }
                internal class RemoveItemCommand
                    : Command<TItem>
                {
                    private readonly AggregateCollection _aggregateCollection;

                    public RemoveItemCommand(AggregateCollection aggregateCollection)
                    {
                        if (aggregateCollection == null)
                            throw new ArgumentNullException("aggregateCollection");

                        _aggregateCollection = aggregateCollection;
                        CanExecuteCommand = true;
                    }

                    public override void Execute(TItem parameter)
                    {
                        _aggregateCollection.Remove(parameter);
                    }
                }
                internal class ClearItemsCommand
                    : Command<object>
                {
                    private readonly AggregateCollection _aggregateCollection;

                    public ClearItemsCommand(AggregateCollection aggregateCollection)
                    {
                        if (aggregateCollection == null)
                            throw new ArgumentNullException("aggregateCollection");

                        _aggregateCollection = aggregateCollection;
                        CanExecuteCommand = true;
                    }

                    public override void Execute(object parameter)
                    {
                        _aggregateCollection.Clear();
                    }
                }
            }

            private sealed class AggregateCollection
            {
                private readonly IEnumerable<ICollection<TItem>> _collections;

                public AggregateCollection(IEnumerable<ICollection<TItem>> collections)
                {
                    if (collections == null)
                        throw new ArgumentNullException("collections");

                    _collections = collections.Where(collection => collection != null);
                }
                public AggregateCollection(params ICollection<TItem>[] collections)
                    : this(collections.AsEnumerable())
                {
                }

                public void Add(TItem item)
                {
                    foreach (var collection in _collections)
                        collection.Add(item);
                }

                public void Clear()
                {
                    foreach (var collection in _collections)
                        collection.Clear();
                }

                public void Remove(TItem item)
                {
                    foreach (var collection in _collections)
                        collection.Remove(item);
                }
            }
        }

        private readonly TDataModel _dataModel;

        protected DataViewModel(TDataModel dataModel)
        {
            if (dataModel == null)
                throw new ArgumentNullException("dataModel");

            _dataModel = dataModel;
        }

        protected TDataModel DataModel
        {
            get
            {
                return _dataModel;
            }
        }

        protected IDataPropertyViewModel<TViewModelProperty> GetPropertyViewModel<TViewModelProperty>(string propertyName, Func<TViewModelProperty> propertyGetter, Action<TViewModelProperty> propertySetter, string setErrorMessage)
        {
            return new DataPropertyViewModel<TViewModelProperty>(this, propertyName, propertyGetter, propertySetter, setErrorMessage);
        }
        protected IDataPropertyViewModel<TViewModelProperty> GetPropertyViewModel<TViewModelProperty>(string propertyName, Func<TViewModelProperty> propertyGetter, Action<TViewModelProperty> propertySetter)
        {
            return GetPropertyViewModel(propertyName, propertyGetter, propertySetter, null);
        }

        protected ICollectionPropertyViewModel<TItem> GetCollectionPropertyViewModel<TItem>(string propertyName, ICollection<TItem> collection)
        {
            return new CollectionPropertyViewModel<TItem>(this, propertyName, collection);
        }
    }
}