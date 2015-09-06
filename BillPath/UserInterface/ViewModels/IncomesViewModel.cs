using System;
using BillPath.DataAccess;
using BillPath.Models;

namespace BillPath.UserInterface.ViewModels
{
    public class IncomesViewModel
    {
        private readonly IIncomeRepository _repository;

        public IncomesViewModel(IIncomeRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            _repository = repository;
            AddIncome = new DelegateAsyncCommand<Income>(
                (income, cancellationToken) => _repository.SaveAsync(income, cancellationToken));
        }

        public AsyncCommand<Income> AddIncome
        {
            get;
        }
    }
}