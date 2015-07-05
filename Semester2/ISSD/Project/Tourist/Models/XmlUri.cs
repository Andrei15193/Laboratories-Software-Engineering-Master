using System;
using System.Runtime.Serialization;
namespace Tourist.Models
{
    [DataContract(Namespace = Constants.XmlNamespace)]
    public class XmlUri
    {
        private Uri _uri;
        [DataMember(Name = "Value")]
        private string _UriString
        {
            get
            {
                if (_uri == null)
                    return null;
                else
                    return _uri.ToString();
            }
            set
            {
                _uri = new Uri(value, UriKind.RelativeOrAbsolute);
            }
        }

        public static implicit operator XmlUri(string uri)
        {
            if (uri == null)
                return null;
            else
                return new XmlUri
                       {
                           _uri = new Uri(uri, UriKind.RelativeOrAbsolute)
                       };
        }
        public static implicit operator Uri(XmlUri xmlUri)
        {
            if (xmlUri == null)
                return null;
            else
                return xmlUri._uri;
        }
        public static implicit operator XmlUri(Uri uri)
        {
            if (uri == null)
                return null;
            else
                return new XmlUri
                       {
                           _uri = uri
                       };
        }

        public XmlUri()
        {
        }

        public override string ToString()
        {
            return _uri.ToString();
        }
    }
}