using System;
using BillPath.Models;
using BillPath.UserInterface.ViewModels;

namespace BillPath.Modern.Mocks
{
    public class IncomeViewModel
        : ViewModel<Income>
    {
        public IncomeViewModel()
            : base(new Income { DateRealized = DateTimeOffset.Now.Date })
        {
            Context = new IncomeModelState();
        }

        public dynamic Context { get; }
    }
}