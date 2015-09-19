using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BillPath.DataAccess.Xml;
using BillPath.Models;
using BillPath.UserInterface.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace BillPath.Tests.IncomeManagement
{
    [Binding]
    [Scope(Feature = "AddIncome")]
    public sealed class AddIncome
    {
        private const string _testFileName = "testFile.xml";
        private readonly IncomesViewModel _viewModel = new IncomesViewModel(new XmlIncomeRepository(new OsFileProvider(), _testFileName));

        [Given("no incomes")]
        [AfterScenario]
        public void GivenNoIncomes()
        {
            File.Delete(_testFileName);
        }

        [When(@"I add a new income in (\w+(?:-\w+)?) currency with the amount of (\d+(?:\.\d+)?) on (\d{1,2}/\d{1,2}/\d{4} \d{1,2}:\d{1,2}:\d{1,2}) and '(.*)' description")]
        public async Task WhenAddingNewIncome(string regionName, decimal amount, DateTime transactionDate, string description)
        {
            await _viewModel.AddIncomeCommand.ExecuteAsync(new Income
            {
                Amount = new Amount(amount, new Currency(new RegionInfo(regionName))),
                DateRealized = transactionDate,
                Description = description
            });
        }

        [Then(@"there should be a total of (\d+) incomes")]
        public async Task ThenIncomeCountIs(int incomeCount)
        {
            await _viewModel.SelectPageCommand.ExecuteAsync(1);
            Assert.AreEqual(incomeCount, _viewModel.SelectedPage.Count());
        }
        [Then(@"the total income in (\w+(?:-\w+)?) currency should be (\d+(?:\.\d+)?)")]
        public async Task ThenTotalIncomeAmountIs(string regionName, decimal totalIncomeAmount)
        {
            await _viewModel.LoadPageInfoCommand.ExecuteAsync(null);

            var currency = new Currency(new RegionInfo(regionName));
            var actualTotalIncomeAmount = _viewModel.TotalAmounts.Single(amount => amount.Currency == currency);

            Assert.AreEqual(totalIncomeAmount, actualTotalIncomeAmount);
        }
        [Then(@"the available funds in (\w+(?:-\w+)?) currency should be (\d+(?:\.\d+)?)")]
        public async Task ThenTotalAvailableAccountIs(string regionName, decimal totalAvailableAmount)
        {
            await _viewModel.LoadPageInfoCommand.ExecuteAsync(null);

            var currency = new Currency(new RegionInfo(regionName));
            var actualTotalIncomeAmount = _viewModel.TotalAmounts.Single(amount => amount.Currency == currency);

            Assert.AreEqual(totalAvailableAmount, actualTotalIncomeAmount);
        }
    }
}