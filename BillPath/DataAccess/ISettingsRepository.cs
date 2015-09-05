using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess
{
    public interface ISettingsRepository
    {
        Task<Settings> GetAsync();
        Task<Settings> GetAsync(CancellationToken cancellationToken);
        Task SaveAsync(Settings settings);
        Task SaveAsync(Settings settings, CancellationToken cancellationToken);
    }
}