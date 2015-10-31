using System;
using System.Collections.Generic;
using System.Globalization;
using BillPath.Models;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.UserInterface.ViewModels.Tests
{
    [TestClass]
    public class IncomeViewModelTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            _ChangedProperties = new List<string>();
            _Model =
                new Income
                {
                    Amount = new Amount(10, new Currency(new RegionInfo("en-US"))),
                    Description = "This is a test description",
                    DateRealized = DateTimeOffset.Now
                };
            _ViewModel = new IncomeViewModel(_Model);

            _ViewModel.PropertyChanged += (sender, e) => _ChangedProperties.Add(e.PropertyName);
        }

        private IList<string> _ChangedProperties { get; set; }
        private Income _Model { get; set; }
        private IncomeViewModel _ViewModel { get; set; }

        [TestCleanup]
        public void TestCleanup()
        {
            _ViewModel = null;
            _Model = null;
            _ChangedProperties = null;
        }

        [TestMethod]
        public void TestAmountReturnedByViewModelIsEqualToTheOneOfTheModel()
            => Assert.AreEqual(
                _Model.Amount,
                _ViewModel.Amount.Model);

        [TestMethod]
        public void TestSettingNewAmountValueAlsoChangesTheModel()
        {
            var newAmount = new Amount(
                20,
                _Model.Amount.Currency);
            _ViewModel.Amount.Value = newAmount.Value;

            Assert.AreEqual(
                newAmount,
               _Model.Amount);
        }
        [TestMethod]
        public void TestSettingNewAmountCurrencyAlsoChangesTheModel()
        {
            var newAmount = new Amount(
                _Model.Amount.Value,
                new Currency(new RegionInfo("en-AU")));
            _ViewModel.Amount.Currency = newAmount.Currency;

            Assert.AreEqual(
                newAmount,
               _Model.Amount);
        }

        [TestMethod]
        public void TestDescriptionReturnedByViewModelIsEqualToTheOneOfTheModel()
            => Assert.AreEqual(
                _Model.Description,
                _ViewModel.Description);

        [TestMethod]
        public void TestSettingNewDescriptionAlsoChangesTheModel()
        {
            var newDescription = "new description";
            _ViewModel.Description = newDescription;

            Assert.AreEqual(
                newDescription,
               _Model.Description);
        }

        [TestMethod]
        public void TestSettingDescriptionRaisesPropertyChangedAccordingly()
        {
            _ViewModel.Description = "new description";

            Assert.IsTrue(_ChangedProperties.Contains(nameof(IncomeViewModel.Description)));
        }


        [TestMethod]
        public void TestDateRealizedReturnedByViewModelIsEqualToTheOneOfTheModel()
            => Assert.AreEqual(
                _Model.DateRealized,
                _ViewModel.DateRealized);

        [TestMethod]
        public void TestSettingNewDateRealizedAlsoChangesTheModel()
        {
            var newDateRealized = DateTimeOffset.Now.AddMonths(-10);
            _ViewModel.DateRealized = newDateRealized;

            Assert.AreEqual(
                newDateRealized,
               _Model.DateRealized);
        }

        [TestMethod]
        public void TestSettingDateRealizedRaisesPropertyChangedAccordingly()
        {
            _ViewModel.DateRealized = DateTimeOffset.Now.AddMonths(-10);

            Assert.IsTrue(_ChangedProperties.Contains(nameof(IncomeViewModel.DateRealized)));
        }
    }
}