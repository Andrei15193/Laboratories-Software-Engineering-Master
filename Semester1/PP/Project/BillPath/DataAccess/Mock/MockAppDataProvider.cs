using BillPath.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Windows.Storage;

namespace BillPath.DataAccess.Mock
{
    internal class MockAppDataProvider
        : IAppDataProvider
    {
        public string FileName
        {
            get;
            set;
        }

        public AppData AppData
        {
            get;
            private set;
        }

        public void LoadAppData()
        {
            decimal sum = 1;
            DateTime dateTimeNow = DateTime.UtcNow.ToLocalTime();

            AppData appData = new AppData();

            Account account = new Account { CurrencyName = Currencies.RON.Name };
            appData.Accounts.Add(account);
            for (int count = 1; count <= 20; count++)
            {
                Income income = new Income
                                {
                                    Account = account,
                                    Sum = sum++,
                                    DateTaken = dateTimeNow.AddDays(20 - count),
                                    Description = "Income # " + count
                                };
                account.Incomes.Add(income);
            }

            account = new Account { CurrencyName = Currencies.EUR.Name };
            appData.Accounts.Add(account);
            for (int count = 1; count <= 20; count++)
            {
                Income income = new Income
                                {
                                    Account = account,
                                    Sum = sum++,
                                    DateTaken = dateTimeNow.AddDays(20 - count),
                                    Description = "Income # " + count
                                };
                account.Incomes.Add(income);
            }

            appData.Categories.Add(new Category
                                   {
                                       Name = "Red",
                                       ColorName = "Red"
                                   });
            for (int count = 1; count <= 5; count++)
                appData.Categories.Last().Expenses.Add(new Expense
                                                       {
                                                           Account = appData.Accounts.ElementAt(count % appData.Accounts.Count),
                                                           Sum = sum++,
                                                           DateTaken = dateTimeNow.AddDays(20 - count),
                                                           Description = "Expense # " + count,
                                                           Category = appData.Categories.Last()
                                                       });

            appData.Categories.Add(new Category
                                   {
                                       Name = "Blue",
                                       ColorName = "Blue"
                                   });
            for (int count = 1; count <= 5; count++)
                appData.Categories.Last().Expenses.Add(new Expense
                                                       {
                                                           Account = account,
                                                           Sum = sum++,
                                                           DateTaken = dateTimeNow.AddDays(20 - count),
                                                           Description = "Expense # " + count,
                                                           Category = appData.Categories.Last()
                                                       });

            appData.Categories.Add(new Category
                                   {
                                       Name = "Green",
                                       ColorName = "Green"
                                   });
            for (int count = 1; count <= 5; count++)
                appData.Categories.Last().Expenses.Add(new Expense
                                                       {
                                                           Account = account,
                                                           Sum = sum++,
                                                           DateTaken = dateTimeNow.AddDays(20 - count),
                                                           Description = "Expense # " + count,
                                                           Category = appData.Categories.Last()
                                                       });

            appData.Categories.Add(new Category
                                   {
                                       Name = "Magenta",
                                       ColorName = "Magenta"
                                   });
            for (int count = 1; count <= 5; count++)
                appData.Categories.Last().Expenses.Add(new Expense
                                                       {
                                                           Account = account,
                                                           Sum = sum++,
                                                           DateTaken = dateTimeNow.AddDays(20 - count),
                                                           Description = "Expense # " + count,
                                                           Category = appData.Categories.Last()
                                                       });

            appData.Categories.Add(new Category
                                   {
                                       Name = "Yellow",
                                       ColorName = "Yellow"
                                   });
            for (int count = 1; count <= 5; count++)
                appData.Categories.Last().Expenses.Add(new Expense
                                                       {
                                                           Account = account,
                                                           Sum = sum++,
                                                           DateTaken = dateTimeNow.AddDays(20 - count),
                                                           Description = "Expense # " + count,
                                                           Category = appData.Categories.Last()
                                                       });

            AppData = appData;
        }

        public void Save()
        {
        }
    }
}