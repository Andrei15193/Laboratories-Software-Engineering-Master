using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
namespace Tourist.ViewModels
{
    public interface IReadOnlyDataPropertyViewModel<TViewModelProperty>
        : INotifyPropertyChanged
    {
        TViewModelProperty Value
        {
            get;
        }
        ReadOnlyObservableCollection<string> Errors
        {
            get;
        }
    }

    public interface IDataPropertyViewModel<TViewModelProperty>
        : IReadOnlyDataPropertyViewModel<TViewModelProperty>
    {
        new TViewModelProperty Value
        {
            get;
            set;
        }
    }

    public interface ICollectionPropertyViewModel<TItem>
        : IReadOnlyDataPropertyViewModel<ReadOnlyObservableCollection<TItem>>
    {
        ICommand AddItemCommand
        {
            get;
        }
        ICommand RemoveItemCommand
        {
            get;
        }
        ICommand ClearItemsCommand
        {
            get;
        }
    }
}