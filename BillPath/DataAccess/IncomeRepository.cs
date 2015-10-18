﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess
{
    public abstract class IncomeRepository
        : IIncomeRepository
    {
        public Task SaveAsync(Income income)
            => SaveAsync(income, CancellationToken.None);
        public abstract Task SaveAsync(Income income, CancellationToken cancellationToken);

        public Task<IEnumerable<Income>> GetAllAsync()
            => GetAllAsync(CancellationToken.None);
        public abstract Task<IEnumerable<Income>> GetAllAsync(CancellationToken cancellationToken);

        public Task<IEnumerable<Income>> GetOnPageAsync(int pageNumber)
            => GetOnPageAsync(pageNumber, CancellationToken.None);
        public abstract Task<IEnumerable<Income>> GetOnPageAsync(int pageNumber, CancellationToken cancellationToken);

        public Task<int> GetPageCountAsync()
            => GetPageCountAsync(CancellationToken.None);
        public abstract Task<int> GetPageCountAsync(CancellationToken cancellationToken);

        public abstract IItemReader<Income> GetReader();

        public Task<int> GetItemCountAsync()
            => GetItemCountAsync(CancellationToken.None);
        public abstract Task<int> GetItemCountAsync(CancellationToken cancellationToken);
    }
}