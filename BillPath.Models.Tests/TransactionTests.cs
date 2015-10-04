using System;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Models.Tests
{
    [TestClass]
    public abstract class TransactionTests<TTransaction>
        : CloningTests<TTransaction>
        where TTransaction : Transaction<TTransaction>
    {
        protected override void SetValidTestDataToInstance()
        {
            Instance.Amount = new Amount(1M, new Currency(new RegionInfo("en-US")));
            Instance.Description = "This is a test description";
            Instance.DateRealized = DateTimeOffset.Now.AddDays(-3);
        }
        protected override void AssertInstanceIsEqualTo(TTransaction other)
        {
            Assert.AreEqual(Instance.Amount, other.Amount);
            Assert.AreEqual(Instance.DateRealized, other.DateRealized);
            Assert.AreEqual(Instance.Description, other.Description, ignoreCase: true);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(-100)]
        [DataRow(-55)]
        [DataRow(-30)]
        [DataRow(-45)]
        public void TestTransactionAmountValueMustBeGreaterThanZero(double amountValue)
        {
            Instance.Amount = new Amount(
                (decimal)amountValue,
                new Currency(new RegionInfo("en-US")));
            var validationResult = ModelValidator.Validate(Instance).Single();

            Assert.AreEqual(
                nameof(Transaction<TTransaction>.Amount),
                validationResult.MemberNames.Single());
        }

        [TestMethod]
        public void TestTransactionCannotHaveAnAmountWithDefaultCurrency()
        {
            Instance.Amount = new Amount(
                1M,
                default(Currency));
            var validationResult = ModelValidator.Validate(Instance).Single();

            Assert.AreEqual(
                nameof(Transaction<TTransaction>.Amount),
                validationResult.MemberNames.Single());
        }

        [TestMethod]
        public void TestTransactionCanHaveNullDescription()
        {
            Instance.Description = null;
            var validationResults = ModelValidator.Validate(Instance);

            Assert.IsFalse(validationResults.Any());
        }
    }
}