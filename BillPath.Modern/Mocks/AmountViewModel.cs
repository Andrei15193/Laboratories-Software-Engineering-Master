using BillPath.Models;

namespace BillPath.Modern.Mocks
{
    internal class AmountViewModel
        : UserInterface.ViewModels.AmountViewModel
    {
        public AmountViewModel()
            : base(new Amount())
        {
        }
    }
}