using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace BillPath.UserInterface.Converters
{
    public class OverviewObservableCollectionConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return new OverviewObservableCollection((IEnumerable)value, System.Convert.ToInt32(parameter));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return ((OverviewObservableCollection)value).OverviewedItems;
        }

        private class OverviewObservableCollection
            : ObservableCollection<object>
        {
            internal OverviewObservableCollection(IEnumerable items, int count)
                : base(items.Cast<object>().Take(count))
            {
                if (count < 1)
                    throw new ArgumentException("Must be at least 1 (one)", "count");

                if (items == null)
                    throw new ArgumentNullException("items");

                _count = count;
                _overviewedItems = items;

                INotifyCollectionChanged observableCollection = items as INotifyCollectionChanged;
                if (observableCollection != null && observableCollection is IList)
                    observableCollection.CollectionChanged += _CollectionChanged;
            }

            internal IEnumerable OverviewedItems
            {
                get
                {
                    return _overviewedItems;
                }
            }

            private void _CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        if (e.NewStartingIndex < _count)
                        {
                            for (int index = e.NewStartingIndex; (index - e.NewStartingIndex) < e.NewItems.Count && index < _count; index++)
                                if (index < Count)
                                    Insert(index, e.NewItems[(index - e.NewStartingIndex)]);
                                else
                                    Add(e.NewItems[(index - e.NewStartingIndex)]);

                            while (Count > _count)
                                RemoveAt(Count - 1);
                        }
                        break;

                    case NotifyCollectionChangedAction.Move:
                        if (e.NewStartingIndex < _count)
                            for (int index = e.NewStartingIndex; (index - e.NewStartingIndex) < e.NewItems.Count && index < _count; index++)
                                if (index < Count)
                                    this[index] = e.NewItems[(index - e.NewStartingIndex)];
                                else
                                    Add(e.NewItems[(index - e.NewStartingIndex)]);
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        if (e.OldStartingIndex < _count)
                        {
                            for (int count = 0; count < e.OldItems.Count && e.OldStartingIndex < Count && e.OldStartingIndex + count < _count; count++)
                                RemoveAt(e.OldStartingIndex);

                            IList wrappedObservableCollection = (IList)sender;
                            for (int index = e.OldStartingIndex; index < wrappedObservableCollection.Count && Count < _count; index++)
                                if (index >= Count)
                                    Add(wrappedObservableCollection[index]);
                        }
                        break;

                    case NotifyCollectionChangedAction.Replace:
                        for (int index = e.NewStartingIndex; (index - e.NewStartingIndex) < e.NewItems.Count && index < _count; index++)
                            if (index < Count)
                                this[index] = e.NewItems[(index - e.NewStartingIndex)];
                            else
                                Add(e.NewItems[(index - e.NewStartingIndex)]);
                        break;

                    default:
                        Clear();
                        foreach (object item in ((IEnumerable)sender).Cast<object>().Take(_count))
                            Add(item);
                        break;
                }
            }

            private readonly int _count;
            private readonly IEnumerable _overviewedItems;
        }
    }
}