using System;
using System.Threading;
using System.Threading.Tasks;
using BillPath.DataAccess;
using BillPath.Models;

namespace BillPath.UserInterface.ViewModels
{
    public class IncomeViewModel
    {
        private ModelState _modelState;
        private readonly IIncomeRepository _repository;
        private readonly DelegateAsyncCommand _saveCommand;
        private readonly DelegateAsyncCommand _removeCommand;
        private readonly DelegateAsyncCommand _updateCommand;
        private readonly DelegateCommand _revertChangesCommand;
        private Income _unmodifiedIncome;

        public IncomeViewModel(IIncomeRepository repository)
            : this(repository, null)
        {
        }
        public IncomeViewModel(IIncomeRepository repository, Income income)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            _repository = repository;
            _saveCommand = new DelegateAsyncCommand(_SaveAsync);
            _removeCommand = new DelegateAsyncCommand(_RemoveAsync);
            _updateCommand = new DelegateAsyncCommand(_UpdateAsync);
            _revertChangesCommand = new DelegateCommand(_RevertChanges);

            if (income != null)
            {
                ModelState = ModelState.GetFor(income);
                _UnmodifiedIncome = income;
            }
            else
            {
                ModelState = null;
                _UnmodifiedIncome = null;
            }
        }

        private Income _UnmodifiedIncome
        {
            get
            {
                return _unmodifiedIncome;
            }
            set
            {
                if (value == null)
                {
                    _unmodifiedIncome = null;
                    _removeCommand.CanExecute = false;
                    _updateCommand.CanExecute = false;
                    _revertChangesCommand.CanExecute = false;
                }
                else
                {
                    _unmodifiedIncome = value.Clone();
                    _removeCommand.CanExecute = true;
                    _updateCommand.CanExecute = true;
                    _revertChangesCommand.CanExecute = true;
                }
            }
        }

        private async Task _SaveAsync(object parameter, CancellationToken cancellationToken)
        {
            await _repository.SaveAsync((Income)ModelState.Model, cancellationToken);
            _UnmodifiedIncome = (Income)ModelState.Model;
        }
        private async Task _RemoveAsync(object parameter, CancellationToken cancellationToken)
            => await _repository.RemoveAsync(_UnmodifiedIncome, cancellationToken);

        private async Task _UpdateAsync(object parameter, CancellationToken cancellationToken)
        {
            if (_HasChanges)
            {
                await _repository.RemoveAsync(_UnmodifiedIncome, cancellationToken);
                await _repository.SaveAsync((Income)ModelState.Model, cancellationToken);
                _UnmodifiedIncome = (Income)ModelState.Model;
            }
        }
        private void _RevertChanges(object parameter)
        {
            if (_HasChanges)
            {
                ModelState[nameof(Income.Amount)] = _UnmodifiedIncome.Amount;
                ModelState[nameof(Income.DateRealized)] = _UnmodifiedIncome.DateRealized;
                ModelState[nameof(Income.Description)] = _UnmodifiedIncome.Description;
            }
        }

        private bool _HasChanges
            => !IncomeEqualityComparer.Instance.Equals((Income)ModelState.Model, _UnmodifiedIncome);

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

        public AsyncCommand UpdateCommand
            => _updateCommand;
        public Command RevertChangesCommand
            => _revertChangesCommand;

        protected virtual void OnModelStateChanged()
        {
            if (_modelState != null)
            {
                _modelState.PropertyChanged +=
                    delegate
                    {
                        _saveCommand.CanExecute = ModelState?.IsValid ?? false;
                        _updateCommand.CanExecute = ModelState?.IsValid ?? false;
                    };
                _saveCommand.CanExecute = _modelState.IsValid;
                _updateCommand.CanExecute = _modelState.IsValid;
            }
            else
            {
                _saveCommand.CanExecute = false;
                _updateCommand.CanExecute = false;
            }
        }
    }
}