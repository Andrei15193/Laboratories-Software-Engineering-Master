using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess
{
    public abstract class SettingsRepository
        : ISettingsRepository
    {
        public Task<Settings> GetAsync()
        {
            return GetAsync(CancellationToken.None);
        }
        public abstract Task<Settings> GetAsync(CancellationToken cancellationToken);

        public Task SaveAsync(Settings settings)
        {
            return SaveAsync(settings, CancellationToken.None);
        }
        public abstract Task SaveAsync(Settings settings, CancellationToken cancellationToken);
    }
}