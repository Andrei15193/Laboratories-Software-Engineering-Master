using System;
using System.Globalization;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Models.Tests
{
    [TestClass]
    public class IncomeEqualityComparerTests
    {
        [TestMethod]
        public void TestSameIncomesAreEqual()
        {
            var income = new Income();

            Assert.IsTrue(IncomeEqualityComparer.Instance.Equals(income, income));
        }

        [TestMethod]
        public void TestClonedIncomeIsEqualToOriginal()
        {
            var income = new Income();

            Assert.IsTrue(IncomeEqualityComparer.Instance.Equals(income, income.Clone()));
        }
        [TestMethod]
        public void TestClonedIncomeHasSameHashCodeAsOriginal()
        {
            var income = new Income();

            Assert.AreEqual(
                IncomeEqualityComparer.Instance.GetHashCode(income),
                IncomeEqualityComparer.Instance.GetHashCode(income.Clone()));
        }

        [TestMethod]
        public void TestDifferentButEqualIncomesAreEqual()
        {
            var income1 =
                new Income
                {
                    Amount = new Amount(100, new Currency(new RegionInfo("en-US"))),
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 3), new TimeSpan()),
                    Description = "Test description"
                };
            var income2 =
                new Income
                {
                    Amount = new Amount(100, new Currency(new RegionInfo("en-US"))),
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 3), new TimeSpan()),
                    Description = "Test description"
                };

            Assert.IsTrue(IncomeEqualityComparer.Instance.Equals(income1, income2));
        }
        [TestMethod]
        public void TestDifferentButEqualIncomesHaveEqualHAshCode()
        {
            var income1 =
                new Income
                {
                    Amount = new Amount(100, new Currency(new RegionInfo("en-US"))),
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 3), new TimeSpan()),
                    Description = "Test description"
                };
            var income2 =
                new Income
                {
                    Amount = new Amount(100, new Currency(new RegionInfo("en-US"))),
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 3), new TimeSpan()),
                    Description = "Test description"
                };

            Assert.AreEqual(
                IncomeEqualityComparer.Instance.GetHashCode(income1),
                IncomeEqualityComparer.Instance.GetHashCode(income2));
        }

        [TestMethod]
        public void TestIncomesDifferentOnlyThroughAmountAreNotEqual()
        {
            var income1 =
                new Income
                {
                    Amount = new Amount(99, new Currency(new RegionInfo("en-US"))),
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 3), new TimeSpan()),
                    Description = "Test description"
                };
            var income2 =
                new Income
                {
                    Amount = new Amount(100, new Currency(new RegionInfo("en-US"))),
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 3), new TimeSpan()),
                    Description = "Test description"
                };

            Assert.IsFalse(IncomeEqualityComparer.Instance.Equals(income1, income2));
        }
        [TestMethod]
        public void TestIncomesDifferentOnlyThroughRealizedDateAreNotEqual()
        {
            var income1 =
                new Income
                {
                    Amount = new Amount(100, new Currency(new RegionInfo("en-US"))),
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 3), TimeSpan.FromHours(1)),
                    Description = "Test description"
                };
            var income2 =
                new Income
                {
                    Amount = new Amount(100, new Currency(new RegionInfo("en-US"))),
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 3), new TimeSpan()),
                    Description = "Test description"
                };

            Assert.IsFalse(IncomeEqualityComparer.Instance.Equals(income1, income2));
        }
        [TestMethod]
        public void TestIncomesDifferentOnlyThroughDescriptionAreNotEqual()
        {
            var income1 =
                new Income
                {
                    Amount = new Amount(99, new Currency(new RegionInfo("en-US"))),
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 3), new TimeSpan()),
                    Description = "Test description1"
                };
            var income2 =
                new Income
                {
                    Amount = new Amount(100, new Currency(new RegionInfo("en-US"))),
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 3), new TimeSpan()),
                    Description = "Test description2"
                };

            Assert.IsFalse(IncomeEqualityComparer.Instance.Equals(income1, income2));
        }
        [TestMethod]
        public void TestIncomesDifferentOnlyThroughDescriptionCasingAreNotEqual()
        {
            var income1 =
                new Income
                {
                    Amount = new Amount(99, new Currency(new RegionInfo("en-US"))),
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 3), new TimeSpan()),
                    Description = "Test description"
                };
            var income2 =
                new Income
                {
                    Amount = new Amount(100, new Currency(new RegionInfo("en-US"))),
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 3), new TimeSpan()),
                    Description = "test description"
                };

            Assert.IsFalse(IncomeEqualityComparer.Instance.Equals(income1, income2));
        }

        [TestMethod]
        public void TestIncomesThatAreEqualButRealizationDateIsInDifferentOffsetAreEqual()
        {
            var income1 =
                new Income
                {
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 3), new TimeSpan()).ToOffset(TimeSpan.FromHours(1))
                };
            var income2 =
                new Income
                {
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 3), new TimeSpan()).ToOffset(TimeSpan.FromHours(2))
                };

            Assert.IsTrue(IncomeEqualityComparer.Instance.Equals(income1, income2));
        }
        [TestMethod]
        public void TestIncomesThatAreEqualButRealizationDateIsInDifferentOffsetHaveEqualHashCode()
        {
            var income1 =
                new Income
                {
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 3), new TimeSpan()).ToOffset(TimeSpan.FromHours(1))
                };
            var income2 =
                new Income
                {
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 3), new TimeSpan()).ToOffset(TimeSpan.FromHours(2))
                };

            Assert.AreEqual(
                IncomeEqualityComparer.Instance.GetHashCode(income1),
                IncomeEqualityComparer.Instance.GetHashCode(income2));
        }
    }
}