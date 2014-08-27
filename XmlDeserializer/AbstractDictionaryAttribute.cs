using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer
{
    public abstract class AbstractDictionaryAttribute : XPathAttribute
    {
        public string KeyFormat { get; set; }
        public string ValueFormat { get; set; }
        public bool IsRequired { get; set; }
        public bool IsKeyRequired { get; set; }
        public bool IsValueRequired { get; set; }
    }
}
