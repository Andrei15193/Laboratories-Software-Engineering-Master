using System;
using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess.Xml
{
    public class XmlIncomeRepository
        : IncomesRepository
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

        public override Task SaveAsync(Income income, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override IItemReader<Income> GetReader()
        {
            throw new NotImplementedException();
        }

        public override Task<int> GetItemCountAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}