using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using BillPath.DataAccess;
using BillPath.Models;

namespace BillPath.UserInterface.ViewModels
{
    public class ExpenseCategoryViewModel
    {
        private ModelState _modelState;
        private readonly IExpenseCategoryRepository _repository;
        private readonly DelegateAsyncCommand _saveCommand;

        public ExpenseCategoryViewModel(IExpenseCategoryRepository repository, ExpenseCategory expenseCategory)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            _repository = repository;
            if (expenseCategory != null)
                ModelState = ModelState.GetFor(expenseCategory);

            _saveCommand = new DelegateAsyncCommand(_SaveAsync) { CanExecute = ModelState?.IsValid ?? false };
        }

        public ExpenseCategoryViewModel(IExpenseCategoryRepository repository)
            : this(repository, null)
        {
        }

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

                if (_modelState != null)
                    _modelState.PropertyChanged += _ModelStatePropertyChanged;
            }
        }
        private void _ModelStatePropertyChanged(object sender, PropertyChangedEventArgs e)
            => _saveCommand.CanExecute = ModelState?.IsValid ?? false;

        public AsyncCommand SaveCommand
            => _saveCommand;
        private Task _SaveAsync(object parameter, CancellationToken cancellationToken)
            => _repository.SaveAsync((ExpenseCategory)ModelState.Model, cancellationToken);
    }
}