using System;
using System.Collections.Generic;
using System.Linq;
using BillPath.UserInterface.ViewModels;
using Windows.UI.Xaml.Data;

namespace BillPath.Modern.Converters
{
    class PieChartSlicesConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var curencyCategoryViewModels = ((IEnumerable<CurrencyCategoryViewModel>)value);
            var totalAmount = curencyCategoryViewModels.Sum(vm => vm.Amount) + curencyCategoryViewModels.Count() / 2;

            var previousEnd = 0m;
            return curencyCategoryViewModels.Select((vm, index)
                => new
                {
                    Start = (double)(previousEnd = previousEnd + 0.5m),
                    End = (double)(previousEnd = previousEnd + vm.Amount * 360 / totalAmount),
                    Color = vm.Color
                });
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}