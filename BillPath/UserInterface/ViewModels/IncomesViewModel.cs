using System;
using System.Threading;
using System.Threading.Tasks;
using BillPath.DataAccess;

namespace BillPath.UserInterface.ViewModels
{
    public class IncomesViewModel
        : ViewModel
    {
        private readonly IIncomesRepository _repository;

        public IncomesViewModel(IIncomesRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            _repository = repository;
            SaveCommand = new DelegateAsyncCommand<IncomeViewModel>(_SaveIncome);
        }

        public AsyncCommand<IncomeViewModel> SaveCommand
        {
            get;
        }
        private async Task _SaveIncome(IncomeViewModel incomeViewModel, CancellationToken cancellationToken)
        {
            if (incomeViewModel == null)
                throw new ArgumentNullException(nameof(incomeViewModel));
            if (!incomeViewModel.IsValid)
                throw new InvalidOperationException();

            await _repository.SaveAsync(incomeViewModel.Model.Clone(), cancellationToken);
        }
    }
}