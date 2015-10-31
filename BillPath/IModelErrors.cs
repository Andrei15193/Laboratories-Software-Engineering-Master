using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Dynamic;

namespace BillPath
{
    public interface IModelErrors
        : IDynamicMetaObjectProvider, INotifyPropertyChanged, IReadOnlyList<string>, INotifyCollectionChanged
    {
        IEnumerable<string> EnumerateAll();
    }
}