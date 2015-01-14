using BillPath.Models;
using System.Threading.Tasks;

namespace BillPath.DataAccess
{
    public interface IAppDataProvider
    {
        AppData AppData
        {
            get;
        }

        void LoadAppData();

        void Save();
    }
}