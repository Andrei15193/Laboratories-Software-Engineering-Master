using System;
using BillPath.DataAccess.Xml;
using BillPath.Models;
using Windows.UI.Xaml;

namespace BillPath.Modern.Mocks
{
    public class IncomeViewModel
        : UserInterface.ViewModels.IncomeViewModel
    {
        public IncomeViewModel()
            : base(Application.Current.GetResource<IncomeXmlObservableRepository>())
        {
            ModelState = new ModelState(
                new Income
                {
                    DateRealized = new DateTimeOffset(DateTime.Now.Date)
                });
        }
    }
}