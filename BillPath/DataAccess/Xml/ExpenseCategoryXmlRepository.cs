using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess.Xml
{
    public class ExpenseCategoryXmlRepository
        : IExpenseCategoryRepository
    {
        private static readonly XmlTranslator<ExpenseCategory> _xmlTranslator = new ExpenseCategoryXmlTranslator();
        private readonly IList<ExpenseCategory> _expenseCategories =
            new List<ExpenseCategory>
            {
                new ExpenseCategory
                {
                    Name = "red",
                    Color = new ArgbColor(0xFF, 0xFF, 0x00, 0x00)
                },
                new ExpenseCategory
                {
                    Name = "Yellow",
                    Color = new ArgbColor(0xFF, 0xFF, 0xFF, 0x00)
                }
            };

        public Task<IEnumerable<ExpenseCategory>> GetAllAsync()
            => GetAllAsync(CancellationToken.None);
        public async Task<IEnumerable<ExpenseCategory>> GetAllAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();

            return _expenseCategories;
        }

        public Task RemoveAsync(string name)
            => RemoveAsync(name, CancellationToken.None);
        public async Task RemoveAsync(string name, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(name))
                if (name == null)
                    throw new ArgumentNullException(nameof(name));
                else
                    throw new ArgumentException("Cannot be empty or white space!", nameof(name));

            await Task.Yield();

            var indexToRemove = _expenseCategories
                .TakeWhile(expenseCategory => name.Equals(expenseCategory.Name, StringComparison.OrdinalIgnoreCase))
                .Count();
            if (indexToRemove < _expenseCategories.Count)
                _expenseCategories.RemoveAt(indexToRemove);
        }

        public Task SaveAsync(ExpenseCategory expenseCategory)
            => SaveAsync(expenseCategory, CancellationToken.None);
        public async Task SaveAsync(ExpenseCategory expenseCategory, CancellationToken cancellationToken)
        {
            if (expenseCategory == null)
                throw new ArgumentNullException(nameof(expenseCategory));

            await Task.Yield();

            _expenseCategories.Add(expenseCategory);
        }

        public Task UpdateAsync(string expenseCategoryName, ExpenseCategory expenseCategory)
            => UpdateAsync(expenseCategoryName, expenseCategory, CancellationToken.None);
        public async Task UpdateAsync(string expenseCategoryName, ExpenseCategory expenseCategory, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(expenseCategoryName))
                if (expenseCategoryName == null)
                    throw new ArgumentNullException(nameof(expenseCategoryName));
                else
                    throw new ArgumentException("Cannot be empty or white space!", nameof(expenseCategoryName));
            if (expenseCategory == null)
                throw new ArgumentNullException(nameof(expenseCategory));

            await Task.Yield();

            var index = _expenseCategories
                .TakeWhile(existingExpenseCategory => expenseCategoryName.Equals(
                    existingExpenseCategory.Name,
                    StringComparison.OrdinalIgnoreCase))
                .Count();

            if (index < _expenseCategories.Count)
                _expenseCategories[index] = expenseCategory;
        }
    }
}