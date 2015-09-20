using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.Modern.Mocks
{
    internal class IncomeRepository
        : DataAccess.IncomeRepository
    {
        private readonly IList<Income> _incomes = new List<Income>(
            Enumerable.Repeat(
                new Income
                {
                    Amount = new Amount(10.2m, new Currency(new RegionInfo("RO"))),
                    DateRealized = DateTimeOffset.Now,
                    Description = "Test description"
                },
                12));

        public int MillisecondsDelay
        {
            get;
            set;
        }

        public IList<Income> Incomes
        {
            get
            {
                return _incomes;
            }
        }

        public override Task<IEnumerable<Income>> GetAllAsync(CancellationToken cancellationToken)
            => Task.FromResult<IEnumerable<Income>>(Incomes);

        public override async Task<IEnumerable<Income>> GetOnPageAsync(int pageNumber, CancellationToken cancellationToken)
        {
            await Task.Delay(MillisecondsDelay);

            return Incomes.Skip((pageNumber - 1) * 10).Take(10);
        }

        public override async Task<int> GetPageCountAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(MillisecondsDelay);

            return 1 + Incomes.Count / 10;
        }

        public override async Task SaveAsync(Income income, CancellationToken cancellationToken)
        {
            await Task.Delay(MillisecondsDelay);
            Incomes.Add(income);
        }
    }
}
