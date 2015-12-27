using System;
using BillPath.Models;
using BillPath.Modern.ResourceBinders;
using Windows.UI.Xaml;

namespace BillPath.Modern.Mocks
{
    internal sealed class IncomeModelState
        : ModelState<Income>
    {
        public IncomeModelState()
            : base(
                  new Income
                  {
                      Amount = new Amount(
                          0,
                          Application.Current.GetResource<SettingsViewModel>().PreferredCurrency),
                      DateRealized = DateTimeOffset.Now.Date
                  })
        {
        }
    }
}