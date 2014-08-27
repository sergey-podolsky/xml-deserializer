using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer
{
    using System.Xml;
    using System.Xml.Linq;

    using Saxon.Api;

    public interface IDeserializer
    {
        IFormatProvider FormatProvider { get; set; }

        IXmlNamespaceResolver XmlNamespaceResolver { get; set; }

        void Deserialize(XdmItem xdmItem, string xpath, ref object deserializable);

        void Deserialize(string xml, string xpath, ref object deserializable);

        void Deserialize(Uri uri, string xpath, ref object deserializable);
    }
}
