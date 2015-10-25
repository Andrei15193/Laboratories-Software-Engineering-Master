using System;

namespace BillPath.Modern.Mocks
{
    public class IncomeViewModel
        : UserInterface.ViewModels.IncomeViewModel
    {
        public IncomeViewModel()
            : base(
                  new Models.Income
                  {
                      DateRealized = DateTimeOffset.Now.Date
                  })
        {
        }
    }
}