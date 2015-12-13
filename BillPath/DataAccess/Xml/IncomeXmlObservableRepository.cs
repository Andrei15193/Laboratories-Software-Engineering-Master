using System;
using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess.Xml
{
    public class IncomeXmlObservableRepository
        : IIncomeXmlRepository
    {
        private readonly IncomeXmlRepository _repository;

        public IncomeXmlObservableRepository(IncomeXmlRepository repository)
        {
            _repository = repository;
        }

        public event EventHandler SavedIncome;
        public event EventHandler RemovedIncome;

        public Task<IIncomeXmlReader> GetReaderAsync()
            => GetReaderAsync(CancellationToken.None);
        public Task<IIncomeXmlReader> GetReaderAsync(CancellationToken cancellationToken)
            => _repository.GetReaderAsync(cancellationToken);

        public Task<int> GetCountAsync()
            => GetCountAsync(CancellationToken.None);
        public Task<int> GetCountAsync(CancellationToken cancellationToken)
            => _repository.GetCountAsync(cancellationToken);

        public Task SaveAsync(Income income)
            => SaveAsync(income, CancellationToken.None);
        public async Task SaveAsync(Income income, CancellationToken cancellationToken)
        {
            await _repository.SaveAsync(income, cancellationToken);
            SavedIncome?.Invoke(this, EventArgs.Empty);
        }
        public Task RemoveAsync(Income income)
            => RemoveAsync(income, CancellationToken.None);
        public async Task RemoveAsync(Income income, CancellationToken cancellationToken)
        {
            await _repository.RemoveAsync(income, cancellationToken);
            RemovedIncome?.Invoke(this, EventArgs.Empty);
        }
    }
}