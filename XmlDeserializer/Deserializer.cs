using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer
{
    using System.IO;
    using System.Net;
    using System.Xml;
    using System.Xml.Linq;

    using Saxon.Api;

    public class Deserializer : IDeserializer
    {
        public Processor Processor { get; private set; }

        public DocumentBuilder DocumentBuilder { get; private set; }

        public IFormatProvider FormatProvider { get; set; }

        public IXmlNamespaceResolver XmlNamespaceResolver { get; set; }

        public Deserializer()
        {
            this.Processor = new Processor();
            this.DocumentBuilder = this.Processor.NewDocumentBuilder();
        }

        public void Deserialize(XdmItem xdmItem, string xpath, ref object deserializable)
        {
        }

        public void Deserialize(string xml, string xpath, ref object deserializable)
        {
            var stringReader = new StringReader(xml);
            XdmItem xdmItem = this.DocumentBuilder.Build(stringReader);
            this.Deserialize(xdmItem, xpath, ref deserializable);
        }

        public void Deserialize(Uri uri, string xpath, ref object deserializable)
        {
            var webClient = new WebClient();
            string xml = webClient.DownloadString(uri);
            this.Deserialize(xml, xpath, ref deserializable);
        }
    }
}