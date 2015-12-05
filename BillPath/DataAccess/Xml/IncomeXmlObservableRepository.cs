using System;
using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess.Xml
{
    public class IncomeXmlObservableRepository
        : IncomeXmlRepository
    {
        private readonly IncomeXmlRepository _repository;

        public IncomeXmlObservableRepository(IncomeXmlRepository repository)
        {
            _repository = repository;
        }

        public event EventHandler SavedIncome;

        public override Reader GetReader()
            => _repository.GetReader();

        public override async Task SaveAsync(Income income, CancellationToken cancellationToken)
        {
            await _repository.SaveAsync(income, cancellationToken);
            SavedIncome?.Invoke(this, EventArgs.Empty);
        }
    }
}