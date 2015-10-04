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
            Instance.Category = null;
            var validationResult = ModelValidator.Validate(Instance).Single();

            Assert.AreEqual(nameof(Expense.Category), validationResult.MemberNames.Single());
        }

        protected override void SetValidTestDataToInstance()
        {
            base.SetValidTestDataToInstance();
            Instance.Category = new ExpenseCategory();
        }

        protected override void AssertInstanceIsEqualTo(Expense other)
        {
            Assert.AreEqual(Instance.Amount, other.Amount);
            Assert.AreEqual(Instance.DateRealized, other.DateRealized);
            Assert.AreEqual(Instance.Description, other.Description);
            Assert.AreEqual(Instance.Category.Name, other.Category.Name);
            Assert.AreEqual(Instance.Category.Color, other.Category.Color);
        }
    }
}