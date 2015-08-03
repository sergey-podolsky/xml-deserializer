using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer
{
    public abstract class AbstractDictionaryXPathAttribute : AbstractXPathAttribute
    {
        public string KeyFormat { get; set; }
        public string ValueFormat { get; set; }
        public Type KeyConverter { get; set; }
        public Type ValueConverter { get; set; }
        public bool IsRequired { get; set; }
        public bool IsKeyRequired { get; set; }
        public bool IsValueRequired { get; set; }
    }
}
