using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using BillPath.UserInterface.ViewModels;
using Windows.UI.Xaml.Data;

namespace BillPath.Modern.Converters
{
    public class SelectModelFromExpenseCategories
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var modelStates = new ObservableCollection<ModelState>();
            var viewModels = (ReadOnlyObservableCollection<ExpenseCategoryViewModel>)value;

            ((INotifyCollectionChanged)viewModels).CollectionChanged +=
                delegate
                {
                    modelStates.Clear();
                    foreach (var viewModel in viewModels)
                        modelStates.Add(viewModel.ModelState);
                };
            foreach (var viewModel in viewModels)
                modelStates.Add(viewModel.ModelState);

            return modelStates;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}