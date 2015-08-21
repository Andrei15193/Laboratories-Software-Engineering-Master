using System.Runtime.Serialization;
using Windows.UI;

namespace BillPath.Models
{
    [DataContract]
    public class ExpenseCategory
    {
        [DataMember]
        public string Name
        {
            get;
            set;
        }
        [DataMember]
        public ArgbColor RgbColor
        {
            get;
            set;
        }
    }
}