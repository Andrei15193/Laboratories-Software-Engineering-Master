using System;
using System.Runtime.Serialization;

namespace BillPath.Models
{
    [DataContract]
    public abstract class Transaction<TTransaction>
        : ICloneable<TTransaction>
        where TTransaction : Transaction<TTransaction>
    {
        [DataMember]
        public string Description
        {
            get;
            set;
        }
        [DataMember]
        public Amount Amount
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

        public abstract TTransaction Clone();
    }
}