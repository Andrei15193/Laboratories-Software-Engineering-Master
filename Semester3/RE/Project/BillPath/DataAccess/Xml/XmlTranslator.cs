using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace BillPath.DataAccess.Xml
{
    public abstract class XmlTranslator<TValue>
    {
        public Task<TValue> ReadFromAsync(XmlReader xmlReader)
            => ReadFromAsync(xmlReader, CancellationToken.None);
        public abstract Task<TValue> ReadFromAsync(XmlReader xmlReader, CancellationToken cancellationToken);

        public Task WriteToAsync(XmlWriter xmlWriter, TValue value)
            => WriteToAsync(xmlWriter, value, CancellationToken.None);
        public abstract Task WriteToAsync(XmlWriter xmlWriter, TValue value, CancellationToken cancellationToken);
    }
}