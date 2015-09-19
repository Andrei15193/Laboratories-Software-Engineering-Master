using System;
using System.Globalization;
using BillPath.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BillPath.Tests
{
    [TestClass]
    public class IncomeEqualityComparerTests
    {
        [TestMethod]
        public void TestIncomeIsEqualToItself()
        {
            var income = new Income
            {
                Amount = new Amount(10.1m, new Currency(new RegionInfo("RO"))),
                DateRealized = DateTimeOffset.Now,
                Description = "test description"
            };

            Assert.IsTrue(IncomeEqualityComparer.Instance.Equals(income, income));
        }
        [TestMethod]
        public void TestTwoIncomesHavingEqualPropertyValuesAreEqual()
        {
            var now = DateTimeOffset.Now;
            var income1 = new Income
            {
                Amount = new Amount(10.1m, new Currency(new RegionInfo("RO"))),
                DateRealized = now,
                Description = "test description"
            };
            var income2 = new Income
            {
                Amount = new Amount(10.1m, new Currency(new RegionInfo("RO"))),
                DateRealized = now,
                Description = "test description"
            };

            Assert.IsTrue(IncomeEqualityComparer.Instance.Equals(income1, income2));
        }
        [TestMethod]
        public void TestTwoIncomesHavingDifferentAmountsAreNotEqual()
        {
            var now = DateTimeOffset.Now;
            var income1 = new Income
            {
                Amount = new Amount(10.1m, new Currency(new RegionInfo("RO"))),
                DateRealized = now,
                Description = "test description"
            };
            var income2 = new Income
            {
                Amount = new Amount(10m, new Currency(new RegionInfo("RO"))),
                DateRealized = now,
                Description = "test description"
            };

            Assert.IsFalse(IncomeEqualityComparer.Instance.Equals(income1, income2));
        }
        [TestMethod]
        public void TestTwoIncomesHavingDifferentCurrenciesAreNotEqual()
        {
            var now = DateTimeOffset.Now;
            var income1 = new Income
            {
                Amount = new Amount(10.1m, new Currency(new RegionInfo("RO"))),
                DateRealized = now,
                Description = "test description"
            };
            var income2 = new Income
            {
                Amount = new Amount(10.1m, new Currency(new RegionInfo("GB"))),
                DateRealized = now,
                Description = "test description"
            };

            Assert.IsFalse(IncomeEqualityComparer.Instance.Equals(income1, income2));
        }
        [TestMethod]
        public void TestTwoIncomesHavingDifferentRealizationDatesAreNotEqual()
        {
            var now = DateTimeOffset.Now;
            var income1 = new Income
            {
                Amount = new Amount(10.1m, new Currency(new RegionInfo("RO"))),
                DateRealized = now,
                Description = "test description"
            };
            var income2 = new Income
            {
                Amount = new Amount(10.1m, new Currency(new RegionInfo("RO"))),
                DateRealized = now.AddMilliseconds(1),
                Description = "test description"
            };

            Assert.IsFalse(IncomeEqualityComparer.Instance.Equals(income1, income2));
        }
        [TestMethod]
        public void TestTwoIncomesHavingDifferentDescriptionsDatesAreNotEqual()
        {
            var now = DateTimeOffset.Now;
            var income1 = new Income
            {
                Amount = new Amount(10.1m, new Currency(new RegionInfo("RO"))),
                DateRealized = now,
                Description = "test description 1"
            };
            var income2 = new Income
            {
                Amount = new Amount(10.1m, new Currency(new RegionInfo("RO"))),
                DateRealized = now,
                Description = "test description 2"
            };

            Assert.IsFalse(IncomeEqualityComparer.Instance.Equals(income1, income2));
        }
        [TestMethod]
        public void TestDescriptionIsCaseInsensitiveCompared()
        {
            var now = DateTimeOffset.Now;
            var income1 = new Income
            {
                Amount = new Amount(10.1m, new Currency(new RegionInfo("RO"))),
                DateRealized = now,
                Description = "test description"
            };
            var income2 = new Income
            {
                Amount = new Amount(10.1m, new Currency(new RegionInfo("RO"))),
                DateRealized = now,
                Description = "TEST DESCRIPTION"
            };

            Assert.IsTrue(IncomeEqualityComparer.Instance.Equals(income1, income2));
        }
        [TestMethod]
        public void TestNullIsEqualToNull()
        {
            Assert.IsTrue(IncomeEqualityComparer.Instance.Equals(null, null));
        }
        [TestMethod]
        public void TestIncomeIsNotEqualToNull()
        {
            var income = new Income
            {
                Amount = new Amount(10.1m, new Currency(new RegionInfo("RO"))),
                DateRealized = DateTimeOffset.Now,
                Description = "test description"
            };

            Assert.IsFalse(IncomeEqualityComparer.Instance.Equals(income, null));
            Assert.IsFalse(IncomeEqualityComparer.Instance.Equals(null, income));
        }

        [TestMethod]
        public void TestGetHashCodeReturnsSameValueWhenCalledTwiceWithSameIncome()
        {
            var income = new Income
            {
                Amount = new Amount(10.1m, new Currency(new RegionInfo("RO"))),
                DateRealized = DateTimeOffset.Now,
                Description = "test description"
            };

            Assert.AreEqual(
                IncomeEqualityComparer.Instance.GetHashCode(income),
                IncomeEqualityComparer.Instance.GetHashCode(income));
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestGetHashCodeThrowsArgumentNullException()
        {
            IncomeEqualityComparer.Instance.GetHashCode(null);
        }
        [TestMethod]
        public void TestGetHashCodeDoesNotThrowExceptionForIncomeWithNullDescription()
        {
            var income = new Income
            {
                Amount = new Amount(10.1m, new Currency(new RegionInfo("RO"))),
                DateRealized = DateTimeOffset.Now,
                Description = null
            };
            IncomeEqualityComparer.Instance.GetHashCode(income);
        }
    }
}