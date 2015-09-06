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
        public FileProvider FileProvider
        {
            get;
            internal set;
        }

        public Task SaveAsync(Income income)
        {
            return SaveAsync(income, CancellationToken.None);
        }
        public async Task SaveAsync(Income income, CancellationToken cancellationToken)
        {
            var incomeSerializer = new DataContractSerializer(typeof(Income), new[] { typeof(List<Income>) });
            List<Income> incomes ;

            if (await FileProvider.FileExistsAsync(cancellationToken))
                using (var incomesStream = await FileProvider.GetReadStreamAsync(cancellationToken))
                    incomes = (List<Income>)incomeSerializer.ReadObject(incomesStream);
            else
                incomes = new List<Income>();

            incomes.Add(income);
            using (var incomesStream = await FileProvider.GetWriteStreamAsync(cancellationToken))
                incomeSerializer.WriteObject(incomesStream, incomes);
        }
    }
}