using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Models.Tests
{
    [TestClass]
    public class IncomeTests
    {
        [TestMethod]
        public void TestCloneRetrievesANewInstance()
        {
            var income = new Income();

            Assert.AreNotSame(income, income.Clone());
        }

        [TestMethod]
        public void TestCloneRetrievesANewInstanceEqualToTheOriginal()
        {
            var income = new Income
            {
                Amount = new Amount(10m, new Currency(new RegionInfo("en-AU"))),
                DateRealized = DateTimeOffset.Now.AddDays(-3),
                Description = "This is a test description"
            };
            var clone = income.Clone();

            _AssertAreEqual(income, clone);
        }

        [TestMethod]
        public void TestSerialization()
        {
            Income deserializedIncome;
            var income = new Income
            {
                Amount = new Amount(7m, new Currency(new RegionInfo("en-US"))),
                DateRealized = DateTimeOffset.Now.AddDays(3),
                Description = "This is a test description"
            };
            var incomeSerializer = new DataContractSerializer(typeof(Income));

            using (var incomeSerializationStream = new MemoryStream())
            {
                incomeSerializer.WriteObject(incomeSerializationStream, income);
                incomeSerializationStream.Seek(0, SeekOrigin.Begin);

                deserializedIncome = (Income)incomeSerializer.ReadObject(incomeSerializationStream);
            }

            _AssertAreEqual(income, deserializedIncome);
        }

        private static void _AssertAreEqual(Income first, Income second)
        {
            Assert.AreEqual(first.Amount, second.Amount);
            Assert.AreEqual(first.DateRealized, second.DateRealized);
            Assert.AreEqual(first.Description, second.Description, ignoreCase: true);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(-100)]
        [DataRow(-55)]
        [DataRow(-30)]
        [DataRow(-45)]
        public void TestIncomeAmountMustBeGreaterThanZero(double amountValue)
        {
            var income = new Income
            {
                Amount = new Amount((decimal)amountValue, new Currency(new RegionInfo("en-US"))),
                Description = "This is a test description"
            };
            var modelValidator = new ModelValidator();
            var validationResult = modelValidator.Validate(income).Single();

            Assert.AreEqual(nameof(Income.Amount), validationResult.MemberNames.Single());
        }

        [TestMethod]
        public void TestIncomeCannotHaveAnAmountWithDefaultCurrency()
        {
            var income = new Income
            {
                Amount = new Amount(1m, default(Currency)),
                Description = "This is a test description"
            };
            var modelValidator = new ModelValidator();
            var validationResult = modelValidator.Validate(income).Single();

            Assert.AreEqual(nameof(Income.Amount), validationResult.MemberNames.Single());
        }

        [TestMethod]
        public void TestIncomeCanHaveNullDescription()
        {
            var income = new Income
            {
                Amount = new Amount(1m, new Currency(new RegionInfo("en-US"))),
                Description = null
            };
            var modelValidator = new ModelValidator();
            Assert.IsFalse(modelValidator.Validate(income).Any());
        }
    }
}