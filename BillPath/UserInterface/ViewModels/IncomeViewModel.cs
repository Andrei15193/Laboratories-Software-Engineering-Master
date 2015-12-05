using System;
using System.Threading;
using System.Threading.Tasks;
using BillPath.DataAccess.Xml;
using BillPath.Models;

namespace BillPath.UserInterface.ViewModels
{
    public class IncomeViewModel
    {
        private ModelState _modelState;
        private readonly DelegateAsyncCommand _saveCommand;
        private readonly IncomeXmlRepository _repository;

        public IncomeViewModel(IncomeXmlRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            _repository = repository;
            _saveCommand = new DelegateAsyncCommand(_SaveAsync);
        }

        private Task _SaveAsync(object parameter, CancellationToken cancellationToken)
            => _repository.SaveAsync((Income)ModelState.Model, cancellationToken);

        public ModelState ModelState
        {
            get
            {
                return _modelState;
            }
            set
            {
                _modelState = value?.Model is Income ? value : null;
                OnModelStateChanged();
            }
        }

        public AsyncCommand SaveCommand
            => _saveCommand;

        protected virtual void OnModelStateChanged()
        {
            _modelState.PropertyChanged += delegate { _saveCommand.CanExecute = ModelState?.IsValid ?? false; };
            _saveCommand.CanExecute = ModelState?.IsValid ?? false;
        }
    }
}