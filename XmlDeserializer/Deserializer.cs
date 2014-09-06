using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer
{
    using System.IO;
    using System.Net;
    using System.Security.Cryptography;
    using System.Xml;
    using System.Xml.Linq;

    using Saxon.Api;

    public class Deserializer : IDeserializer
    {
        public Processor Processor { get; private set; }

        public DocumentBuilder DocumentBuilder { get; private set; }

        public XPathCompiler XPathCompiler { get; private set; }

        public IFormatProvider FormatProvider { get; set; }

        public IXmlNamespaceResolver XmlNamespaceResolver { get; set; }

        public Deserializer()
        {
            this.Processor = new Processor();
            this.DocumentBuilder = this.Processor.NewDocumentBuilder();
            this.XPathCompiler = this.Processor.NewXPathCompiler();
        }

        public void Deserialize<T>(Uri uri, string xpath, ref T deserializable)
        {
            XdmNode xdmNode = this.DocumentBuilder.Build(uri);
            this.Deserialize(xdmNode, xpath, ref deserializable);
        }

        public void Deserialize<T>(XdmNode xdmItem, string xpath, ref T deserializable)
        {
            var attribute = new ItemAttribute(xpath) { IsRequired = true };
            object box = deserializable;
            this.Deserialize(xdmItem, attribute, typeof(T), ref box);
            deserializable = (T)box;
        }

        internal void Deserialize(XdmNode xdmNode, XPathAttribute attribute, Type type, ref object deserializable)
        {
        }
    }
}