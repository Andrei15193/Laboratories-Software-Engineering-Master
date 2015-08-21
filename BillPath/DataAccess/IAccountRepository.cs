using System.Collections.Generic;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAllAsync();

        Task SaveAsync(Account account);

        Task Remove(Account account);
    }
}