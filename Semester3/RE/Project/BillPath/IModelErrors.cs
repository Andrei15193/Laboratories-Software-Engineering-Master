using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace BillPath
{
    public interface IModelErrors
        : INotifyPropertyChanged, IEnumerable<string>, IReadOnlyDictionary<string, ReadOnlyObservableCollection<string>>, INotifyCollectionChanged
    {
        IEnumerable<string> EnumerateAll();
    }
}