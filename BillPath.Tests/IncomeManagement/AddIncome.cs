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
                Amount = amount,
                Currency = new Currency(new RegionInfo(regionName)),
                DateRealized = transactionDate,
                Description = description
            });
        }

        [Then(@"there should be a total of (\d+) incomes")]
        public void ThenIncomeCountIs(int incomeCount)
        {
            Assert.AreEqual(incomeCount, _viewModel.SelectedPage.Count());
        }
        [Then(@"the total income in (\w+(?:-\w+)?) currency should be (\d+(?:\.\d+)?)")]
        public void ThenTotalIncomeAmountIs(string regionName, decimal totalIncomeAmount)
        {
            var currency = new Currency(new RegionInfo(regionName));
            var actualTotalIncomeAmount = _viewModel
                .SelectedPage
                .Where(income => income?.Currency == currency)
                .Sum(income => income.Amount);

            Assert.AreEqual(totalIncomeAmount, actualTotalIncomeAmount);
        }
        [Then(@"the available funds in (\w+(?:-\w+)?) currency should be (\d+(?:\.\d+)?)")]
        public void ThenTotalAvailableAccountIs(string regionName, decimal totalAvailableAmount)
        {
            var currency = new Currency(new RegionInfo(regionName));
            var actualTotalIncomeAmount = _viewModel
                .SelectedPage
                .Where(income => income?.Currency == currency)
                .Sum(income => income.Amount);

            Assert.AreEqual(totalAvailableAmount, actualTotalIncomeAmount);
        }
    }
}