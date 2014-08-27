using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer
{
    public class XmlUriAttribute : ItemAttribute
    {
        public XmlUriAttribute(string xpath)
            : base(xpath)
        {
        }
    }
}
