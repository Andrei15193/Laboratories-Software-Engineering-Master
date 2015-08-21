using System.Runtime.Serialization;

namespace BillPath.Models
{
    [DataContract(IsReference = true)]
    public class Account
    {
        Currency Currency
        {
            get;
            set;
        }
    }
}