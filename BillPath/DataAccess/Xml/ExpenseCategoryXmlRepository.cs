using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess.Xml
{
    public class ExpenseCategoryXmlRepository
        : IExpenseCategoryRepository
    {
        private static readonly XmlTranslator<ExpenseCategory> _xmlTranslator = new ExpenseCategoryXmlTranslator();
        private readonly ICollection<ExpenseCategory> _expenseCategories =
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

        public Task SaveAsync(ExpenseCategory expenseCategory)
            => SaveAsync(expenseCategory, CancellationToken.None);
        public async Task SaveAsync(ExpenseCategory expenseCategory, CancellationToken cancellationToken)
        {
            if (expenseCategory == null)
                throw new ArgumentNullException(nameof(expenseCategory));

            await Task.Yield();

            _expenseCategories.Add(expenseCategory);
        }
    }
}