using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer
{
    public class ItemAttribute : XPathAttribute
    {
        public string XPath { get; private set; }
        public string Format { get; set; }
        public Type Converter { get; set; }
        public bool IsRequired { get; set; }

        public ItemAttribute(string xpath)
        {
            this.XPath = xpath;
        }
    }
}
