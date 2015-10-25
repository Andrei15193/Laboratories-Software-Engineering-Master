using System;
using System.Globalization;
using BillPath.Models;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.UserInterface.ViewModels.Tests
{
    [TestClass]
    public class AmountViewModelTests
    {
        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(5)]
        [DataRow(8)]
        [DataRow(13)]
        [DataRow(21)]
        [DataRow(-1)]
        [DataRow(-2)]
        [DataRow(-3)]
        [DataRow(-5)]
        [DataRow(-8)]
        [DataRow(-13)]
        [DataRow(-21)]
        public void TestViewModelReturnsOnValueTheProvidedAmountValue(double amountValue)
        {
            var expectedAmountValue = (decimal)amountValue;
            var viewModel = new AmountViewModel(new Amount(expectedAmountValue, default(Currency)));

            var actualAmountValue = viewModel.Value;

            Assert.AreEqual(expectedAmountValue, actualAmountValue);
        }

        [TestMethod]
        public void TestChangingAmountValueRaisesPropertyChangedAccordingly()
        {
            var raiseCount = 0;
            var viewModel = new AmountViewModel(new Amount());
            viewModel.PropertyChanged +=
                (sender, e) =>
                {
                    if (nameof(AmountViewModel.Value).Equals(
                        e.PropertyName,
                        StringComparison.OrdinalIgnoreCase))
                        raiseCount++;
                };

            viewModel.Value = 1;

            Assert.AreEqual(1, raiseCount);
        }

        [DataTestMethod]
        [DataRow("en-AU")]
        [DataRow("en-US")]
        [DataRow("en-GB")]
        public void TestViewModelReturnsOnCurrencyTheProvidedAmountCurrency(string regionName)
        {
            var expectedCurrency = new Currency(new RegionInfo(regionName));
            var viewModel = new AmountViewModel(new Amount(default(decimal), expectedCurrency));

            var actualCurrency = viewModel.Currency;

            Assert.AreEqual(expectedCurrency, actualCurrency);
        }

        [TestMethod]
        public void TestChangingAmountCurrencyRaisesPropertyChangedAccordingly()
        {
            var raiseCount = 0;
            var viewModel = new AmountViewModel(new Amount());
            viewModel.PropertyChanged +=
                (sender, e) =>
                {
                    if (nameof(AmountViewModel.Currency).Equals(
                        e.PropertyName,
                        StringComparison.OrdinalIgnoreCase))
                        raiseCount++;
                };

            viewModel.Currency = new Currency(new RegionInfo("ro-RO"));

            Assert.AreEqual(1, raiseCount);
        }
    }
}