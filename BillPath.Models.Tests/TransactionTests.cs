using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Models.Tests
{
    [TestClass]
    public abstract class TransactionTests<TTransaction>
        where TTransaction : Transaction<TTransaction>
    {
        protected static ModelValidator ModelValidator { get; } = new ModelValidator();

        [TestInitialize]
        public void TestInitialize()
        {
            Transaction = GetNewTransaction();
            SetValidTestData(Transaction);
        }

        protected virtual TTransaction GetNewTransaction()
        {
            return Activator.CreateInstance<TTransaction>();
        }
        protected virtual void SetValidTestData(TTransaction transaction)
        {
            transaction.Amount = new Amount(1M, new Currency(new RegionInfo("en-US")));
            transaction.Description = "This is a test description";
            transaction.DateRealized = DateTimeOffset.Now.AddDays(-3);
        }
        protected TTransaction Transaction
        {
            get;
            private set;
        }
        protected abstract void AssertAreEqual(TTransaction first, TTransaction second);

        [TestCleanup]
        public void TestCleanup()
        {
            Transaction = null;
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(-100)]
        [DataRow(-55)]
        [DataRow(-30)]
        [DataRow(-45)]
        public void TestTransactionAmountMustBeGreaterThanZero(double amountValue)
        {
            Transaction.Amount = new Amount(
                (decimal)amountValue,
                new Currency(new RegionInfo("en-US")));
            var validationResult = ModelValidator.Validate(Transaction).Single();

            Assert.AreEqual(
                nameof(Transaction<TTransaction>.Amount),
                validationResult.MemberNames.Single());
        }

        [TestMethod]
        public void TestTransactionCannotHaveAnAmountWithDefaultCurrency()
        {
            Transaction.Amount = new Amount(
                1M,
                default(Currency));
            var validationResult = ModelValidator.Validate(Transaction).Single();

            Assert.AreEqual(
                nameof(Transaction<TTransaction>.Amount),
                validationResult.MemberNames.Single());
        }

        [TestMethod]
        public void TestTransactionCanHaveNullDescription()
        {
            Transaction.Description = null;
            var validationResults = ModelValidator.Validate(Transaction);

            Assert.IsFalse(validationResults.Any());
        }

        [TestMethod]
        public void TestTransactionCloneIsNotTheSameAsTheOriginal()
        {
            Assert.AreNotSame(Transaction, Transaction.Clone());
        }
        [TestMethod]
        public void TestTransactionCloneIsEqualToOriginal()
        {
            AssertAreEqual(Transaction, Transaction.Clone());
        }

        [TestMethod]
        public void TestSerialization()
        {
            TTransaction deserializedExpense;
            var expenseSerializer = new DataContractSerializer(typeof(TTransaction));

            using (var expenseSerializationStream = new MemoryStream())
            {
                expenseSerializer.WriteObject(expenseSerializationStream, Transaction);
                expenseSerializationStream.Seek(0, SeekOrigin.Begin);

                deserializedExpense = (TTransaction)expenseSerializer.ReadObject(expenseSerializationStream);
            }

            AssertAreEqual(Transaction, deserializedExpense);
        }
    }
}