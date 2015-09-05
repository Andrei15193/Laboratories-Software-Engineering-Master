using System;
using TechTalk.SpecFlow;

namespace BillPath.Tests.IncomeManagement
{
    [Binding]
    [Scope(Feature = "AddIncome")]
    public sealed class AddIncome
    {
        [Given("no incomes")]
        public void GivenNoIncomes()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I add a new income in (\w+(?:-\w+)?) currency with the amount of (\d+(?:\.\d+)?) on (\d{1,2}/\d{1,2}/\d{4} \d{1,2}:\d{1,2}:\d{1,2}) and '(.*)' description")]
        public void WhenAddingNewIncome(string regionName, decimal amount, DateTime transactionDate, string description)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"there should be a total of (\d+) incomes")]
        public void ThenIncomeCountIs(int incomeCount)
        {
            ScenarioContext.Current.Pending();
        }
        [Then(@"the total income in (\w+(?:-\w+)?) currency should be (\d+(?:\.\d+)?)")]
        public void ThenTotalIncomeAmountIs(string regionName, decimal totalIncomeAmount)
        {
            ScenarioContext.Current.Pending();
        }
        [Then(@"the available funds in (\w+(?:-\w+)?) currency should be (\d+(?:\.\d+)?)")]
        public void ThenTotalAvailableAccountIs(string regionName, decimal totalAvailableAmount)
        {
            ScenarioContext.Current.Pending();
        }
    }
}