using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer
{
    public class InlineCollectionAttribute : ItemXPathAttribute
    {
        public string Separator { get; private set; }

        public InlineCollectionAttribute(string xpath, string separator)
            : base(xpath)
        {
            this.Separator = separator;
        }
    }
}
