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
        private Income _income;
        private readonly IIncomeXmlRepository _repository;
        private readonly DelegateAsyncCommand _saveCommand;
        private readonly DelegateAsyncCommand _removeCommand;

        public IncomeViewModel(IIncomeXmlRepository repository)
            : this(repository, null)
        {
        }
        public IncomeViewModel(IIncomeXmlRepository repository, Income income)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            _repository = repository;
            _saveCommand = new DelegateAsyncCommand(_SaveAsync);
            _removeCommand =
                new DelegateAsyncCommand(_RemoveAsync)
                {
                    CanExecute = _Income != null
                };

            _Income = income;
        }

        private Income _Income
        {
            get
            {
                return _income;
            }
            set
            {
                _income = value;
                if (_income != null)
                {
                    ModelState = new ModelState(_income.Clone());
                    _removeCommand.CanExecute = true;
                }
                else
                {
                    ModelState = null;
                    _removeCommand.CanExecute = false;
                }
            }
        }

        private async Task _SaveAsync(object parameter, CancellationToken cancellationToken)
        {
            await _repository.SaveAsync((Income)ModelState.Model, cancellationToken);
            _Income = (Income)ModelState.Model;
            _removeCommand.CanExecute = true;
        }
        private async Task _RemoveAsync(object parameter, CancellationToken cancellationToken)
            => await _repository.RemoveAsync(_Income, cancellationToken);

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
        public AsyncCommand RemoveCommand
            => _removeCommand;

        protected virtual void OnModelStateChanged()
        {
            if (_modelState != null)
            {
                _modelState.PropertyChanged += delegate { _saveCommand.CanExecute = ModelState?.IsValid ?? false; };
                _saveCommand.CanExecute = _modelState.IsValid;
            }
            else
                _saveCommand.CanExecute = false;
        }
    }
}