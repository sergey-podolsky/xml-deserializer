using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer
{
    public class InlineDictionaryAttribute : AbstractDictionaryAttribute
    {
        public string XPath { get; private set; }
        public string EntrySeparator { get; private set; }
        public string KeyValueSeparator { get; private set; }

        public InlineDictionaryAttribute(string xpath, string entrySeparator, string keyValueSeparator)
        {
            this.XPath = xpath;
            this.EntrySeparator = entrySeparator;
            this.KeyValueSeparator = keyValueSeparator;
        }

        public override void Apply(Deserializer deserializer, Saxon.Api.XdmItem xdmItem, Type type, ref object value)
        {
            throw new NotImplementedException();
        }
    }
}
