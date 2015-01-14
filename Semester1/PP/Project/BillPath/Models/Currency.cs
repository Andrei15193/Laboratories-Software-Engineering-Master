using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace BillPath.Models
{
    [DataContract]
    public class Currency
    {
        [DataMember]
        public string Name
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