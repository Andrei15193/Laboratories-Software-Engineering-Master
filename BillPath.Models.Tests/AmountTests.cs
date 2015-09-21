using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BillPath.Models.Tests
{
    [TestClass]
    public class AmountTests
    {
        private enum Relationship
        {
            Equal = 1,
            LessThan,
            GreaterThan
        }

        public TestContext TestContext
        {
            get;
            set;
        }

        [TestMethod]
        [DataSource("AmountTests")]
        [DeploymentItem(@"Model\AmountTests.xml", "Model")]
        public void TestAddingTwoAmountsInSameCurrency()
        {
            var first = (decimal)TestContext.DataRow["first"];
            var second = (decimal)TestContext.DataRow["second"];
            var sum = (decimal)TestContext.DataRow["sum"];
            var result = (new Amount(first, default(Currency)) + new Amount(second, default(Currency))).Value;

            Assert.AreEqual(sum, result);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAddingTwoAmountsInDifferentCurrencies()
        {
            var result = new Amount(12, new Currency(new RegionInfo("RO")))
                + new Amount(10, new Currency(new RegionInfo("GB")));
        }

        [TestMethod]
        [DataSource("AmountTests")]
        [DeploymentItem(@"Model\AmountTests.xml", "Model")]
        public void TestSubtractingTwoAmountsInSameCurrency()
        {
            var first = (decimal)TestContext.DataRow["first"];
            var second = (decimal)TestContext.DataRow["second"];
            var difference = (decimal)TestContext.DataRow["difference"];
            var result = (new Amount(first, default(Currency)) - new Amount(second, default(Currency))).Value;

            Assert.AreEqual(difference, result);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSubtractingTwoAmountsInDifferentCurrencies()
        {
            var result = new Amount(12, new Currency(new RegionInfo("RO")))
               - new Amount(10, new Currency(new RegionInfo("GB")));
        }

        [TestMethod]
        [DataSource("AmountTests")]
        [DeploymentItem(@"Model\AmountTests.xml", "Model")]
        public void TestMultiplyingTwoAmountsInSameCurrency()
        {
            var first = (decimal)TestContext.DataRow["first"];
            var second = (decimal)TestContext.DataRow["second"];
            var product = (decimal)TestContext.DataRow["product"];
            var result = (new Amount(first, default(Currency)) * new Amount(second, default(Currency))).Value;

            Assert.AreEqual(product, result);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestMultiplyingTwoAmountsInDifferentCurrencies()
        {
            var result = new Amount(12, new Currency(new RegionInfo("RO")))
               * new Amount(10, new Currency(new RegionInfo("GB")));
        }

        [TestMethod]
        [DataSource("AmountTests")]
        [DeploymentItem(@"Model\AmountTests.xml", "Model")]
        public void TestDividingTwoAmountsInSameCurrency()
        {
            var first = (decimal)TestContext.DataRow["first"];
            var second = (decimal)TestContext.DataRow["second"];
            var quotient = (decimal)TestContext.DataRow["quotient"];
            var result = (new Amount(first, default(Currency)) / new Amount(second, default(Currency))).Value;

            Assert.AreEqual(quotient, result);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDividingTwoAmountsInDifferentCurrencies()
        {
            var result = new Amount(12, new Currency(new RegionInfo("RO")))
               / new Amount(10, new Currency(new RegionInfo("GB")));
        }

        [TestMethod]
        [DataSource("AmountTests")]
        [DeploymentItem(@"Model\AmountTests.xml", "Model")]
        public void TestRelationshipInSameCurrency()
        {
            var first = new Amount((decimal)TestContext.DataRow["first"], default(Currency));
            var second = new Amount((decimal)TestContext.DataRow["second"], default(Currency));

            switch (_GetRelationship())
            {
                case Relationship.LessThan:
                    Assert.IsTrue(first < second, $"{first.Value} < {second.Value}");
                    Assert.IsTrue(first <= second, $"{first.Value} <= {second.Value}");
                    Assert.IsFalse(first == second, $"{first.Value} == {second.Value}");
                    Assert.IsTrue(first != second, $"{first.Value} != {second.Value}");
                    Assert.IsFalse(first >= second, $"{first.Value} >= {second.Value}");
                    Assert.IsFalse(first > second, $"{first.Value} > {second.Value}");
                    break;

                case Relationship.Equal:
                    Assert.IsFalse(first < second, $"{first.Value} < {second.Value}");
                    Assert.IsTrue(first <= second, $"{first.Value} <= {second.Value}");
                    Assert.IsTrue(first == second, $"{first.Value} == {second.Value}");
                    Assert.IsFalse(first != second, $"{first.Value} != {second.Value}");
                    Assert.IsTrue(first >= second, $"{first.Value} >= {second.Value}");
                    Assert.IsFalse(first > second, $"{first.Value} > {second.Value}");
                    break;

                case Relationship.GreaterThan:
                    Assert.IsFalse(first < second, $"{first.Value} < {second.Value}");
                    Assert.IsFalse(first <= second, $"{first.Value} <= {second.Value}");
                    Assert.IsFalse(first == second, $"{first.Value} == {second.Value}");
                    Assert.IsTrue(first != second, $"{first.Value} != {second.Value}");
                    Assert.IsTrue(first >= second, $"{first.Value} >= {second.Value}");
                    Assert.IsTrue(first > second, $"{first.Value} > {second.Value}");
                    break;

                default:
                    throw new InvalidOperationException();
            }
        }
        [TestMethod]
        public void TestEqualInDifferentCurrency()
        {
            Assert.IsFalse(
                new Amount(12, new Currency(new RegionInfo("RO")))
                == new Amount(10, new Currency(new RegionInfo("GB"))));
        }
        [TestMethod]
        public void TestNotEqualInDifferentCurrency()
        {
            Assert.IsTrue(
                new Amount(12, new Currency(new RegionInfo("RO")))
                != new Amount(10, new Currency(new RegionInfo("GB"))));
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestLessThanInDifferentCurrency()
        {
            var result = new Amount(12, new Currency(new RegionInfo("RO")))
               < new Amount(10, new Currency(new RegionInfo("GB")));
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestLessThanOrEqualInDifferentCurrency()
        {
            var result = new Amount(12, new Currency(new RegionInfo("RO")))
               <= new Amount(10, new Currency(new RegionInfo("GB")));
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestGreaterThanEqualInDifferentCurrency()
        {
            var result = new Amount(12, new Currency(new RegionInfo("RO")))
               >= new Amount(10, new Currency(new RegionInfo("GB")));
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestGreaterThanInDifferentCurrency()
        {
            var result = new Amount(12, new Currency(new RegionInfo("RO")))
               > new Amount(10, new Currency(new RegionInfo("GB")));
        }

        private Relationship _GetRelationship()
        {
            return (Relationship)Enum.Parse(typeof(Relationship), (string)TestContext.DataRow["relationship"], true);
        }
    }
}