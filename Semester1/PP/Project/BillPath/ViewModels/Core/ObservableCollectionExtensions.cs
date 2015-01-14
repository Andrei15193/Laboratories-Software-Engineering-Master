using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BillPath.ViewModels.Core
{
    public static class ObservableCollectionExtensions
    {
        public static void AddRange<TItem>(this ObservableCollection<TItem> observableCollection, IEnumerable<TItem> items)
        {
            if (observableCollection == null)
                throw new ArgumentNullException("observableCollection");
            if (items == null)
                throw new ArgumentNullException("items");

            foreach (TItem item in items)
                observableCollection.Add(item);
        }
        public static void AddRange<TItem>(this ObservableCollection<TItem> observableCollection, params TItem[] items)
        {
            AddRange(observableCollection, items.AsEnumerable());
        }
    }
}