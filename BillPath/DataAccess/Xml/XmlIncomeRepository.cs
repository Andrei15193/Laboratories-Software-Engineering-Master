using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess.Xml
{
    public class XmlIncomeRepository
        : IncomeRepository
    {
        private readonly FileProvider _fileProvider;
        private readonly string _fileName;

        public XmlIncomeRepository(FileProvider fileProvider, string fileName)
        {
            if (fileProvider == null)
                throw new ArgumentNullException(nameof(fileProvider));
            if (string.IsNullOrWhiteSpace(fileName))
                if (fileName == null)
                    throw new ArgumentNullException(nameof(fileName));
                else
                    throw new ArgumentException("Cannot be empty or white space!", nameof(fileName));

            _fileProvider = fileProvider;
            _fileName = fileName;
        }

        public override async Task SaveAsync(Income income, CancellationToken cancellationToken)
        {
            var incomeSerializer = new DataContractSerializer(typeof(Income), new[] { typeof(List<Income>) });
            var incomes = (await GetAllAsync(cancellationToken)).ToList();

            incomes.Add(income);
            using (var incomesStream = await _fileProvider.GetWriteStreamForAsync(_fileName, cancellationToken))
                incomeSerializer.WriteObject(incomesStream, incomes);
        }

        public override async Task<IEnumerable<Income>> GetAllAsync(CancellationToken cancellationToken)
        {
            var incomeSerializer = new DataContractSerializer(typeof(Income), new[] { typeof(List<Income>) });

            if (await _fileProvider.FileExistsAsync(_fileName, cancellationToken))
                using (var incomesStream = await _fileProvider.GetReadStreamForAsync(_fileName, cancellationToken))
                    return (IEnumerable<Income>)incomeSerializer.ReadObject(incomesStream);
            else
                return Enumerable.Empty<Income>();

        }
    }
}