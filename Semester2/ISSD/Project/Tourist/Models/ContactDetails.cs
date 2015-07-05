using System.Linq;
using System.Runtime.Serialization;
namespace Tourist.Models
{
    [DataContract(Namespace = Constants.XmlNamespace)]
    public class ContactDetails
        : ValidatableDataModel
    {
        private string _phoneNumber;

        [DataMember]
        public XmlUri Website
        {
            get;
            set;
        }
        [DataMember]
        public string Address
        {
            get;
            set;
        }
        [DataMember]
        public string PhoneNumber
        {
            get
            {
                return _phoneNumber;
            }
            set
            {
                _phoneNumber = value;
                Assert(string.IsNullOrWhiteSpace(_phoneNumber) || _phoneNumber.All(_IsDigitOrWhitespace),
                       "Phone nubmers may contain only digits and whitespaces");
            }
        }

        private bool _IsDigitOrWhitespace(char character)
        {
            return (char.IsDigit(character) || char.IsWhiteSpace(character));
        }
    }
}