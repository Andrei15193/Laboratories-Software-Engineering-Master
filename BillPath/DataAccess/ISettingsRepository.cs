using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess
{
    public interface ISettingsRepository
    {
        Task<Settings> GetAsync();
        Task SaveAsync(Settings settings);
    }
}