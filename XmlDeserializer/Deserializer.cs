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

        //public IFormatProvider FormatProvider { get; set; }

        //public IXmlNamespaceResolver XmlNamespaceResolver { get; set; }

        public Deserializer()
        {
            this.Processor = new Processor();
            this.DocumentBuilder = this.Processor.NewDocumentBuilder();
            this.XPathCompiler = this.Processor.NewXPathCompiler();
        }
    }
}