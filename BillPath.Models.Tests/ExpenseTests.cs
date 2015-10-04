using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Models.Tests
{
    [TestClass]
    public class ExpenseTests
    {
        private ModelValidator ModelValidator { get; } = new ModelValidator();

        [TestMethod]
        public void TestSerialization()
        {
            Expense deserializedExpense;
            var expense = new Expense
            {
                Amount = new Amount(1M, new Currency(new RegionInfo("en-US"))),
                DateRealized = new DateTimeOffset(new DateTime(2015, 7, 20), TimeSpan.FromHours(3D)),
                Description = "This is a test description",
                Category = new ExpenseCategory
                {
                    Name = "This is a test expense category name",
                    Color = new ArgbColor(0xFF, 0xFF, 0xFE, 0xAA)
                }
            };
            var expenseSerializer = new DataContractSerializer(typeof(Expense));

            using (var expenseSerializationStream = new MemoryStream())
            {
                expenseSerializer.WriteObject(expenseSerializationStream, expense);
                expenseSerializationStream.Seek(0, SeekOrigin.Begin);

                deserializedExpense = (Expense)expenseSerializer.ReadObject(expenseSerializationStream);
            }

            _AreEqual(expense, deserializedExpense);
        }

        [TestMethod]
        public void TestExpenseRequiresACategory()
        {
            var expense = new Expense
            {
                Amount = new Amount(0M, new Currency(new RegionInfo("en-US"))),
                Category = null
            };
            var validationResult = ModelValidator.Validate(expense).Single();

            Assert.AreEqual(nameof(Expense.Category), validationResult.MemberNames.Single());
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(-100)]
        [DataRow(-50)]
        [DataRow(-35)]
        [DataRow(-20)]
        [DataRow(-1000)]
        public void TestExpenseCannotHaveNegativeAmount(double amountValue)
        {
            var expense = new Expense
            {
                Amount = new Amount((decimal)amountValue, new Currency(new RegionInfo("en-US"))),
                Category = new ExpenseCategory()
            };
            var validationResult = ModelValidator.Validate(expense).Single();

            Assert.AreEqual(nameof(Expense.Amount), validationResult.MemberNames.Single());
        }

        [TestMethod]
        public void TestExpenseCanHaveZeroAmount()
        {
            var expense = new Expense
            {
                Amount = new Amount(0M, new Currency(new RegionInfo("en-US"))),
                Category = new ExpenseCategory()
            };
            Assert.IsFalse(ModelValidator.Validate(expense).Any());
        }

        [TestMethod]
        public void TestExpenseCannotHaveDefaultAmountCurrency()
        {
            var expense = new Expense
            {
                Amount = new Amount(0M, new Currency()),
                Category = new ExpenseCategory()
            };
            var validationResult = ModelValidator.Validate(expense).Single();

            Assert.AreEqual(nameof(Expense.Amount), validationResult.MemberNames.Single());
        }

        [TestMethod]
        public void TestExpenseCanHaveNullDescription()
        {
            var expense = new Expense
            {
                Amount = new Amount(0M, new Currency(new RegionInfo("en-US"))),
                Description = null,
                Category = new ExpenseCategory()
            };
            Assert.AreEqual(0, ModelValidator.Validate(expense).Count());
        }

        [TestMethod]
        public void TestCloningExpenseRetrievesNewInstance()
        {
            var expense = new Expense();
            Assert.AreNotSame(expense, expense.Clone());
        }

        [TestMethod]
        public void TestClonedExpenseIsEqualToOriginal()
        {
            var expense = new Expense
            {
                Amount = new Amount(1M, new Currency(new RegionInfo("en-US"))),
                DateRealized = new DateTimeOffset(new DateTime(2015, 7, 20), TimeSpan.FromHours(3D)),
                Description = "This is a test description",
                Category = new ExpenseCategory
                {
                    Name = "This is a test expense category name",
                    Color = new ArgbColor(0xFF, 0xFF, 0xFE, 0xAA)
                }
            };
            var clonedExpense = expense.Clone();

            _AreEqual(expense, clonedExpense);
        }

        [TestMethod]
        public void TestClonedExpenseHasSameCategoryAsOriginal()
        {
            var expense = new Expense
            {
                Category = new ExpenseCategory()
            };

            Assert.AreSame(expense.Category, expense.Clone().Category);
        }

        private static void _AreEqual(Expense first, Expense second)
        {
            Assert.AreEqual(first.Amount, second.Amount);
            Assert.AreEqual(first.DateRealized, second.DateRealized);
            Assert.AreEqual(first.Description, second.Description);
            Assert.AreEqual(first.Category.Name, second.Category.Name);
            Assert.AreEqual(first.Category.Color, second.Category.Color);
        }
    }
}