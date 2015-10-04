using System.Runtime.Serialization;

namespace BillPath.Models
{
    [DataContract]
    public class Settings
        : ICloneable<Settings>
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

        public Settings Clone()
        {
            return (Settings)MemberwiseClone();
        }
    }
}