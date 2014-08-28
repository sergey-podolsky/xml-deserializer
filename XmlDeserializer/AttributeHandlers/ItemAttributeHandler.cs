using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer.Converters
{
    using Saxon.Api;

    public abstract class ItemAttributeHandler<T> : XPathAttributeHandler
    {


        protected abstract void Convert(XdmItem xdmItem, string format, ref T deserializable);

        public void Convert(Deserializer deserializer, XdmNode xdmNode, XPathAttribute xpathAttribute, Type type, ref object deserializable)
        {
            var itemAttribute = (ItemAttribute)xpathAttribute;
            
        }
    }
}
