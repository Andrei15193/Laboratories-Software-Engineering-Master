using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using BillPath.DataAccess;
using BillPath.Models;

namespace BillPath.UserInterface.ViewModels
{
    public class ExpenseCategoryViewModel
    {
        private string _initialName;
        private ModelState _modelState;
        private readonly IExpenseCategoryRepository _repository;
        private readonly ExpenseObservableRepository _expensesRepository;
        private readonly DelegateAsyncCommand _saveCommand;
        private readonly DelegateAsyncCommand _removeCommand;
        private readonly DelegateAsyncCommand _updateCommand;

        public ExpenseCategoryViewModel(IExpenseCategoryRepository repository, ExpenseObservableRepository expensesRepository, ExpenseCategory expenseCategory)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            _repository = repository;
            _expensesRepository = expensesRepository;

            _saveCommand = new DelegateAsyncCommand(_SaveAsync) { CanExecute = ModelState?.IsValid ?? false };
            _removeCommand = new DelegateAsyncCommand(_RemoveAsync);
            _updateCommand = new DelegateAsyncCommand(_UpdateAsync);

            if (expenseCategory != null)
            {
                ModelState = ModelState.GetFor(expenseCategory);
                _InitialName = expenseCategory.Name;
                ExpensesPage = new ExpensesPageViewModel(this, expensesRepository);
            }
        }
        public ExpenseCategoryViewModel(IExpenseCategoryRepository repository)
            : this(repository, null, null)
        {
        }

        public ExpensesPageViewModel ExpensesPage { get; }

        public ModelState ModelState
        {
            get
            {
                return _modelState;
            }
            set
            {
                if (_modelState != null)
                    _modelState.PropertyChanged -= _ModelStatePropertyChanged;

                _modelState = value;
                _InitialName = null;

                if (_modelState != null)
                    _modelState.PropertyChanged += _ModelStatePropertyChanged;
            }
        }
        private string _InitialName
        {
            get
            {
                return _initialName;
            }
            set
            {
                _initialName = value;
                if (string.IsNullOrWhiteSpace(_initialName))
                {
                    _removeCommand.CanExecute = false;
                    _updateCommand.CanExecute = false;
                }
                else
                {
                    _removeCommand.CanExecute = true;
                    _updateCommand.CanExecute = true;
                }
            }
        }
        private void _ModelStatePropertyChanged(object sender, PropertyChangedEventArgs e)
            => _saveCommand.CanExecute = ModelState?.IsValid ?? false;

        public AsyncCommand SaveCommand
            => _saveCommand;
        private async Task _SaveAsync(object parameter, CancellationToken cancellationToken)
        {
            await _repository.SaveAsync((ExpenseCategory)ModelState.Model, cancellationToken);
            _InitialName = (string)_modelState[nameof(ExpenseCategory.Name)];
        }

        public AsyncCommand RemoveCommand
            => _removeCommand;
        private async Task _RemoveAsync(object parameter, CancellationToken cancellationToken)
        {
            await _repository.RemoveAsync((string)ModelState[nameof(ExpenseCategory.Name)], cancellationToken);
            await _expensesRepository.RemoveAsync(expense => expense.Category == null);

            _InitialName = null;
        }

        public AsyncCommand UpdateCommand
            => _updateCommand;
        private async Task _UpdateAsync(object parameter, CancellationToken cancellationToken)
        {
            await _repository.UpdateAsync(_InitialName, (ExpenseCategory)ModelState.Model, cancellationToken);
            await _expensesRepository.UpdateCategory(expense => expense.Category == null, (ExpenseCategory)ModelState.Model, cancellationToken);
            _InitialName = (string)ModelState[nameof(ExpenseCategory.Name)];
        }
    }
}