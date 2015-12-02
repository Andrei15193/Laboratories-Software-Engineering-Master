using System;
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
        }
    }
}