using System.Runtime.Serialization;

namespace BillPath.Models
{
    [DataContract]
    public class Currency
    {
        [DataMember]
        public string IsoCode
        {
            get;
            set;
        }
        [DataMember]
        public string Symbol
        {
            get;
            set;
        }
    }
}