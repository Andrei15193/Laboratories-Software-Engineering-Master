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

            Assert.AreEqual(expense.Amount, deserializedExpense.Amount);
            Assert.AreEqual(expense.DateRealized, deserializedExpense.DateRealized);
            Assert.AreEqual(expense.Description, deserializedExpense.Description);
            Assert.AreEqual(expense.Category.Name, deserializedExpense.Category.Name);
            Assert.AreEqual(expense.Category.Color, deserializedExpense.Category.Color);
        }

        [TestMethod]
        public void TestExpenseRequiresACategory()
        {
            var expense = new Expense
            {
                Category = null
            };
            var validationResult = ModelValidator.Validate(expense).Single();

            Assert.AreEqual(nameof(Expense.Category), validationResult.MemberNames.Single());
        }

        [TestMethod]
        public void TestExpenseCannotHaveNegativeAmount()
        {
            var expense = new Expense
            {
                Amount = new Amount(-1M, new Currency()),
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
                Amount = new Amount(0M, new Currency()),
                Category = new ExpenseCategory()
            };
            Assert.AreEqual(0, ModelValidator.Validate(expense).Count());
        }

        [TestMethod]
        public void TestExpenseCanHaveNullDescription()
        {
            var expense = new Expense
            {
                Description = null,
                Category = new ExpenseCategory()
            };
            Assert.AreEqual(0, ModelValidator.Validate(expense).Count());
        }
    }
}