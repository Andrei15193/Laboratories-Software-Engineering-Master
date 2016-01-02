using System;
using System.Collections.ObjectModel;
using System.Linq;
using BillPath.DataAccess;
using BillPath.Models;
using BillPath.Models.States.Providers;

namespace BillPath.UserInterface.ViewModels
{
    public class ExpenseCategoryViewModels
        : ReadOnlyObservableCollection<ExpenseCategoryViewModel>
    {
        private readonly IExpenseCategoryRepository _repository;
        private readonly ExpenseObservableRepository _expenseRepository;

        public ExpenseCategoryViewModels(ExpenseCategoryObservableRepository repository, ExpenseObservableRepository expenseRepository)
            : base(new ObservableCollection<ExpenseCategoryViewModel>())
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));
            if (expenseRepository == null)
                throw new ArgumentNullException(nameof(expenseRepository));

            _repository = repository;
            _expenseRepository = expenseRepository;
            repository.SavedExpenseCategory += _AddExpenseCategory;
            repository.RemovedExpenseCategory += _RemoveExpenseCategory;

            _LoadFromAsync(repository);
        }

        private void _AddExpenseCategory(object sender, ExpenseCategory expenseCategory)
            => Items.Add(new ExpenseCategoryViewModel(_repository, _expenseRepository, expenseCategory));
        private void _RemoveExpenseCategory(object sender, string removedCategoryName)
        {
            var index = Items
                .TakeWhile(
                    expenseCategoryViewModel => !removedCategoryName.Equals(
                        (string)expenseCategoryViewModel.ModelState[nameof(ExpenseCategory.Name)],
                        StringComparison.OrdinalIgnoreCase))
                .Count();

            if (index < Items.Count)
            {
                Items.RemoveAt(index);
                ExpenseCategoryModelStateProvider.RemoveFromCache(removedCategoryName);
            }
        }

        private async void _LoadFromAsync(ExpenseCategoryObservableRepository repository)
        {
            foreach (var expenseCategory in await repository.GetAllAsync())
                Items.Add(new ExpenseCategoryViewModel(repository, _expenseRepository, expenseCategory));
        }
    }
}