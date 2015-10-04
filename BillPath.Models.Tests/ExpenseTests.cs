using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Models.Tests
{
    [TestClass]
    public class ExpenseTests
        : TransactionTests<Expense>
    {
        [TestMethod]
        public void TestExpenseRequiresACategory()
        {
            Transaction.Category = null;
            var validationResult = ModelValidator.Validate(Transaction).Single();

            Assert.AreEqual(nameof(Expense.Category), validationResult.MemberNames.Single());
        }

        [TestMethod]
        public void TestClonedExpenseHasSameCategoryAsOriginal()
        {
            Assert.AreSame(Transaction.Category, Transaction.Clone().Category);
        }

        protected override void SetValidTestData(Expense transaction)
        {
            base.SetValidTestData(transaction);
            transaction.Category = new ExpenseCategory();
        }
        protected override void AssertAreEqual(Expense first, Expense second)
        {
            Assert.AreEqual(first.Amount, second.Amount);
            Assert.AreEqual(first.DateRealized, second.DateRealized);
            Assert.AreEqual(first.Description, second.Description);
            Assert.AreEqual(first.Category.Name, second.Category.Name);
            Assert.AreEqual(first.Category.Color, second.Category.Color);
        }
    }
}