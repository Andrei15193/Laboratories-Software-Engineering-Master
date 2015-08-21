using System;
using System.Runtime.Serialization;

namespace BillPath.Models
{
    [DataContract]
    public class Transaction
    {
        protected Transaction()
        {
        }

        [DataMember]
        public Account Account
        {
            get;
            set;
        }
        [DataMember]
        public string Description
        {
            get;
            set;
        }
        [DataMember]
        public decimal Amount
        {
            get;
            set;
        }
        [DataMember]
        public DateTimeOffset DateRealized
        {
            get;
            set;
        }
    }
}