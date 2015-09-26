using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Models.Tests
{
    [TestClass]
    public class ExpenseCategoryTests
    {
        [TestMethod]
        public void CloningAnExpenseCategoryProvidesDifferentInstance()
        {
            var expenseCategory = new ExpenseCategory();
            Assert.AreNotSame(expenseCategory, expenseCategory.Clone());
        }
        [TestMethod]
        public void TestCloningExpenseCategoryHasSameValuesOnProperties()
        {
            var expenseCategory =
                new ExpenseCategory
                {
                    Name = "test",
                    Color = new ArgbColor(1, 2, 3, 4)
                };

            _AssertAreEqual(expenseCategory, expenseCategory.Clone());
        }
        [TestMethod]
        public void TestSerialization()
        {
            ExpenseCategory deserializedExpenseCategory;
            var expenseCategory =
                new ExpenseCategory
                {
                    Name = "test",
                    Color = new ArgbColor(2, 3, 4, 5)
                };
            var expenseCategorySerializer = new DataContractSerializer(typeof(ExpenseCategory));

            using (var expenseCategorySerializationStream = new MemoryStream())
            {
                expenseCategorySerializer.WriteObject(expenseCategorySerializationStream, expenseCategory);
                expenseCategorySerializationStream.Seek(0, SeekOrigin.Begin);

                deserializedExpenseCategory = (ExpenseCategory)expenseCategorySerializer.ReadObject(expenseCategorySerializationStream);
            }

            _AssertAreEqual(expenseCategory, deserializedExpenseCategory);
        }

        private static void _AssertAreEqual(ExpenseCategory first, ExpenseCategory second)
        {
            Assert.AreEqual(first.Name, second.Name);
            Assert.AreEqual(first.Color, second.Color);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void TestInvalidExpenseCategoryName(string name)
        {
            var expenseCategory =
                new ExpenseCategory
                {
                    Name = name
                };
            var validator = new ModelValidator();

            Assert.AreEqual(1, validator.Validate(expenseCategory).Count());
        }
        [DataTestMethod]
        [DataRow("test")]
        [DataRow(" test")]
        [DataRow("test ")]
        public void TestValidExpenseCategoryName(string name)
        {
            var expenseCategory =
                new ExpenseCategory
                {
                    Name = name
                };
            var validator = new ModelValidator();

            Assert.AreEqual(0, validator.Validate(expenseCategory).Count());
        }
    }
}