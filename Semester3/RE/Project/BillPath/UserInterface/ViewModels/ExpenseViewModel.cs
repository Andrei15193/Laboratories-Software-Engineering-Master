using System;
using System.Threading;
using System.Threading.Tasks;
using BillPath.DataAccess;
using BillPath.Models;

namespace BillPath.UserInterface.ViewModels
{
    public class ExpenseViewModel
    {
        private ModelState _modelState;
        private readonly IExpenseRepository _repository;
        private readonly DelegateAsyncCommand _saveCommand;
        private readonly DelegateAsyncCommand _removeCommand;
        private readonly DelegateAsyncCommand _updateCommand;
        private readonly DelegateCommand _revertChangesCommand;
        private Expense _unmodifiedExpense;

        public ExpenseViewModel(IExpenseRepository repository)
            : this(repository, null)
        {
        }
        public ExpenseViewModel(IExpenseRepository repository, Expense expense)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            _repository = repository;
            _saveCommand = new DelegateAsyncCommand(_SaveAsync);
            _removeCommand = new DelegateAsyncCommand(_RemoveAsync);
            _updateCommand = new DelegateAsyncCommand(_UpdateAsync);
            _revertChangesCommand = new DelegateCommand(_RevertChanges);

            if (expense != null)
            {
                ModelState = ModelState.GetFor(expense);
                _UnmodifiedExpense = expense;
            }
            else
            {
                ModelState = null;
                _UnmodifiedExpense = null;
            }
        }

        private Expense _UnmodifiedExpense
        {
            get
            {
                return _unmodifiedExpense;
            }
            set
            {
                if (value == null)
                {
                    _unmodifiedExpense = null;
                    _removeCommand.CanExecute = false;
                    _updateCommand.CanExecute = false;
                    _revertChangesCommand.CanExecute = false;
                }
                else
                {
                    _unmodifiedExpense = value.Clone();
                    _removeCommand.CanExecute = true;
                    _updateCommand.CanExecute = true;
                    _revertChangesCommand.CanExecute = true;
                }
            }
        }

        private async Task _SaveAsync(object parameter, CancellationToken cancellationToken)
        {
            await _repository.SaveAsync((Expense)ModelState.Model, cancellationToken);
            _UnmodifiedExpense = (Expense)ModelState.Model;
        }
        private async Task _RemoveAsync(object parameter, CancellationToken cancellationToken)
            => await _repository.RemoveAsync(_UnmodifiedExpense, cancellationToken);

        private async Task _UpdateAsync(object parameter, CancellationToken cancellationToken)
        {
            if (_HasChanges)
            {
                await _repository.RemoveAsync(_UnmodifiedExpense, cancellationToken);
                await _repository.SaveAsync((Expense)ModelState.Model, cancellationToken);
                _UnmodifiedExpense = (Expense)ModelState.Model;
            }
        }
        private void _RevertChanges(object parameter)
        {
            if (_HasChanges)
            {
                ModelState[nameof(Expense.Amount)] = _UnmodifiedExpense.Amount;
                ModelState[nameof(Expense.DateRealized)] = _UnmodifiedExpense.DateRealized;
                ModelState[nameof(Expense.Description)] = _UnmodifiedExpense.Description;
                ModelState[nameof(Expense.Category)] = _UnmodifiedExpense.Category;
            }
        }

        private bool _HasChanges
            => !ExpenseEqualityComparer.Instance.Equals((Expense)ModelState.Model, _UnmodifiedExpense);

        public ModelState ModelState
        {
            get
            {
                return _modelState;
            }
            set
            {
                _modelState = value?.Model is Expense ? value : null;
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