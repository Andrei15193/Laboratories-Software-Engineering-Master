using Windows.UI.Xaml;

namespace BillPath.Modern.ResourceBinders
{
    internal sealed class IncomesPageViewModel
        : UserInterface.ViewModels.IncomesPageViewModel
    {
        public IncomesPageViewModel()
            : base(Application.Current.GetResource<IncomeXmlObservableRepository>())
        {
        }
    }
}