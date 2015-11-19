using System;
using BillPath.Models;

namespace BillPath.Modern.Mocks
{
    internal sealed class IncomeModelState
        : ModelState<Income>
    {
        public IncomeModelState()
            : base(new Income { DateRealized = DateTimeOffset.Now.Date })
        {
        }
    }
}