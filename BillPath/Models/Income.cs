using System.Runtime.Serialization;

namespace BillPath.Models
{
    [DataContract]
    public class Income
        : Transaction<Income>
    {
        public override Income Clone()
        {
            return new Income
            {
                Currency = Currency,
                Description = Description,
                Amount = Amount,
                DateRealized = DateRealized
            };
        }
    }
}