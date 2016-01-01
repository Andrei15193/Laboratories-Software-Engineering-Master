﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using BillPath.DataAccess;
using BillPath.Models;

namespace BillPath.UserInterface.ViewModels
{
    public class ExpenseCategoryViewModels
        : ReadOnlyObservableCollection<ExpenseCategoryViewModel>
    {
        private readonly IExpenseCategoryRepository _repository;

        public ExpenseCategoryViewModels(ExpenseCategoryObservableRepository repository)
            : base(new ObservableCollection<ExpenseCategoryViewModel>())
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            _repository = repository;
            repository.SavedExpenseCategory += _AddExpenseCategory;
            repository.RemovedExpenseCategory += _RemoveExpenseCategory;

            _LoadFromAsync(repository);
        }

        private void _AddExpenseCategory(object sender, ExpenseCategory expenseCategory)
            => Items.Add(new ExpenseCategoryViewModel(_repository, expenseCategory));
        private void _RemoveExpenseCategory(object sender, ExpenseCategory expenseCategory)
        {
            var index = Items
                .TakeWhile(
                    expenseCategoryViewModel => !expenseCategory.Name.Equals(
                        (string)expenseCategoryViewModel.ModelState[nameof(ExpenseCategory.Name)],
                        StringComparison.OrdinalIgnoreCase))
                .Count();

            if (index < Items.Count)
                Items.RemoveAt(index);
        }

        private async void _LoadFromAsync(ExpenseCategoryObservableRepository repository)
        {
            foreach (var expenseCategory in await repository.GetAllAsync())
                Items.Add(new ExpenseCategoryViewModel(repository, expenseCategory));
        }
    }
}