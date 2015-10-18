using System.Threading;
using System.Threading.Tasks;

namespace BillPath.DataAccess
{
    public interface IItemReaderProvider<TItem>
    {
        IItemReader<TItem> GetReader();

        Task<int> GetItemCountAsync();
        Task<int> GetItemCountAsync(CancellationToken cancellationToken);
    }
}