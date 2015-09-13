using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using BillPath.Models;

namespace BillPath.UserInterface.ViewModels
{
    public class StatsViewModel
        : ViewModel
    {
        public StatsViewModel()
        {
            CurrencyCategories = new[]
            {
                new CurrencyCategoryViewModel
                {
                    Color = new ArgbColor(0xFF, 0xFF, 0x00, 0x00),
                    Currency = new Currency(new RegionInfo("RO")),
                    Name = "Red",
                    Amount = 100m
                },
                new CurrencyCategoryViewModel
                {
                    Color = new ArgbColor(0xFF, 0xFF, 0x00, 0x00),
                    Currency = new Currency(new RegionInfo("US")),
                    Name = "Red",
                    Amount = 11m
                },
                new CurrencyCategoryViewModel
                {
                    Color = new ArgbColor(0xFF, 0x00, 0xFF, 0x00),
                    Currency = new Currency(new RegionInfo("RO")),
                    Name = "Green",
                    Amount = 54m
                },
                new CurrencyCategoryViewModel
                {
                    Color = new ArgbColor(0xFF, 0x00, 0x00, 0xFF),
                    Currency = new Currency(new RegionInfo("RO")),
                    Name = "Blue",
                    Amount = 111m
                }
            };
        }

        public IEnumerable<CurrencyCategoryViewModel> CurrencyCategories
        {
            get;
        }
    }

    public class CurrencyCategoryViewModel
        : ViewModel
    {
        public ArgbColor Color
        {
            get;
            set;
        }

        public Currency Currency
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public decimal Amount
        {
            get;
            set;
        }
    }
}