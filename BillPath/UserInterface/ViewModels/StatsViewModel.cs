using System.Collections.Generic;
using System.Globalization;
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
                    Amount = new AmountViewModel(new Amount(100m,new Currency(new RegionInfo("RO")))),
                    Color = new ArgbColor(0xFF, 0xFF, 0x00, 0x00),
                    Name = "Red"
                },
                new CurrencyCategoryViewModel
                {
                    Amount = new AmountViewModel(new Amount(11m, new Currency(new RegionInfo("US")))),
                    Color = new ArgbColor(0xFF, 0xFF, 0x00, 0x00),
                    Name = "Red"
                },
                new CurrencyCategoryViewModel
                {
                    Amount = new AmountViewModel(new Amount(54m, new Currency(new RegionInfo("RO")))),
                    Color = new ArgbColor(0xFF, 0x00, 0xFF, 0x00),
                    Name = "Green"
                },
                new CurrencyCategoryViewModel
                {
                    Amount = new AmountViewModel(new Amount(111m,  new Currency(new RegionInfo("RO")))),
                    Color = new ArgbColor(0xFF, 0x00, 0x00, 0xFF),
                    Name = "Blue"
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

        public AmountViewModel Amount
        {
            get;
            set;
        }
    }
}