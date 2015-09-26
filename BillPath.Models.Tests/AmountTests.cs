using System;
using System.Globalization;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Models.Tests
{
    [TestClass]
    public class AmountTests
    {
        public enum Relationship
        {
            Equal = 0,
            LessThan,
            GreaterThan
        }

        [DataTestMethod]
        [DataRow(1, 1, 2)]
        [DataRow(2, 2, 4)]
        [DataRow(1, 2, 3)]
        [DataRow(2, 1, 3)]
        public void TestAddition(double first, double second, double expected)
        {
            var result = new Amount((decimal)first, default(Currency)) + new Amount((decimal)second, default(Currency));

            Assert.AreEqual((decimal)expected, result.Value);
        }
        [TestMethod]
        public void TestAddingTwoAmountsInDifferentCurrencies()
        {
            Assert.ThrowsException<ArgumentException>(
                () =>
                {
                    var result = new Amount(12, new Currency(new RegionInfo("RO")))
                        + new Amount(10, new Currency(new RegionInfo("GB")));
                });
        }

        [DataTestMethod]
        [DataRow(1, 1, 0)]
        [DataRow(2, 2, 0)]
        [DataRow(1, 2, -1)]
        [DataRow(2, 1, 1)]
        public void TestSubtraction(double first, double second, double expected)
        {
            var result = new Amount((decimal)first, default(Currency)) - new Amount((decimal)second, default(Currency));

            Assert.AreEqual((decimal)expected, result.Value);
        }
        [TestMethod]
        public void TestSubtractingTwoAmountsInDifferentCurrencies()
        {
            Assert.ThrowsException<ArgumentException>(
                () =>
                {
                    var result = new Amount(12, new Currency(new RegionInfo("RO")))
                       - new Amount(10, new Currency(new RegionInfo("GB")));
                });
        }

        [DataTestMethod]
        [DataRow(1, 1, 1)]
        [DataRow(2, 2, 4)]
        [DataRow(1, 2, 2)]
        [DataRow(2, 1, 2)]
        public void TestMultiplication(double first, double second, double expected)
        {
            var result = new Amount((decimal)first, default(Currency)) * new Amount((decimal)second, default(Currency));

            Assert.AreEqual((decimal)expected, result.Value);
        }
        [TestMethod]
        public void TestMultiplyingTwoAmountsInDifferentCurrencies()
        {
            Assert.ThrowsException<ArgumentException>(
                () =>
                {
                    var result = new Amount(12, new Currency(new RegionInfo("RO")))
                       * new Amount(10, new Currency(new RegionInfo("GB")));
                });
        }

        [DataTestMethod]
        [DataRow(1, 1, 1)]
        [DataRow(2, 2, 1)]
        [DataRow(1, 2, 0.5)]
        [DataRow(2, 1, 2)]
        public void TestDivision(double first, double second, double expected)
        {
            var result = new Amount((decimal)first, default(Currency)) / new Amount((decimal)second, default(Currency));

            Assert.AreEqual((decimal)expected, result.Value);
        }
        [TestMethod]
        public void TestDividingTwoAmountsInDifferentCurrencies()
        {
            Assert.ThrowsException<ArgumentException>(
                () =>
                {
                    var result = new Amount(12, new Currency(new RegionInfo("RO")))
                       / new Amount(10, new Currency(new RegionInfo("GB")));
                });
        }

        [DataTestMethod]
        [DataRow(1, 1, Relationship.Equal)]
        [DataRow(2, 2, Relationship.Equal)]
        [DataRow(1, 2, Relationship.LessThan)]
        [DataRow(2, 1, Relationship.GreaterThan)]
        public void TestRelationship(double first, double second, Relationship relationship)
        {
            var left = new Amount((decimal)first, default(Currency));
            var right = new Amount((decimal)second, default(Currency));

            switch (relationship)
            {
                case Relationship.LessThan:
                    Assert.IsTrue(left < right, $"{left.Value} < {right.Value}");
                    Assert.IsTrue(left <= right, $"{left.Value} <= {right.Value}");
                    Assert.IsFalse(left == right, $"{left.Value} == {right.Value}");
                    Assert.IsTrue(left != right, $"{left.Value} != {right.Value}");
                    Assert.IsFalse(left >= right, $"{left.Value} >= {right.Value}");
                    Assert.IsFalse(left > right, $"{left.Value} > {right.Value}");
                    break;

                case Relationship.Equal:
                    Assert.IsFalse(left < right, $"{left.Value} < {right.Value}");
                    Assert.IsTrue(left <= right, $"{left.Value} <= {right.Value}");
                    Assert.IsTrue(left == right, $"{left.Value} == {right.Value}");
                    Assert.IsFalse(left != right, $"{left.Value} != {right.Value}");
                    Assert.IsTrue(left >= right, $"{left.Value} >= {right.Value}");
                    Assert.IsFalse(left > right, $"{left.Value} > {right.Value}");
                    break;

                case Relationship.GreaterThan:
                    Assert.IsFalse(left < right, $"{left.Value} < {right.Value}");
                    Assert.IsFalse(left <= right, $"{left.Value} <= {right.Value}");
                    Assert.IsFalse(left == right, $"{left.Value} == {right.Value}");
                    Assert.IsTrue(left != right, $"{left.Value} != {right.Value}");
                    Assert.IsTrue(left >= right, $"{left.Value} >= {right.Value}");
                    Assert.IsTrue(left > right, $"{left.Value} > {right.Value}");
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
        public void TestLessThanInDifferentCurrency()
        {
            Assert.ThrowsException<ArgumentException>(
                () =>
                {
                    var result = new Amount(12, new Currency(new RegionInfo("RO")))
                       < new Amount(10, new Currency(new RegionInfo("GB")));
                });
        }
        [TestMethod]
        public void TestLessThanOrEqualInDifferentCurrency()
        {
            Assert.ThrowsException<ArgumentException>(
                () =>
                {
                    var result = new Amount(12, new Currency(new RegionInfo("RO")))
                       <= new Amount(10, new Currency(new RegionInfo("GB")));
                });
        }
        [TestMethod]
        public void TestGreaterThanEqualInDifferentCurrency()
        {
            Assert.ThrowsException<ArgumentException>(
                () =>
                {
                    var result = new Amount(12, new Currency(new RegionInfo("RO")))
                       >= new Amount(10, new Currency(new RegionInfo("GB")));
                });
        }
        [TestMethod]
        public void TestGreaterThanInDifferentCurrency()
        {
            Assert.ThrowsException<ArgumentException>(
                () =>
                {
                    var result = new Amount(12, new Currency(new RegionInfo("RO")))
                       > new Amount(10, new Currency(new RegionInfo("GB")));
                });
        }
    }
}