using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer
{
    public class DictionaryAttribute : AbstractDictionaryAttribute
    {
        public string EntryXPath { get; private set; }
        public string KeyXPath { get; private set; }
        public string ValueXPath { get; set; }

        public DictionaryAttribute(string entryXPath, string keyXPath, string valueXPath)
        {
            this.EntryXPath = entryXPath;
            this.KeyXPath = keyXPath;
            this.ValueXPath = valueXPath;
        }

        public override void Apply(Deserializer deserializer, Saxon.Api.XdmItem xdmItem, Type type, ref object value)
        {
            throw new NotImplementedException();
        }
    }
}
