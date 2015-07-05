using System.Runtime.Serialization;
namespace Tourist.Models
{
    [DataContract(Namespace = Constants.XmlNamespace)]
    public class Coordinates
    {
        [DataMember]
        public double Longitude
        {
            get;
            set;
        }
        [DataMember]
        public double Latitude
        {
            get;
            set;
        }
        [DataMember]
        public double Altitude
        {
            get;
            set;
        }
    }
}