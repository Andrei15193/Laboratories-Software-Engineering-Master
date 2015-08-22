using System.Runtime.Serialization;

namespace BillPath.Models
{
    [DataContract]
    public class Settings
    {
        [DataMember]
        public Currency PreferredCurrency
        {
            get;
            set;
        }
        [DataMember]
        public CurrencyDisplayFormat CurrencyDisplayFormat
        {
            get;
            set;
        }
    }
}