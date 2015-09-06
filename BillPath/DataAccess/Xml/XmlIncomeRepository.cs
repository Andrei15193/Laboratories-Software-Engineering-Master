using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess.Xml
{
    public class XmlIncomeRepository
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

        public Task SaveAsync(Income income)
        {
            return SaveAsync(income, CancellationToken.None);
        }
        public async Task SaveAsync(Income income, CancellationToken cancellationToken)
        {
            var incomeSerializer = new DataContractSerializer(typeof(Income), new[] { typeof(List<Income>) });
            List<Income> incomes ;

            if (await _fileProvider.FileExistsAsync(_fileName, cancellationToken))
                using (var incomesStream = await _fileProvider.GetReadStreamForAsync(_fileName, cancellationToken))
                    incomes = (List<Income>)incomeSerializer.ReadObject(incomesStream);
            else
                incomes = new List<Income>();

            incomes.Add(income);
            using (var incomesStream = await _fileProvider.GetWriteStreamForAsync(_fileName, cancellationToken))
                incomeSerializer.WriteObject(incomesStream, incomes);
        }
    }
}