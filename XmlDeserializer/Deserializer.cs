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

    using XmlDeserializer.AttributeHandlers;

    public class Deserializer : IDeserializer
    {
        public Processor Processor { get; private set; }

        public DocumentBuilder DocumentBuilder { get; private set; }

        public XPathCompiler XPathCompiler { get; private set; }

        public SupportedTypes SupportedTypes { get; private set; }

        public IFormatProvider FormatProvider { get; set; }

        public IXmlNamespaceResolver XmlNamespaceResolver { get; set; }

        public Deserializer()
        {
            this.Processor = new Processor();
            this.DocumentBuilder = this.Processor.NewDocumentBuilder();
            this.XPathCompiler = this.Processor.NewXPathCompiler();
            this.SupportedTypes = new SupportedTypes();
        }

        public void Deserialize<T>(Uri uri, string xpath, ref T deserializable)
        {
            XdmNode xdmNode = this.DocumentBuilder.Build(uri);
            this.Deserialize(xdmNode, xpath, ref deserializable);
        }

        public void Deserialize<T>(XdmNode xdmItem, string xpath, ref T deserializable)
        {
            var attribute = new ItemAttribute(xpath) { IsRequired = true };
            IAttributeHandler attributeHandler = this.SupportedTypes.Get(typeof(T), attribute.GetType());
            object box = deserializable;
            attributeHandler.Handle(this, xdmItem, attribute, typeof(T), ref box);
            deserializable = (T)box;
        }
    }
}