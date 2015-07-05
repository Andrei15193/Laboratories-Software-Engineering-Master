using System;
using System.Runtime.Serialization;
namespace Tourist.Models
{
    [DataContract(Namespace = Constants.XmlNamespace)]
    public class Comment
        : ValidatableDataModel
    {
        private string _author;
        private string _text;

        [DataMember]
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                Assert(!string.IsNullOrWhiteSpace(_text), "Comments cannot be blank");
            }
        }
        [DataMember]
        public string Author
        {
            get
            {
                return _author;
            }
            set
            {
                _author = value;
                Assert(!string.IsNullOrWhiteSpace(value), "Must provide an author");
            }
        }
        [DataMember]
        public DateTimeOffset PostTime
        {
            get;
            set;
        }
    }
}