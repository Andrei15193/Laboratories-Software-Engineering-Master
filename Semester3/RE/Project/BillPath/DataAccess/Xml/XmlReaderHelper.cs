using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace BillPath.DataAccess.Xml
{
    public static class XmlReaderHelper
    {
        public static string ToXmlName(this string name)
            => string.IsNullOrWhiteSpace(name)
            ? name
            : char.ToLowerInvariant(name[0]).ToString() + name.Substring(1);

        public static bool IsOnNode(this XmlReader xmlReader, string name)
        {
            if (xmlReader == null)
                throw new ArgumentNullException(nameof(xmlReader));
            if (string.IsNullOrWhiteSpace(name))
                if (name == null)
                    throw new ArgumentNullException(nameof(name));
                else
                    throw new ArgumentException("Cannot be empty or white space!", nameof(name));

            return xmlReader.NodeType == XmlNodeType.Element
                && name.Equals(xmlReader.Name, StringComparison.Ordinal);
        }
        public static bool IsOnNode(this XmlReader xmlReader, string name, string namespaceURI)
        {
            if (xmlReader == null)
                throw new ArgumentNullException(nameof(xmlReader));
            if (string.IsNullOrWhiteSpace(name))
                if (name == null)
                    throw new ArgumentNullException(nameof(name));
                else
                    throw new ArgumentException("Cannot be empty or white space!", nameof(name));
            if (string.IsNullOrWhiteSpace(namespaceURI))
                if (namespaceURI == null)
                    throw new ArgumentNullException(nameof(namespaceURI));
                else
                    throw new ArgumentException("Cannot be empty or white space!", nameof(namespaceURI));

            return xmlReader.NodeType == XmlNodeType.Element
                && name.Equals(xmlReader.Name, StringComparison.Ordinal)
                && namespaceURI.Equals(xmlReader.NamespaceURI, StringComparison.Ordinal);
        }

        public static bool ReadUntil(this XmlReader xmlReader, string name)
            => IsOnNode(xmlReader, name)
            || xmlReader.ReadToFollowing(name);
        public static Task<bool> ReadUntilAsync(this XmlReader xmlReader, string name)
            => ReadUntilAsync(xmlReader, name, CancellationToken.None);
        public static async Task<bool> ReadUntilAsync(this XmlReader xmlReader, string name, CancellationToken cancellationToken)
        {
            var isOnNode = IsOnNode(xmlReader, name);

            while (!isOnNode && await xmlReader.ReadAsync())
            {
                cancellationToken.ThrowIfCancellationRequested();
                isOnNode = IsOnNode(xmlReader, name);
            }

            return isOnNode;
        }

        public static bool ReadUntil(this XmlReader xmlReader, string name, string namespaceURI)
            => IsOnNode(xmlReader, name, namespaceURI)
            || xmlReader.ReadToFollowing(name, namespaceURI);
        public static Task<bool> ReadUntilAsync(this XmlReader xmlReader, string name, string namespaceURI)
            => ReadUntilAsync(xmlReader, name, namespaceURI, CancellationToken.None);
        public static async Task<bool> ReadUntilAsync(this XmlReader xmlReader, string name, string namespaceURI, CancellationToken cancellationToken)
        {
            var isOnNode = IsOnNode(xmlReader, name, namespaceURI);

            while (!isOnNode && await xmlReader.ReadAsync())
            {
                cancellationToken.ThrowIfCancellationRequested();
                isOnNode = IsOnNode(xmlReader, name, namespaceURI);
            }

            return isOnNode;
        }
    }
}